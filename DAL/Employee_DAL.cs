
using System.Data;
using System.Data.SqlClient;
using System.IO;



using ADOPROJECT.Models;


namespace ADOPROJECT.DAL
{
    public class Employee_DAL
    {
        SqlConnection _connection = null;
        SqlCommand _command = null;

        public static IConfiguration Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }

        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();
            using (_connection = new SqlConnection(GetConnectionString()))
            {

                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Get_Employees]";
                _connection.Open();

                using (SqlDataReader dr = _command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Employee employee = new Employee();
                        employee.Id = Convert.ToInt32(dr["Id"]);
                        employee.FirstName = dr["FirstName"].ToString();
                        employee.LastName = dr["LastName"].ToString();
                        employee.Email = dr["Email"].ToString();
                        employee.Phone = dr["Phone"].ToString();

                        employeeList.Add(employee);
                    }
                    _connection.Close();
                }

            }

            return employeeList;
        }
        public bool Insert(Employee model)
        {
            int id = 0;

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;


                _command.CommandText = "[DBO].[usp_Insert_Employee]";

                _command.Parameters.AddWithValue("@FirstName", model.FirstName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@Email", model.Email);
                _command.Parameters.AddWithValue("@Phone", model.Phone);

                _connection.Open();


                id = _command.ExecuteNonQuery();


                _connection.Close();
            }


            return id > 0 ? true : false;
        }
        public Employee GetById(int id)
        {
            Employee employee = new Employee();
            using (_connection = new SqlConnection(GetConnectionString()))
            {

                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Get_EmployeeById]";
                _command.Parameters.AddWithValue("@Id", id);
                _connection.Open();

                using (SqlDataReader dr = _command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        employee.Id = Convert.ToInt32(dr["Id"]);
                        employee.FirstName = dr["FirstName"].ToString();
                        employee.LastName = dr["LastName"].ToString();
                        employee.Email = dr["Email"].ToString();
                        employee.Phone = dr["Phone"].ToString();


                    }
                    _connection.Close();
                }

            }

            return employee;
        }

        public bool Update(Employee model)
        {
            int id = 0;

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;


                _command.CommandText = "[DBO].[usp_Update_Employee]";


                _command.Parameters.AddWithValue("@Id", model.Id);
                _command.Parameters.AddWithValue("@FirstName", model.FirstName);
                _command.Parameters.AddWithValue("@LastName", model.LastName);
                _command.Parameters.AddWithValue("@Email", model.Email);
                _command.Parameters.AddWithValue("@Phone", model.Phone);

                _connection.Open();


                id = _command.ExecuteNonQuery();


                _connection.Close();
            }
            return id > 0 ? true : false;
        }
        public bool Delete(int id)
        {
            

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;

                _command.CommandText = "[DBO].[usp_Delete_Employee]";


                _command.Parameters.AddWithValue("@Id", id);
                

                _connection.Open();

    
                id = _command.ExecuteNonQuery();


                _connection.Close();
            }
            return id > 0 ? true : false;
        }
    }
}