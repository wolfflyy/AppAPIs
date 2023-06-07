using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppAPIs.Repos.Models;

[Table("Subject")]
public partial class Subject
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? SubId { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [InverseProperty("Sub")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
