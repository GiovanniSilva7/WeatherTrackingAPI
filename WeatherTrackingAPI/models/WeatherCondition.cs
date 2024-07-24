using System;

public class WeatherCondition 
{
    public string Location {  get; set; }
    public string Condition { get; set; }
    public DateTime? Timestamp { get; set; }
    public bool IsAlert { get; set; }
}