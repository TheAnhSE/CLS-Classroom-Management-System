CREATE DATABASE cls_db;
GO

USE cls_db;
GO

-- 1. Roles
CREATE TABLE roles (
    role_id INT IDENTITY(1,1) PRIMARY KEY,
    role_name NVARCHAR(50) NOT NULL UNIQUE,
    description NVARCHAR(255),
    is_active BIT DEFAULT 1
);

-- 2. Users
CREATE TABLE users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password_hash NVARCHAR(255) NOT NULL,
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    phone_number NVARCHAR(20),
    date_of_birth DATE,
    address NVARCHAR(MAX),
    role_id INT NOT NULL,
    is_active BIT DEFAULT 1,
    avatar_url NVARCHAR(255),
    created_by INT,
    created_time DATETIME2 DEFAULT GETDATE(),
    updated_by INT,
    updated_time DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT fk_users_role FOREIGN KEY (role_id) REFERENCES roles(role_id)
);

-- 3. Parents
CREATE TABLE parents (
    parent_id INT IDENTITY(1,1) PRIMARY KEY,
    full_name NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    phone_number NVARCHAR(20) NOT NULL,
    address NVARCHAR(MAX),
    relationship NVARCHAR(50) NOT NULL,
    created_time DATETIME2 DEFAULT GETDATE(),
    updated_time DATETIME2 DEFAULT GETDATE()
);

-- 4. Learners
CREATE TABLE learners (
    learner_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(50) NOT NULL,
    last_name NVARCHAR(50) NOT NULL,
    date_of_birth DATE,
    gender NVARCHAR(10) NOT NULL CHECK (gender IN ('Male','Female','Other')),
    parent_id INT NOT NULL,
    enrollment_date DATE NOT NULL,
    status NVARCHAR(20) DEFAULT 'Active'
        CHECK (status IN ('Active','Inactive','Suspended')),
    notes NVARCHAR(MAX),
    created_by INT NOT NULL,
    created_time DATETIME2 DEFAULT GETDATE(),
    updated_time DATETIME2 DEFAULT GETDATE(),

    FOREIGN KEY (parent_id) REFERENCES parents(parent_id) ON DELETE CASCADE,
    FOREIGN KEY (created_by) REFERENCES users(user_id)
);

-- 5. Subjects
CREATE TABLE subjects (
    subject_id INT IDENTITY(1,1) PRIMARY KEY,
    subject_name NVARCHAR(100) NOT NULL UNIQUE,
    description NVARCHAR(MAX),
    is_active BIT DEFAULT 1
);

-- 6. Learning Packages
CREATE TABLE learning_packages (
    package_id INT IDENTITY(1,1) PRIMARY KEY,
    package_name NVARCHAR(100) NOT NULL,
    subject_id INT NOT NULL,
    total_sessions INT NOT NULL,
    duration_months INT NOT NULL,
    tuition_fee DECIMAL(10,2) NOT NULL,
    description NVARCHAR(MAX),
    is_active BIT DEFAULT 1,
    created_by INT NOT NULL,
    created_time DATETIME2 DEFAULT GETDATE(),
    updated_time DATETIME2 DEFAULT GETDATE(),

    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id),
    FOREIGN KEY (created_by) REFERENCES users(user_id)
);

-- 7. Learner Packages
CREATE TABLE learner_packages (
    learner_package_id INT IDENTITY(1,1) PRIMARY KEY,
    learner_id INT NOT NULL,
    package_id INT NOT NULL,
    assigned_date DATE NOT NULL,
    expiry_date DATE NOT NULL,
    total_sessions INT NOT NULL,
    remaining_sessions INT NOT NULL,
    status NVARCHAR(20) DEFAULT 'Active'
        CHECK (status IN ('Active','Depleted','Expired')),
    assigned_by INT NOT NULL,
    created_time DATETIME2 DEFAULT GETDATE(),
    updated_time DATETIME2 DEFAULT GETDATE(),

    FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE CASCADE,
    FOREIGN KEY (package_id) REFERENCES learning_packages(package_id),
    FOREIGN KEY (assigned_by) REFERENCES users(user_id)
);

-- 8. Classrooms
CREATE TABLE classrooms (
    classroom_id INT IDENTITY(1,1) PRIMARY KEY,
    room_name NVARCHAR(50) NOT NULL UNIQUE,
    capacity INT NOT NULL,
    location NVARCHAR(100),
    is_active BIT DEFAULT 1
);

-- 9. Sessions
CREATE TABLE sessions (
    session_id INT IDENTITY(1,1) PRIMARY KEY,
    subject_id INT NOT NULL,
    classroom_id INT NOT NULL,
    teacher_id INT NOT NULL,
    session_date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    status NVARCHAR(20) DEFAULT 'Scheduled'
        CHECK (status IN ('Scheduled','Ongoing','Completed','Cancelled')),
    notes NVARCHAR(MAX),
    created_by INT NOT NULL,
    created_time DATETIME2 DEFAULT GETDATE(),
    updated_time DATETIME2 DEFAULT GETDATE(),

    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id),
    FOREIGN KEY (classroom_id) REFERENCES classrooms(classroom_id),
    FOREIGN KEY (teacher_id) REFERENCES users(user_id),
    FOREIGN KEY (created_by) REFERENCES users(user_id)
);

-- 10. Session Learners
CREATE TABLE session_learners (
    session_learner_id INT IDENTITY(1,1) PRIMARY KEY,
    session_id INT NOT NULL,
    learner_id INT NOT NULL,

    FOREIGN KEY (session_id) REFERENCES sessions(session_id) ON DELETE CASCADE,
    FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE CASCADE,
    CONSTRAINT unique_session_learner UNIQUE (session_id, learner_id)
);

-- 11. Attendance
CREATE TABLE attendance (
    attendance_id INT IDENTITY(1,1) PRIMARY KEY,
    session_id INT NOT NULL,
    learner_id INT NOT NULL,
    status NVARCHAR(10) NOT NULL
        CHECK (status IN ('Present','Absent','Late')),
    recorded_by INT NOT NULL,
    recorded_time DATETIME2 DEFAULT GETDATE(),
    notes NVARCHAR(255),

    FOREIGN KEY (session_id) REFERENCES sessions(session_id) ON DELETE CASCADE,
    FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE CASCADE,
    FOREIGN KEY (recorded_by) REFERENCES users(user_id),
    CONSTRAINT unique_att_log UNIQUE (session_id, learner_id)
);

-- 12. Feedback
CREATE TABLE feedback (
    feedback_id INT IDENTITY(1,1) PRIMARY KEY,
    session_id INT NOT NULL,
    learner_id INT NOT NULL,
    teacher_id INT NOT NULL,
    performance_rating INT CHECK (performance_rating BETWEEN 1 AND 5),
    behavioral_notes NVARCHAR(MAX),
    recommendations NVARCHAR(MAX),
    submitted_time DATETIME2 DEFAULT GETDATE(),
    sla_deadline DATETIME2 NOT NULL,
    is_on_time BIT NOT NULL,
    created_time DATETIME2 DEFAULT GETDATE(),

    FOREIGN KEY (session_id) REFERENCES sessions(session_id) ON DELETE CASCADE,
    FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE CASCADE,
    FOREIGN KEY (teacher_id) REFERENCES users(user_id),
    CONSTRAINT unique_feedback UNIQUE (session_id, learner_id)
);

-- 13. Notifications
CREATE TABLE notifications (
    notification_id INT IDENTITY(1,1) PRIMARY KEY,
    type NVARCHAR(50) NOT NULL
        CHECK (type IN ('Attendance_Alert','Feedback_Alert','Package_Warning','Schedule_Change')),
    recipient_email NVARCHAR(100) NOT NULL,
    parent_id INT NOT NULL,
    learner_id INT,
    subject NVARCHAR(255) NOT NULL,
    body NVARCHAR(MAX) NOT NULL,
    status NVARCHAR(20) DEFAULT 'Pending'
        CHECK (status IN ('Pending','Sent','Failed')),
    sent_time DATETIME2,
    delivery_status NVARCHAR(50),
    external_message_id NVARCHAR(100),
    created_time DATETIME2 DEFAULT GETDATE(),

    FOREIGN KEY (parent_id) REFERENCES parents(parent_id) ON DELETE CASCADE,
    FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE SET NULL
);

-- 14. Settings
CREATE TABLE settings (
    setting_id INT IDENTITY(1,1) PRIMARY KEY,
    setting_name NVARCHAR(100) NOT NULL UNIQUE,
    setting_type NVARCHAR(50) NOT NULL,
    setting_value NVARCHAR(255) NOT NULL,
    priority INT DEFAULT 0,
    description NVARCHAR(MAX),
    is_active BIT DEFAULT 1,
    created_by INT,
    created_time DATETIME2 DEFAULT GETDATE(),
    updated_time DATETIME2 DEFAULT GETDATE()
);

-- 15. Activity Log
CREATE TABLE activity_log (
    log_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    action NVARCHAR(100) NOT NULL,
    entity_type NVARCHAR(50) NOT NULL,
    entity_id INT NOT NULL,
    details NVARCHAR(MAX),
    ip_address NVARCHAR(45),
    created_time DATETIME2 DEFAULT GETDATE(),

    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

-- Indexes
CREATE INDEX idx_sessions_time ON sessions(session_date, start_time, end_time);
CREATE INDEX idx_learner_pkg_status ON learner_packages(status);
CREATE INDEX idx_feedback_sla ON feedback(is_on_time);
CREATE INDEX idx_activity_log_entity ON activity_log(entity_type, entity_id);