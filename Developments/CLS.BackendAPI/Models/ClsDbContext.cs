using System;
using System.Collections.Generic;
using CLS.BackendAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CLS.BackendAPI.Models;

public partial class ClsDbContext : DbContext
{
    public ClsDbContext(DbContextOptions<ClsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Learner> Learners { get; set; }

    public virtual DbSet<LearnerPackage> LearnerPackages { get; set; }

    public virtual DbSet<LearningPackage> LearningPackages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SessionLearner> SessionLearners { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__activity__9E2397E0E006D676");

            entity.ToTable("activity_log");

            entity.HasIndex(e => new { e.EntityType, e.EntityId }, "idx_activity_log_entity");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .HasColumnName("action");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasMaxLength(50)
                .HasColumnName("entity_type");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__activity___user___01142BA1");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__attendan__20D6A968DB9441C0");

            entity.ToTable("attendance");

            entity.HasIndex(e => new { e.SessionId, e.LearnerId }, "unique_att_log").IsUnique();

            entity.Property(e => e.AttendanceId).HasColumnName("attendance_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.RecordedBy).HasColumnName("recorded_by");
            entity.Property(e => e.RecordedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("recorded_time");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");

            entity.HasOne(d => d.Learner).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__attendanc__learn__656C112C");

            entity.HasOne(d => d.RecordedByNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.RecordedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__attendanc__recor__66603565");

            entity.HasOne(d => d.Session).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__attendanc__sessi__6477ECF3");
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("PK__classroo__448E90B8EC220621");

            entity.ToTable("classrooms");

            entity.HasIndex(e => e.RoomName, "UQ__classroo__1B7D99CD037DB25F").IsUnique();

            entity.Property(e => e.ClassroomId).HasColumnName("classroom_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.RoomName)
                .HasMaxLength(50)
                .HasColumnName("room_name");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__feedback__7A6B2B8CACC9C639");

            entity.ToTable("feedback");

            entity.HasIndex(e => e.IsOnTime, "idx_feedback_sla");

            entity.HasIndex(e => new { e.SessionId, e.LearnerId }, "unique_feedback").IsUnique();

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.BehavioralNotes).HasColumnName("behavioral_notes");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.IsOnTime).HasColumnName("is_on_time");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.PerformanceRating).HasColumnName("performance_rating");
            entity.Property(e => e.Recommendations).HasColumnName("recommendations");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.SlaDeadline).HasColumnName("sla_deadline");
            entity.Property(e => e.SubmittedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("submitted_time");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Learner).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__feedback__learne__6E01572D");

            entity.HasOne(d => d.Session).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__feedback__sessio__6D0D32F4");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedback__teache__6EF57B66");
        });

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.HasKey(e => e.LearnerId).HasName("PK__learners__82783B41AA959847");

            entity.ToTable("learners");

            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.EnrollmentDate).HasColumnName("enrollment_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_time");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Learners)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__learners__create__398D8EEE");

            entity.HasOne(d => d.Parent).WithMany(p => p.Learners)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__learners__parent__38996AB5");
        });

        modelBuilder.Entity<LearnerPackage>(entity =>
        {
            entity.HasKey(e => e.LearnerPackageId).HasName("PK__learner___A7931AA45F8880E8");

            entity.ToTable("learner_packages");

            entity.HasIndex(e => e.Status, "idx_learner_pkg_status");

            entity.Property(e => e.LearnerPackageId).HasColumnName("learner_package_id");
            entity.Property(e => e.AssignedBy).HasColumnName("assigned_by");
            entity.Property(e => e.AssignedDate).HasColumnName("assigned_date");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.RemainingSessions).HasColumnName("remaining_sessions");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.TotalSessions).HasColumnName("total_sessions");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_time");

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.LearnerPackages)
                .HasForeignKey(d => d.AssignedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__learner_p__assig__4CA06362");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearnerPackages)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__learner_p__learn__4AB81AF0");

            entity.HasOne(d => d.Package).WithMany(p => p.LearnerPackages)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__learner_p__packa__4BAC3F29");
        });

        modelBuilder.Entity<LearningPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__learning__63846AE848AC38F3");

            entity.ToTable("learning_packages");

            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DurationMonths).HasColumnName("duration_months");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PackageName)
                .HasMaxLength(100)
                .HasColumnName("package_name");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.TotalSessions).HasColumnName("total_sessions");
            entity.Property(e => e.TuitionFee)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("tuition_fee");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_time");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LearningPackages)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__learning___creat__440B1D61");

            entity.HasOne(d => d.Subject).WithMany(p => p.LearningPackages)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__learning___subje__4316F928");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__notifica__E059842F1B74C964");

            entity.ToTable("notifications");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.DeliveryStatus)
                .HasMaxLength(50)
                .HasColumnName("delivery_status");
            entity.Property(e => e.ExternalMessageId)
                .HasMaxLength(100)
                .HasColumnName("external_message_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.RecipientEmail)
                .HasMaxLength(100)
                .HasColumnName("recipient_email");
            entity.Property(e => e.SentTime).HasColumnName("sent_time");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .HasColumnName("subject");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.Learner).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__notificat__learn__76969D2E");

            entity.HasOne(d => d.Parent).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__notificat__paren__75A278F5");
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.ParentId).HasName("PK__parents__F2A6081980DD9D0F");

            entity.ToTable("parents");

            entity.HasIndex(e => e.Email, "UQ__parents__AB6E6164231CDE2C").IsUnique();

            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Relationship)
                .HasMaxLength(50)
                .HasColumnName("relationship");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_time");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CC1983C6AE");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "UQ__roles__783254B1D606EC74").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__sessions__69B13FDC48AF44FD");

            entity.ToTable("sessions");

            entity.HasIndex(e => new { e.SessionDate, e.StartTime, e.EndTime }, "idx_sessions_time");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.ClassroomId).HasColumnName("classroom_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.SessionDate).HasColumnName("session_date");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Scheduled")
                .HasColumnName("status");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_time");

            entity.HasOne(d => d.Classroom).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.ClassroomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__sessions__classr__5812160E");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SessionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__sessions__create__59FA5E80");

            entity.HasOne(d => d.Subject).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__sessions__subjec__571DF1D5");

            entity.HasOne(d => d.Teacher).WithMany(p => p.SessionTeachers)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__sessions__teache__59063A47");
        });

        modelBuilder.Entity<SessionLearner>(entity =>
        {
            entity.HasKey(e => e.SessionLearnerId).HasName("PK__session___66A9484D32296D3B");

            entity.ToTable("session_learners");

            entity.HasIndex(e => new { e.SessionId, e.LearnerId }, "unique_session_learner").IsUnique();

            entity.Property(e => e.SessionLearnerId).HasColumnName("session_learner_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.SessionId).HasColumnName("session_id");

            entity.HasOne(d => d.Learner).WithMany(p => p.SessionLearners)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__session_l__learn__5EBF139D");

            entity.HasOne(d => d.Session).WithMany(p => p.SessionLearners)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__session_l__sessi__5DCAEF64");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PK__settings__256E1E323A1A7B18");

            entity.ToTable("settings");

            entity.HasIndex(e => e.SettingName, "UQ__settings__9C01CC5AE8FF0612").IsUnique();

            entity.Property(e => e.SettingId).HasColumnName("setting_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Priority)
                .HasDefaultValue(0)
                .HasColumnName("priority");
            entity.Property(e => e.SettingName)
                .HasMaxLength(100)
                .HasColumnName("setting_name");
            entity.Property(e => e.SettingType)
                .HasMaxLength(50)
                .HasColumnName("setting_type");
            entity.Property(e => e.SettingValue)
                .HasMaxLength(255)
                .HasColumnName("setting_value");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_time");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__subjects__5004F6606CE41DF4");

            entity.ToTable("subjects");

            entity.HasIndex(e => e.SubjectName, "UQ__subjects__40817661926E6FE9").IsUnique();

            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(100)
                .HasColumnName("subject_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F6689C559");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164F59D5A9E").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatar_url");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_time");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_time");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
