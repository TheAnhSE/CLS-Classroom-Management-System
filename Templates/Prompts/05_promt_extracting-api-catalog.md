# Chat Conversation


## Phase 1: Extract API Catalog (Danh mục toàn bộ endpoints)

### Thời điểm sử dụng
- Sau khi SRS hoàn thiện (Use Cases, ERD, Mockups đã duyệt)
- Trước khi bắt đầu Infrastructure-First prompts
- Output sẽ lưu vào `Documents/03_Design/04_APIs/`

### Context cần nạp
- `@[.ai-context/project_context.md]` (L1 — Scope, Modules, Glossary)
- `@[.ai-context/api_design_rules.md]` (L2 — Routing, Response format, Security)
- `@[Documents/02_Requirements/ATS_Software Requirement Specification_v0.2.docx]` (SRS)

---

### User Input 1

[API Catalog] Bám sát thông tin trong @[project_context.md] và @[api_design_rules.md]

Với vai trò Senior API Architect, hãy phân tích SRS document @[ATS_Software Requirement Specification_v0.2.docx] và extract toàn bộ RESTful API endpoints cần thiết cho hệ thống ATS.

**Định dạng output:** Bảng API Catalog, nhóm theo Module (Bounded Context):

| # | Module | HTTP Method | Endpoint | Summary | Auth | Actor(s) |
|---|--------|-------------|----------|---------|------|----------|

**Quy tắc bắt buộc:**
1. Tuân thủ TOÀN BỘ routing rules trong `api_design_rules.md`:
   - Global prefix: `/api/v1`
   - URL path: `kebab-case`
   - Resource naming: plural nouns
   - Nested resources max 2 levels
2. Mỗi Use Case trong SRS phải có ít nhất 1 API endpoint tương ứng
3. Phân loại Auth theo endpoint_classification trong api_design_rules:
   - `Public` = No auth required
   - `Protected` = JWT required
   - `Role-restricted(ROLE)` = JWT + specific role (ghi rõ role)
4. Actor phải map chính xác với actors đã định nghĩa trong SRS
5. Bao gồm cả Action/RPC endpoints cho business operations (VD: `POST /candidates/{id}/move-stage`)

**Modules cần cover (theo project_context.md):**
- `auth` — Authentication & Authorization
- `job` — Job Posting Management
- `candidate` — Candidate & Application Management
- `pipeline` — Kanban Pipeline & Stage Management
- `interview` — Interview Scheduling & Feedback
- `notification` — Email Automation
- `analytics` — Dashboard & Reports
- `career-site` — Public Career Page

**Bổ sung cuối bảng:**
- Đánh dấu endpoints: `[CRUD]` vs `[ACTION]` (business logic)
- Ghi chú endpoints liên quan cross-module
- Tổng kết số lượng endpoints mỗi module

**Điều KHÔNG làm:**
- KHÔNG thiết kế chi tiết Request/Response body (sẽ làm ở Phase 2)
- KHÔNG viết OpenAPI/Swagger YAML
- KHÔNG tự sáng tạo tính năng ngoài scope SRS

Ngôn ngữ: sử dụng tiếng Anh (thuật ngữ IT chuyên ngành, rõ nghĩa)
Output: export thành file HTML chuyên nghiệp, lưu vào @[Documents/03_Design/04_APIs/]

## Khi Architect review nếu cần update thì prompt tiếp
### User Input 2

Với vai trò Senior API Architect / Reviewer, hãy rà soát lại API Catalog trên:
1. Có endpoint nào bị thiếu so với Use Case list trong SRS không?
2. Có naming nào vi phạm api_design_rules không?
3. Có endpoint nào nên gộp hoặc tách không?
4. Cross-module dependencies có hợp lý không?

## Sau khi duyệt xong API Catalog → chuyển sang Infrastructure-First prompts → rồi Phase 2
