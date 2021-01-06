using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Models
{
    public class CompanyDtoParameters
    {
        public const int MaxPageSize = 10;
        public string CompanyName { set; get; }
        public string SearchTerm { set; get; }
        public int PageNumber { set; get; } = 1;
        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
