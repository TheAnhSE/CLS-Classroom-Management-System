using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string Type { get; set; } = null!;

    public string RecipientEmail { get; set; } = null!;

    public int ParentId { get; set; }

    public int? LearnerId { get; set; }

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? SentTime { get; set; }

    public string? DeliveryStatus { get; set; }

    public string? ExternalMessageId { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual Learner? Learner { get; set; }

    public virtual Parent Parent { get; set; } = null!;
}
