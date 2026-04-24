using System;
using System.Collections.Generic;

namespace CLS.BackendAPI.Models.Entities;

public partial class SessionLearner
{
    public int SessionLearnerId { get; set; }

    public int SessionId { get; set; }

    public int LearnerId { get; set; }

    public virtual Learner Learner { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;
}
