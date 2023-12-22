using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services.Abstract;
using TaskHub.Common.Constants;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Reponse.Token;
using TaskHub.Common.DTO.User;
using TaskHub.Common.Helpers;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.UserSpecifications;

namespace TaskHub.Bll.Services
{
    public class AuthService: BaseService, IAuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<UserEntity> userManager, 
            RoleManager<RoleEntity> roleManager,
            IOptions<JwtSettings> jwtSettings): base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterModel model)
        {
            var users = await _unitOfWork.UserRepository.GetAsync(new GetUserByNameOrEmailSpecification(model.Username, model.Email));
            if (users.Any()) 
            {
                return CreateErrorResponse(ResponseMessages.UserIsAlreadyRegistered);
            }
            var user = _mapper.Map<UserEntity>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) 
            {
                return CreateErrorResponse(ResponseMessages.ErrorWhileCreatingUser, result.Errors.Select(u => u.Description));
            }
            return CreateSucсessfullResponse(message: ResponseMessages.UserSuccessfullyRegistered);
        }

        public async Task<ApiResponse> LoginAsync(LoginModel model)
        {
            var user = (await _unitOfWork.UserRepository.GetAsync(new GetUserByUserNameSpecification(model.Username))).FirstOrDefault();
            if (user == null) 
            {
                return CreateErrorResponse(ResponseMessages.UserNotFound);
            }
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return CreateErrorResponse(ResponseMessages.IncorrectPassword);
            }
            return CreateSucсessfullResponse(new TokenResponseDTO(await GenerateJWTToken(user)));
        }

        private async Task<string> GenerateJWTToken(UserEntity user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            userRoles.ToList().ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
