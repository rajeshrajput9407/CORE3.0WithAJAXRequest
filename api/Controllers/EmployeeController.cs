using System.Collections.Generic;
using System.Data;
using api.DbHelpers;
using api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        Database db = new Database();

        // GET: api/Employee
        [HttpGet]
        public IEnumerable<Employee> get()
        {
            string query = @"select * 
                            from 
                            Employee_Tbl";
            DataTable dt = db.GetData(query);
            List<Employee> emps = new List<Employee>();
            foreach (DataRow row in dt.Rows)
            {
                emps.Add(new Employee
                {
                    Id = row[0].ToString(),
                    Name = row[1].ToString(),
                    Profile = row[2].ToString(),
                    Address = row[3].ToString(),
                });
            }
            return emps;
        }

        // GET: api/Employee/5
        [HttpGet("{id}", Name = "Get")]
        public Employee Get(int id)
        {
            string query = @"select * from
                            Employee_Tbl
                            where Id =" + id;
            DataTable dt = db.GetData(query);
            Employee emps = new Employee();
            foreach (DataRow row in dt.Rows)
            {
                emps.Id = row[0].ToString();
                emps.Name = row[1].ToString();
                emps.Profile = row[2].ToString();
                emps.Address = row[3].ToString();

            }
            return emps;
        }

        // POST: api/Employee
        [HttpPost]
        public ActionResult Post([FromBody] Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Id))
            {
                string query = @"insert into Employee_Tbl (Name,Profile,Address) 
                                 values (@Name,@Profile,@Address);";
                var parameters = new IDataParameter[]
                {
                    new SqlParameter("@Name",employee.Name),
                    new SqlParameter("@Profile", employee.Profile),
                    new SqlParameter("@Address",employee.Address)
               };
                if (db.ExecuteData(query, parameters) > 0)
                {
                    return Ok(new { Result = "Record Save Successfully!" });
                }
                else
                {
                    return NotFound(new { Result = "something went wrong" });
                }
            }
            else
            {
                string query = @"update Employee_Tbl
                                set Name = '" + employee.Name + "', Profile = '" 
                                 + employee.Profile + "', Address = '" + employee.Address + 
                                 "' where Id ='" + employee.Id + "'";
                if (db.ExecuteData(query) > 0)
                {

                    return Ok(new { Result = "Record Update Successfully!" });
                }
                else
                {
                    return NotFound(new { Result = "something went wrong" });

                }
            }
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            string query = @"delete from
                            Employee_Tbl 
                             where Id = '" + id + "'";

            if (db.ExecuteData(query) > 0)
            {
                return Ok(new { Result = "Record Delete Successfully!" });
            }
            else
            {
                return NotFound(new { Result = "something went wrong" });

            }
        }
    }
}
