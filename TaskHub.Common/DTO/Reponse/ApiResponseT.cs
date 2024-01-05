namespace TaskHub.Common.DTO.Reponse
{
    public record ApiResponse<T> : ApiResponse where T : class
    {
        public ApiResponse(T data, string? message = null) : base(Status.Success, message)
        {
            Data = data;
        }

        public ApiResponse(Status status, string? message = null) : base(status, message)
        {
        }
        public ApiResponse() : base()
        {
        }

        public T? Data { get; init; }
    }
}
