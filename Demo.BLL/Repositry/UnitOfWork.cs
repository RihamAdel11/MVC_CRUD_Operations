using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositry
{
    public class UnitOfWork : IUnitWork,IDisposable 
    {
        private readonly MVCAppContext _dbContext;

        public IEmployeeRepositry employeeRepositry { get  ; set ; }
        public IDepartmentRepositry departmentRepositry { get ; set ; }
        public UnitOfWork(MVCAppContext dbContext)
        {
            employeeRepositry = new EmployeeRepositry(dbContext );
            departmentRepositry = new DepartmentRepositry(dbContext );
            _dbContext = dbContext;
        }

        public async Task<int> Complete()
        {return await _dbContext .SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext .Dispose ();
        }
    }
}
