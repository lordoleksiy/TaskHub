using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services.Abstract;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Reponse.Token;
using TaskHub.Common.DTO.User;
using TaskHub.Common.Helpers;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.UserSpecifications;

namespace TaskHub.Bll.Services
{
    public class AuthService: BaseService, IAuthSerivce
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
            var response = new ApiResponse();
            var users = await _unitOfWork.UserRepository.GetAsync(new FindByNameOrEmailSpecification(model.Username, model.Email));
            if (users.Any()) 
            {
                response.Message = "User is already registered!";
                response.Status = Status.Error;
                return response;
            }
            var user = _mapper.Map<UserEntity>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) 
            {
                response.Status = Status.Error;
                response.Message = "Error while creating user.";
                response.Errors = result.Errors.Select(u => u.Description).ToList();
            }
            else
            {
                response.Message = "User successfully registered.";
            }
            return response;
        }

        public async Task<ApiResponse> LoginAsync(LoginModel model)
        {
            var response = new ApiResponse();
            var user = (await _unitOfWork.UserRepository.GetAsync(new FindByNameOrEmailSpecification(model.Username, null))).FirstOrDefault();
            if (user == null) 
            {
                response.Message = "User not found.";
                response.Status = Status.Error;
                return response;
            }
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                response.Message = "Incorrect password.";
                response.Status = Status.Error;
                return response;
            }
            response.Data = new TokenResponseDTO(await GenerateJWTToken(user));
            return response;
        }

        private async Task<string> GenerateJWTToken(UserEntity user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString())
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
