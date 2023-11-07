using System.Text.Json.Serialization;

namespace TaskHub.Common.DTO.Reponse
{
    public class ApiResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public List<string>? Errors { get; set; }
        public ApiResponse() 
        {
            Status = Status.Success;
            Message = nameof(Status.Success);
        }
    }
    public enum Status
    {
        Success,
        Error
    }
}
