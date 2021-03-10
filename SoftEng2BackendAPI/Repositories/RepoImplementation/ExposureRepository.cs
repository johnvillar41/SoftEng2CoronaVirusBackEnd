﻿using SoftEng2BackendAPI.ApikeyAttribute;
using SoftEng2BackendAPI.Models;
using SoftEng2BackendAPI.Repositories.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SoftEng2BackendAPI.Repositories.RepoImplementation
{
    public class ExposureRepository : IExposureRepository
    {
        public async Task<IEnumerable> FetchAllExposedStudents()
        {
            List<ExposureModel> exposedList = new List<ExposureModel>();
            using (SqlConnection connection = new SqlConnection(DBCredentials.CONNECTION_STRING))
            {
                connection.Open();
                string queryString = "SELECT * FROM Exposure_Table";
                SqlCommand command = new SqlCommand(queryString, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        ExposureModel exposureModel = new ExposureModel
                        {
                            Exposure_ID = int.Parse(reader["exposure_id"].ToString()),
                            User_ID = int.Parse(reader["user_id"].ToString().ToString()),
                            Exposed_To_ID = int.Parse(reader["exposed_to_id"].ToString()),
                            Exposed_Date = Convert.ToDateTime(reader["exposed_date"].ToString())
                        };
                        exposedList.Add(exposureModel);
                    }                    
                }
            }
            return exposedList;
        }

        public Task<IEnumerable<ExposureModel>> FetchExposedStudentsGivenByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
