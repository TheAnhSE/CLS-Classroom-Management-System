# Nguyên Tắc & Kiến Trúc Phần Mềm (Software Architecture Guidelines)

Tài liệu này định dạng mô hình kiến trúc cốt lõi, quy hoạch các layers và luồng giao tiếp dữ liệu cho Backend hệ thống CLS (Classroom Management System).

## 1. Kiến Trúc Cốt Lõi: N-Tier Architecture (Monolith)
Để đảm bảo sự tinh gọn và đẩy nhanh quá trình phát triển MVP, hệ thống CLS Backend được xây dựng dưới dạng **một project Web API duy nhất** (`CLS.BackendAPI`). 
Kiến trúc bên trong project sử dụng mô hình **N-Tier / MVC cơ bản** với sự phân tách rõ ràng qua cấu trúc thư mục.

### 1.1 Cấu trúc thư mục (Layers)
Toàn bộ code được tổ chức trong các thư mục chính:

- **`Controllers` (Presentation Layer)**
  - Chứa các API Endpoints.
  - Nhậm Request, điều phối sang tầng Service và trả kết quả (Response) về cho người dùng.
  - Tuyệt đối **không** chứa Business Logic hoặc gọi trực tiếp Database.
  
- **`Services` (Business Logic Layer)**
  - Chứa toàn bộ logic nghiệp vụ của hệ thống (ví dụ: `LearnerService`, `AcademicService`).
  - Là cầu nối giữa Controller và Data.
  - Thực hiện các phép tính toán, validation nghiệp vụ phức tạp trước khi yêu cầu lưu trữ dữ liệu.
  
- **`Models` (Domain & DTOs Layer)**
  - Chứa các class biểu diễn dữ liệu.
  - Bao gồm:
    - **Entities**: Các lớp ánh xạ trực tiếp với bảng trong Database (ví dụ: `Learner`, `LearningPackage`).
    - **DTOs (Data Transfer Objects)**: Các lớp dùng để giao tiếp với Client (ví dụ: `CreateLearnerRequest`, `LearnerResponse`).
    - **Enums**: Danh mục trạng thái tĩnh (ví dụ: `LearnerStatus`).
    
- **`Data` (Data Access Layer)**
  - Nơi duy nhất chịu trách nhiệm giao tiếp trực tiếp với Cơ sở dữ liệu thông qua Entity Framework Core.
  - Chứa `ApplicationDbContext` (cấu hình DbSets và Fluent API).
  - Có thể chứa pattern `Repositories` nếu việc truy vấn quá phức tạp, nếu không, Services có thể tiêm (inject) `ApplicationDbContext` trực tiếp để gọi lưu trữ.

### 1.2 Luồng dữ liệu (Data Flow)
**Client** -> **Controller** (Nhận DTO Request) -> **Service** (Xử lý Logic & gọi Data) -> **Data (DbContext)** (Truy vấn / Lưu DB) -> **Database**

1. Khách hàng gửi yêu cầu tới Endpoints trong `Controllers`.
2. `Controller` validate Input cơ bản (kiểu dữ liệu, required) và chuyển DTO xuống `Service`.
3. `Service` xử lý logic. Nếu cần đọc/ghi dữ liệu, `Service` gọi `ApplicationDbContext` ở gói `Data`.
4. `Data` cập nhật thông tin trong CSDL.
5. `Service` lấy kết quả, chuyển đổi thành DTO Response và trả về cho `Controller`.
6. `Controller` trả HTTP Status và dữ liệu về Client.

## 2. Dependency Injection & Cấu hình (IoC Rules)
- Tận dụng container DI mặc định của ASP.NET Core (`builder.Services`).
- Mọi dependency (Ví dụ: `ILearnerService`, `ApplicationDbContext`) phải được đăng ký trong `Program.cs`.
- Tầng `Controllers` tiêm logic qua Interface (VD: `ILearnerService`). Không new các class một cách lỏng lẻo.

## 3. Global Exception Handling
- Hệ thống sử dụng một Middleware toàn cục (`GlobalExceptionHandlingMiddleware`) để gom và chuẩn hóa tất cả các exceptions.
- **Không** sử dụng Try/Catch rải rác trong `Controllers`. Khi có lỗi nghiệp vụ cần chặn người dùng, hãy dùng `throw new CustomException(...)` (VD: `NotFoundException`, `ConflictException`) bên trong `Services`, hệ thống Middleware sẽ tự catch và format response.

## 4. Database & Transactions
- Sử dụng Entity Framework Core.
- Đối với các thao tác Update/Create phức tạp đòi hỏi ACID, hãy đảm bảo chỉ gọi `await _dbContext.SaveChangesAsync()` ở bước cuối cùng sau khi mọi lệnh thay đổi đã được add vào context.
