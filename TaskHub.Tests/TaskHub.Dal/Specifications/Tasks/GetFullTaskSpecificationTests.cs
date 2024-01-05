using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Specification.TaskSpecifications;

namespace TaskHub.Dal.Tests.Specifications
{
    [TestFixture]
    public class GetFullTaskSpecificationTests
    {
        [Test]
        public void GetFullTaskSpecification_ShouldIncludeSubtasksAssignedUsersCategoriesAndParentTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    Id = taskId,
                    Subtasks = new List<TaskEntity> { new TaskEntity() },
                    AssignedUsers = new List<UserEntity> { new UserEntity() },
                    Categories = new List<CategoryEntity> { new CategoryEntity() },
                    ParentTask = new TaskEntity()
                }
            };
            var spec = new GetFullTaskSpecification(taskId);

            // Act
            var result = spec.Evaluate(tasks.AsQueryable()).FirstOrDefault();

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(taskId);
            result.Subtasks.Should().NotBeNull();
            result.AssignedUsers.Should().NotBeNull();
            result.Categories.Should().NotBeNull();
            result.ParentTask.Should().NotBeNull();
        }
    }
}
