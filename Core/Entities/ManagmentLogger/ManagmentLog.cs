﻿using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.ManagmentLogger;

public class ManagmentLog
{
    [Key]
    public int Id { get; set; }
    public DateTime LogTime { get; set; }
    [MaxLength(100)]
    public string Title { get; set; }
    [Required]
    public LogEventTypes EventType { get; set; }
    [Required]
    [MaxLength(255)]
    public string User { get; set; }
    [MaxLength(2000)]
    public string AdditionalData { get; set; }
}