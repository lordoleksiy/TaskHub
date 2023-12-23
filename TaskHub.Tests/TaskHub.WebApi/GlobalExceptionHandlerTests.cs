using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;
using System.Text.Json;
using TaskHub.Common.DTO.Reponse;
using TaskHub.WebApi.Middlewares;

namespace TaskHub.Tests.TaskHub.WebApi
{
    public class GlobalExceptionHandlerTests
    {
        [Test]
        public async Task Invoke_ExceptionThrown_Returns500WithErrorMessage()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            var next = Substitute.For<RequestDelegate>();
            next(Arg.Any<HttpContext>()).Throws(new Exception("Test error"));

            var middleware = new GlobalExceptionHandler(next);

            // Act
            await middleware.Invoke(context);

            // Assert
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            apiResponse.Should().NotBeNull();
            apiResponse!.Status.Should().Be(Status.Error);
            apiResponse.Message.Should().Be("Test error");
        }

        [Test]
        public async Task Invoke_NoExceptionThrown_DoesNotChangeResponse()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var originalStatusCode = context.Response.StatusCode;

            var next = Substitute.For<RequestDelegate>();
            var middleware = new GlobalExceptionHandler(next);

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.StatusCode.Should().Be(originalStatusCode);
        }
    }
}
