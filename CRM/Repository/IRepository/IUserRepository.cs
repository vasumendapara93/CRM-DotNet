using CRM.Models;
using CRM.Models.DTOs;

namespace CRM.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsUniqueUser(string email);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<User> Register(RegisterationRequestDTO registerationRequestDTO);

        Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);

        Task<TokenDTO> CreateToken(bool populateExp);

        string HashPassword(string password);

        Task Update(User user);
    }
}
