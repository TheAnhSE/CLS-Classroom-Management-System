USE cls_db;
GO

-- ========================================
-- BƯỚC 1: TẮT TOÀN BỘ RÀNG BUỘC FK
-- ========================================
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- ========================================
-- BƯỚC 2: XÓA SẠCH DỮ LIỆU TẤT CẢ BẢNG
-- ========================================
DELETE FROM session_learners;
DELETE FROM sessions;
DELETE FROM learner_packages;
DELETE FROM attendance;
DELETE FROM feedback;
DELETE FROM notifications;
DELETE FROM activity_log;
DELETE FROM learners;
DELETE FROM parents;
DELETE FROM learning_packages;
DELETE FROM subjects;
DELETE FROM classrooms;
DELETE FROM settings;
DELETE FROM users;
DELETE FROM roles;
GO

-- ========================================
-- BƯỚC 3: RESET IDENTITY VỀ 0
-- ========================================
DBCC CHECKIDENT ('roles', RESEED, 0);
DBCC CHECKIDENT ('users', RESEED, 0);
DBCC CHECKIDENT ('parents', RESEED, 0);
DBCC CHECKIDENT ('learners', RESEED, 0);
DBCC CHECKIDENT ('subjects', RESEED, 0);
DBCC CHECKIDENT ('learning_packages', RESEED, 0);
GO

-- ========================================
-- BƯỚC 4: BẬT LẠI RÀNG BUỘC FK
-- ========================================
EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

-- ========================================
-- BƯỚC 5: THÊM DỮ LIỆU THEO THỨ TỰ CHA → CON
-- ========================================

-- 5.1 Roles (Bảng gốc, không phụ thuộc ai)
SET IDENTITY_INSERT roles ON;
INSERT INTO roles (role_id, role_name, description, is_active) VALUES
(1, 'Admin', 'System Administrator', 1);
SET IDENTITY_INSERT roles OFF;
GO

-- 5.2 Users (Phụ thuộc Roles)
SET IDENTITY_INSERT users ON;
INSERT INTO users (user_id, email, password_hash, first_name, last_name, role_id, is_active, created_time) VALUES
(1, 'admin@cls.edu.vn', '$2a$11$FIfXU2HjAOhwB9LzLXX8z.hO9uU1Zp2Xb/0N5V8F7M9P5Y2v4T5S6', N'Giáo Vụ', N'Nguyễn', 1, 1, GETDATE());
SET IDENTITY_INSERT users OFF;
GO

-- 5.3 Parents (Không phụ thuộc ai)
SET IDENTITY_INSERT parents ON;
INSERT INTO parents (parent_id, full_name, phone_number, email, address, relationship, created_time) VALUES
(1, N'Nguyễn Thị Lan',   '0901234567', 'lan.nguyen@gmail.com',   N'123 Lê Lợi, Q1, HCM',            'Mother', GETDATE()),
(2, N'Trần Văn Hải',     '0912345678', 'hai.tran@gmail.com',     N'45 Võ Văn Tần, Q3, HCM',          'Father', GETDATE()),
(3, N'Lê Thị Mai',       '0923456789', 'mai.le@yahoo.com',       N'67 Nguyễn Trãi, Q5, HCM',         'Mother', GETDATE()),
(4, N'Phạm Văn Dũng',    '0934567890', 'dung.pham@outlook.com',  N'89 Điện Biên Phủ, Bình Thạnh',    'Father', GETDATE()),
(5, N'Võ Thị Hạnh',      '0945678901', 'hanh.vo@gmail.com',      N'12 Nguyễn Thị Minh Khai, Q1',     'Mother', GETDATE());
SET IDENTITY_INSERT parents OFF;
GO

-- 5.4 Learners (Phụ thuộc Parents + Users)
SET IDENTITY_INSERT learners ON;
INSERT INTO learners (learner_id, first_name, last_name, date_of_birth, gender, enrollment_date, status, parent_id, created_by, created_time) VALUES
( 1, N'Minh Anh',     N'Nguyễn', '2015-05-12', 'Female', '2023-01-10', 'Active',    1, 1, GETDATE()),
( 2, N'Tuấn Kiệt',    N'Trần',   '2014-08-20', 'Male',   '2023-02-15', 'Active',    2, 1, GETDATE()),
( 3, N'Phương Linh',  N'Lê',     '2016-01-05', 'Female', '2023-03-20', 'Active',    3, 1, GETDATE()),
( 4, N'Hữu Nghĩa',   N'Phạm',   '2013-11-30', 'Male',   '2023-04-10', 'Suspended', 4, 1, GETDATE()),
( 5, N'Thanh Trúc',   N'Võ',     '2015-07-25', 'Female', '2023-05-12', 'Active',    5, 1, GETDATE()),
( 6, N'Gia Bảo',      N'Nguyễn', '2014-09-15', 'Male',   '2023-06-01', 'Active',    1, 1, GETDATE()),
( 7, N'Bảo Ngọc',     N'Trần',   '2016-03-18', 'Female', '2023-07-05', 'Active',    2, 1, GETDATE()),
( 8, N'Nhật Minh',    N'Lê',     '2013-12-05', 'Male',   '2023-08-10', 'Inactive',  3, 1, GETDATE()),
( 9, N'Khánh Thi',    N'Phạm',   '2015-02-28', 'Female', '2023-09-15', 'Active',    4, 1, GETDATE()),
(10, N'Quốc Cường',   N'Võ',     '2014-10-10', 'Male',   '2023-10-20', 'Active',    5, 1, GETDATE()),
(11, N'Hoàng Bách',   N'Nguyễn', '2015-04-22', 'Male',   '2023-11-05', 'Active',    1, 1, GETDATE()),
(12, N'Tú Anh',       N'Trần',   '2016-08-11', 'Female', '2023-12-12', 'Suspended', 2, 1, GETDATE()),
(13, N'Gia Khang',    N'Lê',     '2014-01-30', 'Male',   '2024-01-08', 'Active',    3, 1, GETDATE()),
(14, N'Bảo Hân',      N'Phạm',   '2015-06-15', 'Female', '2024-02-14', 'Active',    4, 1, GETDATE()),
(15, N'Thiên Ân',     N'Võ',     '2016-09-25', 'Male',   '2024-03-01', 'Inactive',  5, 1, GETDATE()),
(16, N'Ngọc Diệp',   N'Nguyễn', '2013-05-05', 'Female', '2024-04-10', 'Active',    1, 1, GETDATE()),
(17, N'Đăng Khoa',    N'Trần',   '2014-11-18', 'Male',   '2024-05-15', 'Active',    2, 1, GETDATE()),
(18, N'Trâm Anh',     N'Lê',     '2015-12-20', 'Female', '2024-06-20', 'Active',    3, 1, GETDATE()),
(19, N'Gia Huy',      N'Phạm',   '2016-04-12', 'Male',   '2024-07-25', 'Suspended', 4, 1, GETDATE()),
(20, N'Minh Thư',     N'Võ',     '2013-08-08', 'Female', '2024-08-30', 'Active',    5, 1, GETDATE());
SET IDENTITY_INSERT learners OFF;
GO

-- 5.5 Subjects
SET IDENTITY_INSERT subjects ON;
INSERT INTO subjects (subject_id, subject_name, description, is_active) VALUES
(1, N'Tiếng Anh',   N'Tiếng Anh giao tiếp', 1),
(2, N'Kids Coding', N'Lập trình cho trẻ em', 1);
SET IDENTITY_INSERT subjects OFF;
GO

-- 5.6 Learning Packages (Phụ thuộc Subjects + Users)
SET IDENTITY_INSERT learning_packages ON;
INSERT INTO learning_packages (package_id, package_name, description, subject_id, total_sessions, duration_months, tuition_fee, is_active, created_by, created_time) VALUES
(1, 'English Pro',    N'Khóa tiếng Anh chuẩn', 1, 40, 3, 5000000, 1, 1, GETDATE()),
(2, 'Coding Starter', N'Lập trình cơ bản',     2, 20, 2, 3000000, 1, 1, GETDATE());
SET IDENTITY_INSERT learning_packages OFF;
GO

PRINT '=== SEED DATA HOÀN TẤT! ==='
PRINT 'Tài khoản: admin@cls.edu.vn'
PRINT 'Tổng: 1 Role, 1 User, 5 Parents, 20 Learners, 2 Subjects, 2 Packages'
