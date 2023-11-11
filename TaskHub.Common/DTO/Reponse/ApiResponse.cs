using System.Text.Json.Serialization;

namespace TaskHub.Common.DTO.Reponse
{
    public record ApiResponse
    {
        public Status Status { get; init; }
        public string Message { get; init; }
        public object? Data { get; init; }
        public IEnumerable<string>? Errors { get; init; }
    }
    public enum Status
    {
        Success,
        Error
    }
}
