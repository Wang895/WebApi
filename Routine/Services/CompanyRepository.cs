using Microsoft.EntityFrameworkCore;
using Routine.Date;
using Routine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            foreach(var employee in company.Employees)
            {
                employee.Id = Guid.NewGuid();
            }
            _routineContext.Companies.Add(company);
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
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

        public async Task<IEnumerable<Company>> GetCompanies(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }
            return await _routineContext.Companies.Where(x => companyIds.Contains(x.Id)).OrderBy(x => x.Name).ToListAsync();
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
            return await _routineContext.Employees.Where(x => x.Id == companyId && x.CompanyId == companyId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _routineContext.Employees.Where(x => x.CompanyId == companyId).OrderBy(x=>x.EmployeeNo).ToListAsync();
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
