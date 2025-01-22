﻿using DAL.Enums;

namespace DAL.Entities;

public class Report
{
    public int Id { get; set; }
    public ReportType Type { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Data { get; set; }
}