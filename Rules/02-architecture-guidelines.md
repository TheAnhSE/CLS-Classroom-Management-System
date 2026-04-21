# Nguyển Tắc & Kiến Trúc Phần Mềm (Software Architecture Guidelines)

Tài liệu này định dạng các mô hình kiến trúc cốt lõi, quy hoạch các layers và luồng giao tiếp dữ liệu cho toàn bộ hệ thống CLS (Classroom Management System).

## 1. Kiến Trúc Cốt Lõi: Modular Monolith
Hệ thống sử dụng **Modular Monolith Architecture**. So với Microservices, Modular Monolith giúp đơn giản hóa việc quản lý mã nguồn, triển khai (deployment) và giao dịch cơ sở dữ liệu (Database Transactions) ở giai đoạn MVP trong khi vẫn giữ nguyên tính độc lập của các nghiệp vụ, giúp dễ dàng chia tách thành Microservices trong tương lai.

### 1.1 Phân chia Module (Logical Boundaries)
Hệ thống được chia thành các Bounded Contexts (Modules) độc lập tương ứng với nghiệp vụ:
- **`Module.Identity`**: Quản lý Xác thực (Authentication), Phân quyền (Authorization), Quản lý tải khoản và OTP.
- **`Module.Learner`**: Quản lý vòng đời học viên, thông tin nhân trắc, thông tin phụ huynh, trạng thái (Active/Inactive).
- **`Module.Academic`**: Quản lý Gói học phí (Learning Package), Lên lịch học (Scheduling), Điểm danh (Attendance), Đánh giá (Feedback) và rà soát Xung đột (Conflict Detection).
- **`Module.Notification`**: Xử lý việc gửi định tuyến thông báo qua Email tới Phụ huynh (SendGrid/SMTP).

### 1.2 Nguyên tắc giao tiếp giữa các Module
- **Tuyệt đối không có Cross-Database Queries trực tiếp**: Module A không được JOIN trực tiếp vào Table của Module B ở Database layer.
- **Giao tiếp đồng bộ (Synchronous)**: Sử dụng các `public interface` (ví dụ `IUserContextService`) được bộc lộ bởi Module kia để lấy thông tin. Chỉ dùng khi cần dữ liệu hiển thị (Read).
- **Giao tiếp bất đồng bộ (Asynchronous)**: Sử dụng **In-Memory Event Bus (MediatR)**. Khi một Module thực hiện xong Command, nó sẽ publish một Domain Event (VD: `LearnerCreatedEvent`). Các Module khác sẽ Subscribe event này và tự động cập nhật Database của riêng nó hoặc thực hiện trigger logic khác. (VD: `Module.Notification` nghe event `AttendanceRecordedEvent` để gửi Email).

## 2. Kiến trúc Layer bên trong mỗi Module (Clean Architecture / Layered)
Bên trong mỗi Module (Vd: `Module.Academic`) bắt buộc phải tuân theo Clean Architecture với độ sâu 4 layers:

### 2.1 Web API Layer (Presentation)
- **Nhiệm vụ**: Expose RESTful Endpoints.
- **Thành phần**: `Controllers`, `Middlewares`, `Filters` (Exception/Validation), `Request/Response DTOs`.
- **Ràng buộc**: Tuyệt đối không chứa Business Logic, không truy cập DbContext. Controller cực mỏng (nhận DTO -> Validation -> đẩy Request vào MediatR -> Trả về Client).

### 2.2 Application Layer (Use Cases)
- **Nhiệm vụ**: Tổ chức các Use Cases của hệ thống.
- **Thành phần**: Áp dụng Pattern **CQRS** thông qua thư viện `MediatR` với 2 thư mục rõ ràng:
  - `Commands`: (Create/Update/Delete) Xử lý logic làm thay đổi state.
  - `Queries`: (Get/List) Xử lý logic query dữ liệu. Trực tiếp dùng Dapper/EF Core AsNoTracking để tối ưu tốc độ đọc.
- **Quy tắc**: Tầng này định nghĩa các Interface cho cơ sở hạ tầng (ví dụ: `IAcademicDbContext`, `IEmailService`). Nhưng nó không quan tâm bên dưới sử dụng SQL Server hay PostgreSQL.

### 2.3 Domain Layer (Core Enterprise Logic)
- **Nhiệm vụ**: Trái tim của hệ thống. Nơi chứa mọi quy tắc nghiệp vụ (Business Rules).
- **Thành phần**: `Entities`, `Value Objects`, `Domain Exceptions`, `Domain Events`.
- **Ràng buộc**: **Lớp này KHÔNG có bất kỳ dependency nào (Framework-agnostic)**. Không có Entity Framework, không có ASP.NET. Các class ở đây phải là các class C# nguyên thủy (POCOs). Các Entities tự kiểm soát trạng thái của mình qua encapsulation (private setters, update qua method xử lý).

### 2.4 Infrastructure Layer (Cơ sở hạ tầng)
- **Nhiệm vụ**: Implement các Interfaces định nghĩa ở Application. Tương tác với môi trường bên ngoài.
- **Thành phần**: `DbContext` (EF Core), `Repositories`, Cấu hình giao tiếp 3rd Party API (SendGrid, Twilio), và `FileStorage`.
- **Ràng buộc**: Lớp này là nơi duy nhất tham chiếu tới Database Drivers và External SDKs.

## 3. Dependency Injection & Cấu hình (IoC Rules)
- Mọi dependency (như services, repositories) phải được tiêm vào (inject) qua constructor. `(IServiceCollection)`
- Tuân thủ thiết kế **Extension Method** cho DI. VD: Tầng Infrastructure phải tự khai báo `public static IServiceCollection AddAcademicInfrastructure(...)` để Program.cs gọi, thay vì cấu hình chằng chịt trong Program.cs.

## 4. Quản trị Transactions (Unit of Work)
- Sử dụng EF Core. Các thao tác Command (Update/Create/Delete) phải được gom lại một Transaction (Unit of Work). Khi Command Handler làm xong mọi xử lý, gọi `await _dbContext.SaveChangesAsync()` ở bước cuối cùng, để đảm bảo tính nguyên vẹn dữ liệu (ACID).
