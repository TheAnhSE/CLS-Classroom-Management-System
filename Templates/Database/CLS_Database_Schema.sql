-- ======================================================================
-- CLS (Classroom Management System) - Logical Database Schema (MySQL)
-- Auto-generated from CLS_ERD_Logical_DataModel
-- ======================================================================

CREATE DATABASE IF NOT EXISTS cls_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE cls_db;

-- 1. Roles Table
CREATE TABLE IF NOT EXISTS roles (
    role_id INT AUTO_INCREMENT PRIMARY KEY,
    role_name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    is_active BOOLEAN DEFAULT TRUE
);

-- 2. Users Table (Admin, Teacher, Center Director)
CREATE TABLE IF NOT EXISTS users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    phone_number VARCHAR(20),
    date_of_birth DATE,
    address TEXT,
    role_id INT NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    avatar_url VARCHAR(255),
    created_by INT,
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_by INT,
    updated_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_users_role FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE RESTRICT
);

-- 3. Parents Table
CREATE TABLE IF NOT EXISTS parents (
    parent_id INT AUTO_INCREMENT PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    phone_number VARCHAR(20) NOT NULL,
    address TEXT,
    relationship VARCHAR(50) NOT NULL,
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 4. Learners Table
CREATE TABLE IF NOT EXISTS learners (
    learner_id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    date_of_birth DATE,
    gender ENUM('Male', 'Female', 'Other') NOT NULL,
    parent_id INT NOT NULL,
    enrollment_date DATE NOT NULL,
    status ENUM('Active', 'Inactive', 'Suspended') DEFAULT 'Active',
    notes TEXT,
    created_by INT NOT NULL,
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_learners_parent FOREIGN KEY (parent_id) REFERENCES parents(parent_id) ON DELETE CASCADE,
    CONSTRAINT fk_learners_creator FOREIGN KEY (created_by) REFERENCES users(user_id) ON DELETE RESTRICT
);

-- 5. Subjects Table
CREATE TABLE IF NOT EXISTS subjects (
    subject_id INT AUTO_INCREMENT PRIMARY KEY,
    subject_name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE
);

-- 6. Learning Packages Master Table
CREATE TABLE IF NOT EXISTS learning_packages (
    package_id INT AUTO_INCREMENT PRIMARY KEY,
    package_name VARCHAR(100) NOT NULL,
    subject_id INT NOT NULL,
    total_sessions INT NOT NULL,
    duration_months INT NOT NULL,
    tuition_fee DECIMAL(10, 2) NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_by INT NOT NULL,
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_packages_subject FOREIGN KEY (subject_id) REFERENCES subjects(subject_id) ON DELETE RESTRICT,
    CONSTRAINT fk_packages_creator FOREIGN KEY (created_by) REFERENCES users(user_id) ON DELETE RESTRICT
);

-- 7. Learner Packages (Tuition ledger mapped to students)
CREATE TABLE IF NOT EXISTS learner_packages (
    learner_package_id INT AUTO_INCREMENT PRIMARY KEY,
    learner_id INT NOT NULL,
    package_id INT NOT NULL,
    assigned_date DATE NOT NULL,
    expiry_date DATE NOT NULL,
    total_sessions INT NOT NULL,
    remaining_sessions INT NOT NULL,
    status ENUM('Active', 'Depleted', 'Expired') DEFAULT 'Active',
    assigned_by INT NOT NULL,
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_lp_learner FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE CASCADE,
    CONSTRAINT fk_lp_package FOREIGN KEY (package_id) REFERENCES learning_packages(package_id) ON DELETE RESTRICT,
    CONSTRAINT fk_lp_assigner FOREIGN KEY (assigned_by) REFERENCES users(user_id) ON DELETE RESTRICT
);

-- 8. Classrooms Table
CREATE TABLE IF NOT EXISTS classrooms (
    classroom_id INT AUTO_INCREMENT PRIMARY KEY,
    room_name VARCHAR(50) NOT NULL UNIQUE,
    capacity INT NOT NULL,
    location VARCHAR(100),
    is_active BOOLEAN DEFAULT TRUE
);

-- 9. Sessions Table (Timetable Blocks)
CREATE TABLE IF NOT EXISTS sessions (
    session_id INT AUTO_INCREMENT PRIMARY KEY,
    subject_id INT NOT NULL,
    classroom_id INT NOT NULL,
    teacher_id INT NOT NULL,
    session_date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    status ENUM('Scheduled', 'Ongoing', 'Completed', 'Cancelled') DEFAULT 'Scheduled',
    notes TEXT,
    created_by INT NOT NULL,
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_sessions_subject FOREIGN KEY (subject_id) REFERENCES subjects(subject_id) ON DELETE RESTRICT,
    CONSTRAINT fk_sessions_room FOREIGN KEY (classroom_id) REFERENCES classrooms(classroom_id) ON DELETE RESTRICT,
    CONSTRAINT fk_sessions_teacher FOREIGN KEY (teacher_id) REFERENCES users(user_id) ON DELETE RESTRICT,
    CONSTRAINT fk_sessions_creator FOREIGN KEY (created_by) REFERENCES users(user_id) ON DELETE RESTRICT
);

-- 10. Session Learners (Mapping Learners to scheduled Sessions)
CREATE TABLE IF NOT EXISTS session_learners (
    session_learner_id INT AUTO_INCREMENT PRIMARY KEY,
    session_id INT NOT NULL,
    learner_id INT NOT NULL,
    CONSTRAINT fk_sl_session FOREIGN KEY (session_id) REFERENCES sessions(session_id) ON DELETE CASCADE,
    CONSTRAINT fk_sl_learner FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE CASCADE,
    UNIQUE KEY unique_session_learner (session_id, learner_id)
);

-- 11. Attendance Table
CREATE TABLE IF NOT EXISTS attendance (
    attendance_id INT AUTO_INCREMENT PRIMARY KEY,
    session_id INT NOT NULL,
    learner_id INT NOT NULL,
    status ENUM('Present', 'Absent', 'Late') NOT NULL,
    recorded_by INT NOT NULL,
    recorded_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    notes VARCHAR(255),
    CONSTRAINT fk_att_session FOREIGN KEY (session_id) REFERENCES sessions(session_id) ON DELETE CASCADE,
    CONSTRAINT fk_att_learner FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE CASCADE,
    CONSTRAINT fk_att_recorder FOREIGN KEY (recorded_by) REFERENCES users(user_id) ON DELETE RESTRICT,
    UNIQUE KEY unique_att_log (session_id, learner_id)
);

-- 12. Feedback (Academic SLA module)
CREATE TABLE IF NOT EXISTS feedback (
    feedback_id INT AUTO_INCREMENT PRIMARY KEY,
    session_id INT NOT NULL,
    learner_id INT NOT NULL,
    teacher_id INT NOT NULL,
    performance_rating INT CHECK (performance_rating BETWEEN 1 AND 5),
    behavioral_notes TEXT,
    recommendations TEXT,
    submitted_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    sla_deadline DATETIME NOT NULL,
    is_on_time BOOLEAN NOT NULL,
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_fb_session FOREIGN KEY (session_id) REFERENCES sessions(session_id) ON DELETE CASCADE,
    CONSTRAINT fk_fb_learner FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE CASCADE,
    CONSTRAINT fk_fb_teacher FOREIGN KEY (teacher_id) REFERENCES users(user_id) ON DELETE RESTRICT,
    UNIQUE KEY unique_feedback (session_id, learner_id)
);

-- 13. Notifications Table
CREATE TABLE IF NOT EXISTS notifications (
    notification_id INT AUTO_INCREMENT PRIMARY KEY,
    type ENUM('Attendance_Alert', 'Feedback_Alert', 'Package_Warning', 'Schedule_Change') NOT NULL,
    recipient_email VARCHAR(100) NOT NULL,
    parent_id INT NOT NULL,
    learner_id INT,
    subject VARCHAR(255) NOT NULL,
    body TEXT NOT NULL,
    status ENUM('Pending', 'Sent', 'Failed') DEFAULT 'Pending',
    sent_time DATETIME,
    delivery_status VARCHAR(50),
    external_message_id VARCHAR(100),
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_notif_parent FOREIGN KEY (parent_id) REFERENCES parents(parent_id) ON DELETE CASCADE,
    CONSTRAINT fk_notif_learner FOREIGN KEY (learner_id) REFERENCES learners(learner_id) ON DELETE SET NULL
);

-- 14. Settings Table (Master config)
CREATE TABLE IF NOT EXISTS settings (
    setting_id INT AUTO_INCREMENT PRIMARY KEY,
    setting_name VARCHAR(100) NOT NULL UNIQUE,
    setting_type VARCHAR(50) NOT NULL,
    setting_value VARCHAR(255) NOT NULL,
    priority INT DEFAULT 0,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_by INT,
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 15. Activity Log (Audit trailing)
CREATE TABLE IF NOT EXISTS activity_log (
    log_id BIGINT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    action VARCHAR(100) NOT NULL,
    entity_type VARCHAR(50) NOT NULL,
    entity_id INT NOT NULL,
    details JSON,
    ip_address VARCHAR(45),
    created_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_user FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE RESTRICT
);

-- Create specific Indexes for Performance
CREATE INDEX idx_sessions_time ON sessions(session_date, start_time, end_time);
CREATE INDEX idx_learner_pkg_status ON learner_packages(status);
CREATE INDEX idx_feedback_sla ON feedback(is_on_time);
CREATE INDEX idx_activity_log_entity ON activity_log(entity_type, entity_id);
