# AI Layered Context Rules & Architecture Toàn Diện (SDLC)

Thư mục `Rules/` đóng vai trò là "bộ não" trung tâm quy định cách thức thiết kế, triển khai và viết code cho toàn bộ dự án CLS. Hệ thống rules được phân lớp theo đúng **Chu Trình Sống Của Phát Triển Phần Mềm (Software Development Life Cycle - SDLC)**.

Bất kỳ lập trình viên hoặc trợ lý AI nào làm việc trong dự án bắt buộc phải đọc và tham chiếu đến cấu trúc này để đảm bảo tính nhất quán chuẩn Enterprise.

## Cấu trúc 9 Tầng Quy Tắc Mô Phỏng SDLC

1. **[01-business-context.md](./01-business-context.md)**: (Giai đoạn Lấy Yêu cầu) Chứa mục tiêu dự án, Core System Directives, Glossary (TỪ VỰNG NGHIỆP VỤ BẮT BUỘC) và danh sách tính năng In-scope.
2. **[02-architecture-guidelines.md](./02-architecture-guidelines.md)**: (Giai đoạn Thiết kế Kiến trúc) Định nghĩa chiến lược Modular Monolith, thiết kế Clean Architecture 4 lớp cho khối nghiệp vụ.
3. **[03-database-design-rules.md](./03-database-design-rules.md)**: (Giai đoạn Thiết kế DB) Hệ thống quy tắc thiết kế bảng (Schema), kiểu dữ liệu (Decimal, PK), Soft Delete và Auditing trước khi viết mã nguồn.
4. **[04-api-design-rules.md](./04-api-design-rules.md)**: (Giai đoạn Thiết kế API Contract) Giao kèo HTTP API giữa Frontend và Backend. Các Method, mã lỗi HTTP, format JSON, phân trang tĩnh.
5. **[05-backend-coding-convention.md](./05-backend-coding-convention.md)**: (Giai đoạn Coding Backend) Các quy chuẩn khắt khe cho C# .NET: SOLID, Tối ưu hóa Database N+1, Async/Await.
6. **[06-frontend-coding-convention.md](./06-frontend-coding-convention.md)**: (Giai đoạn Coding Frontend) Cơ cấu tổ chức Component của Vite/React/TS, Stateless logic, CSS và API Interceptors.
7. **[07-testing-guidelines.md](./07-testing-guidelines.md)**: (Giai đoạn Kiểm Thử - QA) Đặc tả luật Unit Test bảo vệ nghiệp vụ, Automation Integration Test, và End-to-End E2E Testing.
8. **[08-git-and-workflow-rules.md](./08-git-and-workflow-rules.md)**: (Giai đoạn Version Control) Quản lý luồng công việc qua nhánh Git (GitFlow), viết Conventional Commit, nguyên tắc Review Code.
9. **[09-ci-cd-and-deployment.md](./09-ci-cd-and-deployment.md)**: (Giai đoạn Vận hành - Operations) Luồng CI/CD, Containerization bằng Docker, tự động hóa Pipelines và thiết lập Proxy Server, quản lý Mật khẩu ngầm.

## Hướng Dẫn Truy Xuất Luật Cho AI / Con Người
- Muốn setup repo từ đầu: Đọc `08`, `09`.
- Muốn code tính năng Backend: Đọc `01`, `02`, `03`, `04`, `05`, `07`.
- Muốn thiết kế giao diện UI: Đọc `01`, `04`, `06`.
