using AutoMapper;
using System.Threading.Tasks;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services.Abstract;
using TaskHub.Common.Constants;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;
using TaskHub.Common.Enums;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.CategorySpecifications;
using TaskHub.Dal.Specification.TaskSpecifications;
using TaskHub.Dal.Specification.UserSpecifications;

namespace TaskHub.Bll.Services
{
    public class TaskService : BaseService, ITaskService
    {
        public TaskService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            ) : base(unitOfWork, mapper)
        { }

        #region public methods
        public async Task<ApiResponse> GetTasksByUserNameAsync(string userName)
        {
            var users = await _unitOfWork.UserRepository.GetAsync(new GetUserByUserNameSpecification(userName));
            if (!users.Any())
            {
                return CreateErrorResponse(ResponseMessages.UserNotFound);
            }
            var tasks = await _unitOfWork.TaskRepository.GetAsync(new GetTasksByUserNameSpecification(userName));
            return CreateSucсessfullResponse(_mapper.Map<IEnumerable<TaskDTO>>(tasks));
        }

        public async Task<ApiResponse> CreateTaskAsync(NewTaskDTO task)
        {
            var taskEntity = _mapper.Map<TaskEntity>(task);
            taskEntity.Name = GenerateTaskName();
            taskEntity.CreatedDate = DateTime.UtcNow;
            taskEntity.AssignedUsers = (await _unitOfWork.UserRepository.GetAsync(new GetUsersByNamesSpecification(task.AssignedUserNames))).ToList();

            await UpdateCategories(task.Categories, taskEntity);
            await _unitOfWork.TaskRepository.AddAsync(taskEntity);
            await _unitOfWork.Commit();
            return CreateSucсessfullResponse(_mapper.Map<TaskDTO>(taskEntity), ResponseMessages.TaskIsSuccessfullyCreated);
        }

        public async Task<ApiResponse> UpdateTaskAsync(UpdateTaskDTO task, string userName)
        {
            var taskEntity = (await _unitOfWork.TaskRepository.GetAsync(new GetTaskByNameAndUserNameSpecification(task.Name, userName))).FirstOrDefault();
            if (taskEntity == null)
            {
                return CreateErrorResponse(ResponseMessages.NoTaskFound);
            }

            UpdateTaskDetails(task, taskEntity);
            await UpdateAssignedUsers(task, taskEntity);
            await UpdateCategories(task.Categories, taskEntity);

            await _unitOfWork.Commit();
            return CreateSucсessfullResponse(_mapper.Map<TaskDTO>(taskEntity));
        }
        public async Task<ApiResponse> DeleteTaskAsync(string taskName, string userName)
        {
            var taskEntity = (await _unitOfWork.TaskRepository.GetAsync(new GetTaskByNameAndUserNameSpecification(taskName, userName))).FirstOrDefault();
            if (taskEntity == null)
            {
                return CreateErrorResponse(ResponseMessages.NoTaskFound);
            }
            _unitOfWork.TaskRepository.Remove(taskEntity);
            await _unitOfWork.Commit();
            return CreateSucсessfullResponse(message:ResponseMessages.TaskDeletedSuccessfully);
        }
        #endregion
        #region helpers
        private static string GenerateTaskName()
        {
            return $"task#{Guid.NewGuid():N}";
        }

        private static void UpdateTaskDetails(UpdateTaskDTO task, TaskEntity taskEntity)
        {
            taskEntity.Title = task.Title ?? taskEntity.Title;
            taskEntity.Description = task.Description ?? taskEntity.Description;
            taskEntity.DueDate = task.DueDate == null ? taskEntity.DueDate : DateTime.Parse(task.DueDate);
            taskEntity.Status = task.Status;
            if (taskEntity.Status == TaskStatusCode.Closed)
            {
                taskEntity.CompletedDate = DateTime.UtcNow;
            }
        }

        private async Task UpdateAssignedUsers(UpdateTaskDTO task, TaskEntity taskEntity)
        {
            if (task.AssignedUserNames != null)
            {
                var newUsersNames = task.AssignedUserNames.Except(taskEntity.AssignedUsers.Select(u => u.UserName));
                if (newUsersNames.Any())
                {
                    var users = await _unitOfWork.UserRepository.GetAsync(new GetUsersByNamesSpecification(newUsersNames));
                    foreach (var user in users)
                    {
                        taskEntity.AssignedUsers.Add(user);
                    }
                }
            }
        }

        private async Task UpdateCategories(ICollection<string>? tasks, TaskEntity taskEntity)
        {
            if (tasks != null && tasks.Any())
            {
                var categoryNames = taskEntity.Categories != null ? tasks.Except(taskEntity.Categories.Select(u => u.Name)) : tasks;
                if (categoryNames.Any())
                {
                    var categories = (await _unitOfWork.CategoryRepository.GetAsync(new GetCategoriesByNamesSpecification(categoryNames))).ToList();
                    var newCategories = categoryNames.Except(categories.Select(u => u.Name));
                    categories.AddRange(newCategories.Select(c => new CategoryEntity { Name = c }));
                    if (taskEntity.Categories != null)
                    {
                        categories.AddRange(taskEntity.Categories);
                    }
                    taskEntity.Categories = categories;
                }
            }
        }
        #endregion
    }
}
