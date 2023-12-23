using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;
using TaskHub.Common.QueryParams;

namespace TaskHub.Bll.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse<IEnumerable<TaskDTO>>> GetTasksByUserNameAsync(string userName, TaskQueryParams filter);
        Task<ApiResponse<TaskDTO>> CreateTaskAsync(NewTaskDTO task);
        Task<ApiResponse<TaskDTO>> UpdateTaskAsync(UpdateTaskDTO task, string userName);
        Task<ApiResponse> DeleteTaskAsync(string taskName, string userName);
    }
}
