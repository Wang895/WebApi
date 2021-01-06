﻿using Routine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.Helper;
using Routine.Models;

namespace Routine.Services
{
    public interface ICompanyRepository
    {
        Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters);
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task<Company> GetCompanyAsync(Guid companyId);
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds);
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteComppany(Company company);
        Task<bool> CompanyExistsAsync(Guid companyId);
        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,string genderDisplay,string q);
        Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId);
        void AddEmployee(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        Task<bool> SaveAsync();

    }
}
