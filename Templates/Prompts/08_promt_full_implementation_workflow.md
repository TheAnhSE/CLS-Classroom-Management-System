# Full Implementation Workflow Prompts — ATS Project

## PHASE 1: INFRASTRUCTURE-FIRST (ONE-TIME SETUP)

_Thực hiện các Prompt P1 đến P6 tuần tự để xây dựng nền móng (Foundation Layer). Quá trình này **CHỈ THỰC HIỆN 1 LẦN** lúc bắt đầu dự án. Bối cảnh cần cung cấp thay đổi theo từng bài tập._

### P1: Project Scaffolding
**Context Files Attached:** `project_context.md` (L1), `coding_conventions/01_backend.md` (L3), `ADR-001-adopt-modular-monolith_ats.md`
> **Prompt:**
> "Bám sát L1, L3 và bản thiết kế kiến trúc. Với vai trò Senior Spring Boot Architect, hãy tạo toàn bộ khung project scaffolding.
> 1. Tạo `pom.xml` cho Java 21, Spring Boot 3.5.x, PostgreSQL 17, Flyway, JWT, OpenAPI.
> 2. Tạo toàn bộ cấu trúc package `com.ats.*` theo đúng chuẩn Modular Monolith (tạo các thư mục rỗng với `.gitkeep`).
> 3. Tạo `application.yml` + dev profile, `AppConfig.java`, và cấu hình Swagger (OpenAPI).
> Không tạo Entity hay Logic cụ thể nào. Trả về cấu trúc cây thư mục và nội dung các file gốc."

### P2: BaseEntity
**Context Files Attached:** `coding_conventions/01_backend.md` (L3)
> **Prompt:**
> "Dựa vào quy chuẩn Coding L3, hãy tạo `BaseEntity.java` trong package `com.ats.common.entity`.
> Yêu cầu: Sử dụng UUID với `GenerationType.UUID`. Các trường audit bao gồm: `createdAt`, `updatedAt`, `createdBy`, `updatedBy` (tự động via mapped JPA Auditing) và `isDeleted` + `deletedAt` cho soft delete. Tuyệt đối không dùng `@Data` của Lombok."

### P3: Universal API Wrappers
**Context Files Attached:** `api_design_rules.md` (L2), `coding_conventions/01_backend.md` (L3)
> **Prompt:**
> "Quán triệt quy ước API từ tài liệu L2, hãy generate record `ApiResponse<T>` và `PageResponse<T>` trong `com.ats.common.dto`.
> Định dạng bắt buộc: `{ "code": 200, "message": "...", "data": {...} }`. Hãy implement các static factory methods (`success`, `created`, `error`) để tái sử dụng."

### P4: Exceptions Hierarchy
**Context Files Attached:** `api_design_rules.md` (L2), `coding_conventions/01_backend.md` (L3), `ApiResponse.java` (đã có)
> **Prompt:**
> "Triển khai `GlobalExceptionHandler.java` sử dụng `@RestControllerAdvice` trong package common. Bắt buộc tạo các Custom Exceptions sau: `ResourceNotFoundException (404)`, `BusinessException (422)`, `DuplicateResourceException (409)`. Map mọi exception về thống nhất `ApiResponse<T>` để controller luôn sạch."

### P5: Security Configuration
**Context Files Attached:** `api_design_rules.md` (L2), `coding_conventions/01_backend.md` (L3)
> **Prompt:**
> "Hãy setup `SecurityConfig.java` và `JwtAuthenticationFilter.java` sử dụng Spring Security 6.x (lambda DSL). Define STATELESS session management, disable CSRF. Đọc danh sách endpoints từ L2 để chia rule `permitAll()` hoặc `authenticated()`. Chưa implement `UserDetailsService`, chỉ tạo skeleton để filter token."

### P6: Database Base Migrations (Flyway)
**Context Files Attached:** `coding_conventions/03_database.md` (L3), Logical ERD
> **Prompt:**
> "Với tư cách Database Architect, hãy viết các script Flyway gốc vào `src/main/resources/db/migration/`:
> 1. `V1__baseline_extensions.sql` (gen_random_uuid).
> 2. `V2__create_shared_enums.sql` (ENUMs chung như Role, Status).
> 3. `V3__create_users_table.sql` (bảng users cốt lõi gồm đủ audit field giống lớp BaseEntity)."

---

## 🚀 PHASE 2: VERTICAL SLICE IMPLEMENTATION

_Thực hiện lặp lại vòng lặp P7 -> P13 cho **từng tính năng / User Story** riêng biệt (ví dụ: Tạo Job Posting, Apply Candidate). "Vertical slice" nạp Context tập trung hơn và phân làm 4 chặng: Data → Business → API → QA._

### P7: Data Layer (Entity & Migration)
**Context Files Attached:** `coding_conventions/01_backend.md` (L3), `coding_conventions/03_database.md` (L3), Logical ERD
> **Prompt:**
> "Nhiệm vụ: Hiện thực hóa Vertical Slice cho tính năng: [TÊN_FEATURE].
> Dựa vào Database Conventions (L3) và ERD Logical, hãy generate:
> 1. Script Flyway DDL `V{NextVersion}__create_[module].sql`.
> 2. Java JPA `Entity`. Extend từ `BaseEntity.java`. Config FetchType.LAZY trên mọi relation."

### P8: Persistence & Transfer Layer (Repository/DTOs)
**Context Files Attached:** Tệp API Specs (Swagger/OpenAPI), `coding_conventions/01_backend.md` (L3)
> **Prompt:**
> "Nhiệm vụ: Cung cấp layer giao tiếp cho [TÊN_FEATURE].
> 1. Generate Spring Data JPA Repository giao tiếp với Entity từ bước trước. Kèm một số custom `findBy...` nếu API Specs yêu cầu lọc dữ liệu.
> 2. Căn cứ API spec, generate toàn bộ Request DTOs và Response DTOs. Tích hợp validation annotiation (`@NotNull`, `@Size`...)."

### P9: Business Logic Layer (Service Component)
**Context Files Attached:** User Story / Acceptance Criteria (AC), Sequence Diagrams, `coding_conventions/01_backend.md` (L3)
> **Prompt:**
> "Nhiệm vụ: Xây dựng bộ não nghiệp vụ cho [TÊN_FEATURE].
> Dựa trên User Story + AC và Sequence Diagram (nếu có), hãy cài đặt Service logic `[Feature]ServiceImpl.java`. Bắt buộc thoả mãn các luồng Exception nếu vi phạm AC. Tránh "God Classes" hoặc gọi sang Repository của Module khác."

### P10: API Surface (Controller)
**Context Files Attached:** Tệp API Specs, `api_design_rules.md` (L2), `coding_conventions/01_backend.md` (L3)
> **Prompt:**
> "Nhiệm vụ: Publish API Endpoint cho [TÊN_FEATURE].
> Căn cứ API rules (L2) và API specs: tạo file RestController. Đảm bảo url là kebab-case, request/response được bọc bởi `ApiResponse<T>`. Controller chỉ làm nhiệm vụ ủy quyền sang Interface của Service, không nhúng logic nghiệp vụ."

---

## 🔍 PHASE 3: QA AND DELIVERY

_Đây là bước AI Augment Review (AI đóng vai Tester và Code Reviewer) nhằm đảm bảo sự minh bạch của Vertical Slice trước khi commit._

### P11: AI Code Review (Clean Code & Solid)
**Context Files Attached:** Các tệp codebase của toàn bộ Slice vừa lập trình, Anti-patterns L3.
> **Prompt:**
> "Đóng vai là Tech Lead, hãy kiểm tra chéo (review) toàn bộ Vertical Slice vừa cung cấp.
> Kiểm tra các Checklist sau:
> - Có xuất hiện N+1 querry problem không?
> - Các fields nhạy cảm có rơi rớt vào DTO response không?
> - Entity có tuân thủ hoàn toàn cấu trúc của BaseEntity không?
> Trả về danh sách issue nếu có và code cải thiện (refactor)."

### P12: Unit Testing Generation (BDD)
**Context Files Attached:** User Story / AC, Slice Codebase, `coding_conventions/04_testing.md` (L3)
> **Prompt:**
> "Ánh xạ trực tiếp Acceptance Criteria dạng **Given-When-Then** thành cấu trúc JUnit 5 + Mockito tests cho Service vừa làm.
> Đảm bảo coverage trên 80%: Hãy viết test cho Happy path và dứt khoát viết riêng test cases khi mock repository chọc vào Exception branch (như Conflict, Not Found)."

### P13: Conventional Commit & Pull Request
**Context Files Attached:** `coding_conventions/05_git_workflow.md` (L3), List of Slice Files.
> **Prompt:**
> "Tính năng Vertical Slice [TÊN_FEATURE] đã hoàn tất và thông qua (Passed) bài kiểm tra chất lượng.
> Hãy gen template Pull Request điền sẵn nội dung 'What was changed' và kèm luôn dòng Git commit message bám sát chuẩn Conventional Commits trong `05_git_workflow.md`."
