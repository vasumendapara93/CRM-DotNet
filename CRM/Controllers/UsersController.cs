using AutoMapper;
using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepo, IBranchRepository branchRepo, IRoleRepository roleRepo, ApplicationDbContext db, IMapper mapper)
        {
            _userRepo = userRepo;
            _response = new APIResponse();
            _roleRepo = roleRepo;
            _roles = _roleRepo.GetAllAsync().GetAwaiter().GetResult();
            _db = db;
            _branchRepo = branchRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Get(string userId)
        {
            try
            {
                if (userId == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("UserID Is Not Provided");
                    return BadRequest(_response);
                }
                User user = await _userRepo.GetAsync(u => u.Id == userId,IncludeProperties: "Role",Trecked: false);
                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User Not Found");
                    return NotFound(_response);
                }
                
                _response.StatusCode = HttpStatusCode.OK;
                user.Password = "";

                UserResponseDTO responseDTO = _mapper.Map<UserResponseDTO>(user);
                _response.Data = responseDTO;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                if (loginRequestDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Data not provided");
                    return BadRequest(_response);
                }
                LoginResponseDTO loginResponseDTO = await _userRepo.Login(loginRequestDTO);
                if (loginResponseDTO.UserId == null || loginResponseDTO.TokenDTO == new TokenDTO())
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Email or Password Is Invalid");
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = loginResponseDTO;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = SD.Role_MasterUser)]
        [HttpGet("organization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetOrganization()
        {
            try
            {
                var OrgRole = await _roleRepo.GetAsync(u => u.RoleName == SD.Role_Organization);
                IEnumerable<User> organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id);
                IEnumerable<UserResponseDTO> orgUserResponseDTO =  organizations.Select(org => _mapper.Map<User, UserResponseDTO>(org));
                
                if (organizations == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User Not Found");
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = orgUserResponseDTO;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = SD.Role_Organization)]
        [HttpGet("employee/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetEmployees(string id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Id Not Provided");
                    return BadRequest(_response);
                }
                var org = await _userRepo.GetAsync(u => u.Id == id);

                if (org == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Organization Not Exists");
                    return BadRequest(_response);
                }

                var DataEntryOpraterRole = _roles.FirstOrDefault(u => u.RoleName == SD.Role_DataEntryOperator);
                var AssignerRole = _roles.FirstOrDefault(u => u.RoleName == SD.Role_Assiner);
                var SalesPersonRole = _roles.FirstOrDefault(u => u.RoleName == SD.Role_SalesPerson);
                IEnumerable<User> organizations = await _userRepo.GetAllAsync(u => u.OrganizationId == id && (u.RoleId == DataEntryOpraterRole.Id || u.RoleId == AssignerRole.Id || u.RoleId == SalesPersonRole.Id), IncludeProperties : "Branch");
                IEnumerable<UserResponseDTO> orgUserResponseDTO = organizations.Select(org => _mapper.Map<User, UserResponseDTO>(org));

                if (organizations == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User Not Found");
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = orgUserResponseDTO;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = SD.Role_MasterUser)]
        [HttpPost("organization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateOrganization(RegisterationRequestDTO registerationRequestDTO)
        {
            try
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
                    OrganizationId = "",
                    CreateDate = DateTime.Now,
                };

                user.BranchId = branch.Id;
                user.OrganizationId = user.Id;
                branch.OrganizationId = user.Id;
                await _branchRepo.CreateAsync(branch);

                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = SD.Role_MasterUser + "," + SD.Role_Organization)]
        [HttpPut("organization/{id}", Name = "organization/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateOrganization(string id, [FromBody] User user)
        {
            try
            {
                if (id == null || user.Id != id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                bool isUniqueEmail = await _userRepo.IsUniqueUser(user.Email);
                if (!isUniqueEmail)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Email Already Registered");
                    return BadRequest(_response);
                }


                User userFormDB = await _userRepo.GetAsync(u => u.Id == user.Id, Trecked: false);
                if (userFormDB == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User Not Exists");
                    return NotFound(_response);
                }

                user.Password = userFormDB.Password;
                await _userRepo.Update(user);

                _response.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }

            return _response;
        }

        [Authorize(Roles = SD.Role_MasterUser)]
        [HttpDelete("organization/{id}", Name = "organization/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> RomoveOrganization(string id)
        {
            try
            {
                User userFormDB = await _userRepo.GetAsync(u => u.Id == id, Trecked: false);
                if (userFormDB == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User Not Exists");
                    return BadRequest(_response);
                }
                await _userRepo.RemoveAsync(userFormDB);

                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = SD.Role_Organization)]
        [HttpPost("employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateEmployee(RegisterationRequestDTO registerationRequestDTO)
        {
            try
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
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator + "," +SD.Role_Assiner + "," + SD.Role_SalesPerson)]
        [HttpPut("employee/{id}", Name = "employee/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateEmployee(string id, [FromBody] User user)
        {
            try
            {
                if(id == null || user.Id == id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                User userFormDB = await _userRepo.GetAsync(u => u.Id == user.Id, Trecked: false);
                if (userFormDB == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User Not Exists");
                    return NotFound(_response);
                }

                user.Password = userFormDB.Password;
                await _userRepo.Update(user);
                await _userRepo.SaveAsync();

                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }


        [Authorize(Roles = SD.Role_Organization)]
        [HttpDelete("employee/{id}", Name = "employee/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> RemoveEmployee(string id)
        {
            try
            {
                if(id == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                User userFormDB = await _userRepo.GetAsync(u => u.Id == id, Trecked: false);
                if (userFormDB == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User Not Exists");
                    return BadRequest(_response);
                }
                await _userRepo.RemoveAsync(userFormDB);
                await _userRepo.SaveAsync();

                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("password/change")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                if(changePasswordDTO.Email == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Email Not Provided");
                    return BadRequest(_response);
                }
                User user = await _userRepo.GetAsync(u => u.Email == changePasswordDTO.Email);
                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Email Not Registered");
                    return NotFound(_response);
                }
                user.Password = _userRepo.HashPassword(changePasswordDTO.NewPassword);
                user.RefreshTokenExpiryTime = DateTime.UtcNow;
                await _userRepo.SaveAsync();
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePartialUser(string id, JsonPatchDocument<UserUpdateDTO> userUpadteDTOPatch)
        {
            try
            {
                if (userUpadteDTOPatch == null || id is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Id or UserUpdatePatch not Provided");
                    return BadRequest(_response);
                }
                User userFormDB = await _userRepo.GetAsync(u => u.Id == id, Trecked: false);
                if (userFormDB == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User Not Existes");
                    return NotFound(_response);
                }
                var updateDTO = _mapper.Map<UserUpdateDTO>(userFormDB);
                userUpadteDTOPatch.ApplyTo(updateDTO);

                var user = _mapper.Map<User>(updateDTO);
                await _userRepo.Update(user);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
