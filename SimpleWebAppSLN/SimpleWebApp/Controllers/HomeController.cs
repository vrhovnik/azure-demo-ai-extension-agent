using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using SimpleWebApp.Models;

namespace SimpleWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => View();

        public async Task<ActionResult> Sql(string fullName)
        {
            var connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["SqlConn"].ConnectionString);

            var list = new List<Person>();
            try
            {
                var query = "SELECT PersonId,FullName,Age FROM Persons ";
                var sqlCommand = new SqlCommand();
                if (!string.IsNullOrEmpty(fullName))
                {
                    query += "WHERE FullName LIKE '%' + @fullName + '%' ORDER BY FullName ASC";
                    sqlCommand.Parameters.AddWithValue("@fullName", fullName);
                }

                sqlCommand.CommandText = query;
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                sqlCommand.Connection = connection;
                var dataReader = await sqlCommand.ExecuteReaderAsync();

                while (dataReader.Read())
                {
                    var person = new Person
                    {
                        FullName = dataReader["FullName"].ToString(),
                        Age = Convert.ToInt32(dataReader["Age"]),
                        PersonId = Convert.ToInt32(dataReader["PersonId"])
                    };
                    list.Add(person);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            ViewBag.Query = fullName;
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Web app for AI extension sample";
            return View();
        }
    }
}