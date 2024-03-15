using AutoMapper;
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
    [Route("api/lead")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadRepository _leadRepo;
        private APIResponse _response;
        private readonly IMapper _mapper;
        public LeadController(ILeadRepository leadRepo, IMapper mapper) { 
            _leadRepo = leadRepo;
            _response = new APIResponse();
            _mapper = mapper;
        }


        [Authorize(Roles = "Organization, Data Entry Operator, Assiner, Sells Person")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Lead> leads = await _leadRepo.GetAllAsync();
                _response.Data = leads;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return Ok(_response);
        }

        [Authorize(Roles = "Organization, Data Entry Operator")]
        [HttpPost(Name = "create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] LeadCreateDTO leadCreateDTO)
        {
            try
            {
                Lead lead = _mapper.Map<Lead>(leadCreateDTO);
                await _leadRepo.CreateAsync(lead);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return Ok(_response);
        }

        [Authorize(Roles = "Organization, Data Entry Operator, Assigner, SellsPerson")]
        [HttpPut("{id}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody]LeadUpdateDTO leadUpdateDTO)
        {
                try
                {
                    if (id is null || leadUpdateDTO.Id != id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }
                    var leadFromDB = await _leadRepo.GetAsync(u => u.Id == id, Trecked: false);
                    if (leadFromDB == null)
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                        return NotFound(_response);
                    }
                    await _leadRepo.UpdateAsync(leadUpdateDTO);
                    _response.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    _response.ErrorMessages.Add(e.Message);
                    _response.IsSuccess = false;
                }
            return Ok(_response);
        }

        [Authorize(Roles = "Organization, Data Entry Operator")]
        [HttpDelete("{id}", Name = "Remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove(string id)
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
                var lead = await _leadRepo.GetAsync(u=>u.Id == id, Trecked: false);
                if (lead == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Lead Not Exists");
                    return NotFound(_response);
                }
                await _leadRepo.RemoveAsync(lead);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return Ok(_response);
        }
    }
}
