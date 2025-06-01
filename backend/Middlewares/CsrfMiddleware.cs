namespace Todo.Middlewares
{
    public class CsrfMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (
                path is not null
                && (path.Contains("api/account/login") || path.Contains("api/account/register"))
            )
            {
                await _next(context);
                return;
            }

            var method = context.Request.Method;

            if (
                HttpMethods.IsPost(method)
                || HttpMethods.IsPut(method)
                || HttpMethods.IsDelete(method)
                || HttpMethods.IsPatch(method)
            )
            {
                var csrfHeader = context.Request.Headers["X-CSRF-TOKEN"].FirstOrDefault();
                context.Request.Cookies.TryGetValue("csrfToken", out var csrfToken);

                if (
                    string.IsNullOrEmpty(csrfHeader)
                    || string.IsNullOrEmpty(csrfToken)
                    || csrfHeader != csrfToken
                )
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Невалидный CSRF токен");
                    return;
                }
            }

            await _next(context);
        }
    }
}
