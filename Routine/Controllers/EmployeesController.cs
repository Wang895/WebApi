using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Entity;
using Routine.Models;
using Routine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Routine.Controllers
{
    [ApiController]
    [Authorize]
    [Route("companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeesController(IMapper mapper, ICompanyRepository companyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(Guid companyId,[FromQuery(Name ="gender")] string genderDisplay,[FromQuery]string q)
        {
            var employees = await _companyRepository.GetEmployeesAsync(companyId,genderDisplay,q);
            if (employees == null)
            {
                return NotFound();
            }
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }
        [Route("{employeeId}",Name =nameof(GetEmployee))]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid companyId, Guid employeeId){
            var employee = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            Console.WriteLine("ok");
            if (employee == null)
            {
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
            }

        [HttpPost]
        public async Task<ActionResult<EmployeeAddDto>> AddEmployee(Guid companyId, EmployeeAddDto employee)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var entity = _mapper.Map<Employee>(employee);
            _companyRepository.AddEmployee(companyId, entity);
            await _companyRepository.SaveAsync();
            var employeeDto = _mapper.Map<EmployeeDto>(entity);
            return CreatedAtRoute(nameof(GetEmployee), new
            {
                companyId = companyId,
                employeeId = entity.Id
            }, employeeDto);
        }
        [Route("{employeeId}")]
        [HttpPatch]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId,
            Guid employeeId,
            JsonPatchDocument<EmployeeAddDto> patchDocument)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var entity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (entity == null)
            {
                return NotFound();
            }

            var entityAdd = _mapper.Map<EmployeeAddDto>(entity);
            patchDocument.ApplyTo(entityAdd,ModelState);  //需要处理验证错误
            if (!TryValidateModel(entityAdd))
            {
                //return ValidationProblem(ModelState); //返回400状态码
                return UnprocessableEntity(ModelState); //返回422状态码
            }
            _mapper.Map(entityAdd, entity);
            _companyRepository.UpdateEmployee(entity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }
    }
}

