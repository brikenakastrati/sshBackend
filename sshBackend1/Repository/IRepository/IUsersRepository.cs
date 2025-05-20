
using Nest;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IUsersRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<ApplicationUserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
