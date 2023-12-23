using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Reponse.Token;
using TaskHub.Common.DTO.User;

namespace TaskHub.Bll.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(RegisterModel model);
        Task<ApiResponse<TokenResponseDTO>> LoginAsync(LoginModel model);
    }
}
