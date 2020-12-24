using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Entity
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { set; get; }
        public string Introduction { set; get; }

        public ICollection<Employee> Employees { set; get; }
    }
}
