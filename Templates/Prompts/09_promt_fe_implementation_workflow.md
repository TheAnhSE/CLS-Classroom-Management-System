# Full Implementation Workflow Prompts — FE Project (React)

_Bộ lệnh Prompt dành cho AI (AI Coding Formula) giúp xây dựng Frontend (React) tuân thủ cực kỳ chuẩn xác triết lý Infrastructure-First và quy tắc Modular_ của file L3 (`02_frontend.md`)._

---

## 🏗 PHASE 1: FRONTEND INFRASTRUCTURE-FIRST (ONE-TIME SETUP)

_Quá trình nạp khung sườn một lần duy nhất lúc khởi tạo dự án. Mọi Component và Feature sau này đều sẽ tái sử dụng cấu trúc ở đây._

### P_FE1: Project Scaffolding & Configs
**Context Files:** `project_context.md` (L1), `coding_conventions/02_frontend.md` (L3)
> **Prompt:**
> "Dựa vào quy chuẩn Frontend L3, đóng vai Senior React/Vite Architect. Hãy generate khung sườn (scaffolding) cho dự án:
> 1. Tạo file cấu hình `package.json` (React 19, Vite 8, TypeScript, Zustand, React Router, TailwindCSS 4, Shadcn, Axios).
> 2. Tạo toàn bộ cây folder chuẩn xác theo kiến trúc Modular: `src/app`, `src/assets`, `src/features`, `src/shared`, `src/styles`. 
> Đặt các file `.gitkeep` bên trong để giữ cấu trúc. Chưa cần viết component logic."

### P_FE2: The API Client Wrapper
**Context Files:** `coding_conventions/02_frontend.md` (L3), `api_design_rules.md` (L2)
> **Prompt:**
> "Nhiệm vụ: Setup cầu nối API cốt lõi. Hãy tạo thư mục `src/shared/services/` và viết `api.client.ts`.
> Yêu cầu:
> 1. Setup Axios Instance với BaseURL đọc từ env.
> 2. Khởi tạo **Request Interceptor** tự động móc JWT Bearer Token (từ localStorage/Zustand) đính vào Headers.
> 3. Khởi tạo **Response Interceptor** xử lý Unwrap định dạng `ApiResponse<T>` từ L2, tức là chỉ trả về `.data` cho các hàm fetch. Mapping tự động catch lỗi Http 401 để kích hoạt trigger log-out. Không được bỏ qua logic này."

### P_FE3: Global Types & Design System Skeleton
**Context Files:** `coding_conventions/02_frontend.md` (L3), Mẫu ApiResponse của BE.
> **Prompt:**
> "Nhiệm vụ: Cấu hình hệ thống Type bảo vệ và Khóa UI Component.
> 1. Tạo `src/shared/types/api.types.ts`: Define Interface `ApiResponse<T>`, `PageResponse<T>` và `BaseEntity` (id, createdAt, etc... theo config backend).
> 2. Tạo khung cho 3 dumb component cơ bản ở `src/shared/components/`: `Button`, `InputField`, và `ToastProvider`. Các code UI này hãy sử dụng Tailwind/Shadcn tiêu chuẩn, ưu tiên Clean Code."

### P_FE4: Routing Layouts & Guard
**Context Files:** `coding_conventions/02_frontend.md` (L3)
> **Prompt:**
> "Nhiệm vụ: Cấu trúc App Shell và Điều hướng (React Router). 
> 1. Tại `src/app/layouts`, tạo `MainLayout.tsx` (có Topbar + Sidebar placeholder) và `AuthLayout.tsx`.
> 2. Tại `src/app/guards`, tạo một rào cản `PrivateRoute.tsx` (check auth state).
> 3. Kết nối toàn bộ vào `AppRouter.tsx` và mount vào `App.tsx` gốc."

---

## 🧩 PHASE 2: VERTICAL SLICE IMPLEMENTATION (PER FEATURE)

_Tương tự Backend, AI sẽ phát triển Frontend theo từng module dọc (Job, Candidate...). Mỗi User Story sẽ chạy liên tiếp 2 lệnh sau._

### P_FE5: Feature Service & Hooks Layer (Data Access)
**Context Files:** Tài liệu User Story / API Swagger Json của BE, `02_frontend.md` (L3)
> **Prompt:**
> "Nhiệm vụ: Code layer kết nối data cho Feature: [TÊN_FEATURE]. 
> 1. Tạo file Interface `src/features/[feature]/types/[feature].types.ts` quét theo API response models.
> 2. Tạo Service class `[feature].service.ts` gọi đến `apiClient`. Sử dụng triệt để Generics (Ví dụ: `apiClient.get<PageResponse<Job>>`).
> 3. Kéo Service này lên file Hook `use[Feature]Queries.ts` (Sử dụng Zustand hoặc custom Hook để handle loading/error states)."

### P_FE6: UI Presentation Layer (Pages & Components)
**Context Files:** Mockups/UI Designs, L3 Frontend rules, Interface Types từ bước trước.
> **Prompt:**
> "Nhiệm vụ: Render giao diện [TÊN_PAGE/ACTION] cho [TÊN_FEATURE].
> Dựa trên Hook và Service vừa làm, hãy tạo:
> 1. Các Smart Components (Ví dụ: `ListBoard`, `FilterBar`) tách biệt ở thư mục `features/[feature]/components/`.
> 2. Tích hợp Component vào `[Feature]Page.tsx`. 
> Quy tắc Strict: Component TUYỆT ĐỐI KHÔNG CHỨA LOGIC axios calls. Giao diện phải handle rành mạch cả loading skeleton và empty-state error."

---

## 🔍 PHASE 3: QA & REFINEMENT

### P_FE7: Frontend AI Review (Clean React Code)
**Context Files:** Code của Slice vừa làm, Anti-Patterns từ `02_frontend.md`.
> **Prompt:**
> "Đóng vai là Senior Frontend Reviewer. Hãy dò quét TẤT CẢ các file React vừa code cho Feature: [TÊN_FEATURE].
> Checklist:
> 1. Có Prop Drilling sâu quá 3 levels không?
> 2. Có gọi trực tiếp Axios bên trong `useEffect` của UI component không?
> 3. Performance: Có map list data mà dùng Error key (bằng index) không?
> Hãy tự động generate đoạn mã refactor sửa chữa (nếu phát hiện có vi phạm anti-patterns mang tên `L3 - Frontend`). Cập nhật nốt Pull Request msg cho Git."
