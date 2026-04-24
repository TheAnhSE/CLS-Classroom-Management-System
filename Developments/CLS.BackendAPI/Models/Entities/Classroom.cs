using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Classroom
{
    public int ClassroomId { get; set; }

    public string RoomName { get; set; } = null!;

    public int Capacity { get; set; }

    public string? Location { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
