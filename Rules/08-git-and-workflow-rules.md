# Quy Trình Git & Làm Việc Nhóm (Git Workflow Constraints)

Để giữ Repository ổn định, chặn lỗi phát sinh môi trường và kiểm duyệt code, toàn bộ quy trình đẩy code phải được tuân theo chuẩn sau.

## 1. Hệ thống Phân Nhánh (Branching Model)
Sử dụng mô hình **GitFlow** làm kim chỉ nam. Tuy nhiên, nếu vòng lặp Release ngắn, có thể dùng **GitHub Flow** lược giản.
- `main` / `master`: Môi trường Production. TUYỆT ĐỐI không Push code trực tiếp vào đây. Nhánh này luôn phải build thành công.
- `develop`: Môi trường Staging/Test tích hợp. Nơi đón Code từ các feature.
- `feature/{Jira-Id}-ten-tinh-nang`: Nhánh làm tính năng riêng lẻ. Được tách ra từ develop.
  - VD: `feature/UC-10-create-session-schedule`
- `hotfix/{Jira-Id}-ten-bug`: Nhánh vá lỗi gấp. Cắt trực tiếp từ `main`. Code vá xong gộp ngược lại cả `main` lẫn `develop`.
- `release/{version}` (Tuỳ chọn): Nhánh khoanh vùng chuẩn bị lên Live.

## 2. Quy Chuẩn Commit Message
Sử dụng **Conventional Commits** để tự động hoá việc theo dõi lịch sử và sinh tài liệu Release Note. Chặn bằng công cụ `Husky` hoặc `Git Hooks`.
- **Cấu trúc**: `<type>(<scope>): <subject>` (tiếng Anh).
- **Các type thông dụng**:
  - `feat`: Tính năng mới hoàn toàn (VD: `feat(attendance): Add absent calculation rule`)
  - `fix`: Sửa lỗi bug (VD: `fix(auth): Resolve token expiration bug 401`)
  - `refactor`: Sửa lại mã nguồn mà không ảnh hưởng tới tính năng bên ngoài.
  - `style`: Định dạng code (xuống dòng, dấu phẩy).
  - `docs`: Bổ sung README, viết Document.
  - `chore`: Thay đổi cấu hình build, cấu hình npm, update dependencies.
- **Quy tắc**: Message phải viết ở thể mệnh lệnh (VD: dùng `Add` thay vì `Added`). KHÔNG chửi rủa trong Commit message. Mã Jira id có thể để ở cuối `(US-13)`.

## 3. Pull Request (PR) & Code Review
Khi một Feature/BugFix xong, KHÔNG tự merge. Bắt buộc mở Pull Request hướng vào Develop.
- **Kích thước PR**: Quy định khắt khe không đưa quá 500 dòng code sửa đổi vào 1 cái PR. Tách nhỏ PR để giảm tải review.
- **Tiêu chí tự kiểm trước (Self-Check)**:
  - Code đã format và dọn dẹp sạch `console.log`, `TODO` trống.
  - Phải có Test Coverage cho nghiệp vụ thay đổi nếu có Unit Test setup.
  - Chạy code lọt qua Linter và Build Success locally.
- **Trách nhiệm Review**: Phải có ít nhất 1 Developer duyệt (Approve) thì mới được phép Merge. Sử dụng `Squash and Merge` để gom commit.

## 4. Kiểm Soát Liên Tục (CI)
- Mỗi lần Push lên PR, CI tự động chạy Build, Linter, và Testing. Gắn gác cổng bảo vệ repo.
