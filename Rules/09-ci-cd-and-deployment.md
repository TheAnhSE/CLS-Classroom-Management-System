# Quy Chuẩn CI/CD Và Triển Khai Hạ Tầng (Deployment)

Việc phát hành phần mềm phải được tự động hoá và hoàn toàn tách biệt khỏi máy tính cá nhân của lập trình viên.

## 1. Môi trường (Environments)
Hệ thống trải qua 2 môi trường tối thiểu:
- **Staging / UAT (User Acceptance Testing)**: Môi trường nội bộ đón code từ nhánh `develop`. Sử dụng Database riêng (Staging DB) và các 3rd party APIs (Email) ở chế độ Sandbox.
- **Production**: Môi trường Live chạy code từ nhánh `main`.

## 2. Tiêu chuẩn Continuous Integration (CI)
Mọi PR được mở trên Github/Gitlab bắt buộc phải đi qua Pipeline quét lỗi:
1. **Linter Check**: Đội Frontend quét qua ESLint/Prettier. Đội Backend quét bộ C# Compiler Warnings. Bất cứ Warning lạ nào cũng làm gãy Pipeline.
2. **Build Test**: Thử build mã nguồn `dotnet build` và `npm run build` để ngăn chặn đẩy code lỗi dấu chấm phẩy hoặc sai cú pháp lên server.
3. **Automated Testing**: Chạy toàn bộ Unit Tests của hệ thống 07-testing-guidelines. Báo cáo Tỷ lệ Coverage (Coverage Report). Nếu dưới 80% (đối phó xài hack) sẽ tự rớt CI.
Chỉ khi Pipeline xanh lá cây (Passed), nút Merge mới hoạt động.

## 3. Triển Khai Docker Mức Phân Nhỏ (Containerization)
Tất cả các dịch vụ (Frontend, Backend) phải được gói trong Container để chặn triệt để chuyện "Code chạy được rổ trên máy tôi".
- Cấu trúc `Dockerfile` sử dụng kỹ thuật Multi-stage build.
- Hình ảnh cuối cùng (Final Image) phải là bản nhẹ gọn nhất (Alpine/Distroless). Bản dùng để Build chứa SDK không được phép mang vào môi trường chạy.

## 4. Quản lý Cấu hình & Biến môi trường (Secrets Configuration)
- **TUYỆT ĐỐI**: Không hardcode các thông tin DB Connection String, SendGrid API Key hay JWT Secret vào trong Mã Nguồn hay Commit lên Git.
- Trên môi trường Live: Sử dụng `Docker Environment Variables`, `Kubernetes Secrets` hoặc hệ thống `Azure Key Vault`/`AWS Secrets Manager`.
- Trên máy local: Chỉ lưu trong `appsettings.Development.json` (bị ignores git) hoặc `secrets.json` của hệ điều hành cá nhân.

## 5. Kiến trúc Mạng & Theo dõi Giám sát (Monitoring)
- Cấu hình **Nginx / YARP Reverse Proxy** ở lớp rìa để định tuyến SSL/HTTPS trước khi đẩy Request xuống Backend (Kestrel).
- Triển khai **Health Checks** theo chuẩn ASP.NET Core (`/health`). Load Balancer sẽ đá văng container nếu API sập.
- Gắn hệ thống logging tập trung (Ví dụ: ELK Stack, Serilog Seq, Datadog) đễ đọc lỗi mà không cần SSH vào Cloud Server tra file text thủ công.
