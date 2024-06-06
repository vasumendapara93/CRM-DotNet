using AutoMapper;
using Azure;
using CRM.Model;
using CRM.Models;
using CRM.Models.DTOs;
using CRM.Repository.IRepository;
using CRM.StaticData.ModelFileds;
using CRM.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
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
        private readonly IUserRepository _userRepo;
        private readonly IBranchRepository _branchRepo;
        private readonly ILeadNoteRepository _leadNoteRepo;
        public LeadController(ILeadRepository leadRepo, IMapper mapper, IUserRepository userRepo, IBranchRepository branchRepo, ILeadNoteRepository leadNoteRepo)
        {
            _leadRepo = leadRepo;
            _response = new APIResponse();
            _mapper = mapper;
            _userRepo = userRepo;
            _branchRepo = branchRepo;
            _leadNoteRepo = leadNoteRepo;
        }

        [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator + "," + SD.Role_Assiner + "," + SD.Role_SalesPerson)]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] string id )
        {
            try
            {
                if (id is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Id Not Provided");
                    return BadRequest(_response);
                }
                var leadFromDB = await _leadRepo.GetAsync(u => u.Id == id, Trecked: false);
                if (leadFromDB == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Lead Not Found");
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = leadFromDB;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }


     [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator + "," + SD.Role_Assiner + "," + SD.Role_SalesPerson)]
        [HttpGet("all/{organizationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetAll(string organizationId, [FromQuery] string? search, [FromQuery] string? orderBy, [FromQuery] string? order = Order.ASC, [FromQuery] int PageSize = 0, [FromQuery] int PageNo = 1)
        {
            try
            {
                if (organizationId == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Id Not Provided");
                    return NotFound(_response);
                }

                var org = await _userRepo.GetAsync(u => u.Id == organizationId);
                if (org == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Organization Not Exists");
                }

                IEnumerable<Lead> leads = new List<Lead>();
                var totalRecords = 0;
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower().Trim();
                    if (string.IsNullOrEmpty(orderBy) || orderBy == "null")
                    {
                        leads = await _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId && (u.FirstName.ToLower().Contains(search) || u.LastName.ToLower().Contains(search) || (u.LeadSource != null && u.LeadSource.ToLower().Contains(search)) || (u.Title != null && u.Title.ToLower().Contains(search)) || (u.Email != null && u.Email.ToLower().Contains(search)) || (u.Phone != null && u.Phone.ToLower().Contains(search)) || (u.Street != null && u.Street.ToLower().Contains(search)) || (u.City != null && u.City.ToLower().Contains(search)) || (u.ZipCode != null && u.ZipCode.ToLower().Contains(search)) || (u.Country != null && u.Country.ToLower().Contains(search))), PageSize: PageSize, PageNo: PageNo);
                    }
                    else if (string.IsNullOrEmpty(order) || order == "null" || order == Order.ASC)
                    {
                        /*order ASC*/
                        leads = await _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId && (u.FirstName.ToLower().Contains(search) || u.LastName.ToLower().Contains(search) || (u.LeadSource != null && u.LeadSource.ToLower().Contains(search)) || (u.Title != null && u.Title.ToLower().Contains(search)) || (u.Email != null && u.Email.ToLower().Contains(search)) || (u.Phone != null && u.Phone.ToLower().Contains(search)) || (u.Street != null && u.Street.ToLower().Contains(search)) || (u.City != null && u.City.ToLower().Contains(search)) || (u.ZipCode != null && u.ZipCode.ToLower().Contains(search)) || (u.Country != null && u.Country.ToLower().Contains(search))), Order: Order.ASC, PageSize: PageSize, PageNo: PageNo);
                    }
                    else
                    {
                        /*order DSE*/
                        leads = await _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId && (u.FirstName.ToLower().Contains(search) || u.LastName.ToLower().Contains(search) || (u.LeadSource != null && u.LeadSource.ToLower().Contains(search)) || (u.Title != null && u.Title.ToLower().Contains(search)) || (u.Email != null && u.Email.ToLower().Contains(search)) || (u.Phone != null && u.Phone.ToLower().Contains(search)) || (u.Street != null && u.Street.ToLower().Contains(search)) || (u.City != null && u.City.ToLower().Contains(search)) || (u.ZipCode != null && u.ZipCode.ToLower().Contains(search)) || (u.Country != null && u.Country.ToLower().Contains(search))), OrderBy: _leadRepo.CreateSelectorExpression(orderBy), Order: Order.DESC, PageSize: PageSize, PageNo: PageNo);
                    }
                    totalRecords = _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId && (u.FirstName.ToLower().Contains(search) || u.LastName.ToLower().Contains(search) || (u.LeadSource != null && u.LeadSource.ToLower().Contains(search)) || (u.Title != null && u.Title.ToLower().Contains(search)) || (u.Email != null && u.Email.ToLower().Contains(search)) || (u.Phone != null && u.Phone.ToLower().Contains(search)) || (u.Street != null && u.Street.ToLower().Contains(search)) || (u.City != null && u.City.ToLower().Contains(search)) || (u.ZipCode != null && u.ZipCode.ToLower().Contains(search)) || (u.Country != null && u.Country.ToLower().Contains(search)))).GetAwaiter().GetResult().Count();
                }
                else
                {
                    Console.WriteLine(orderBy);
                    if (string.IsNullOrEmpty(orderBy) || orderBy == "null")
                    {
                        leads = await _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId, PageSize: PageSize, PageNo: PageNo);
                    }
                    else if (string.IsNullOrEmpty(order) || order == "null" || order == Order.ASC)
                    {
                        /*order ASC*/
                        if (orderBy != LeadFields.CreateDate)
                        {
                            leads = await _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId, OrderBy: _leadRepo.CreateSelectorExpression(orderBy), Order: Order.ASC, PageSize: PageSize, PageNo: PageNo);
                        }
                        else
                        {
                            leads = await _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId, OrderBy: u => u.CreateDate.ToString(), Order: Order.ASC, PageSize: PageSize, PageNo: PageNo);
                        }
                    }
                    else
                    {
                        /*order DSE*/
                        if (orderBy != LeadFields.CreateDate)
                        {
                            leads = await _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId, OrderBy: _leadRepo.CreateSelectorExpression(orderBy), Order: Order.DESC, PageSize: PageSize, PageNo: PageNo);
                        }
                        else
                        {
                            leads = await _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId, OrderBy: u => u.CreateDate.ToString(), Order: Order.DESC, PageSize: PageSize, PageNo: PageNo);
                        }
                    }
                    totalRecords = _leadRepo.GetAllAsync(u => u.OrganizationId == organizationId).GetAwaiter().GetResult().Count();
                    var pagination = new Pagination { PageNo = PageNo, PageSize = PageSize };
                    Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(pagination));
                }
                IEnumerable<LeadResponseDTO> leadResponseDTOs = leads.Select(lead => _mapper.Map<LeadResponseDTO>(lead));

                _response.Data = new RecordsResponse
                {
                    TotalRecords = totalRecords,
                    Records = leadResponseDTOs
                };
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator)]
        [HttpPost(Name = "LeadCreate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] LeadCreateDTO leadCreateDTO)
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
            return _response;
        }

        [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator)]
        [HttpPost("range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateRange([FromBody] List<LeadCreateDTO> leadCreateDTOList)
        {
            try
            {
               
                if (leadCreateDTOList is null || leadCreateDTOList.Count == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Branch List Not Provided");
                    return BadRequest(_response);
                }

                List<LeadCreateDTO> leadCreateDTOToBeCreated = new List<LeadCreateDTO>();
                foreach (var leadDTO in leadCreateDTOList)
                {
                    if(!String.IsNullOrEmpty(leadDTO.BranchId))
                    {
                        Branch branch = await _branchRepo.GetAsync(u => u.BranchCode == leadDTO.BranchId);
                        if(branch != null) {
                            leadDTO.BranchId = branch.Id;
                            leadCreateDTOToBeCreated.Add(leadDTO);
                        }
                    }
                    else
                    {
                        leadCreateDTOToBeCreated.Add(leadDTO);
                    }
                }

                List<Lead> leadList = leadCreateDTOToBeCreated.Select(lead => _mapper.Map<LeadCreateDTO, Lead>(lead)).ToList();
                await _leadRepo.CreateRangeAsync(leadList);
                await _leadRepo.SaveAsync();

                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }


        [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator + "," + SD.Role_Assiner + "," + SD.Role_SalesPerson)]
        [HttpPut("{id}", Name = "LeadUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Update(string id, [FromBody]LeadUpdateDTO leadUpdateDTO)
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
            return _response;
        }

        [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator )]
        [HttpDelete("{id}", Name = "LeadRemove")]
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
            return _response;
        }

        [Authorize(Roles = SD.Role_Organization + ", " + SD.Role_DataEntryOperator)]
        [HttpDelete("range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> RemoveRange(List<string> idList)
        {
            try
            {
                if (idList is null || idList.Count == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("ID List Not Provided");
                    return BadRequest(_response);
                }
                List<Lead> LeadsToBeDeleted = new List<Lead>();
                List<LeadNote> LeadNotesToBeDeleted = new List<LeadNote>();
                foreach (string id in idList)
                {
                    var lead = await _leadRepo.GetAsync(u => u.Id == id);
                    if (lead != null)
                    {
                        LeadsToBeDeleted.Add(lead);
                        if( lead.Notes != null && lead.Notes.Count != 0)
                        {
                            LeadNotesToBeDeleted.Concat(lead.Notes);
                        }
                    }
                }

                await _leadNoteRepo.RemoveRangeAsync(LeadNotesToBeDeleted);
                await _leadRepo.RemoveRangeAsync(LeadsToBeDeleted);
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
