using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Entity;
using Routine.Models;
using Routine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.Controllers;
    using Routine.Helper;

namespace Routine.Controllers
{
    [ApiController]
    [Route("companycollections")]
    public class CompanyCollectionController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyCollectionController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("{ids}", Name = nameof(GetCompanyCollection))]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanyCollection([FromQuery][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var companyCollection = await _companyRepository.GetCompaniesAsync(ids);
            if (companyCollection.Count() != ids.Count())
            {
                return NotFound();
            }
            var companyCollectionDto = _mapper.Map<CompanyDto>(companyCollection);
            return Ok(companyCollectionDto);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanyCollection(IEnumerable<CompanyAddDto> companyCollection)
        {
            var entity = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var item in entity)
            {
                _companyRepository.AddCompany(item);
            }
            await _companyRepository.SaveAsync();
            var companyCollectionDto = _mapper.Map<IEnumerable<CompanyDto>>(entity);
            return CreatedAtRoute(nameof(GetCompanyCollection), new { ids = string.Join(",", companyCollectionDto.Select(x => x.Id)) }, companyCollectionDto);
        }
    }
}
