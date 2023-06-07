using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppAPIs.Repos.Models;

[Table("Schedule")]
public partial class Schedule
{
    [Key]
    public int Id { get; set; }

    public int? StudentId { get; set; }

    public int? ClassId { get; set; }

    public int? SubId { get; set; }

    public int? LecturerId { get; set; }

    public int? RoomId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(255)]
    public string? Slot { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Schedules")]
    public virtual Class? Class { get; set; }

    [ForeignKey("LecturerId")]
    [InverseProperty("Schedules")]
    public virtual Lecturer? Lecturer { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("Schedules")]
    public virtual Classroom? Room { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Schedules")]
    public virtual Student? Student { get; set; }

    [ForeignKey("SubId")]
    [InverseProperty("Schedules")]
    public virtual Subject? Sub { get; set; }
}
