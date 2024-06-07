//using AutoMapper;
//using CRM.Model;
//using CRM.Models;
//using CRM.Models.DTOs;
//using CRM.Models.Tables;
//using CRM.Repository.IRepository;
//using CRM.StaticData;
//using CRM.StaticData.ModelFileds;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System.Net;
//using static System.Net.WebRequestMethods;

//namespace CRM.Controllers
//{
//    [Route("api/user")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly IUserRepository _userRepo;
//        private readonly IBranchRepository _branchRepo;
//        private APIResponse _response;
//        private IEnumerable<Role> _roles;
//        private readonly IRoleRepository _roleRepo;
//        private readonly ApplicationDbContext _db;
//        private readonly IMapper _mapper;
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        private readonly IMailServiceRepository _mailService;
//        private readonly IUserActivationTokenRepository _userActivationTokenRepo;
//        private readonly IConfiguration _configuration;

//        public UsersController(IUserRepository userRepo, IConfiguration configuration, IBranchRepository branchRepo, IUserActivationTokenRepository userActivationTokenRepo, IRoleRepository roleRepo, IMailServiceRepository mailService, ApplicationDbContext db, IMapper mapper, IWebHostEnvironment webHostEnvironment)
//        {
//            _userRepo = userRepo;
//            _response = new APIResponse();
//            _roleRepo = roleRepo;
//            _roles = _roleRepo.GetAllAsync().GetAwaiter().GetResult();
//            _db = db;
//            _branchRepo = branchRepo;
//            _mapper = mapper;
//            _webHostEnvironment = webHostEnvironment;
//            _mailService = mailService;
//            _userActivationTokenRepo = userActivationTokenRepo;
//            _configuration = configuration;
//        }

//        [HttpGet]
//        [Authorize]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> Get(string userId)
//        {
//            try
//            {
//                if (userId == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("UserID Is Not Provided");
//                    return BadRequest(_response);
//                }
//                User user = await _userRepo.GetAsync(u => u.Id == userId, IncludeProperties: "Role", Trecked: false);
//                if (user == null)
//                {
//                    _response.StatusCode = HttpStatusCode.NotFound;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Found");
//                    return NotFound(_response);
//                }

//                _response.StatusCode = HttpStatusCode.OK;
//                user.Password = "";

//                UserResponseDTO responseDTO = _mapper.Map<UserResponseDTO>(user);
//                _response.Data = responseDTO;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [HttpPost("login")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<ActionResult<APIResponse>> Login(LoginRequestDTO loginRequestDTO)
//        {
//            try
//            {
//                if (loginRequestDTO == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Data not provided");
//                    return BadRequest(_response);
//                }
//                LoginResponseDTO loginResponseDTO = await _userRepo.Login(loginRequestDTO);
//                if (loginResponseDTO.UserId == null || loginResponseDTO.TokenDTO == new TokenDTO())
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Email or Password Is Invalid");
//                    return BadRequest(_response);
//                }
//                _response.StatusCode = HttpStatusCode.OK;
//                _response.Data = loginResponseDTO;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [Authorize(Roles = SD.Role_MasterUser)]
//        [HttpGet("organization")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<ActionResult<APIResponse>> GetOrganization([FromQuery] string? search, [FromQuery] string? orderBy, [FromQuery] string? order = Order.ASC, [FromQuery] int PageSize = 0, [FromQuery] int PageNo = 1)
//        {
//            try
//            {
//                var OrgRole = await _roleRepo.GetAsync(u => u.RoleName == SD.Role_Organization);

//                IEnumerable<User> organizations = new List<User>();
//                var totalRecords = 0;
//                if (!string.IsNullOrEmpty(search))
//                {
//                    search = search.ToLower().Trim();
//                    if (string.IsNullOrEmpty(orderBy) || orderBy == "null")
//                    {
//                        organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id && (u.Name.ToLower().Contains(search) || u.Email.ToLower().Contains(search) || (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(search)) || (u.ContactPerson != null && u.ContactPerson.ToLower().Contains(search)) || u.CreateDate.Date.ToString().ToLower().Contains(search)), PageSize: PageSize, PageNo: PageNo);
//                    }
//                    else if (string.IsNullOrEmpty(order) || order == "null" || order == Order.ASC)
//                    {
//                        /*order ASC*/
//                        organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id && (u.Name.ToLower().Contains(search) || u.Email.ToLower().Contains(search) || (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(search)) || (u.ContactPerson != null && u.ContactPerson.ToLower().Contains(search)) || u.CreateDate.Date.ToString().ToLower().Contains(search)) || u.CreateDate.Date.ToString().ToLower().Contains(search), OrderBy: _userRepo.CreateSelectorExpression(orderBy), Order: Order.ASC, PageSize: PageSize, PageNo: PageNo);
//                    }
//                    else
//                    {
//                        /*order DSE*/
//                        organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id && (u.Name.ToLower().Contains(search) || u.Email.ToLower().Contains(search) || (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(search)) || (u.ContactPerson != null && u.ContactPerson.ToLower().Contains(search)) || u.CreateDate.Date.ToString().ToLower().Contains(search)), OrderBy: _userRepo.CreateSelectorExpression(orderBy), Order: Order.DESC, PageSize: PageSize, PageNo: PageNo);
//                    }
//                    totalRecords = _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id && (u.Name.ToLower().Contains(search) || u.Email.ToLower().Contains(search) || (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(search)) || (u.ContactPerson != null && u.ContactPerson.ToLower().Contains(search)) || u.CreateDate.Date.ToString().ToLower().Contains(search))).GetAwaiter().GetResult().Count();
//                }
//                else
//                {
//                    Console.WriteLine(orderBy);
//                    if (string.IsNullOrEmpty(orderBy) || orderBy == "null")
//                    {
//                        organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id, PageSize: PageSize, PageNo: PageNo);
//                    }
//                    else if (string.IsNullOrEmpty(order) || order == "null" || order == Order.ASC)
//                    {
//                        /*order ASC*/
//                        if (orderBy != UserFields.CreateDate)
//                        {
//                            organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id, OrderBy: _userRepo.CreateSelectorExpression(orderBy), Order: Order.ASC, PageSize: PageSize, PageNo: PageNo);
//                        }
//                        else
//                        {
//                            organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id, OrderBy: u => u.CreateDate.ToString(), Order: Order.ASC, PageSize: PageSize, PageNo: PageNo);
//                        }
//                    }
//                    else
//                    {
//                        /*order DSE*/
//                        if (orderBy != UserFields.CreateDate)
//                        {
//                            organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id, OrderBy: _userRepo.CreateSelectorExpression(orderBy), Order: Order.DESC, PageSize: PageSize, PageNo: PageNo);
//                        }
//                        else
//                        {
//                            organizations = await _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id, OrderBy: u => u.CreateDate.ToString(), Order: Order.DESC, PageSize: PageSize, PageNo: PageNo);
//                        }
//                    }
//                    totalRecords = _userRepo.GetAllAsync(u => u.RoleId == OrgRole.Id).GetAwaiter().GetResult().Count();
//                    var pagination = new Pagination { PageNo = PageNo, PageSize = PageSize };
//                    Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(pagination));
//                }
//                IEnumerable<UserResponseDTO> userResponseDTOs = organizations.Select(user => _mapper.Map<UserResponseDTO>(user));

//                _response.Data = new RecordsResponse
//                {
//                    TotalRecords = totalRecords,
//                    Records = userResponseDTOs
//                };

//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [Authorize(Roles = SD.Role_Organization)]
//        [HttpGet("employee/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<ActionResult<APIResponse>> GetEmployees(string id)
//        {
//            try
//            {
//                if (id == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Id Not Provided");
//                    return BadRequest(_response);
//                }
//                var org = await _userRepo.GetAsync(u => u.Id == id);

//                if (org == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Organization Not Exists");
//                    return BadRequest(_response);
//                }

//                var DataEntryOpraterRole = _roles.FirstOrDefault(u => u.RoleName == SD.Role_DataEntryOperator);
//                var AssignerRole = _roles.FirstOrDefault(u => u.RoleName == SD.Role_Assiner);
//                var SalesPersonRole = _roles.FirstOrDefault(u => u.RoleName == SD.Role_SalesPerson);
//                IEnumerable<User> organizations = await _userRepo.GetAllAsync(u => u.OrganizationId == id && (u.RoleId == DataEntryOpraterRole.Id || u.RoleId == AssignerRole.Id || u.RoleId == SalesPersonRole.Id), IncludeProperties: "Branch");
//                IEnumerable<UserResponseDTO> orgUserResponseDTO = organizations.Select(org => _mapper.Map<User, UserResponseDTO>(org));

//                if (organizations == null)
//                {
//                    _response.StatusCode = HttpStatusCode.NotFound;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Found");
//                    return NotFound(_response);
//                }

//                _response.StatusCode = HttpStatusCode.OK;
//                _response.Data = orgUserResponseDTO;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [Authorize(Roles = SD.Role_MasterUser)]
//        [HttpPost("organization")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<ActionResult<APIResponse>> CreateOrganization(RegisterationRequestDTO registerationRequestDTO)
//        {
//            try
//            {bool isUniqueEmail = await _userRepo.IsUniqueUser(registerationRequestDTO.Email);
//                if (!isUniqueEmail)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Email Already Registered");
//                    return BadRequest(_response);
//                }

//                registerationRequestDTO.BranchId = null;
//                registerationRequestDTO.OrganizationId = null;
//                registerationRequestDTO.RoleId = _roles.FirstOrDefault(u => u.RoleName == SD.Role_Organization).Id;
//                User user = _mapper.Map<User>(registerationRequestDTO);

//                await _userRepo.CreateAsync(user);
//                if (user == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Some Error While Requesting");
//                    return BadRequest(_response);
//                }
//                UserActivationToken userActivationToken = new UserActivationToken();
//                userActivationToken = await _userActivationTokenRepo.GetAsync(u => u.UserId == user.Id);
//                if (userActivationToken != null)
//                {
//                    userActivationToken.ActivationToken = _userActivationTokenRepo.CreateActivationToken();
//                    userActivationToken.ActivationTokenExpiryTime = DateTime.UtcNow.AddDays(7);
//                    await _userActivationTokenRepo.SaveAsync();
//                }
//                else
//                {
//                    userActivationToken = new()
//                    {
//                        UserId = user.Id,
//                        ActivationToken = _userActivationTokenRepo.CreateActivationToken(),
//                        ActivationTokenExpiryTime = DateTime.UtcNow.AddDays(7)
//                    };

//                    await _userActivationTokenRepo.CreateAsync(userActivationToken);
//                }



//                string mailHtmlBody = $"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Activate Your Account</title>\r\n    <style>\r\n        body, table, td, p, a, h1 {{\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n            margin: 0;\r\n            padding: 0;\r\n            border: 0;\r\n            font-family: Arial, sans-serif;\r\n            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\r\n        }}\r\n        body{{\r\n            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\r\n            background-color: gray;\r\n        }}\r\n        td, p{{\r\n            text-align: start;\r\n        }}\r\n        .container {{\r\n            width: auto;\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            border: 1px solid #cccccc;\r\n            gap: 1rem;\r\n            border: none;\r\n            border-top: 5px solid red;\r\n        }}\r\n        .logo {{\r\n            text-align: center;\r\n            padding: 20px;\r\n        }}\r\n        .content {{\r\n            padding-left: 3rem;\r\n            padding-right: 3rem;\r\n            margin-bottom: 2rem;\r\n            text-align: center;\r\n            letter-spacing: 0.5px;\r\n            font-weight: 400;\r\n            color: black;\r\n            line-height: 25px;\r\n            font-size: 0.95rem;\r\n        }}\r\n        .content-p{{\r\n            margin-top: 0.2rem;\r\n        }}\r\n        .button {{\r\n            display: inline-block;\r\n            background-color: #4CAF50;\r\n            color: white;\r\n            padding: 10px 20px;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n            margin-top: 1rem;\r\n            margin-bottom: 1rem;\r\n        }}\r\n        .footer {{\r\n            background-color: #f2f2f2;\r\n            padding: 10px;\r\n            text-align: center;\r\n            font-size: 14px;\r\n            color: #202020;\r\n            height:3rem;\r\n            background: #fbe0e0;\r\n        }}\r\n    </style>\r\n</head>\r\n<body style=\"background: #023b46;\">\r\n    <table class=\"container\" cellpadding=\"0\" cellspacing=\"0\" >\r\n        <tr>\r\n            <td>\r\n                <div class=\"logo\">\r\n                    <img style=\"width: 200px;\" src=\"https://r1dq4k84-7246.inc1.devtunnels.ms/Storage/Images/logo-dark-mail.png\" alt=\"Limpid Systems\">\r\n                </div>\r\n                <div class=\"content\">\r\n                    <h1>Activate your account!</h1>\r\n                    <p class=\"content-p\" style=\"margin-top: 2rem;\" style=\"font-weight:700;\">Hello, {user.Name}</p>\r\n                    <p class=\"content-p\" >Welcome to Limpid CRM! We're so happy you're here.</p>\r\n                    <p class=\"content-p\" style=\"margin-top: 1rem;\">Activate your account by clicking the link below:</p>\r\n                   " +
//                    $" <a href=\"" + $"{_configuration.GetValue<string>("AllowedURL")}/account/a?id={user.Id}&token={userActivationToken.ActivationToken}" + "\" class=\"button\" style=\"color: #ffffff;\">Activate Your Account</a>\r\n                    <p class=\"content-p\" style=\"margin-top: 1rem;\">Questions about CRM?</p>\r\n                    <p class=\"content-p\">We'll be in touch with you, but should you need to reach our team sooner, feel free to contact us at:\r\n                        <a style=\"color:#0068ff;\" href=\"mailto:support@limpidsystems.com\">support@limpidsystems.com</a>\r\n                    </p>\r\n                    <p class=\"content-p\">We're happy to help!</p>\r\n                    <p class=\"content-p\" style=\"margin-top: 2rem;\">\r\n                        Cheers,\r\n                        <br>\r\n                        <p style=\"font-weight:700;\">The Limpid CRM Team</p>\r\n                    </p>\r\n                </div>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td class=\"footer\">© 2024, LimpidSystems All Rights Reserved.</td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";
//                _mailService.sendMail(user.Email, "Welcome to Limpid CRM", mailHtmlBody);
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }


//        [HttpPost("activate")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<ActionResult<APIResponse>> ActiveAccount(UserActivationRequestDTO userActivationRequestDTO)
//        {
//            try
//            {
//                if (userActivationRequestDTO is null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Request Data Not Provided");
//                    return BadRequest(_response);
//                }

//                User user = await _userRepo.GetAsync(u => u.Id == userActivationRequestDTO.UserId, IncludeProperties: "Role");
//                if (user == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("The URL you're trying to access doesn't exists. Please check the link and try again.");
//                    return BadRequest(_response);
//                }

//                if (user.IsAccountActivated)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Your Account Is Already Activated");
//                    return BadRequest(_response);
//                }

//                UserActivationToken userActivationToken = await _userActivationTokenRepo.GetAsync(u => u.UserId == userActivationRequestDTO.UserId);
//                if (userActivationToken == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("The URL you're trying to access doesn't exists. Please check the link and try again.");
//                    return BadRequest(_response);
//                }

//                if (userActivationToken.ActivationTokenExpiryTime <= DateTime.Now)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    if (user.Role.RoleName == SD.Role_Organization)
//                    {
//                        _response.ErrorMessages.Add("Your Account Activation Link is Expire, Contact Limpid CRM To Resend");
//                    }
//                    else
//                    {
//                        _response.ErrorMessages.Add("Your Account Activation Link is Expire, Contact You Organization To Resend");
//                    }
//                    return BadRequest(_response);
//                }
//                if (userActivationToken.ActivationToken != userActivationRequestDTO.ActivationToken)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("The URL you're trying to access doesn't exists. Please check the link and try again.");
//                    return BadRequest(_response);
//                }

//                user.Name = userActivationRequestDTO.Name;
//                user.Password = _userRepo.HashPassword(userActivationRequestDTO.Password);
//                user.IsAccountActivated = true;

//                if (user.Role.RoleName == SD.Role_Organization)
//                {
//                    Branch branch = new Branch
//                    {
//                        BranchName = "Default Branch",
//                        BranchCode = "01",
//                        OrganizationId = "",
//                        CreateDate = DateTime.Now,
//                    };

//                    user.BranchId = branch.Id;
//                    user.OrganizationId = user.Id;
//                    branch.OrganizationId = user.Id;
//                    await _branchRepo.CreateAsync(branch);
//                }
//                await _userRepo.SaveAsync();

//                _response.StatusCode = HttpStatusCode.OK;


//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [HttpPost("activatation/resend/{id}")]
//        [Authorize]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<ActionResult<APIResponse>> ResendActivationRequest(string id)
//        {
//            try
//            {
//                if (id is null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Id Data Not Provided");
//                    return BadRequest(_response);
//                }

//                User user = await _userRepo.GetAsync(u => u.Id == id);
//                if (user == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Exists");
//                    return BadRequest(_response);
//                }

//                if (user.IsAccountActivated)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Account Is Already Active");
//                    return BadRequest(_response);
//                }

//                UserActivationToken userActivationToken = new UserActivationToken();
//                userActivationToken = await _userActivationTokenRepo.GetAsync(u => u.UserId == user.Id);
//                if (userActivationToken != null)
//                {
//                    userActivationToken.ActivationToken = _userActivationTokenRepo.CreateActivationToken();
//                    userActivationToken.ActivationTokenExpiryTime = DateTime.UtcNow.AddDays(7);
//                    await _userActivationTokenRepo.SaveAsync();
//                }
//                else
//                {
//                    userActivationToken = new()
//                    {
//                        UserId = user.Id,
//                        ActivationToken = _userActivationTokenRepo.CreateActivationToken(),
//                        ActivationTokenExpiryTime = DateTime.UtcNow.AddDays(7)
//                    };

//                    await _userActivationTokenRepo.CreateAsync(userActivationToken);
//                }



//                string mailHtmlBody = $"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Activate Your Account</title>\r\n    <style>\r\n        body, table, td, p, a, h1 {{\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n            margin: 0;\r\n            padding: 0;\r\n            border: 0;\r\n            font-family: Arial, sans-serif;\r\n            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\r\n        }}\r\n        body{{\r\n            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\r\n            background-color: gray;\r\n        }}\r\n        td, p{{\r\n            text-align: start;\r\n        }}\r\n        .container {{\r\n            width: auto;\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            border: 1px solid #cccccc;\r\n            gap: 1rem;\r\n            border: none;\r\n            border-top: 5px solid red;\r\n        }}\r\n        .logo {{\r\n            text-align: center;\r\n            padding: 20px;\r\n        }}\r\n        .content {{\r\n            padding-left: 3rem;\r\n            padding-right: 3rem;\r\n            margin-bottom: 2rem;\r\n            text-align: center;\r\n            letter-spacing: 0.5px;\r\n            font-weight: 400;\r\n            color: black;\r\n            line-height: 25px;\r\n            font-size: 0.95rem;\r\n        }}\r\n        .content-p{{\r\n            margin-top: 0.2rem;\r\n        }}\r\n        .button {{\r\n            display: inline-block;\r\n            background-color: #4CAF50;\r\n            color: white;\r\n            padding: 10px 20px;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n            margin-top: 1rem;\r\n            margin-bottom: 1rem;\r\n        }}\r\n        .footer {{\r\n            background-color: #f2f2f2;\r\n            padding: 10px;\r\n            text-align: center;\r\n            font-size: 14px;\r\n            color: #202020;\r\n            height:3rem;\r\n            background: #fbe0e0;\r\n        }}\r\n    </style>\r\n</head>\r\n<body style=\"background: #023b46;\">\r\n    <table class=\"container\" cellpadding=\"0\" cellspacing=\"0\" >\r\n        <tr>\r\n            <td>\r\n                <div class=\"logo\">\r\n                    <img style=\"width: 200px;\" src=\"https://r1dq4k84-7246.inc1.devtunnels.ms/Storage/Images/logo-dark-mail.png\" alt=\"Limpid Systems\">\r\n                </div>\r\n                <div class=\"content\">\r\n                    <h1>Activate your account!</h1>\r\n                    <p class=\"content-p\" style=\"margin-top: 2rem;\" style=\"font-weight:700;\">Hello, {user.Name}</p>\r\n                    <p class=\"content-p\" >Welcome to Limpid CRM! We're so happy you're here.</p>\r\n                    <p class=\"content-p\" style=\"margin-top: 1rem;\">Activate your account by clicking the link below:</p>\r\n                   " +
//                    $" <a href=\"" + $"{_configuration.GetValue<string>("AllowedURL")}/account/a?id={user.Id}&token={userActivationToken.ActivationToken}" + "\" class=\"button\" style=\"color: #ffffff;\">Activate Your Account</a>\r\n                    <p class=\"content-p\" style=\"margin-top: 1rem;\">Questions about CRM?</p>\r\n                    <p class=\"content-p\">We'll be in touch with you, but should you need to reach our team sooner, feel free to contact us at:\r\n                        <a style=\"color:#0068ff;\" href=\"mailto:support@limpidsystems.com\">support@limpidsystems.com</a>\r\n                    </p>\r\n                    <p class=\"content-p\">We're happy to help!</p>\r\n                    <p class=\"content-p\" style=\"margin-top: 2rem;\">\r\n                        Cheers,\r\n                        <br>\r\n                        <p style=\"font-weight:700;\">The Limpid CRM Team</p>\r\n                    </p>\r\n                </div>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td class=\"footer\">© 2024, LimpidSystems All Rights Reserved.</td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";
//                _mailService.sendMail(user.Email, "Welcome to Limpid CRM", mailHtmlBody);
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [Authorize(Roles = SD.Role_MasterUser + "," + SD.Role_Organization)]
//        [HttpPut("organization/{id}", Name = "organization/update")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> UpdateOrganization(string id, [FromBody] User user)
//        {
//            try
//            {
//                if (id == null || user.Id != id)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    return BadRequest(_response);
//                }

//                bool isUniqueEmail = await _userRepo.IsUniqueUser(user.Email);
//                if (!isUniqueEmail)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Email Already Registered");
//                    return BadRequest(_response);
//                }


//                User userFormDB = await _userRepo.GetAsync(u => u.Id == user.Id, Trecked: false);
//                if (userFormDB == null)
//                {
//                    _response.StatusCode = HttpStatusCode.NotFound;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Exists");
//                    return NotFound(_response);
//                }

//                user.Password = userFormDB.Password;
//                await _userRepo.Update(user);

//                _response.StatusCode = HttpStatusCode.OK;

//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }

//            return _response;
//        }

//        [Authorize(Roles = SD.Role_MasterUser)]
//        [HttpDelete("organization/{id}", Name = "organization/remove")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> RomoveOrganization(string id)
//        {
//            try
//            {
//                User userFormDB = await _userRepo.GetAsync(u => u.Id == id, Trecked: false);
//                if (userFormDB == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Exists");
//                    return BadRequest(_response);
//                }
//                UserActivationToken userActivationToken = await _userActivationTokenRepo.GetAsync(u=>u.UserId == id);
//                if(userActivationToken != null)
//                {
//                       await _userActivationTokenRepo.RemoveAsync(userActivationToken);
//                }
//                await _userRepo.RemoveAsync(userFormDB);

//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [Authorize(Roles = SD.Role_MasterUser)]
//        [HttpDelete("organization/range")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> RemoveRange(List<string> idList)
//        {
//            try
//            {
//                if (idList is null || idList.Count == 0)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("ID List Not Provided");
//                    return BadRequest(_response);
//                }
//                List<User> userTobeDelete = new List<User>();
//                List<UserActivationToken> userActivationTokensTobeDelete = new List<UserActivationToken>();
//                foreach (string id in idList)
//                {
//                    var user = await _userRepo.GetAsync(u => u.Id == id);
//                    if (user != null)
//                    {
//                        userTobeDelete.Add(user);
//                    }
//                    var userActivationToken = await _userActivationTokenRepo.GetAsync(u => u.UserId == id);
//                    if (user != null)
//                    {
//                        userActivationTokensTobeDelete.Add(userActivationToken);
//                    }
//                }

//                await _userActivationTokenRepo.RemoveRangeAsync(userActivationTokensTobeDelete);
//                await _userRepo.RemoveRangeAsync(userTobeDelete);
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [Authorize(Roles = SD.Role_Organization)]
//        [HttpPost("employee")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<ActionResult<APIResponse>> CreateEmployee(RegisterationRequestDTO registerationRequestDTO)
//        {
//            try
//            {
//                bool isUniqueEmail = await _userRepo.IsUniqueUser(registerationRequestDTO.Email);
//                if (!isUniqueEmail)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Email Already Registered");
//                    return BadRequest(_response);
//                }

//                if (registerationRequestDTO.RoleName == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Role Not Provided");
//                    return BadRequest(_response);
//                }

//                if (registerationRequestDTO.OrganizationId == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Organization Id Not Provided");
//                    return BadRequest(_response);
//                }
//                if (registerationRequestDTO.BranchId == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Branch Id Not Provided");
//                    return BadRequest(_response);
//                }
//                registerationRequestDTO.BranchId = null;
//                registerationRequestDTO.OrganizationId = null;
//                var roleId = _roles.FirstOrDefault(u => u.RoleName == registerationRequestDTO.RoleName).Id;
//                if(roleId is null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Role Does Not Exists");
//                    return BadRequest(_response);
//                }
//                registerationRequestDTO.RoleId = roleId;
                
//                User user = _mapper.Map<User>(registerationRequestDTO);

//                await _userRepo.CreateAsync(user);
//                if (user == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Some Error While Requesting");
//                    return BadRequest(_response);
//                }
//                UserActivationToken userActivationToken = new UserActivationToken();
//                userActivationToken = await _userActivationTokenRepo.GetAsync(u => u.UserId == user.Id);
//                if (userActivationToken != null)
//                {
//                    userActivationToken.ActivationToken = _userActivationTokenRepo.CreateActivationToken();
//                    userActivationToken.ActivationTokenExpiryTime = DateTime.UtcNow.AddDays(7);
//                    await _userActivationTokenRepo.SaveAsync();
//                }
//                else
//                {
//                    userActivationToken = new()
//                    {
//                        UserId = user.Id,
//                        ActivationToken = _userActivationTokenRepo.CreateActivationToken(),
//                        ActivationTokenExpiryTime = DateTime.UtcNow.AddDays(7)
//                    };

//                    await _userActivationTokenRepo.CreateAsync(userActivationToken);
//                }



//                string mailHtmlBody = $"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Activate Your Account</title>\r\n    <style>\r\n        body, table, td, p, a, h1 {{\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n            margin: 0;\r\n            padding: 0;\r\n            border: 0;\r\n            font-family: Arial, sans-serif;\r\n            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\r\n        }}\r\n        body{{\r\n            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\r\n            background-color: gray;\r\n        }}\r\n        td, p{{\r\n            text-align: start;\r\n        }}\r\n        .container {{\r\n            width: auto;\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            border: 1px solid #cccccc;\r\n            gap: 1rem;\r\n            border: none;\r\n            border-top: 5px solid red;\r\n        }}\r\n        .logo {{\r\n            text-align: center;\r\n            padding: 20px;\r\n        }}\r\n        .content {{\r\n            padding-left: 3rem;\r\n            padding-right: 3rem;\r\n            margin-bottom: 2rem;\r\n            text-align: center;\r\n            letter-spacing: 0.5px;\r\n            font-weight: 400;\r\n            color: black;\r\n            line-height: 25px;\r\n            font-size: 0.95rem;\r\n        }}\r\n        .content-p{{\r\n            margin-top: 0.2rem;\r\n        }}\r\n        .button {{\r\n            display: inline-block;\r\n            background-color: #4CAF50;\r\n            color: white;\r\n            padding: 10px 20px;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n            margin-top: 1rem;\r\n            margin-bottom: 1rem;\r\n        }}\r\n        .footer {{\r\n            background-color: #f2f2f2;\r\n            padding: 10px;\r\n            text-align: center;\r\n            font-size: 14px;\r\n            color: #202020;\r\n            height:3rem;\r\n            background: #fbe0e0;\r\n        }}\r\n    </style>\r\n</head>\r\n<body style=\"background: #023b46;\">\r\n    <table class=\"container\" cellpadding=\"0\" cellspacing=\"0\" >\r\n        <tr>\r\n            <td>\r\n                <div class=\"logo\">\r\n                    <img style=\"width: 200px;\" src=\"https://r1dq4k84-7246.inc1.devtunnels.ms/Storage/Images/logo-dark-mail.png\" alt=\"Limpid Systems\">\r\n                </div>\r\n                <div class=\"content\">\r\n                    <h1>Activate your account!</h1>\r\n                    <p class=\"content-p\" style=\"margin-top: 2rem;\" style=\"font-weight:700;\">Hello,</p>\r\n                    <p class=\"content-p\" >Welcome to Limpid CRM! We're so happy you're here.</p>\r\n                    <p class=\"content-p\" style=\"margin-top: 1rem;\">Activate your account by clicking the link below:</p>\r\n                   " +
//                    $" <a href=\"" + $"{_configuration.GetValue<string>("AllowedURL")}/account/a?id={user.Id}&token={userActivationToken.ActivationToken}" + "\" class=\"button\" style=\"color: #ffffff;\">Activate Your Account</a>\r\n                    <p class=\"content-p\" style=\"margin-top: 1rem;\">Questions about CRM?</p>\r\n                    <p class=\"content-p\">We'll be in touch with you, but should you need to reach our team sooner, feel free to contact us at:\r\n                        <a style=\"color:#0068ff;\" href=\"mailto:support@limpidsystems.com\">support@limpidsystems.com</a>\r\n                    </p>\r\n                    <p class=\"content-p\">We're happy to help!</p>\r\n                    <p class=\"content-p\" style=\"margin-top: 2rem;\">\r\n                        Cheers,\r\n                        <br>\r\n                        <p style=\"font-weight:700;\">The Limpid CRM Team</p>\r\n                    </p>\r\n                </div>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td class=\"footer\">© 2024, LimpidSystems All Rights Reserved.</td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";
//                _mailService.sendMail(user.Email, "Welcome to Limpid CRM", mailHtmlBody);
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator + "," + SD.Role_Assiner + "," + SD.Role_SalesPerson)]
//        [HttpPut("employee/{id}", Name = "employee/update")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<ActionResult<APIResponse>> UpdateEmployee(string id, [FromBody] User user)
//        {
//            try
//            {
//                if (id == null || user.Id == id)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    return BadRequest(_response);
//                }

//                User userFormDB = await _userRepo.GetAsync(u => u.Id == user.Id, Trecked: false);
//                if (userFormDB == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Exists");
//                    return NotFound(_response);
//                }

//                user.Password = userFormDB.Password;
//                await _userRepo.Update(user);
//                await _userRepo.SaveAsync();

//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }


//        [Authorize(Roles = SD.Role_Organization)]
//        [HttpDelete("employee/{id}", Name = "employee/remove")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> RemoveEmployee(string id)
//        {
//            try
//            {
//                if (id == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    return BadRequest(_response);
//                }
//                User userFormDB = await _userRepo.GetAsync(u => u.Id == id, Trecked: false);
//                if (userFormDB == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Exists");
//                    return BadRequest(_response);
//                }
//                await _userRepo.RemoveAsync(userFormDB);
//                await _userRepo.SaveAsync();

//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [HttpPost("password/change")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> ChangePassword(ChangePasswordDTO changePasswordDTO)
//        {
//            try
//            {
//                if (changePasswordDTO.Email == null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Email Not Provided");
//                    return BadRequest(_response);
//                }
//                User user = await _userRepo.GetAsync(u => u.Email == changePasswordDTO.Email);
//                if (user == null)
//                {
//                    _response.StatusCode = HttpStatusCode.NotFound;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Email Not Registered");
//                    return NotFound(_response);
//                }
//                user.Password = _userRepo.HashPassword(changePasswordDTO.NewPassword);
//                user.RefreshTokenExpiryTime = DateTime.UtcNow;
//                await _userRepo.SaveAsync();
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [HttpPatch("{id}")]
//        [Authorize]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> UpdatePartialUser(string id, JsonPatchDocument<UserUpdateDTO> userUpadteDTOPatch)
//        {
//            try
//            {
//                if (userUpadteDTOPatch == null || id is null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Id or UserUpdatePatch not Provided");
//                    return BadRequest(_response);
//                }
//                User userFormDB = await _userRepo.GetAsync(u => u.Id == id, Trecked: false);
//                if (userFormDB == null)
//                {
//                    _response.StatusCode = HttpStatusCode.NotFound;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Existes");
//                    return NotFound(_response);
//                }
//                var updateDTO = _mapper.Map<UserUpdateDTO>(userFormDB);
//                userUpadteDTOPatch.ApplyTo(updateDTO);

//                var user = _mapper.Map<User>(updateDTO);
//                user.IsAccountActivated = userFormDB.IsAccountActivated;
//                user.Password = userFormDB.Password;
//                await _userRepo.Update(user);
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }


//        [Authorize]
//        [HttpPost("image/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> UpdateImage(string id, [FromForm] IFormFile imageFile)
//        {
//            try
//            {
//                if (id is null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Id not Provided");
//                    return BadRequest(_response);
//                }
//                if (imageFile is null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Image File not Provided");
//                    return BadRequest(_response);
//                }
//                User userFormDB = await _userRepo.GetAsync(u => u.Id == id);
//                if (userFormDB == null)
//                {
//                    _response.StatusCode = HttpStatusCode.NotFound;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Existes");
//                    return NotFound(_response);
//                }

//                string storageFolder = _webHostEnvironment.ContentRootPath;
//                if (!(string.IsNullOrEmpty(userFormDB.Image)))
//                {
//                    var oldImagePath = Path.Combine(storageFolder, userFormDB.Image.TrimStart('\\'));
//                    if (System.IO.File.Exists(oldImagePath))
//                    {
//                        System.IO.File.Delete(oldImagePath);
//                    }
//                }
//                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
//                string imagePath = @"Storage\Images\ProfileImages";
//                string finalPath = Path.Combine(storageFolder, imagePath);
//                if (!Directory.Exists(finalPath))
//                {
//                    Directory.CreateDirectory(finalPath);
//                }

//                using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
//                {
//                    imageFile.CopyTo(fileStream);
//                }
//                userFormDB.Image = @"\" + imagePath + @"\" + fileName;
//                await _userRepo.SaveAsync();
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [Authorize]
//        [HttpDelete("image/{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> DeleteImage(string id)
//        {
//            try
//            {
//                if (id is null)
//                {
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("Id not Provided");
//                    return BadRequest(_response);
//                }
//                User userFormDB = await _userRepo.GetAsync(u => u.Id == id);
//                if (userFormDB == null)
//                {
//                    _response.StatusCode = HttpStatusCode.NotFound;
//                    _response.IsSuccess = false;
//                    _response.ErrorMessages.Add("User Not Existes");
//                    return NotFound(_response);
//                }

//                string storageFolder = _webHostEnvironment.ContentRootPath;
//                if (!(string.IsNullOrEmpty(userFormDB.Image)))
//                {
//                    var oldImagePath = Path.Combine(storageFolder, userFormDB.Image.TrimStart('\\'));
//                    if (System.IO.File.Exists(oldImagePath))
//                    {
//                        System.IO.File.Delete(oldImagePath);
//                    }
//                }
//                userFormDB.Image = null;
//                await _userRepo.SaveAsync();
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }
//    }
//}
