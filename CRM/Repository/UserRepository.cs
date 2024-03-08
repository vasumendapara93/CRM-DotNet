using AutoMapper;
using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRM.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        string secret;
        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {

            _db = db;
            _mapper = mapper;
            secret = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        public async Task<bool> IsUniqueUser(string email)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());
            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(loginRequestDTO.Password, user.Password, BCrypt.Net.HashType.SHA256))
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var role = _db.Roles.FirstOrDefault(u=>u.Id == user.RoleId);

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new(ClaimTypes.Email, user.Email),
                        new(ClaimTypes.Role, role.RoleName),
                        new(ClaimTypes.Name, user.Name),
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescripter);

            LoginResponseDTO loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = user,
            };

            return loginResponseDTO;

        }

        public async Task<User> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            User user = _mapper.Map<User>(registerationRequestDTO);
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, BCrypt.Net.HashType.SHA256);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}

