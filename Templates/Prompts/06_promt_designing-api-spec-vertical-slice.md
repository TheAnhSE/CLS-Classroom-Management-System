# Chat Conversation

Note: _This is purely the output of the chat conversation and does not contain any raw data, codebase snippets, etc. used to generate the output._

## Phase 2: Full API Specification cho từng Vertical Slice

### Thời điểm sử dụng
- Sau khi API Catalog (Phase 1) đã được duyệt
- Sau khi Infrastructure-First đã scaffold xong
- Khi bắt đầu implement từng module/feature
- Lặp lại prompt này cho MỖI module

### Context cần nạp (theo thứ tự)
- `@[.ai-context/project_context.md]` (L1)
- `@[.ai-context/api_design_rules.md]` (L2)
- `@[.ai-context/coding_conventions.md]` (L3 — nếu cần generate code)
- API Catalog đã duyệt từ Phase 1 (file trong `Documents/03_Design/04_APIs/`)
- Use Case Specifications liên quan trong SRS
- Logical ERD (`@[Documents/02_Requirements/ATP_ERD_Logical.drawio]`)

---

### User Input 1 — Template (thay [MODULE_NAME] khi sử dụng)

[API Spec] Bám sát @[project_context.md] + @[api_design_rules.md]

Với vai trò Senior API Architect + Detail Designer, hãy thiết kế Full API Specification cho module **[MODULE_NAME]** dựa trên:
- Danh sách endpoints đã phê duyệt trong API Catalog @[API Catalog file]
- Use Case Specifications liên quan: [liệt kê UC-ID]
- Logical ERD @[ATP_ERD_Logical.drawio]

**Cho MỖI endpoint trong module, cung cấp:**

1. **Endpoint Info:** HTTP Method, URL, Summary, Auth requirement
2. **Request:**
   - Path parameters + Query parameters (nếu có)
   - Request Body schema (JSON) với data types
   - Validation rules (theo `validation_rules` trong api_design_rules.md)
   - DTO naming: `Create{Entity}Request`, `Update{Entity}Request`, `{Entity}SearchCriteria`
3. **Response (Success):**
   - Wrapped trong `{ code, message, data }` format
   - Response DTO: `{Entity}Response` (detail) hoặc `{Entity}SummaryResponse` (list)
   - Pagination: `PageResponse<T>` cho list endpoints
4. **Response (Error):**
   - Chỉ liệt kê mã lỗi phù hợp (400, 401, 403, 404, 409, 422)
   - Error response format theo api_design_rules
5. **Security:**
   - Gắn `bearerAuth` cho protected/role-restricted endpoints
   - Ghi rõ required roles

**Output format:** Structured Markdown tables hoặc OpenAPI 3.0 YAML
Ngôn ngữ: Tiếng Anh

**Ràng buộc:**
- KHÔNG thêm endpoints ngoài API Catalog đã duyệt (nếu phát hiện thiếu → ghi chú riêng)
- Response KHÔNG chứa fields nhạy cảm (password, token secrets)
- JSON fields dùng `camelCase`

## Khi review xong → chuyển sang implement Vertical Slice (Controller → Service → Repository)

---

### Ví dụ sử dụng cho Auth Module

### User Input — Auth Module

[API Spec] Bám sát @[project_context.md] + @[api_design_rules.md]

Thiết kế Full API Specification cho module **auth** dựa trên:
- API Catalog đã duyệt @[Documents/03_Design/04_APIs/ATS_API_Catalog.html]
- Use Cases: UC-01 (Login), UC-02 (Register), UC-03 (Manage Roles)
- ERD @[Documents/02_Requirements/ATP_ERD_Logical.drawio]

(Nội dung prompt giống template trên)

---

### Ví dụ sử dụng cho Candidate Module

### User Input — Candidate Module

[API Spec] Bám sát @[project_context.md] + @[api_design_rules.md]

Thiết kế Full API Specification cho module **candidate** dựa trên:
- API Catalog đã duyệt @[Documents/03_Design/04_APIs/ATS_API_Catalog.html]
- Use Cases: UC-05 (Submit Application), UC-06 (Parse CV), UC-07 (Search Candidates)
- ERD @[Documents/02_Requirements/ATP_ERD_Logical.drawio]

(Nội dung prompt giống template trên)
