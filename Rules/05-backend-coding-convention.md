# Quy Chuẩn Lập Trình Backend (C# / .NET 8+)

Tài liệu này xác định các quy tắc khắt khe nhằm đảm bảo mã nguồn Backend đạt chất lượng chuẩn Enterprise, duy trì tính dễ đọc, hiệu năng và khả năng bảo trì.

## 1. Naming Conventions (Quy chuẩn đặt tên)
Tuân thủ nghiêm ngặt chuẩn đặt tên của Microsoft.
- **PascalCase**: Class (`LearnerService`), Record (`UpdateLearnerCommand`), Interface (`ILearnerRepository`), Struct, Enum, Public Properties, Methods, Events.
- **camelCase**: Parameter trong methods (`string firstName`), biến local (`var totalCount = 0;`).
- **Tên Interfaces**: Bắt buộc bắt đầu bằng chữ `I` viết hoa. VD: `IHttpContextAccessor`.
- **Private Fields**: Bắt buộc bắt đầu bằng ký tự gạch dưới `_` + camelCase. VD: `private readonly IEmailService _emailService;`
- **Hằng số (Constants)**: Sử dụng PascalCase cho `public const` (VD: `public const int MaxPageSize = 50;`). Các trường `private const` cũng sử dụng PascalCase.

## 2. SOLID, DRY & YAGNI Principles
Mọi dòng code viết ra bắt buộc phải bám sát các nguyên lý:
- **Single Responsibility (SRP)**: Một lớp (Class) hoặc phương thức (Method) chỉ phục vụ một mục đích duy nhất. Controller chỉ nhận Request và trả Response. Tách ngay logic thành các Application Service hoặc Domain Service nếu thấy phương thức dài quá 150 dòng hoặc class có quá 5 dependencies trong Constructor.
- **Open/Closed (OCP)**: Mã nguồn phải đóng với việc sửa chữa (hạn chế đi vào method cũ sửa code) và mở đối với việc mở rộng (sử dụng Strategy Pattern, Polymorphism để add behavior mới).
- **Dependency Inversion (DIP)**: Không bao giờ được khởi tạo (`new()`) trực tiếp một Class làm nhiêm vụ nghiệp vụ (ngoại trừ Entity, DTO, Value Object). Mọi tương tác Service, Repo đều phải inject qua Interface trong constructor.
- **DRY (Don't Repeat Yourself)**: Gom nhóm logic trùng lặp vào Extension Methods hoặc Base Classes. Nhưng **không DRY sai cách**, không gộp logic của hai Module hoàn toàn khác nhau chỉ vì chúng "trông giống nhau" (tránh Coupling sai lầm).
- **YAGNI (You Aren't Gonna Need It)**: KHÔNG viết trước các tính năng, interface trừu tượng rườm rà nếu MVP không cần dùng đến.

## 3. Data Transfer Objects (DTO) và Mapping
- **An toàn Dữ liệu**: Domain Entities (ví dụ: `Learner`) mang thông tin Database TUYỆT ĐỐI KHÔNG ĐƯỢC trả về trực tiếp bởi Controller.
- Tất cả request từ User đi vào Controller phải được biểu diễn qua `RequestDTO` hoặc `Command`/`Query` object.
- Tất cả payload trả ra phải biểu diễn qua `ResponseDTO`.
- Khuyên dùng `C# 9+ record` cho các DTO để bảo toàn tính bất biến (Immutable):
  ```csharp
  public record LearnerResponseDto(Guid Id, string FullName, int RemainingSessions);
  ```
- **Mapping**: Khuyến khích sử dụng `Mapster` (hoặc viết AutoMapper profile) để convert giữa DTO và Entity.

## 4. Error Handling & Global Exceptions Middleware
Tuyệt đối cấm lập trình viên rải `try/catch` lộn xộn trong Controller hoặc Application Service chỉ để return HTTP `400` hay `500`.

- **Domain Exceptions**: Tạo các class exception tuỳ chỉnh mang Business semantic (VD: `NotFoundException`, `SchedulingConflictException`, `SlaViolationException`).
- **Ném Exception từ Core**: Lớp Domain/Application cứ chủ động ném exception nếu vi phạm business rule.
  ```csharp
  if (room.Capacity < request.LearnersCount) 
      throw new RoomCapacityExceededException("Sĩ số vượt mức dung lượng phòng.");
  ```
- **Xử lý tập trung**: Dùng một `GlobalExceptionHandlingMiddleware` cấu hình ở tầng API (Program.cs) để catch mọi exception ném ra. Map tự động `NotFoundException => 404`, `ConflictException => 409`, `ValidationException => 400`, mọi Exception không được handle sẽ văng `500 Server Error` và bị giấu message thật để ngừa lộ bảo mật.

## 5. Asynchronous Programming (Bất đồng bộ)
- Cấm sử dụng `.Wait()`, `.Result` trên kiểu `Task` hay `Task<T>`. Làm như vậy sẽ gây "Thread Starvation" và "Deadlock".
- 100% các hoạt động I/O DB, File System, Network Calls phải dùng `async` và `await`.
- Khai báo name convention `.Async` ở cuối: `public async Task<Learner> GetByIdAsync(Guid id, CancellationToken cancellationToken)`
- **Bắt buộc truyền CancellationToken** dọc theo chuỗi gọi hàm để hủy quá trình query database sớm nếu Client disconnect sớm.

## 6. Entity Framework Core Standards
- **Chống N+1 Query**: Dùng `.Include()` cẩn thận, ưu tiên dùng Projection (`.Select()`) sớm nhất có thể.
- **AsNoTracking**: Mọi Query dùng cho việc ĐỌC (không chỉnh sửa Data state) CẦN PHẢI GẮN `.AsNoTracking()` để tiết kiệm hàng chục MB RAM và cải thiện tốc độ EF Core đáng kể.
- **Không thực hiện Data logic ở Client**: Lọc, đếm, tính toán phải thực hiện dưới CSDL (`.Where(...)` trước khi `.ToList()`), tuyệt đối không `.ToList()` bảng 100,000 dòng lên RAM backend rồi mới lọc.
