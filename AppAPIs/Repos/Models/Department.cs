using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppAPIs.Repos.Models;

[Table("Department")]
public partial class Department
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
}
