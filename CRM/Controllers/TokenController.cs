//using CRM.Models.DTOs;
//using CRM.Models;
//using CRM.Repository.IRepository;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;

//namespace CRM.Controllers
//{
//    [Route("api/token")]
//    [ApiController]
//    public class TokenController : ControllerBase
//    {

//        private readonly IUserRepository _userRepo;
//        private APIResponse _response;
//        public TokenController(IUserRepository userRepo) { 
//            _userRepo = userRepo;
//            _response = new APIResponse();
//        }

//        [HttpPost("refresh")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<APIResponse>> Refresh(TokenDTO tokenDTO)
//        {
//            try
//            {
//                var tokenDTOToReturn = await _userRepo.RefreshAccessToken(tokenDTO);
//                _response.Data = tokenDTOToReturn;
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
