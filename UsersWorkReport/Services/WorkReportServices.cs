﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UsersWorkReport.Models;

namespace UsersWorkReport.Services
{
    public interface IWorkReportServices
    {
        List<WorkReport> GetWorkReport();
    }
    public class WorkReportServices: IWorkReportServices
    {
        private readonly string _connection;

        public WorkReportServices(IOptions<ConnectionString> options)
        {
            _connection = options.Value.Connection;
        }
        public List<WorkReport> GetWorkReport()
        {
          
            List<WorkReport> workReports = new List<WorkReport>();
            try
            {
                SqlConnection con = new SqlConnection(_connection);
                {
                    SqlCommand cmd = new SqlCommand("USP_WorkReport", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        WorkReport Workobj = new WorkReport
                        {
                            WorkingDate = dr["WorkingDate"].ToString(),
                            ScreenName = dr["ScreenName"].ToString(),
                            SideMenu = dr["SideMenu"].ToString(),
                            Module = dr["Module"].ToString(),
                            TestingStatus = Convert.ToBoolean(dr["TestingStatus"]),
                            Comment = dr["Comment"].ToString()
                        };
                         workReports.Add(Workobj);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
           
            return workReports;
        }
    }
}
