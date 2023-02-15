using System;
using DPU_AQD_API.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]
public class RoomController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("getRoom")]
    public async Task<IActionResult> GetRoom () 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getRoom"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            while(reader.Read()){
                RoomResponse roomResponse = new RoomResponse();
                roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                roomResponse.RoomName = reader["RoomName"].ToString();
                roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                roomResponses.Add(roomResponse);
            }
            await connection.CloseAsync();
            return Ok(roomResponses);
        }
    }
    [HttpGet("getRoomByID")]
    public async Task<IActionResult> GetRoomByID (int _roomID) 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getRoomByID"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_RoomID", MySqlDbType.Int32).Value = _roomID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            while(reader.Read()){
                RoomResponse roomResponse = new RoomResponse();
                roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                roomResponse.RoomName = reader["RoomName"].ToString();
                roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                roomResponses.Add(roomResponse);
            }
            await connection.CloseAsync();
            return Ok(roomResponses);
        }
    }
}