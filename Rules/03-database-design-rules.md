# Tiêu Chuẩn Thiết Kế Cơ Sở Dữ Liệu (Database Design Rules)

Tài liệu này xác định các quy tắc áp dụng cho Database Layer (giả định dùng SQL Server / PostgreSQL). Nếu sử dụng Entity Framework Core, các Model/Entity Migration sinh ra cũng phải đáp ứng chuẩn DB này.

## 1. Naming Convention cho Schema và Tables
- **Tên Bảng (Table Names)**: Sử dụng danh từ số nhiều, định dạng PascalCase hoặc snake_case tuỳ engine, nhưng tại đây thống nhất sử dụng **PascalCase Số Nhiều**.
  - Ví dụ: `Learners`, `Sessions`, `LearningPackages`.
- **Tên Cột (Column Names)**: Thống nhất format **PascalCase**.
  - Ví dụ: `FirstName`, `DateOfBirth`, `RemainingSessions`.
- **Primary Keys (PK)**: Cột ID chính mặc định đặt tên là `Id`. Cấm đặt tên lặp lại rườm rà như `LearnerId` trong bảng `Learners`.
- **Foreign Keys (FK)**: Sử dụng định dạng `[EntityName]Id`. Ví dụ bảng `Sessions` trỏ tới `Teacher`, hãy đặt cột đó là `TeacherId`.
- **Liên kết nhiều-nhiều (Many-to-Many)**: Bảng nối (Junction Table) sử dụng tên ghép. VD: Bản nháp Session và Learner sẽ gọi là `SessionLearner`. (Trong C#, EF Core tự quản lý nhưng cấu trúc DB vật lý phải rõ ràng).

## 2. Tiêu chuẩn kiểu dữ liệu (Data Types)
- **Primary Keys**: Nên sử dụng kiểu `UNIQUEIDENTIFIER` (UUID) hoặc `BIGINT IDENTITY(1,1)` để bảo mật và chống Enumeration Attack (không cho Client đoán số lượng Rows dễ dàng). Nếu dùng GUID gen trên C# (v7), hãy chắc chắn lưu nó làm Clustered Index nếu không CSDL sẽ bị Fragmentation nghiêm trọng. Tối ưu nhất là dùng Int Identity đi kèm một cột public `Code / Slug` riêng.
- **Tiền tệ (Currency)**: Tuyệt đối KHÔNG DÙNG kiểu `Float`/`Real` vì sai số làm tròn dính tới kế toán học phí. Dùng `DECIMAL(18,2)` hoặc lưu `BIGINT` giá trị nhỏ nhất (ví dụ nhân lên x 100).
- **String Text**: 
  - Tên/Email/Title: Dùng `NVARCHAR(255)` hoặc `VARCHAR(Kích_thước_cụ_thể)`. Cấm dùng dạng MAX vô tội vạ. Tối ưu bộ đệm Index.
  - Đoạn text dài như Teacher Feedback: Dùng `NVARCHAR(MAX)` hoặc `TEXT`.

## 3. Ràng Buộc Dữ Liệu (Constraints & Indexes)
Để duy trì tính toàn vẹn và tối tốc độ tải:
- **Index**: Trươc khi Launch Production, thiết lập Index (`CREATE INDEX`) lên toàn bộ các Cột chuyên dùng cho logic tìm kiếm (`Email`, `Phone`) hoặc cột Filter/Order (`CreatedAt`, `DateOfBirth`). Foreign Keys phải bắt buộc dọn dẹp Index.
- **Unique Constraint**: Các trường bắt buộc tính duy nhất như Username, Phụ huynh Email, Mã số học viên thì phải kẹp Constraint UNIQUE vào DB để chặn triệt để chuyện Race condition sinh rác.
- **Nullability**: Khai báo chặt chẽ. Cột nào bắt buộc phải có giá trị thì trong C# Entity bắt buộc dùng cấu hình required/Non-nullable và map xuống DB là `NOT NULL`. Tránh nhồi Null lộn xộn.

## 4. Xóa Mềm (Soft Delete) và Tracking Lịch Sử
- Ở các bảng nghiệp vụ quan trọng như Học viên, Giáo viên, Tài khoản, Tuyệt đối KHÔNG chạy câu lệnh DELETE FROM vật lý trừ phi GDPR xoá dữ liệu vĩnh viễn. 
- Mọi bảng cần cài đặt cờ **`IsDeleted`** (BIT/BOOLEAN) mặc định False. Việc Query hệ thống phải append clause `WHERE IsDeleted = 0`.
- **Auditing Columns**: Tất cả các bảng MasterData/Transaction cần gắn tối thiểu 4 cột bám dấu vết:
  - `CreatedAt` (DATETIME2)
  - `CreatedBy` (Gắn ID tài khoản hoặc SYSTEM)
  - `UpdatedAt` (DATETIME2, Nullable)
  - `UpdatedBy`
