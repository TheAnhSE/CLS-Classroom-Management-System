using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Parent
{
    public int ParentId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Address { get; set; }

    public string Relationship { get; set; } = null!;

    public DateTime? CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
