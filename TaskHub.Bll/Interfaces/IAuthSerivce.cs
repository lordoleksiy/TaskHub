using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.User;

namespace TaskHub.Bll.Interfaces
{
    public interface IAuthSerivce
    {
        Task<ApiResponse> RegisterAsync(RegisterModel model);
        Task<ApiResponse> LoginAsync(LoginModel model);
    }
}
