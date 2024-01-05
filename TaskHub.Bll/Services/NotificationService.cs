using AutoMapper;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services.Abstract;
using TaskHub.Common.DTO.Notification;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.NotificationSpecifications;

namespace TaskHub.Bll.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(IUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork, mapper)
        { }
        public async Task CreateForTask(TaskEntity task)
        {
            foreach (var user in task.AssignedUsers)
            {
                var notification = new NotificationEntity
                {
                    User = user,
                    Task = task
                };
                await _unitOfWork.NotificationRepository.AddAsync(notification);
            }
            await _unitOfWork.Commit();
        }
        public async Task UpdateForTask(TaskEntity task)
        {
            foreach (var user in task.AssignedUsers)
            {
                var notification = (await _unitOfWork.NotificationRepository.GetAsync(new GetNotificationsByUserName(user.UserName))).FirstOrDefault();
                if (notification == null)
                {
                    notification = new NotificationEntity
                    {
                        User = user,
                        Task = task
                    };
                    await _unitOfWork.NotificationRepository.AddAsync(notification);
                }
                else
                {
                    notification.DueDate = task.DueDate;
                    _unitOfWork.NotificationRepository.Update(notification);
                }
            }
            await _unitOfWork.Commit();
        }

        public async Task<ApiResponse<IEnumerable<NotificationDTO>>> Get(string userName)
        {
            var notificationsEntities = await _unitOfWork.NotificationRepository.GetAsync(new GetNotificationsByUserName(userName));
            var notifications = notificationsEntities.Select(n => new NotificationDTO()
            {
                TaskName = n.Task.Title,
                LastTime = new DateTime() - n.DueDate
            });
            foreach (var notification in notificationsEntities) 
            {
                _unitOfWork.NotificationRepository.Remove(notification);
            }
            return new(notifications);
        }
    }
}
