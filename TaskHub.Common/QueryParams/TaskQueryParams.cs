using TaskHub.Common.Enums;

namespace TaskHub.Common.QueryParams
{
    public record TaskQueryParams
    {
        public TaskStatusCode[]? Status { get; init; }
        public string[]? Category { get; init; }
    }
}
