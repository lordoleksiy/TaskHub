using Ardalis.Specification;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services;
using TaskHub.Common.Constants;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.User;
using TaskHub.Common.Helpers;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Tests.TaskHub.Bll.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private IAuthService _authService;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private UserManager<UserEntity> _userManager;
        private RoleManager<RoleEntity> _roleManager;
        private IOptions<JwtSettings> _jwtSettings;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _userManager = Substitute.For<UserManager<UserEntity>>(Substitute.For<IUserStore<UserEntity>>(), null, null, null, null, null, null, null, null);
            _roleManager = Substitute.For<RoleManager<RoleEntity>>(Substitute.For<IRoleStore<RoleEntity>>(), null, null, null, null);
            _jwtSettings = Substitute.For<IOptions<JwtSettings>>();
            _jwtSettings.Value.Returns(new JwtSettings { Key = "testkey123123123123123", Issuer = "testissuer", Audience = "testaudience" });

            _authService = new AuthService(_unitOfWork, _mapper, _userManager, _roleManager, _jwtSettings);
        }

        [TestCase("testuser", "testemail@gmail.com", "testpassword")]
        public async Task RegisterAsync_WhenUserIsNotRegistered_ShouldReturnSuccessResponse(string name, string email, string password)
        {
            // Arrange
            var model = new RegisterModel(name, email, password);
            var entity = new UserEntity() { Email = email, UserName = name };
            _unitOfWork.UserRepository.GetAsync(Arg.Any<ISpecification<UserEntity>>()).Returns(new List<UserEntity>());
            _mapper.Map<UserEntity>(model).Returns(entity);

            _userManager.CreateAsync(Arg.Is(entity), Arg.Is<string>(password))
                        .Returns(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterAsync(model);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Success);
        }

        [TestCase("testuser", "testemail@gmail.com", "testpassword")]
        public async Task RegisterAsync_WhenUserIsRegistered_ShouldReturnErrorResponseAndErrorMessage(string name, string email, string password)
        {
            // Arrange
            var model = new RegisterModel(name, email, password);
            var entity = new UserEntity() { Email = email, UserName = name };
            _unitOfWork.UserRepository.GetAsync(Arg.Any<ISpecification<UserEntity>>()).Returns(new List<UserEntity>() { entity });
            _mapper.Map<UserEntity>(model).Returns(entity);

            _userManager.CreateAsync(Arg.Is(entity), Arg.Is<string>(password))
                        .Returns(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterAsync(model);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.UserIsAlreadyRegistered);
        }

        [TestCase("testuser", "testemail@gmail.com", "testpassword")]
        public async Task RegisterAsync_WhenUserManagerReturnsSuceededFalse_ShouldReturnErrorResponseAndErrorMessageAndErrors(string name, string email, string password)
        {
            // Arrange
            var model = new RegisterModel(name, email, password);
            var entity = new UserEntity() { Email = email, UserName = name };
            var errors = new IdentityError[] {
                new IdentityError() { Code = "fail1", Description = "description for fail 1" },
                new IdentityError() { Code = "fail2", Description = "description for fail 2" }
            };
            _unitOfWork.UserRepository.GetAsync(Arg.Any<ISpecification<UserEntity>>()).Returns(new List<UserEntity>() { });
            _mapper.Map<UserEntity>(model).Returns(entity);

            _userManager.CreateAsync(Arg.Any<UserEntity>(), Arg.Any<string>())
                        .Returns(IdentityResult.Failed(errors));

            // Act
            var result = await _authService.RegisterAsync(model);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.ErrorWhileCreatingUser);
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(err => errors.Any(expectedErr => expectedErr.Description == err));
        }

        [TestCase("testuser", "testpassword")]
        public async Task LoginAsync_WhenUserExists_ShouldReturnSuccessResponseAndSpecificRoles(string username, string password)
        {
            // Arrange
            var model = new LoginModel(username, password);
            var user = new UserEntity { UserName = username };
            _unitOfWork.UserRepository.GetAsync(Arg.Any<ISpecification<UserEntity>>()).Returns(new List<UserEntity> { user });
            _userManager.CheckPasswordAsync(Arg.Is(user), Arg.Is(password)).Returns(true);
            _userManager.GetRolesAsync(Arg.Is(user)).Returns(new List<string>());

            // Act
            var result = await _authService.LoginAsync(model);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Success);
        }

        [TestCase("testuser", "testpassword")]
        public async Task LoginAsync_WhenUserDoesNotExist_ShouldReturnErrorResponse(string username, string password)
        {
            // Arrange
            var model = new LoginModel(username, password);
            _unitOfWork.UserRepository.GetAsync(Arg.Any<ISpecification<UserEntity>>()).Returns(new List<UserEntity>());

            // Act
            var result = await _authService.LoginAsync(model);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.UserNotFound);
        }

        [TestCase("testuser", "testpassword")]
        public async Task LoginAsync_WhenUserExistsButWrongPassword_ShouldReturnErrorResponse(string username, string password)
        {
            // Arrange
            var model = new LoginModel(username, password);
            var user = new UserEntity { UserName = username };
            _unitOfWork.UserRepository.GetAsync(Arg.Any<ISpecification<UserEntity>>()).Returns(new List<UserEntity> { user });
            _userManager.CheckPasswordAsync(Arg.Is(user), Arg.Is(password)).Returns(false);

            // Act
            var result = await _authService.LoginAsync(model);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.IncorrectPassword);
        }
    }
}
