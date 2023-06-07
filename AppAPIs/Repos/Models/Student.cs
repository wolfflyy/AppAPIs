using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppAPIs.Repos.Models;

[Table("Student")]
public partial class Student
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Fullname { get; set; } = null!;

    [Column("MSSV")]
    [StringLength(255)]
    public string Mssv { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? DoB { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string? Major { get; set; }

    [StringLength(255)]
    public string? Course { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
