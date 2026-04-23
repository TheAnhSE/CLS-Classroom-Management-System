using CLS.Module.Learner.Domain.Enums;

namespace CLS.Module.Learner.Application.DTOs;

public record LearnerResponseDto(
    Guid Id, 
    string FullName, 
    string ParentEmail,
    string ParentPhone,
    LearnerStatus Status, 
    int RemainingSessions,
    DateTime EnrollmentDate);

public record EnrollLearnerRequestDto(
    string FullName, 
    string ParentEmail, 
    string ParentPhone, 
    Guid InitialLearningPackageId, 
    int InitialSessions);
