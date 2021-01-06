using Microsoft.EntityFrameworkCore;
using Routine.Date;
using Routine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.Helper;
using Routine.Models;

namespace Routine.Services
{
    public class CompanyRepository:ICompanyRepository
    {
        private readonly RoutineDbContext _routineContext;

        public CompanyRepository(RoutineDbContext routineDbContext)
        {
            this._routineContext = routineDbContext?? throw new ArgumentNullException(nameof(routineDbContext));
        }

        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            company.Id = Guid.NewGuid();
            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }
            _routineContext.Companies.Add(company);
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            employee.Id = Guid.NewGuid();
            employee.CompanyId = companyId;
            _routineContext.Employees.Add(employee);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _routineContext.Companies.AnyAsync(x => x.Id == companyId);
        }

        public void DeleteComppany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _routineContext.Companies.Remove(company);
        }

        public void DeleteEmployee(Employee employee)
        {
            _routineContext.Employees.Remove(employee);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }
            return await _routineContext.Companies.Where(x => companyIds.Contains(x.Id)).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {
            var QueryExpression = _routineContext.Companies as IQueryable<Company>;
            if (!string.IsNullOrWhiteSpace((parameters.CompanyName)))
            {
                string CompanyName = parameters.CompanyName.Trim();
                QueryExpression = QueryExpression.Where(x => x.Name.Contains(CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                string searchTerm = parameters.SearchTerm.Trim();
                QueryExpression = QueryExpression.Where(x => x.Name.Contains(searchTerm)
                                                             || x.Introduction.Contains(searchTerm));
            }
            return await PagedList<Company>.CreateAsync(QueryExpression, QueryExpression.Count(), parameters.PageNumber,parameters.PageSize);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _routineContext.Companies.ToListAsync();
        }
        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _routineContext.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if(employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }
            return await _routineContext.Employees.Where(x => x.Id == employeeId && x.CompanyId == companyId).FirstOrDefaultAsync();
           
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,String genderDisplay,string q)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (string.IsNullOrEmpty(genderDisplay)&&string.IsNullOrEmpty(q))
            {
                Console.WriteLine("kong");
                return await _routineContext.Employees.Where(x => x.CompanyId == companyId).OrderBy(x => x.EmployeeNo).ToListAsync();
            }
            var item = _routineContext.Employees.Where(x=>x.CompanyId==companyId) as IQueryable<Employee>;
            if (!string.IsNullOrEmpty(genderDisplay))
            {
                genderDisplay = genderDisplay.Trim();
                Gender gender = Enum.Parse<Gender>(genderDisplay);
                item = item.Where(x => x.Gender == gender);
            }
            if (!string.IsNullOrEmpty(q))
            {
                q = q.Trim();
                item = item.Where(x => x.FirstName.Contains(q)
                || x.LastName.Contains(q)
                || x.EmployeeNo.Contains(q)).OrderBy(x => x.EmployeeNo);
            }
            return await item.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _routineContext.SaveChangesAsync() >= 0;
        }

        public void UpdateCompany(Company company)
        {
        }

        public void UpdateEmployee(Employee employee)
        {

        }
    }
}
