using System.Text.Json.Serialization;

namespace TaskHub.Common.DTO.Reponse
{
    public record ApiResponse
    {
        public ApiResponse(string message, IEnumerable<string> errors): this(Status.Error, message)
        {
            Errors = errors;
        }
        public ApiResponse(Status status, string? message = null)
        {
            Status = status;
            Message = message ?? status.ToString();
        }
        public ApiResponse() : this(Status.Success)
        {}
        public Status Status { get; init; }
        public string Message { get; init; }
        public IEnumerable<string>? Errors { get; init; }
    }

    public enum Status
    {
        Success,
        Error
    }
}
