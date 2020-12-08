using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class TaskController : ApiController
    {
        public HttpResponseMessage Get(string date)
        {
            string query = @"
                        select TaskId as id, TaskTitle as title, TaskDate as date from dbo.Tasks
                        where TaskDate='" + date + @"'
                    ";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["TasksAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public string Post (Task task)
        {
            try
            {
                string query = @"insert into dbo.Tasks values 
                                ('"+task.TaskId + @"',
                                '"+task.TaskTitle + @"',
                                '"+task.TaskDate + @"'
                                )
                                ";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["TasksAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }
                return "Added successfully";
            }

            catch (Exception)
            {
                return "Failed to post";
            }
        }

        public string Delete(Task task)
        {
            try
            {
                string query = @"delete from dbo.Tasks where
                                TaskId = '" + task.TaskId +@"' and 
                                TaskDate = '" + task.TaskDate +@"'
                                ";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["TasksAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }
                return "Deleted successfully";
            }

            catch (Exception)
            {
                return "Failed to delete";
            }
        }
    }
}
