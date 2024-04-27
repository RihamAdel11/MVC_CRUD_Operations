using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositry
{
    public class EmployeeRepositry : GenericRepositry<Employee>, IEmployeeRepositry
    {
        private readonly MVCAppContext _dbContext;

        public EmployeeRepositry(MVCAppContext dbContext) :base(dbContext)
        {
           _dbContext = dbContext;
        }
        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
            return _dbContext .Employees .Where (E=>E.Address==address);
        }

        public IQueryable<Employee> GetEmployeeByName(string name)
        {
            return _dbContext.Employees .Where(E=>E.Name.ToLower ().Contains (name.ToLower ()));
        }
    }
}
