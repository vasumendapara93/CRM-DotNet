using Azure;
using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private APIResponse _response;
        private IEnumerable<Role> _roles;
        private readonly IRoleRepository _roleRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _response = new APIResponse();
            _roles = _roleRepo.GetAllAsync().GetAwaiter().GetResult();
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
            _response.Result = loginResponseDTO;
            return Ok(_response);
        }

        [Authorize(Roles = "Master User")]
        [HttpPost("registerorganization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterOrganization(RegisterationRequestDTO registerationRequestDTO)
        {
            bool isUniqueEmail = await _userRepo.IsUniqueUser(registerationRequestDTO.Email);
            if (!isUniqueEmail)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email Already Registered");
                return BadRequest(_response);
            }

            registerationRequestDTO.RoleId = _roles.FirstOrDefault(u => u.RoleName == "Organization").Id;

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
