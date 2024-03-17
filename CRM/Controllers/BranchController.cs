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
        [HttpGet("{organizationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetAll(string organizationId)
        {
            try
            {
                if (organizationId == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                IEnumerable<Branch> branches = await _branchRepo.GetAllAsync(u => u.OrganizationId == organizationId);
                _response.Data = branches;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = "Organization")]
        [HttpPost()]
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

        [Authorize(Roles = "Organization")]
        [HttpPut("{id}", Name = "BranchUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Update(string id, [FromBody] BranchUpdateDTO branchUpdateDTO)
        {
            try
            {
                if (id is null || branchUpdateDTO.Id != id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var branchFromDB = await _branchRepo.GetAsync(u => u.Id == id, Trecked: false);
                if (branchFromDB == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                await _branchRepo.UpdateAsync(branchUpdateDTO);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = "Organization")]
        [HttpDelete("{id}", Name = "BranchRemove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Remove(string id)
        {
            try
            {
                if (id is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("ID Not Provided");
                    return BadRequest(_response);
                }
                var branch = await _branchRepo.GetAsync(u => u.Id == id, Trecked: false);
                if (branch == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Lead Not Exists");
                    return NotFound(_response);
                }
                await _branchRepo.RemoveAsync(branch);
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
