using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using DPU_AQD_API.Models;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]

public class FormController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("GetAllForm")]
    public async Task<IActionResult> GetAllForm()
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getAllForms"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<FormResponse> formResponses = new List<FormResponse>();
            while (reader.Read())
            {
                FormResponse formResponse = new FormResponse();
                formResponse.FormID =  Convert.ToInt32(reader["FormID"]);
                formResponse.FormURL = reader["FormURL"].ToString();
                formResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                formResponse.FormStatus = Convert.ToChar(reader["FormStatus"].ToString());

                formResponses.Add(formResponse);
            }
            await connection.CloseAsync();
            return Ok(formResponses);
        }
    }
    [HttpGet("GetLatestForm")]
    public async Task<IActionResult> GetLatestForm()
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getForm"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<FormResponse> formResponses = new List<FormResponse>();
            while (reader.Read())
            {
                FormResponse formResponse = new FormResponse();
                formResponse.FormID =  Convert.ToInt32(reader["FormID"]);
                formResponse.FormURL = reader["FormURL"].ToString();
                formResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                formResponse.FormStatus = Convert.ToChar(reader["FormStatus"].ToString());

                formResponses.Add(formResponse);
            }
            await connection.CloseAsync();
            return Ok(formResponses);
        }
    }
    [HttpGet("AddNewForm")]
    public async Task<IActionResult> AddNewForm(string _formURL)
    {
        int count = 1;
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getAllForms";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            try{
                while (reader.Read())
                {
                    count++;
                }
            } catch (MySqlException ex){
                throw;
            }
            await connection.CloseAsync();
        }

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "setNewForm"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_FormID", MySqlDbType.Int32).Value = Convert.ToInt32(DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", count));
            cmd.Parameters.Add("_FormURL", MySqlDbType.VarChar).Value = _formURL;
            cmd.Parameters.Add("_CreateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_FormStatus", MySqlDbType.VarChar).Value = "T";
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<FormResponse> formResponses = new List<FormResponse>();
            while (reader.Read())
            {
                FormResponse formResponse = new FormResponse();
                formResponse.FormID =  Convert.ToInt32(reader["FormID"]);
                formResponse.FormURL = reader["FormURL"].ToString();
                formResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                formResponse.FormStatus = Convert.ToChar(reader["FormStatus"].ToString());

                formResponses.Add(formResponse);
            }
            await connection.CloseAsync();
            return Ok(formResponses);
        }
    }
}