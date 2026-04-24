using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<LearningPackage> LearningPackages { get; set; } = new List<LearningPackage>();

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
