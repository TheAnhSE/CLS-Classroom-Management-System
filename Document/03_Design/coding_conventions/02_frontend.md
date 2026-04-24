# 02. Frontend Development Standards (React + Vite)

## 1. Core Architecture: Modular & Infrastructure-First
Frontend CLS sử dụng kiến trúc **Feature-Sliced Design (FSD)** kết hợp **Vertical Slice**. Mọi logic, component, state của một nghiệp vụ sẽ nằm chung một thư mục.

### Cấu trúc thư mục chuẩn
```
src/
├── app/                  # App shell, global routing, global layouts
├── assets/               # Hình ảnh, fonts, icons (Static files)
├── shared/               # Code dùng chung (UI components, utils, hooks, api client)
│   ├── components/       # Dumb components (Button, Input, Table...)
│   ├── services/         # api.client.ts (Axios wrapper)
│   ├── types/            # Global types (ApiResponse, PageResponse)
│   └── utils/            # Helper functions
├── features/             # Vertical Slices (Nghiệp vụ cốt lõi)
│   ├── auth/             # Login, Token management
│   ├── learners/         # Learner List, Enroll Learner
│   ├── packages/         # Package Management
│   ├── sessions/         # Scheduling, Timetable
│   └── attendances/      # Roster, Mark Attendance
└── styles/               # Global CSS (Tailwind entry)
```

## 2. Công nghệ Cốt lõi
- **Framework**: React 19 + Vite.
- **Language**: TypeScript (Strict mode).
- **State Management**: Zustand (ưu tiên) hoặc React Context. KHÔNG dùng Redux.
- **Data Fetching**: Axios.
- **Styling**: TailwindCSS 4 + Shadcn UI (để có các UI Component đẹp, chuẩn).
- **Routing**: React Router v7 (hoặc v6).

## 3. Quy chuẩn viết Code (Anti-Patterns cần tránh)

### 3.1. Phân tách Smart / Dumb Component
- **Dumb Component** (ở `src/shared/components/`): Chỉ nhận Props và render UI. TUYỆT ĐỐI không chứa logic gọi API hay truy xuất Zustand Store.
- **Smart Component** (ở `src/features/...`): Chứa logic gọi API, quản lý State, và truyền data xuống Dumb Component.

### 3.2. KHÔNG gọi API trực tiếp trong UI Component
- **Anti-pattern**: Dùng `axios.get` trực tiếp trong `useEffect` của file `.tsx`.
- **Chuẩn mực**:
  1. Viết `[Feature]Service.ts` chứa các hàm fetch data.
  2. Dùng custom hook (VD: `useLearners()`) bọc lại logic gọi service, xử lý trạng thái Loading/Error.
  3. Component UI chỉ gọi hook để lấy data và render.

### 3.3. Xử lý API Response đồng nhất
Tất cả dữ liệu từ Backend sẽ trả về định dạng `ApiResponse<T>`. Phải có một file `api.client.ts` xử lý Unwrap dữ liệu tự động qua Interceptors để Component không cần gọi `res.data.data`.

### 3.4. Không Lạm Dụng Prop Drilling
Nếu phải truyền prop qua nhiều hơn 3 lớp component, BẮT BUỘC phải chuyển sang dùng Zustand Store hoặc React Context.

## 4. UI/UX & Styling Guidelines
- Sử dụng các lớp utility của Tailwind thay vì viết CSS chay.
- Sử dụng bảng màu chuyên nghiệp (Tránh đỏ tươi, xanh chói).
- Mọi component phải thiết kế Responsive (Hỗ trợ Mobile & Desktop).
- Bắt buộc phải có Skeleton Loading (khi đang fetch data) và Empty State (khi mảng dữ liệu rỗng).
