using System;
using DPU_AQD_API.Models;

public class IAQResponse
{
    public int IAQ_ID {get; set;}
    public DateTime timestamp {get; set;}
    public int Value {get; set;}
    public int x {get; set;}
    int min_x {get; set;}
    int max_x {get; set;}
    int i {get; set;}
    int min_i {get; set;}
    int max_i {get; set;}
}