﻿using HireHub.AllUsers.Models;
using HireHub.Employers.Models;
using HireHub.JobSeekers.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireHub.Database.Queries
{
    public class JobQueries : DatabaseConnection
    {
        SqlConnection connection;

        public JobQueries()
        {
            connection = new SqlConnection(dbConnectionString);
        }
        public async Task<bool> AddAJob(JobModel jobModel)
        {
            try
            {
                connection.Open();
                string insertQuery = $"INSERT INTO Job (employerID, jobStatus, jobType, roleName, companyName, experienceLevel, jobDetails, salary, jobLocation, hiringManager) VALUES ('{jobModel.EmployerId}','{jobModel.JobStatus}','{jobModel.JobType}', '{jobModel.RoleName}', '{jobModel.CompanyName}','{jobModel.ExperienceLevel}','{jobModel.JobDetails}', '{jobModel.Salary}', '{jobModel.JobLocation}','{jobModel.HiringManager}')";
                SqlCommand cmd = new SqlCommand(insertQuery, connection);

                int rowAffected = (int)await cmd.ExecuteNonQueryAsync();
                connection.Close();

                ////Updated
                return (rowAffected > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<JobDetail> SearchJob(string searchString)
        {
            List<JobDetail> jobDetails = new List<JobDetail>();

            string selectQueryBasedOnSearchString = $"SELECT jobId, employerId,jobStatus, roleName, companyName, jobType, experienceLevel, jobDetails, salary, jobLocation, hiringManager from Job jobInstance where jobInstance.roleName LIKE '%{searchString}%'";
            string searchStringGeneric = $"SELECT jobId,employerId,jobStatus, roleName, companyName, jobType, experienceLevel, jobDetails, salary, jobLocation, hiringManager from Job";

            SqlCommand cmd;
            if (searchString == "Generic")
            {
                cmd = new SqlCommand(searchStringGeneric, connection);
            }
            else
            {
                cmd = new SqlCommand(selectQueryBasedOnSearchString, connection);
            }
           
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    throw new ApplicationException("MISSING returned transaction");
                }
                while (reader.HasRows)
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            JobDetail jobDetail = new JobDetail();

                            jobDetail.roleName = reader["roleName"].ToString();
                            jobDetail.jobDetails = reader["jobDetails"].ToString();
                            jobDetail.jobId = Convert.ToInt32(reader["jobId"]);
                            jobDetail.jobType = reader["jobType"].ToString();
                            jobDetail.experienceLevel = reader["experienceLevel"].ToString();
                            jobDetail.jobStatus = reader["jobStatus"].ToString();
                            jobDetail.hiringManager = reader["hiringManager"].ToString();
                            jobDetail.salary = Convert.ToDecimal(reader["salary"]);
                            jobDetail.companyName = reader["companyName"].ToString();

                            jobDetails.Add(jobDetail);

                            Debug.WriteLine("Job Id " + jobDetail.jobId);
                            Debug.WriteLine("Role Name " + jobDetail.roleName);
                            Debug.WriteLine("Job Details " + jobDetail.jobDetails);
                        }
                        reader.NextResult();
                    }
                }
                reader.Close();
                return jobDetails;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                connection.Close();
            }

        }

    }
}
