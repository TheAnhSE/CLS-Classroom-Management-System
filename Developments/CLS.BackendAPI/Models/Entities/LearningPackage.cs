using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class LearningPackage
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } = null!;

    public int SubjectId { get; set; }

    public int TotalSessions { get; set; }

    public int DurationMonths { get; set; }

    public decimal TuitionFee { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<LearnerPackage> LearnerPackages { get; set; } = new List<LearnerPackage>();

    public virtual Subject Subject { get; set; } = null!;
}
