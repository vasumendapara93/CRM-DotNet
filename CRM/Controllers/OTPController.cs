//using Azure;
//using CRM.Models;
//using CRM.Models.Tables;
//using CRM.Repository.IRepository;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;

//namespace CRM.Controllers
//{
//    [Route("api/otp")]
//    [ApiController]
//    public class OTPController : ControllerBase
//    {
//        private APIResponse _response;
//        private readonly IUserRepository _userRepo;
//        private readonly IOTPRepository _otpRepo;
//        private readonly IMailServiceRepository _mailService;

//        public OTPController( IUserRepository userRepository, IOTPRepository otpRepository, IMailServiceRepository mailService)
//        {
//            _otpRepo = otpRepository;
//            _userRepo = userRepository;
//            _response = new APIResponse();
//            _mailService = mailService;
//        }

//        [HttpPost("send")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<ActionResult<APIResponse>> SendOTP(string email)
//        {
//            try
//            {
//                User user =  await _userRepo.GetAsync(u => u.Email == email);
//                if (user == null)
//                {
//                    _response.ErrorMessages.Add("Email Is Not Registered");
//                    _response.IsSuccess = false;
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    return BadRequest(_response);
//                }
//                string OTP = _otpRepo.GenerateOTP(user.Id);
//                _mailService.sendMail(email, "Forgot Password OTP", $"Your reset password OTP is {OTP}");
//                _response.StatusCode = HttpStatusCode.OK;
//            }
//            catch (Exception e)
//            {
//                _response.ErrorMessages.Add(e.Message);
//                _response.IsSuccess = false;
//            }
//            return _response;
//        }

//        [HttpPost("verify")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<ActionResult<APIResponse>> VerifyOTPAsync([FromBody] OTPVerification otpVerification)
//        { 
//             try
//            {
//                if (otpVerification.OTP is null)
//                {
//                    _response.ErrorMessages.Add("OTP Not Provided");
//                    _response.IsSuccess = false;
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    return BadRequest(_response);
//                }
//                User user = await _userRepo.GetAsync(u => u.Email == otpVerification.Email);
//                if (user is null)
//                {
//                    _response.ErrorMessages.Add("Email Is Not Registered");
//                    _response.IsSuccess = false;
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    return BadRequest(_response);
//                }
//                if (!_otpRepo.VerifyOTP(user.Id, otpVerification.OTP))
//                {
//                    _response.ErrorMessages.Add("Invaid OTP");
//                    _response.IsSuccess = false;
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    return BadRequest(_response);
//                }
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
