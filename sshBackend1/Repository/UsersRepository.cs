
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using sshBackend1.Data;
using sshBackend1.Helpers;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JwtTokenHelper _jwtHelper;

        public UsersRepository(
            ApplicationDbContext db,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            JwtTokenHelper jwtHelper)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtHelper = jwtHelper;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == username);
            return user == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDTO.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
            {
                return new LoginResponseDTO { User = null, Token = string.Empty };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = _mapper.Map<ApplicationUserDTO>(user);
            userDto.Role = roles.FirstOrDefault();

            var token = _jwtHelper.GenerateToken(user.Id,user.UserName, userDto.Role);

            return new LoginResponseDTO
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<ApplicationUserDTO> Register(RegisterationRequestDTO registerRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName,
                Name = registerRequestDTO.Name
            };

            var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
            if (!result.Succeeded) return null;

            // Ensure roles exist
            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole("admin"));
            if (!await _roleManager.RoleExistsAsync("CLIENT"))
                await _roleManager.CreateAsync(new IdentityRole("CLIENT"));

            // Assign requested role or default
            var role = string.IsNullOrWhiteSpace(registerRequestDTO.Role) ? "CLIENT" : registerRequestDTO.Role;
            await _userManager.AddToRoleAsync(user, role);

            var createdUser = await _userManager.FindByNameAsync(user.UserName);
            var roles = await _userManager.GetRolesAsync(createdUser);

            var userDto = _mapper.Map<ApplicationUserDTO>(createdUser);
            userDto.Role = roles.FirstOrDefault();

            return userDto;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(Expression<Func<ApplicationUser, bool>> filter = null)
        {
            IQueryable<ApplicationUser> query = _db.Users;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
    }
}
