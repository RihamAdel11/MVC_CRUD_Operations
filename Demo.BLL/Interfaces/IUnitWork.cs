using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitWork
    {
        public IEmployeeRepositry employeeRepositry { get; set; }
        public IDepartmentRepositry departmentRepositry { get; set; }
         Task<int> Complete();


    }
}
