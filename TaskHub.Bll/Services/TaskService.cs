﻿using AutoMapper;
using System.Threading.Tasks;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services.Abstract;
using TaskHub.Common.Constants;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;
using TaskHub.Common.Enums;
using TaskHub.Common.QueryParams;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.CategorySpecifications;
using TaskHub.Dal.Specification.TaskSpecifications;
using TaskHub.Dal.Specification.UserSpecifications;

namespace TaskHub.Bll.Services
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly INotificationService _notificationService;
        public TaskService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            INotificationService notificationService
            ) : base(unitOfWork, mapper)
        {
            _notificationService = notificationService;
        }

        #region public methods
        public async Task<ApiResponse<IEnumerable<TaskDTO>>> GetTasksByUserNameAsync(string userName, TaskQueryParams? filter)
        {
            var users = await _unitOfWork.UserRepository.GetAsync(new GetUserByUserNameSpecification(userName));
            if (!users.Any())
            {
                return new (Status.Error, ResponseMessages.UserNotFound);
            }
            
            var tasks = await _unitOfWork.TaskRepository.GetAsync(new GetFilteredTasksSpecification(userName, filter));
            return new(_mapper.Map<IEnumerable<TaskDTO>>(tasks));
        }

        public async Task<ApiResponse<TaskDTO>> CreateTaskAsync(NewTaskDTO task)
        {
            var taskEntity = _mapper.Map<TaskEntity>(task);
            taskEntity.CreatedDate = DateTime.UtcNow;
            taskEntity.AssignedUsers = (await _unitOfWork.UserRepository.GetAsync(new GetUsersByNamesSpecification(task.AssignedUserNames))).ToList();

            if (task.ParentTaskId != null)
            {
                var parentTask = await _unitOfWork.TaskRepository.GetByIdAsync(Guid.Parse(task.ParentTaskId));
                if (parentTask == null)
                {
                    return new(Status.Error, ResponseMessages.ErrorParentTask);
                }
                taskEntity.ParentTaskId = parentTask.Id;
            }


            await UpdateCategories(task.Categories, taskEntity);
            await _unitOfWork.TaskRepository.AddAsync(taskEntity);
            await _unitOfWork.Commit();
            await _notificationService.CreateForTask(taskEntity);
            return new(_mapper.Map<TaskDTO>(taskEntity), ResponseMessages.TaskIsSuccessfullyCreated);
        }

        public async Task<ApiResponse<TaskDTO>> UpdateTaskAsync(UpdateTaskDTO task, string userName)
        {
            var taskEntities = await _unitOfWork.TaskRepository.GetAsync(new GetFullTaskSpecification(new Guid(task.Id)));
            if (!taskEntities.Any())
            {
                return new(Status.Error, ResponseMessages.NoTaskFound);
            }
            var taskEntity = taskEntities.First();
            if (!taskEntity.AssignedUsers.Any(u => u.UserName == userName))
            {
                return new(Status.Error, ResponseMessages.TaskCannotBeUpdated);
            }
            taskEntity.Title = task.Title;
            taskEntity.Description = task.Description;
            taskEntity.Status = task.Status;
            if (task.Status == TaskStatusCode.Closed)
            {
                if (taskEntity.Subtasks != null && !taskEntity.Subtasks.All(s => s.Status == TaskStatusCode.Closed))
                {
                    return new(Status.Error, ResponseMessages.SubTasksMustBeClosed);
                }
                taskEntity.CompletedDate = DateTime.UtcNow;
            }

            await UpdateAssignedUsers(task, taskEntity);
            await UpdateCategories(task.Categories, taskEntity);

            if (DateTime.Parse(task.DueDate) != taskEntity.DueDate)
            {
                taskEntity.DueDate = DateTime.Parse(task.DueDate);
                await _notificationService.UpdateForTask(taskEntity);
            }

            await _unitOfWork.Commit();
            return new(_mapper.Map<TaskDTO>(taskEntity));
        }
        public async Task<ApiResponse> DeleteTaskAsync(string taskId, string userName)
        {
            var taskEntity = await _unitOfWork.TaskRepository.GetByIdAsync(new Guid(taskId));
            if (taskEntity == null)
            {
                return new(Status.Error, ResponseMessages.NoTaskFound);
            }
            _unitOfWork.TaskRepository.Remove(taskEntity);
            await _unitOfWork.Commit();
            return new(Status.Success, ResponseMessages.TaskDeletedSuccessfully);
        }
        #endregion
        #region helpers
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

        private async Task UpdateCategories(ICollection<string>? categories, TaskEntity taskEntity)
        {
            if (categories != null && categories.Any())
            {
                var categoriesInDB = (await _unitOfWork.CategoryRepository.GetAsync(new GetCategoriesByNamesSpecification(categories))).ToList();
                var newCategories = categories.Except(categoriesInDB.Select(u => u.Name));
                categoriesInDB.AddRange(newCategories.Select(c => new CategoryEntity { Name = c }));
                
                taskEntity.Categories = categoriesInDB;
            }
            else
            {
                taskEntity.Categories = null;
            }
        }
        #endregion
    }
}
