using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Entity;
using Routine.Helper;
using Routine.Models;
using Routine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Routine.Filters;

namespace Routine.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("companies")]
    public class CompaniesController:ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        public CompaniesController(ICompanyRepository companyRepository,IMapper mapper)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        //{
        //    var companies = await _companyRepository.GetCompaniesAsync();
        //    var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        //    return Ok(companyDtos);
        //    //return companuDtos;   使用ActionResult<T>作为返回类型时，可以直接返回数据，也可以返回OK();
        //}
        [HttpGet(Name = nameof(GetCompanies))]
        [AddHeader("wang", "tian")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters parameters)
        {
            var company = await _companyRepository.GetCompaniesAsync(parameters);
            var previousPageLink = company.HasPrevious
                ? CreateCompanyResourceUri(parameters, ResourceUriType.PrevoiusPage)
                : null;
            var nextPageLink = company.HasNext
                ? CreateCompanyResourceUri(parameters, ResourceUriType.NextPage)
                : null;
            var pageMetadata = new
            {
                totalCount = company.ItemCount,
                pageCount = company.PageCount,
                currentPage = company.CurrentPage,
                pageSize = company.PageSize,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("x-Pagenation",JsonSerializer.Serialize(pageMetadata,new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
            var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(company);
            return Ok(companyDto);
        }
        [HttpGet("{companyId}" , Name =nameof(GetCompany))]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CompanyDto>(company));
        }
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto company)
        {
            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();
            var companyDto = _mapper.Map<CompanyDto>(entity);
            return CreatedAtRoute(nameof(GetCompany), new { companyId = companyDto.Id }, companyDto);
        }
        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }

        public string CreateCompanyResourceUri(CompanyDtoParameters companyDtoParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PrevoiusPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = companyDtoParameters.PageNumber - 1,
                        pageSize = companyDtoParameters.PageSize,
                        companyName = companyDtoParameters.CompanyName,
                        searchTerm = companyDtoParameters.SearchTerm
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = companyDtoParameters.PageNumber + 1,
                        pageSize = companyDtoParameters.PageSize,
                        companyName = companyDtoParameters.CompanyName,
                        searchTerm = companyDtoParameters.SearchTerm
                    });
                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = companyDtoParameters.PageNumber,
                        pageSize = companyDtoParameters.PageSize,
                        companyName = companyDtoParameters.CompanyName,
                        searchTerm = companyDtoParameters.SearchTerm
                    });

            }
        }
        
        
    }
}
