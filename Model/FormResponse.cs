using System;
using DPU_AQD_API.Models;

public class FormResponse
{
    public int FormID {get; set;}
    public string FormURL {get; set;}
    public DateTime CreateDate {get; set;}
    public char FormStatus {get; set;}
}