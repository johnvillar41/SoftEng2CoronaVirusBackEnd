﻿using Microsoft.AspNetCore.Mvc;
using SoftEng2BackendAPI.ApikeyAttribute;
using SoftEng2BackendAPI.Models;
using SoftEng2BackendAPI.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SoftEng2BackendAPI.Repositories.RepoImplementation
{
    public class SymptomsRepository : ISymptomRepository
    {
        /// <summary>
        ///     This function will fetch all the 
        ///     students which is sick from the local db
        /// </summary>
        /// <returns>
        ///     Will return all the list of students that are sick
        /// </returns>
        public async Task<IEnumerable<SymptomsModel>> FetchAllPeopleWithSymptomsAsync()
        {
            List<SymptomsModel> symptomsList = new List<SymptomsModel>();
            using (SqlConnection connection = new SqlConnection(DBCredentials.CONNECTION_STRING))
            {
                string queryString = "SELECT * FROM Symptoms_Table";
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    SymptomsModel symptomsModel = new SymptomsModel
                    {
                        Symptoms_ID = int.Parse(reader["symptoms_id"].ToString()),
                        User_ID = int.Parse(reader["user_id"].ToString()),
                        Symptoms_Name = reader["symptoms_name"].ToString()
                    };
                    symptomsList.Add(symptomsModel);
                }
                return symptomsList;
            }
        }
        /// <summary>
        ///     This will fetch all the symptoms for a specific student
        /// </summary>
        /// <param name="student_id">
        ///     this param will then be passed as parameters
        /// </param>
        /// <returns>
        ///     Will return a list of Symptoms for the student
        /// </returns>
        public async Task<IEnumerable<SymptomsModel>> FetchSymptomsForSpecificStudentAsync(int student_id)
        {
            List<SymptomsModel> symptomsList = new List<SymptomsModel>();
            using (SqlConnection connection = new SqlConnection(DBCredentials.CONNECTION_STRING))
            {
                await connection.OpenAsync();
                string queryString = "SELECT * FROM Symptoms_Table WHERE user_id = @user_id";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@user_id", student_id);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        SymptomsModel symptomsModel = new SymptomsModel
                        {
                            Symptoms_ID = int.Parse(reader["symptoms_id"].ToString()),
                            User_ID = int.Parse(reader["user_id"].ToString()),
                            Symptoms_Name = reader["symptoms_name"].ToString()
                        };
                        symptomsList.Add(symptomsModel);
                    }
                }
            }
            return symptomsList;
        }
        /// <summary>
        ///     Will search students that have specific symptoms
        /// </summary>
        /// <param name="symptom_name">
        ///     Will pass a symptom name as parameter to be searched
        /// </param>
        /// <returns>
        ///     Will return a list of students with specific symptoms
        /// </returns>
        public async Task<IEnumerable<UserModel>> FetchStudentsWithSpecificSymptomAsync(string symptom_name)
        {
            List<UserModel> userListWithSymptoms = new List<UserModel>();
            using (SqlConnection connection = new SqlConnection(DBCredentials.CONNECTION_STRING))
            {
                await connection.OpenAsync();
                string queryString = "SELECT Symptoms_Table.user_id,User_Table.user_username,User_Table.profile_picture,User_Table.user_status FROM User_Table INNER JOIN Symptoms_Table ON Symptoms_Table.user_id = User_Table.user_id WHERE Symptoms_Table.symptoms_name=@symptoms";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@symptoms", symptom_name);                
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        UserModel user = new UserModel
                        {
                            User_ID = int.Parse(reader["user_id"].ToString()),
                            User_Username = reader["user_username"].ToString(),                            
                            StringProfilePic = reader["profile_picture"].ToString(),
                            User_Status = reader["user_status"].ToString()
                        };
                        userListWithSymptoms.Add(user);
                    }
                }
            }
            return userListWithSymptoms;
        }
        /// <summary>
        ///     This will delete the students all symptoms from db
        /// </summary>
        /// <param name="id">
        ///     ID of student
        /// </param>        
        public async Task DeleteAllStudentSymptomsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DBCredentials.CONNECTION_STRING))
            {
                await connection.OpenAsync();
                string queryString = "DELETE FROM Symptoms_Table WHERE user_id=@user_id";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@user_id", id);
                await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        ///     Inserts a new type of symptom for the student
        /// </summary>
        /// <param name="symptoms">
        ///     New Symptom model
        /// </param>
        public async Task InsertNewSymptoms(SymptomsModel symptoms)
        {
            using (SqlConnection connection = new SqlConnection(DBCredentials.CONNECTION_STRING))
            {
                await connection.OpenAsync();
                string queryString = "INSERT INTO Symptoms_Table(user_id,symptoms_name)VALUES(@user_id,@symptoms_name)";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@user_id", symptoms.User_ID);
                command.Parameters.AddWithValue("@symptoms_name", symptoms.Symptoms_Name);
                await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        ///     Will Delete a specific symptom in the database
        /// </summary>
        /// <param name="symptom_id">
        ///     Given the symptom_id pk
        /// </param>
        public async Task DeleteSpecificSymptomAsync(int symptom_id)
        {
            using (SqlConnection connection = new SqlConnection(DBCredentials.CONNECTION_STRING))
            {
                await connection.OpenAsync();
                string queryString = "DELETE FROM Symptoms_Table WHERE symptoms_id=@symptoms_id";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@symptoms_id", symptom_id);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
