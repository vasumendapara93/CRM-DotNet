using AutoMapper;
using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CRM.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
/*        private readonly SqlTableDependency<User> _sqlTableDependency;
        private readonly IHubContext<UserHub> _hubContext;*/
        User _user;
        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration) : base(db)
        {
           /* _hubContext = hubContext;  */ 
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
/*            var connectionString = _configuration.GetConnectionString("DefaultSQLConnection");
            Console.WriteLine(connectionString);
            _sqlTableDependency = new SqlTableDependency<User>(connectionString,"Users");
            _sqlTableDependency.OnChanged += NotifyChange;
            _sqlTableDependency.Start();*/
        }

   /*     private async void NotifyChange(object sender, RecordChangedEventArgs<User> e)
        {
            var users = await GetAllAsync();
            await _hubContext.Clients.All.SendAsync("RefreshUsers", users);
        }*/

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
           User user = await this.GetAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());
            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(loginRequestDTO.Password, user.Password, BCrypt.Net.HashType.SHA256))
            {
                return new LoginResponseDTO()
                {
                    TokenDTO = new TokenDTO(),
                    UserId = null
                };
            }

            _user = user;

            TokenDTO tokenDTO = await this.CreateToken(populateExp : true);

            LoginResponseDTO loginResponseDTO = new()
            {
                TokenDTO = tokenDTO,
                UserId = _user.Id,
            };
          

            return loginResponseDTO;

        }

        public async Task<TokenDTO> CreateToken(bool populateExp)
        {
            var accessToken = GenerateAccessToken();
            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;
            if (populateExp)
            {
                _user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }
            await this.SaveAsync();

            return new TokenDTO()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private string GenerateAccessToken()
        {
            var role = _db.Roles.FirstOrDefault(u => u.Id == _user.RoleId);
            var tokenHandler = new JwtSecurityTokenHandler();


            var secret = _configuration.GetValue<string>("JTWSettings:SecretKey");
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new(ClaimTypes.Email, _user.Email),
                        new(ClaimTypes.Role, role.RoleName),
                        new(ClaimTypes.Name, _user.Name),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescripter);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken() {
            var randomNumber = new Byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

 /*       private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JTWSettings");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JTWSettings:SecretKey"))),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token,tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if(jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) ){
                throw new SecurityTokenException("Invalid Token");
            }
            return principal;
        }*/

        public async Task<User> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            User user = _mapper.Map<User>(registerationRequestDTO);
            user.Password = HashPassword(user.Password);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task Update(User user)
        {
            user.UpdateDate = DateTime.UtcNow;
           _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
        { 
          /*  var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenDTO.AccessToken);

            var EmailFromToken = jwtSecurityToken.Claims.First(claim => claim.Type == "Email").Value;
            var user = await GetAsync(u=>u.Email == EmailFromToken);*/

            var user = await GetAsync(u=>u.RefreshToken == tokenDTO.RefreshToken);

            if(user is null || !user.RefreshToken.Equals(tokenDTO.RefreshToken) || user.RefreshTokenExpiryTime <= DateTime.UtcNow) {
                throw new Exception("Refresh Token Bad Request");
            }

            _user = user;

            return await this.CreateToken(populateExp: false);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, BCrypt.Net.HashType.SHA256);
        }
    }
}

