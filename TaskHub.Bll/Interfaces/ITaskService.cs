using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;

namespace TaskHub.Bll.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse> GetTasksByUserNameAsync(string userName);
        Task<ApiResponse> CreateTaskAsync(NewTaskDTO task);
        Task<ApiResponse> UpdateTaskAsync(UpdateTaskDTO task, string userName);
        Task<ApiResponse> DeleteTaskAsync(string taskName, string userName);
    }
}
