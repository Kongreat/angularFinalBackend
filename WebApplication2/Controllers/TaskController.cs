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
                        select TaskId, TaskTitle, TaskDate from dbo.Tasks
                        where TaskDate='" + date + @"'
                        order by TaskId
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

        public int Post (Task task)
        {
            int res;
            try
            {
                string query = @"insert into dbo.Tasks values 
                                (
                                '"+task.TaskTitle + @"',
                                '"+task.TaskDate + @"'
                                )
                                ";

                string query2 = @"SELECT TaskId
                                FROM dbo.Tasks
                                ORDER BY TaskId DESC
                                ";
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["TasksAppDB"].ConnectionString))
                {
                    using (var cmd = new SqlCommand(query, con))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.Text;
                        da.Fill(table);
                    }

                    using (SqlCommand cmd = new SqlCommand(query2, con))

                    {
                        con.Open();
                        res = (int)cmd.ExecuteScalar();
                    }
                }
                return res;
        }

            catch (Exception)
            {
                return -1;
            }
}

        public string Delete(int id)
        {
            try
            {
                string query = @"delete from dbo.Tasks where
                                TaskId = " + id + @"
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
