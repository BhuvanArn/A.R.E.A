public class ResponseBufferingMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseBufferingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            try
            {
                await _next(context);

                memoryStream.Position = 0;

                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                context.Response.ContentLength = responseBody.Length;

                memoryStream.Position = 0;

                await memoryStream.CopyToAsync(originalBodyStream);
            }
            catch
            {
                context.Response.Body = originalBodyStream;
                throw;
            }
        }
    }
}