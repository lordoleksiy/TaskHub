namespace TaskHub.Common.DTO.Reponse
{
    public record ApiResponse<T> : ApiResponse
    {
        public T? Data { get; init; }
    }

    public record ApiResponse
    {
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
