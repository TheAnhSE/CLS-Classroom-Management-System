using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public string? AvatarUrl { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<LearnerPackage> LearnerPackages { get; set; } = new List<LearnerPackage>();

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();

    public virtual ICollection<LearningPackage> LearningPackages { get; set; } = new List<LearningPackage>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Session> SessionCreatedByNavigations { get; set; } = new List<Session>();

    public virtual ICollection<Session> SessionTeachers { get; set; } = new List<Session>();
}
