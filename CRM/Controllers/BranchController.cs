using AutoMapper;
using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRM.Controllers
{
    [Route("api/branch")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchRepository _branchRepo;
        private readonly IMapper _mapper;
        private APIResponse _response;
  

        public BranchController( IBranchRepository branchRepo, IMapper mapper)
        {
            _branchRepo = branchRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [Authorize(Roles = "Organization")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> Create(BranchCreateDTO branchCreateDTO)
        {
            try
            {
                bool isUniqueBranch = await _branchRepo.IsUniqueBranch(branchCreateDTO);
                if (!isUniqueBranch)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Branch Already Exists");
                    return BadRequest(_response);
                }

                Branch branch = _mapper.Map<Branch>(branchCreateDTO);
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

    }
}
