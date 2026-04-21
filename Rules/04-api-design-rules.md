# Chuẩn Mực Thiết Kế REST API (API Design Rules)

Tài liệu này định nghĩa cấu trúc khắt khe nhất quy chuẩn việc thiết kế, xây dựng Request/Response, và bảo mật của toàn bộ các Endpoints do hệ thống cung cấp ra bên ngoài.

## 1. Định nghĩa Routing và Versioning
- **Global Route Prefix**: Tất cả các API do CLS quản trị bắt buộc phải tuân theo định dạng có namespace versioning: `/api/v{version}/...`. Đối với MVP, version cố định là `v1`.
- **Naming URL**: 100% sử dụng định dạng `kebab-case` và sử dụng DANH TỪ SỐ NHIỀU (plural nouns) để mô tả resource.  
  ✅ Ví dụ đúng: `GET /api/v1/learners`, `POST /api/v1/learning-packages`  
  ❌ Ví dụ sai: `/api/v1/getLearner`, `/api/v1/LearningPackages`

## 2. Http Methods Constraints
Tuân thủ chuẩn REST trong việc ánh xạ hành động vào HTTP Method.
- **GET**: Chỉ dùng để đọc, FETCH dữ liệu. Cấm thay đổi DB ở các route GET.
- **POST**: Thêm mới (Create data) hoặc thực hiện các thao tác Complex Business Action (vd: `/api/v1/auth/login`).
- **PUT**: Cập nhật toàn phần một entity. Backend nhận Request sẽ ghi đè record.
- **PATCH**: Cập nhật một phần entity (Partial Update). Chỉ những field có dữ liệu mới được lưu.
- **DELETE**: Dành riêng cho thao tác xóa dữ liệu (Hard delete/Soft delete tuỳ business).

## 3. Standard Response Format (Chuẩn Wrapper)
Tuyệt đối không để API trả về Array List thô hoặc object Entity trần, nhằm mục tiêu đồng bộ việc đọc lỗi ở Frontend Client. Mọi Response (kể cả Status OK hay Failed) đều bọc trong 1 BaseResponse object với 3 tham số cố định.

```json
{
  "code": 200,                // HTTP Status code hoặc Business Error Code đặc thù
  "message": "Nạp dữ liệu thành công.",  // Thông điệp rõ ràng trả về (có thể dịch locale)
  "data": { ... }             // Bất kỳ Object/Array nào trả về. Null nếu lỗi.
}
```

## 4. HTTP Status Codes Trả Về Khuyên Dùng
- `200 OK`: Request thành công chung chung, đọc dữ liệu.
- `201 Created`: Tạo thành công. Kèm resource hoặc URL trỏ tới resource vừa tạo ra.
- `204 No Content`: Thành công chỉnh sửa/xoá mà không có body trả ngược lại.
- `400 Bad Request`: Lỗi ValidationError (Client truyền sai format body JSON, thiếu required field, business conflict data do user nhầm lẫn). Có thể kèm array `errors` trong `data`. 
- `401 Unauthorized`: Lỗi do JWT Token hết hạn, sai hoặc chưa đăng nhập.
- `403 Forbidden`: Role của User không có quyền thực hiện. (Vd: Teacher cố can thiệp vào trang Setting của Admin).
- `404 Not Found`: Không tìm thấy Resource theo path param ID truyền vào.
- `409 Conflict`: Dành riêng cho lỗi mâu thuẫn hệ thống. Ví dụ: US-13 Phát hiện **Scheduling Conflict** phòng học.
- `500 Server Error`: Bug hệ thống/Code thối, lỗi connection string, null reference exception. Rơi vào Middleware.

## 5. Security & Authentication
- **JWT Authorization**: Tất cả private resources đều khoá bằng token chuẩn `Bearer {JWT_TOKEN}` trong `Authorization Base Header`.
- Hệ thống hỗ trợ Stateless Authentication. Giải mã trực tiếp Role/Identity qua token claims mà không chọc database phụ trợ ngoài ý muốn (trừ phi tracking blacklist/logout).
- **Hide Sensitive Info**: Tuyệt đối không serialize `PasswordHash`, `TokenSecret`, `ParentPhoneCVC` ra JSON public API. Use cases như `UserResponseDto` phải xóa trắng các keys nhạy cảm này.

## 6. Phân trang (Pagination), Lọc (Filtering) và Sắp xếp (Sorting)
Với các API Get List (Get All), áp dụng bắt buộc cơ chế Phân trang bằng Query Params `pageIndex` và `pageSize`:
  - GET `/api/v1/sessions?pageIndex=1&pageSize=10&sortBy=dateAsc&teacherId=1`
  - Object trả về phải nằm trong object `data` đi kèm các property Pagination:
```json
"data": {
  "items": [ ... ],
  "totalCount": 150,
  "totalPages": 15,
  "currentPage": 1
}
```
