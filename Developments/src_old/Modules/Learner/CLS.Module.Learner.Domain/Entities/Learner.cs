using CLS.Module.Learner.Domain.Enums;

namespace CLS.Module.Learner.Domain.Entities;

public class Learner
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string ParentEmail { get; private set; }
    public string ParentPhone { get; private set; }
    public LearnerStatus Status { get; private set; }
    public Guid InitialLearningPackageId { get; private set; }
    public int RemainingSessions { get; private set; }
    public DateTime EnrollmentDate { get; private set; }

    // EF Core constructor
    private Learner() { }

    public Learner(string fullName, string parentEmail, string parentPhone, Guid initialLearningPackageId, int initialSessions)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        ParentEmail = parentEmail;
        ParentPhone = parentPhone;
        InitialLearningPackageId = initialLearningPackageId;
        RemainingSessions = initialSessions;
        Status = LearnerStatus.Active;
        EnrollmentDate = DateTime.UtcNow;
    }

    public void UpdateStatus(LearnerStatus newStatus)
    {
        Status = newStatus;
    }

    public void DecrementSession()
    {
        if (RemainingSessions > 0)
        {
            RemainingSessions--;
        }
    }
}
