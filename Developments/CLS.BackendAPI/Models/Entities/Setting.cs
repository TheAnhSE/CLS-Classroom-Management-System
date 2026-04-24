using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class Setting
{
    public int SettingId { get; set; }

    public string SettingName { get; set; } = null!;

    public string SettingType { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    public int? Priority { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }
}
