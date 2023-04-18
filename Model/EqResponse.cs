using System;
using DPU_AQD_API.Models;

public class EqResponse
{
    public double avg_pm2_5 {get; set;}
    public double avg_pm10 {get; set;}
    public int min_pm2_5 {get; set;}
    public int max_pm2_5 {get; set;}
    public int min_pm10 {get; set;}
    public int max_pm10 {get; set;}
    
    public int x {get; set;}
    int min_x {get; set;}
    int max_x {get; set;}
    int min_i {get; set;}
    int max_i {get; set;}
}