﻿using TaskHub.Common.DTO.Notification;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Dal.Entities;

namespace TaskHub.Bll.Interfaces
{
    public interface INotificationService
    {
        Task CreateForTask(TaskEntity task);
        Task UpdateForTask(TaskEntity task);
        Task<ApiResponse<IEnumerable<NotificationDTO>>> Get(string userName);
    }
}
