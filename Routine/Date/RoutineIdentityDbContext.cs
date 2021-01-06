using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Routine
{
    public class RoutineIdentityDbContext:IdentityDbContext
    {
        public RoutineIdentityDbContext(DbContextOptions<RoutineIdentityDbContext> options) : base(options)
        {
        }
        
    }
}
