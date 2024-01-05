using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using System;
using TaskHub.Bll.Mappers;
using TaskHub.Common.DTO.User;
using TaskHub.Dal.Entities;

namespace TaskHub.Tests.TaskHub.Bll.Mappers
{
    [TestFixture]
    public class UserMapperProfileTests
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserMapperProfile());
            });
            _mapper = configuration.CreateMapper();
        }

        [Test]
        public void Map_RegisterModelToUserEntity_ShouldMapCorrectly()
        {
            // Arrange
            var registerModel = new RegisterModel("TestUser", "TestEmail.@gmail.com", "TestPassword");

            // Act
            var userEntity = _mapper.Map<UserEntity>(registerModel);

            // Assert
            userEntity.Should().NotBeNull();
            userEntity.UserName.Should().Be(registerModel.Username);
        }
    }
}
