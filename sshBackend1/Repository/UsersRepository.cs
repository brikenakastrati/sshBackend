//using AutoMapper;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using sshBackend1.Data;
//using sshBackend1.Models;
//using sshBackend1.Models.DTOs;
//using sshBackend1.Repository.IRepository;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace MagicVilla_VillaAPI.Repository
//{
//    public class UsersRepository : IUsersRepository
//    {
//        private readonly ApplicationDbContext _db;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private string secretKey;
//        private readonly IMapper _mapper;

//        public UsersRepository(ApplicationDbContext db, IConfiguration configuration,
//            UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
//        {
//            _db = db;
//            _mapper = mapper;
//            _userManager = userManager;
//            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
//            _roleManager = roleManager;
//        }

//        public bool IsUniqueUser(string username)
//        {
//            var user = _db.Users.FirstOrDefault(x => x.UserName == username);

//            if (user == null)
//            {
//                return true;
//            }
//            return false;
//        }

//        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
//        {
//            var user = await _userManager.FindByNameAsync(loginRequestDTO.UserName);

//            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
//            {
//                return new LoginResponseDTO { User = null, Token = string.Empty };
//            }

//            var roles = await _userManager.GetRolesAsync(user);
//            var key = Encoding.ASCII.GetBytes(secretKey);

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id),        
//                new Claim(JwtRegisteredClaimNames.Sub, user.Id),     
//                new Claim(ClaimTypes.Name, user.UserName),
//                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "CLIENT")
//            };

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Expires = DateTime.UtcNow.AddDays(7),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };





//            var tokenHandler = new JwtSecurityTokenHandler();
//            var token = tokenHandler.CreateToken(tokenDescriptor);

//            var userDto = _mapper.Map<ApplicationUserDTO>(user);
//            userDto.Role = roles.FirstOrDefault();

//            return new LoginResponseDTO
//            {
//                Token = tokenHandler.WriteToken(token),
//                User = userDto
//            };
//        }


//        public async Task<ApplicationUserDTO> Register(RegisterationRequestDTO registerRequestDTO)
//        {
//            ApplicationUser user = new()
//            {
//                UserName = registerRequestDTO.UserName,
//                Email = registerRequestDTO.UserName,
//                Name = registerRequestDTO.Name
//            };

//            var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
//            if (!result.Succeeded) return null;

//            // Ensure roles exist
//            if (!await _roleManager.RoleExistsAsync("admin"))
//                await _roleManager.CreateAsync(new IdentityRole("admin"));
//            if (!await _roleManager.RoleExistsAsync("customer"))
//                await _roleManager.CreateAsync(new IdentityRole("customer"));

//            // Assign requested role or default
//            var role = string.IsNullOrWhiteSpace(registerRequestDTO.Role) ? "customer" : registerRequestDTO.Role;
//            await _userManager.AddToRoleAsync(user, role);

//            var createdUser = await _userManager.FindByNameAsync(user.UserName);
//            var roles = await _userManager.GetRolesAsync(createdUser);

//            var userDto = _mapper.Map<ApplicationUserDTO>(createdUser);
//            userDto.Role = roles.FirstOrDefault();

//            return userDto;
//        }

//    }
//}
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

namespace MagicVilla_VillaAPI.Repository
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
            if (!await _roleManager.RoleExistsAsync("client"))
                await _roleManager.CreateAsync(new IdentityRole("client"));

            // Assign requested role or default
            var role = string.IsNullOrWhiteSpace(registerRequestDTO.Role) ? "client" : registerRequestDTO.Role;
            await _userManager.AddToRoleAsync(user, role);

            var createdUser = await _userManager.FindByNameAsync(user.UserName);
            var roles = await _userManager.GetRolesAsync(createdUser);

            var userDto = _mapper.Map<ApplicationUserDTO>(createdUser);
            userDto.Role = roles.FirstOrDefault();

            return userDto;
        }
    }
}
