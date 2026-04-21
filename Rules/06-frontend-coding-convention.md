# Quy Chuẩn Lập Trình Frontend (ReactJS / TypeScript / Vite)

Phần Frontend của CLS Dashboard sử dụng kỹ thuật SPA (Single Page Application). Bản tiêu chuẩn này áp dụng cho thiết kế ứng dụng React dùng Vite bundler và ngôn ngữ TypeSafe.

## 1. Kiến Trúc Thư Mục Hệ Thống (Folder Structure)
Mã nguồn đặt trọn vẹn trong `/src`. Phân tán theo đặc thù "Feature-based" thay vì "File-type-based".
```
src/
 ├─ assets/         # Tài nguyên tĩnh, hình ảnh tĩnh, fonts svg nhỏ
 ├─ components/     # UI shared elements (Buttons, Modal, Inputs... - Dumb components)
 ├─ features/       # 80% logic nghiệp vụ nằm tại đây. Chia theo từng Business Feature (Learner, Session, Schedule)
 │   └─ LearnerManagement/
 │       ├─ components/     # Các component UI chỉ riêng LearnerManagement sử dụng
 │       ├─ hooks/          # Logic gọi API hoặc xử lý state của riêng LearnerManagement
 │       ├─ types/          # Interfaces TS dành riêng cho Learner
 │       └─ LearnDashboard.tsx # Component chính ráp tầng logic
 ├─ hooks/          # Global Custom Hooks dùng chung (useMediaQuery, useClickOutside)
 ├─ layouts/        # Layout tổng thể (Sidebar, Header, MainContent container)
 ├─ pages/          # Thành phần gốc map routing URL với các Features. Rất ít logic.
 ├─ routes/         # Khai báo React Router DOM v6
 ├─ services/       # Nơi tập trung cấu hình Axios/Fetch, định nghĩa Endpoints REST Api
 ├─ store/          # Cấu hình Global State (Zustand/Redux) 
 └─ utils/          # Hàm dùng chung không gắn với React (date formatter, math calc)
```

## 2. Component Design & Hook Rules
- **React Hooks**: Code mới dùng 100% functional component + hooks. CẤM dùng class Components.
- **Hook Rules**: Hook chỉ được khởi tạo trên top level scope. Custom hooks phải bắt đầu bằng chữ `use...`.
- **Dumb vs Smart Component**: 
  - Dumb Component: Chỉ nhận `props` để vẽ UI, xuất events bằng callback `onChange`. Tránh tự nó thực thi logic side effect.
  - Smart Component (Container): Quản lý state ngầm, thực hiện load dữ liệu `useEffect` hoặc Query.

## 3. Quản lý trạng thái (State Management)
- **Local State** (`useState`, `useReducer`): Dùng cho UI toggle như Open/Close modal, collapse accordion, form inputs.
- **Server Cache State** (SWR hoặc React Query / TanStack Query): Tất cả các lời gọi get Data API phải gói qua thư viện Caching để tránh giật lác re-fetch (đặc biệt là bảng Lịch Session Timetable hay Attendance).
- **Global UI State**: Dùng `Context API` hoặc `Zustand` (Giỏ hàng, User Session, Theme Dark/Light...). Giải phóng não khỏi việc đẩy Props drilling quá sâu.

## 4. Xử lý API Calls và Axios Instances
- Build sẵn một `Axios Instance` bắt interceptors. Tự động mớm token hiện tại (nếu có) vào request headers Header `Authorization: Bearer <token>`.
- Nếu response về mã `401 Unauthorized`, interceptor tự động bắt và redirect user về trang đăng nhập hoặc đá refreshToken.

## 5. CSS, Styling & UI Library
Sử dụng **Tailwind CSS** là cốt lõi để duy trì tốc độ code. 
Tránh CSS inline lằng nhằng hoặc đẻ quá nhiều file `.css` lộn xộn. (Trừ các custom keyframes không chạy được ở TW).
Khi cần xây dashboard nhanh có thể xài bộ thư viện kèm theo tích hợp tốt Tailwind (Headless UI hoặc Shadcn UI) để xây hệ thống Menu, Table, Dropdown nhất quán.

## 6. TypeScript Typing & Khước từ `any`
Hệ thống là TypeSafe.
- BẮT BUỘC định nghĩa Interface/Type rõ ràng cho bất cứ parameters/props nào.
- KHUYẾN CÁO KHÔNG DÙNG kiểu `any` lười biếng. Nếu chưa biết cấu trúc data trả về, sử dụng kiểu `unknown` rồi check typeof / Type Guards bảo vệ, thay vì type cast bất chấp sang `any` gây hiểm hoạ runtime bugs.
