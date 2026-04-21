<api_design_rules>
<overview>
Tài liệu này định nghĩa các quy tắc cốt lõi (Core Rules) cho việc thiết kế và đặc tả API trong dự án. Tài liệu này được sử dụng để onboarding AI và đảm bảo tính nhất quán trong toàn bộ hệ thống.
</overview>

<rule tag="global-route-prefix">
<title>Global Route Prefix</title>
<description>
Tất cả các API route bắt buộc phải bắt đầu bằng global prefix: `/api/v1`.
Không có ngoại lệ cho quy tắc này nhằm đảm bảo quản lý phiên bản API đồng nhất.
</description>
</rule>

<rule tag="path-url-format">
<title>Path URL Naming Convention</title>
<description>
Tất cả các định dạng đường dẫn (Path URL) phải sử dụng quy tắc `kebab-case`.
Tuyệt đối không sử dụng `camelCase`, `snake_case` hay `PascalCase` trong URI.
Ví dụ đúng: `/api/v1/student-profiles`, `/api/v1/course-materials`
Ví dụ sai: `/api/v1/studentProfiles`, `/api/v1/course_materials`
</description>
</rule>

<rule tag="standard-response-format">
<title>Standard Response Wrapped Format</title>
<description>
Tất cả các API response (kể cả thành công hay thất bại) đều phải được bọc (wrapped) trong một chuẩn cấu trúc JSON chứa 3 trường cố định: `code`, `message`, `data`.
- `code` (integer): Mã HTTP status code hoặc custom business code.
- `message` (string): Thông báo mô tả kết quả xử lý.
- `data` (object/array/null): Dữ liệu trả về (payload).
</description>
</rule>

<rule tag="authentication-security">
<title>JWT Bearer Authentication</title>
<description>
Áp dụng xác thực bằng JWT Bearer cho tất cả các secured endpoints (API cần bảo mật).
AI khi generate OpenAPI spec hoặc detail design phải luôn luôn đánh dấu các locked routes bằng cấu hình security thích hợp.
</description>
</rule>

<rule tag="sensitive-data-protection">
<title>Hide Sensitive Fields</title>
<description>
TUYỆT ĐỐI KHÔNG ĐƯỢC trả về các trường thông tin nhạy cảm (như `password`, `secret_key`, `token` ở những API không phải xác thực, v.v.) trong các cấu trúc schema (Response Schema). AI phải tự động loại bỏ các trường này khỏi các Object trả về.
</description>
</rule>

<example>
<title>OpenAPI Specifiction Example</title>
<content>
```yaml
paths:
  /api/v1/auth/login:
    post:
      summary: "User Login"
      security:
        - bearerAuth: [] # AI will learn to append this to all locked routes
      responses:
        '200':
          content:
            application/json:
              schema:
                properties:
                  code: { type: integer, example: 200 }
                  message: { type: string, example: "Login successful" }
                  data:
                    $ref: '#/components/schemas/AuthToken'
```
</content>
</example>
</api_design_rules>
