using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppAPIs.Repos.Models;

[Table("Classroom")]
public partial class Classroom
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    public int? DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Classrooms")]
    public virtual Department? Department { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
