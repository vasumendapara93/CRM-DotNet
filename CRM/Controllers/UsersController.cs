using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRM.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IBranchRepository _branchRepo;
        private APIResponse _response;
        private IEnumerable<Role> _roles;
        private readonly IRoleRepository _roleRepo;
        private readonly ApplicationDbContext _db;

        public UsersController(IUserRepository userRepo, IBranchRepository branchRepo, IRoleRepository roleRepo, ApplicationDbContext db)
        {
            _userRepo = userRepo;
            _response = new APIResponse();
            _roleRepo = roleRepo;
            _roles = _roleRepo.GetAllAsync().GetAwaiter().GetResult();
            _db = db;
            _branchRepo = branchRepo;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Data not provided");
                return BadRequest(_response);
            }
            LoginResponseDTO loginResponseDTO = await _userRepo.Login(loginRequestDTO);
            if (loginResponseDTO.User == null || String.IsNullOrEmpty(loginResponseDTO.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or Password is invalid");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = loginResponseDTO;
            return Ok(_response);
        }

        [Authorize(Roles = "Master User")]
        [HttpPost("organization/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateOrganization(RegisterationRequestDTO registerationRequestDTO)
        {
            bool isUniqueEmail = await _userRepo.IsUniqueUser(registerationRequestDTO.Email);
            if (!isUniqueEmail)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email Already Registered");
                return BadRequest(_response);
            }

            registerationRequestDTO.BranchId = null;
            registerationRequestDTO.OrganizationId = null;
            registerationRequestDTO.RoleId = _roles.FirstOrDefault(u => u.RoleName == "Organization").Id;

            User user = await _userRepo.Register(registerationRequestDTO);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Some Error While Registering");
                return BadRequest(_response);
            }

            Branch branch = new Branch
            {
                BranchName = "Default Branch",
                BranchCode = "01",
                OrganizationId = ""
            };

            user.BranchId = branch.Id;
            user.OrganizationId = user.Id;
            branch.OrganizationId = user.Id;
            await _branchRepo.CreateAsync(branch);

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }



        [Authorize(Roles = "Organization")]
        [HttpPost("employee/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEmployee(RegisterationRequestDTO registerationRequestDTO)
        {
            bool isUniqueEmail = await _userRepo.IsUniqueUser(registerationRequestDTO.Email);
            if (!isUniqueEmail)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email Already Registered");
                return BadRequest(_response);
            }

            User user = await _userRepo.Register(registerationRequestDTO);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Some Error While Registering");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
