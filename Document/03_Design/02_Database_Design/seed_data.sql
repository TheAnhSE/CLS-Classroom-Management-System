-- ============================================================
-- CLS - Classroom Management System
-- SEED DATA SCRIPT (Tổng hợp - Đã kiểm chứng)
-- Ngày tạo : 24/04/2026
-- Mục đích  : Khởi tạo dữ liệu mẫu để test Frontend & Backend
-- Lưu ý    : Chạy trên database cls_db đã tạo bằng CLS_Database_Schema.sql
-- ============================================================

USE cls_db;
GO

-- ============================================================
-- BƯỚC 1: TẮT TOÀN BỘ RÀNG BUỘC FK
-- Mục đích: Cho phép xóa dữ liệu tự do không bị khóa ngoại chặn
-- ============================================================
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- ============================================================
-- BƯỚC 2: XÓA SẠCH DỮ LIỆU TẤT CẢ BẢNG
-- Thứ tự: Bảng con (có FK) → Bảng cha (không FK)
-- ============================================================
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

-- ============================================================
-- BƯỚC 3: RESET IDENTITY VỀ 0
-- Đảm bảo các bảng auto-increment bắt đầu lại từ 1
-- ============================================================
DBCC CHECKIDENT ('roles', RESEED, 0);
DBCC CHECKIDENT ('users', RESEED, 0);
DBCC CHECKIDENT ('parents', RESEED, 0);
DBCC CHECKIDENT ('learners', RESEED, 0);
DBCC CHECKIDENT ('subjects', RESEED, 0);
DBCC CHECKIDENT ('learning_packages', RESEED, 0);
DBCC CHECKIDENT ('classrooms', RESEED, 0);
DBCC CHECKIDENT ('sessions', RESEED, 0);
GO

-- ============================================================
-- BƯỚC 4: BẬT LẠI RÀNG BUỘC FK
-- ============================================================
EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

-- ============================================================
-- BƯỚC 5: THÊM DỮ LIỆU THEO THỨ TỰ CHA → CON
-- ============================================================

-- ────────────────────────────────────────────────
-- 5.1 ROLES
-- Giá trị hợp lệ: Admin, Teacher, Director
-- ────────────────────────────────────────────────
SET IDENTITY_INSERT roles ON;
INSERT INTO roles (role_id, role_name, description, is_active) VALUES
(1, 'Admin',    'Quản trị hệ thống',        1),
(2, 'Teacher',  'Giảng viên / Giáo viên',    1),
(3, 'Director', 'Ban Giám đốc trung tâm',   1);
SET IDENTITY_INSERT roles OFF;
GO

-- ────────────────────────────────────────────────
-- 5.2 USERS
-- Phụ thuộc: roles (role_id)
-- Mật khẩu: Lưu plain-text (MVP). AuthService dùng so sánh trực tiếp.
-- ────────────────────────────────────────────────
SET IDENTITY_INSERT users ON;
INSERT INTO users (user_id, email, password_hash, first_name, last_name, phone_number, role_id, is_active, created_time) VALUES
(1, 'admin@cls.edu.vn',     '123456', N'Giáo Vụ',   N'Nguyễn', '0900000001', 1, 1, GETDATE()),
(2, 'gv.a@cls.edu.vn',      '123456', N'Văn A',     N'Nguyễn', '0900000002', 2, 1, GETDATE()),
(3, 'gv.b@cls.edu.vn',      '123456', N'Thị B',     N'Trần',   '0900000003', 2, 1, GETDATE()),
(4, 'gv.c@cls.edu.vn',      '123456', N'Văn C',     N'Lê',     '0900000004', 2, 1, GETDATE()),
(5, 'director@cls.edu.vn',  '123456', N'Minh Đức',  N'Phạm',   '0900000005', 3, 1, GETDATE());
SET IDENTITY_INSERT users OFF;
GO

-- ────────────────────────────────────────────────
-- 5.3 PARENTS
-- Không phụ thuộc bảng nào
-- ────────────────────────────────────────────────
SET IDENTITY_INSERT parents ON;
INSERT INTO parents (parent_id, full_name, phone_number, email, address, relationship, created_time) VALUES
(1,  N'Nguyễn Thị Lan',   '0901234567', 'lan.nguyen@gmail.com',    N'123 Lê Lợi, Q1, TP.HCM',             'Mother', GETDATE()),
(2,  N'Trần Văn Hải',     '0912345678', 'hai.tran@gmail.com',      N'45 Võ Văn Tần, Q3, TP.HCM',           'Father', GETDATE()),
(3,  N'Lê Thị Mai',       '0923456789', 'mai.le@yahoo.com',        N'67 Nguyễn Trãi, Q5, TP.HCM',          'Mother', GETDATE()),
(4,  N'Phạm Văn Dũng',    '0934567890', 'dung.pham@outlook.com',   N'89 Điện Biên Phủ, Bình Thạnh',        'Father', GETDATE()),
(5,  N'Võ Thị Hạnh',      '0945678901', 'hanh.vo@gmail.com',       N'12 Nguyễn Thị Minh Khai, Q1',         'Mother', GETDATE()),
(6,  N'Hoàng Văn Tùng',   '0956789012', 'tung.hoang@gmail.com',    N'34 Trần Hưng Đạo, Q5, TP.HCM',       'Father', GETDATE()),
(7,  N'Đỗ Thị Hồng',     '0967890123', 'hong.do@gmail.com',       N'56 Cách Mạng Tháng 8, Q10, TP.HCM',  'Mother', GETDATE()),
(8,  N'Bùi Văn Thành',    '0978901234', 'thanh.bui@outlook.com',   N'78 Nguyễn Đình Chiểu, Q3, TP.HCM',   'Father', GETDATE()),
(9,  N'Ngô Thị Yến',      '0989012345', 'yen.ngo@gmail.com',       N'90 Hai Bà Trưng, Q1, TP.HCM',        'Mother', GETDATE()),
(10, N'Dương Văn Phúc',   '0990123456', 'phuc.duong@gmail.com',    N'11 Pasteur, Q3, TP.HCM',              'Father', GETDATE());
SET IDENTITY_INSERT parents OFF;
GO

-- ────────────────────────────────────────────────
-- 5.4 LEARNERS (20 học viên)
-- Phụ thuộc: parents (parent_id), users (created_by)
-- CHECK constraint trên status: 'Active' | 'Inactive' | 'Suspended'
-- CHECK constraint trên gender: 'Male' | 'Female' | 'Other'
-- ────────────────────────────────────────────────
SET IDENTITY_INSERT learners ON;
INSERT INTO learners (learner_id, first_name, last_name, date_of_birth, gender, enrollment_date, status, parent_id, created_by, created_time) VALUES
( 1, N'Minh Anh',     N'Nguyễn', '2015-05-12', 'Female', '2023-01-10', 'Active',    1,  1, GETDATE()),
( 2, N'Tuấn Kiệt',    N'Trần',   '2014-08-20', 'Male',   '2023-02-15', 'Active',    2,  1, GETDATE()),
( 3, N'Phương Linh',  N'Lê',     '2016-01-05', 'Female', '2023-03-20', 'Active',    3,  1, GETDATE()),
( 4, N'Hữu Nghĩa',   N'Phạm',   '2013-11-30', 'Male',   '2023-04-10', 'Suspended', 4,  1, GETDATE()),
( 5, N'Thanh Trúc',   N'Võ',     '2015-07-25', 'Female', '2023-05-12', 'Active',    5,  1, GETDATE()),
( 6, N'Gia Bảo',      N'Nguyễn', '2014-09-15', 'Male',   '2023-06-01', 'Active',    1,  1, GETDATE()),
( 7, N'Bảo Ngọc',     N'Trần',   '2016-03-18', 'Female', '2023-07-05', 'Active',    2,  1, GETDATE()),
( 8, N'Nhật Minh',    N'Lê',     '2013-12-05', 'Male',   '2023-08-10', 'Inactive',  3,  1, GETDATE()),
( 9, N'Khánh Thi',    N'Phạm',   '2015-02-28', 'Female', '2023-09-15', 'Active',    4,  1, GETDATE()),
(10, N'Quốc Cường',   N'Võ',     '2014-10-10', 'Male',   '2023-10-20', 'Active',    5,  1, GETDATE()),
(11, N'Hoàng Bách',   N'Hoàng',  '2015-04-22', 'Male',   '2023-11-05', 'Active',    6,  1, GETDATE()),
(12, N'Tú Anh',       N'Đỗ',     '2016-08-11', 'Female', '2023-12-12', 'Suspended', 7,  1, GETDATE()),
(13, N'Gia Khang',    N'Bùi',    '2014-01-30', 'Male',   '2024-01-08', 'Active',    8,  1, GETDATE()),
(14, N'Bảo Hân',      N'Ngô',    '2015-06-15', 'Female', '2024-02-14', 'Active',    9,  1, GETDATE()),
(15, N'Thiên Ân',     N'Dương',  '2016-09-25', 'Male',   '2024-03-01', 'Inactive',  10, 1, GETDATE()),
(16, N'Ngọc Diệp',   N'Nguyễn', '2013-05-05', 'Female', '2024-04-10', 'Active',    1,  1, GETDATE()),
(17, N'Đăng Khoa',    N'Trần',   '2014-11-18', 'Male',   '2024-05-15', 'Active',    2,  1, GETDATE()),
(18, N'Trâm Anh',     N'Lê',     '2015-12-20', 'Female', '2024-06-20', 'Active',    3,  1, GETDATE()),
(19, N'Gia Huy',      N'Phạm',   '2016-04-12', 'Male',   '2024-07-25', 'Suspended', 4,  1, GETDATE()),
(20, N'Minh Thư',     N'Võ',     '2013-08-08', 'Female', '2024-08-30', 'Active',    5,  1, GETDATE());
SET IDENTITY_INSERT learners OFF;
GO

-- ────────────────────────────────────────────────
-- 5.5 SUBJECTS
-- ────────────────────────────────────────────────
SET IDENTITY_INSERT subjects ON;
INSERT INTO subjects (subject_id, subject_name, description, is_active) VALUES
(1, N'Tiếng Anh',      N'Tiếng Anh giao tiếp & IELTS',      1),
(2, N'Kids Coding',    N'Lập trình Scratch & Python cho trẻ', 1),
(3, N'Toán tư duy',    N'Toán tư duy & giải quyết vấn đề',   1);
SET IDENTITY_INSERT subjects OFF;
GO

-- ────────────────────────────────────────────────
-- 5.6 LEARNING PACKAGES
-- Phụ thuộc: subjects (subject_id), users (created_by)
-- ────────────────────────────────────────────────
SET IDENTITY_INSERT learning_packages ON;
INSERT INTO learning_packages (package_id, package_name, description, subject_id, total_sessions, duration_months, tuition_fee, is_active, created_by, created_time) VALUES
(1, N'English Pro 40',        N'Khóa tiếng Anh giao tiếp 40 buổi',      1, 40, 3, 5000000,  1, 1, GETDATE()),
(2, N'IELTS Intensive 60',    N'Luyện thi IELTS chuyên sâu 60 buổi',    1, 60, 4, 8500000,  1, 1, GETDATE()),
(3, N'Coding Starter 20',     N'Lập trình Scratch cơ bản 20 buổi',      2, 20, 2, 3000000,  1, 1, GETDATE()),
(4, N'Coding Pro 30',         N'Python nâng cao cho Kids 30 buổi',       2, 30, 3, 4500000,  1, 1, GETDATE()),
(5, N'Toán Tư Duy 24',        N'Toán logic & sáng tạo 24 buổi',         3, 24, 2, 3500000,  1, 1, GETDATE());
SET IDENTITY_INSERT learning_packages OFF;
GO

-- ────────────────────────────────────────────────
-- 5.7 CLASSROOMS
-- ────────────────────────────────────────────────
SET IDENTITY_INSERT classrooms ON;
INSERT INTO classrooms (classroom_id, room_name, location, capacity, is_active) VALUES
(1, N'Phòng 101', N'Tầng 1 - Tòa A', 20, 1),
(2, N'Phòng 102', N'Tầng 1 - Tòa A', 15, 1),
(3, N'Phòng 201', N'Tầng 2 - Tòa A', 25, 1),
(4, N'Phòng 202', N'Tầng 2 - Tòa A', 20, 1),
(5, N'Lab 301',   N'Tầng 3 - Tòa B', 30, 1);
SET IDENTITY_INSERT classrooms OFF;
GO

-- ────────────────────────────────────────────────
-- 5.8 SESSIONS (Lịch học mẫu - tuần hiện tại)
-- Phụ thuộc: subjects, classrooms, users (teacher + created_by)
-- CHECK constraint trên status: Kiểm tra giá trị hợp lệ nếu có
-- ────────────────────────────────────────────────
SET IDENTITY_INSERT sessions ON;
INSERT INTO sessions (session_id, subject_id, classroom_id, teacher_id, session_date, start_time, end_time, status, created_by, created_time, notes) VALUES
-- Hôm nay
(1, 1, 1, 2, CAST(GETDATE() AS DATE), '08:00:00', '09:30:00', 'Scheduled', 1, GETDATE(), N'Tiếng Anh - Lớp A1'),
(2, 2, 5, 3, CAST(GETDATE() AS DATE), '09:45:00', '11:15:00', 'Scheduled', 1, GETDATE(), N'Kids Coding - Python cơ bản'),
(3, 1, 3, 4, CAST(GETDATE() AS DATE), '14:00:00', '15:30:00', 'Scheduled', 1, GETDATE(), N'IELTS Prep'),
(4, 1, 1, 2, CAST(GETDATE() AS DATE), '15:45:00', '17:15:00', 'Scheduled', 1, GETDATE(), N'Tiếng Anh - Giao tiếp B2'),
-- Ngày mai
(5, 3, 2, 4, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '08:00:00', '09:30:00', 'Scheduled', 1, GETDATE(), N'Toán tư duy - Lớp T1'),
(6, 2, 5, 3, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), '09:45:00', '11:15:00', 'Scheduled', 1, GETDATE(), N'Scratch nâng cao');
SET IDENTITY_INSERT sessions OFF;
GO

-- ────────────────────────────────────────────────
-- 5.9 LEARNER_PACKAGES (Gắn gói học phí cho học viên)
-- Phụ thuộc: learners, learning_packages, users (assigned_by)
-- ────────────────────────────────────────────────
INSERT INTO learner_packages (learner_id, package_id, total_sessions, remaining_sessions, assigned_date, expiry_date, status, assigned_by, created_time) VALUES
-- Học viên dùng English Pro 40
( 1, 1, 40,  2, '2023-01-10', '2025-06-30', 'Active', 1, GETDATE()),  -- Minh Anh: sắp hết
( 6, 1, 40, 28, '2023-06-01', '2025-12-31', 'Active', 1, GETDATE()),  -- Gia Bảo: còn nhiều
(16, 1, 40, 35, '2024-04-10', '2026-07-31', 'Active', 1, GETDATE()),  -- Ngọc Diệp: mới bắt đầu

-- Học viên dùng IELTS Intensive 60
( 3, 2, 60, 48, '2023-03-20', '2026-03-31', 'Active', 1, GETDATE()),  -- Phương Linh
(10, 2, 60, 15, '2023-10-20', '2025-10-31', 'Active', 1, GETDATE()),  -- Quốc Cường: sắp hết

-- Học viên dùng Coding Starter 20
( 2, 3, 20, 13, '2023-02-15', '2025-08-31', 'Active', 1, GETDATE()),  -- Tuấn Kiệt
( 7, 3, 20, 18, '2023-07-05', '2025-12-31', 'Active', 1, GETDATE()),  -- Bảo Ngọc: mới bắt đầu

-- Học viên dùng Coding Pro 30
( 5, 4, 30, 12, '2023-05-12', '2025-11-30', 'Active', 1, GETDATE()),  -- Thanh Trúc
(11, 4, 30, 25, '2023-11-05', '2026-05-31', 'Active', 1, GETDATE()),  -- Hoàng Bách

-- Học viên dùng Toán Tư Duy 24
( 9, 5, 24, 20, '2023-09-15', '2025-09-30', 'Active', 1, GETDATE()),  -- Khánh Thi
(13, 5, 24, 22, '2024-01-08', '2026-01-31', 'Active', 1, GETDATE()),  -- Gia Khang

-- Gói hết hạn / inactive
( 4, 1, 40,  0, '2023-04-10', '2024-07-31', 'Expired', 1, GETDATE()), -- Hữu Nghĩa: hết gói
(17, 3, 20,  3, '2024-05-15', '2025-05-31', 'Active',  1, GETDATE()); -- Đăng Khoa: sắp hết
GO

-- ────────────────────────────────────────────────
-- 5.10 SESSION_LEARNERS (Gắn học viên vào buổi học)
-- Phụ thuộc: sessions, learners
-- ────────────────────────────────────────────────
INSERT INTO session_learners (session_id, learner_id) VALUES
-- Session 1: Tiếng Anh A1
(1, 1), (1, 6), (1, 16),
-- Session 2: Kids Coding
(2, 2), (2, 7),
-- Session 3: IELTS
(3, 3), (3, 10),
-- Session 4: Giao tiếp B2
(4, 5), (4, 11),
-- Session 5: Toán tư duy (ngày mai)
(5, 9), (5, 13),
-- Session 6: Scratch nâng cao (ngày mai)
(6, 2), (6, 7);
GO

-- ============================================================
-- KIỂM TRA KẾT QUẢ
-- ============================================================
PRINT '============================================';
PRINT '  CLS SEED DATA - HOÀN TẤT!';
PRINT '============================================';
PRINT '';

SELECT 'roles'             AS [Table], COUNT(*) AS [Records] FROM roles             UNION ALL
SELECT 'users'             AS [Table], COUNT(*) AS [Records] FROM users             UNION ALL
SELECT 'parents'           AS [Table], COUNT(*) AS [Records] FROM parents           UNION ALL
SELECT 'learners'          AS [Table], COUNT(*) AS [Records] FROM learners          UNION ALL
SELECT 'subjects'          AS [Table], COUNT(*) AS [Records] FROM subjects          UNION ALL
SELECT 'learning_packages' AS [Table], COUNT(*) AS [Records] FROM learning_packages UNION ALL
SELECT 'classrooms'        AS [Table], COUNT(*) AS [Records] FROM classrooms        UNION ALL
SELECT 'sessions'          AS [Table], COUNT(*) AS [Records] FROM sessions          UNION ALL
SELECT 'learner_packages'  AS [Table], COUNT(*) AS [Records] FROM learner_packages  UNION ALL
SELECT 'session_learners'  AS [Table], COUNT(*) AS [Records] FROM session_learners
ORDER BY [Table];

PRINT '';
PRINT 'Tài khoản test:';
PRINT '  Admin   : admin@cls.edu.vn    / 123456';
PRINT '  GV A    : gv.a@cls.edu.vn     / 123456';
PRINT '  GV B    : gv.b@cls.edu.vn     / 123456';
PRINT '  GV C    : gv.c@cls.edu.vn     / 123456';
PRINT '  Director: director@cls.edu.vn / 123456';
