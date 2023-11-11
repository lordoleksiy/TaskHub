using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskHub.Bll.Interfaces;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;

namespace TaskHub.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService) 
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyTasksAsync()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(await _taskService.GetTasksByUserNameAsync(userName));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTaskAsync(NewTaskDTO task)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!task.AssignedUserNames.Contains(userName))
            {
                task.AssignedUserNames.Add(userName);
            }
            var response = await _taskService.CreateTaskAsync(task);
            return Ok(response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateTaskAsync(UpdateTaskDTO task)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var response = await _taskService.UpdateTaskAsync(task, userName);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTaskAsync([FromQuery] string name)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var response = await _taskService.DeleteTaskAsync(name, userName);
            return Ok(response);
        }
    }
}
