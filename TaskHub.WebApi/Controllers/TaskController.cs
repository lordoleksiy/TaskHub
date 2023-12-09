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
            var userName = User.FindFirst(ClaimTypes.Name)?.Value!;
            var response = await _taskService.GetTasksByUserNameAsync(userName);
            if (response.Status == Status.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTaskAsync(NewTaskDTO task)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value!;
            if (!task.AssignedUserNames.Contains(userName))
            {
                task.AssignedUserNames.Add(userName);
            }
            var response = await _taskService.CreateTaskAsync(task);
            if (response.Status == Status.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateTaskAsync(UpdateTaskDTO task)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value!;
            var response = await _taskService.UpdateTaskAsync(task, userName);
            if (response.Status == Status.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTaskAsync([FromQuery] string Id)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value!;
            var response = await _taskService.DeleteTaskAsync(Id, userName);
            if (response.Status == Status.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
