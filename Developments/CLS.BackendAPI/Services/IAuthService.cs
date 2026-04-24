using CLS.BackendAPI.Models.DTOs.Auth;

namespace CLS.BackendAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
