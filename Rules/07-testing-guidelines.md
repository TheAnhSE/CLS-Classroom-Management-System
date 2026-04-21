# Quy Chuẩn Kiểm Thử Kỹ Thuật (Testing Guidelines)

Đảm bảo chất lượng mã nguồn là giai đoạn không thể thiếu trước khi tích hợp vào nhánh chính. Mọi tính năng nghiệp vụ cốt lõi phải được test tự động để tránh hồi quy lỗi (Regression Bugs).

## 1. Unit Testing (Kiểm thử mức đơn vị)
- **Công cụ**: Sử dụng `xUnit` kết hợp `Moq` (hoặc `NSubstitute`) cho Backend (.NET). Dùng `Vitest` hoặc `Jest` kết hợp `React Testing Library` cho Frontend.
- **Tiêu chuẩn bắt buộc (Coverage)**:
  - **Tầng Domain (Backend)**: Bắt buộc Coverage > 80%. Bất kỳ hàm tính toán học phí, kiểm tra logic SLA, thuật toán Validation conflict nào cũng phải có 100% case chạy qua Unit Test.
  - **Tầng Application / Use Cases**: Phải có Test để đảm bảo gọi đúng Dependencies (Verifying interaction).
- **Nguyên tắc "AAA"**: Mọi Unit test phải viết rõ 3 phần:
  - **Arrange**: Chuẩn bị dữ liệu Mock.
  - **Act**: Gọi phương thức cần test.
  - **Assert**: Khẳng định kết quả đầu ra khớp với kỳ vọng.
- **Tính Độc Lập**: Unit Test tuyệt đối không gọi vào Database thật, ổ cứng hay File System. Nếu có dính I/O, tức là viết sai cấu trúc, hãy tách ra Integration Test.

## 2. Integration Testing (Kiểm thử tích hợp)
- Dùng để test sự phối hợp giữa Service và Database.
- **Công cụ**: Sử dụng **Testcontainers** để sinh Database tạm thời chạy qua Docker, hoặc dùng **WebApplicationFactory** trong ASP.NET Core để gọi full luồng API (Request -> Controller -> Db -> Response) trong RAM.
- **Mục tiêu**: Kiểm tra xem viết truy vấn EF Core (`.Where()`, `.Include()`) có sinh lỗi SQL hay không trước khi đẩy code.

## 3. Quy Tắc Naming Unit Test
Tên của hàm Test phải vô cùng rõ ràng, theo định dạng: 
`[MethodName]_[StateUnderTest]_[ExpectedBehavior]`
- *Ví dụ đúng*: `CreateSession_WithOverlappingTeacher_ThrowsSchedulingConflictException`
- *Ví dụ đúng*: `CalculateSla_FeedbackSubmittedWithin12h_ReturnsTrue`

## 4. End-to-End (E2E) Testing (UI)
- Đối với luồng chạy cốt lõi (Happy Path) trên Web Dashboard (Ví dụ: Thao tác click menu, điền form Tạo Schedule và Save), nên bổ sung bộ test E2E.
- **Công cụ**: Sử dụng **Playwright** hoặc **Cypress**.
- Chú ý: E2E test dễ gãy vỡ (Flaky) khi UI thay đổi, chỉ nên áp dụng vài luồng quan trọng nhất để cảnh báo Dev nếu màn hình chính bị hỏng cục bộ (trắng trang).
