# Chat Conversation

### User Input Promt 1 - tạo file testing context

[Layered Context Structure] - Testing QA

Với vai trò Senior QA cho dự án ATS

Context: ùng làm đầu vào cho AI thực hiện các công việc trong testing

Requirements:
- Hãy tạo testing_conventions.md lưu vào folder @[Applicant_Tracking_System/.ai-context]
- Sử dụng XML tag với các thông tin đầy đủ về tech stack cho từng test levels (UT, IT, ST), testing rules, output rules, naming conventions
- Output tương ứng cho từng levels theo templates sau @[Applicant_Tracking_System/Templates/Report5.1_Unit Test.xls]@[Applicant_Tracking_System/Templates/Report5.2_Integration Test.xlsx]@[Applicant_Tracking_System/Templates/Report5.3_System Test.xlsx] 


### User Input promt 2

Tách thành các file độc lập theo test levels, những thông tin chung thì giữ trong testing_conventions.md

### Output from AI -
---------------
Done! Here's the final structure:

```
.ai-context/
├── testing_conventions.md              ← Shared: index, global rules, output rules, AI workflow
└── testing_conventions/
    ├── 01_unit_test.md                 ← UT: tech stack, naming, rules, Report5.1 format
    ├── 02_integration_test.md          ← IT: tech stack, naming, rules, Report5.2 format
    └── 03_system_test.md              ← ST: tech stack, naming, rules, Report5.3 format
```


### User Input Promt 3 - Viết test case Unit Test

[Step 1: Test Design] Viết test case Unit Test
@[Applicant_Tracking_System/.ai-context/testing_conventions/01_unit_test.md]  @[Applicant_Tracking_System/.ai-context/testing_conventions.md] 

Requirements:
- Viết đầy đủ test case cho method getPublishedJobs() trong Service Layer của UC-02.2 Publish Job Listing tại sheet "getPublishedJobs" trong file @[Applicant_Tracking_System/Testing/01_UT/ATP_Unit Test_v0.1.xls] 


### User Input Promt 4 - Promt sau tinh chỉnh viết test case UT

[Step 1: Test Design] Viết Unit Test Case

Context:
- File mô tả nghiệp vụ: 01_unit_test.md
- Quy ước test: testing_conventions.md
- Use Case: UC-02.2 Publish Job Listing
- Method cần test: getPublishedJobs() (Service Layer)
- Output phải match template sheet "getPublishedJobs" trong file ATP_Unit Test_v0.1.xls xuất file định dạng html và lưu vào folder @[Applicant_Tracking_System/Testing/01_UT]

Requirements:

1. Viết test case theo EXACT structure của template gồm các section:
   - Condition
       - Precondition
   - Input
       - Date
       - Month
       - Year
   - Confirm
       - Return
       - Exception
       - Log message
   - Result
       - Type (N/A/B)
       - Passed/Failed
       - Executed Date
       - Defect ID

2. Mỗi test case phải là 1 column (UTCD01, UTCD02,...)

3. Phải bao phủ:
   - Normal cases
   - Boundary cases
   - Abnormal cases
   - Null input
   - Invalid date
   - Month boundary
   - Year boundary
   - Server connection condition
   - Return True/False
   - Exception scenario
   - Log message validation

4. Mapping ký hiệu:
   - "O" = giá trị được sử dụng trong test case
   - Blank = không áp dụng
   - Type:
       N = Normal
       A = Abnormal
       B = Boundary

5. Output format:
   - Trả về dưới dạng Markdown Table
   - Layout phải giống Excel matrix (column-based test design)
   - Không viết narrative text
   - Không giải thích
   - Chỉ output bảng test case

6. Số lượng test case:
   - Tối thiểu 12 test cases
   - Bao gồm boundary date (29,30,31)
   - Boundary month (2,3,4)
   - Boundary year (2000,2009)


### User Input Promt 5 - Viết Intergration Test Case

[Step 1 - Test Case Design] Intergration Test
@[Applicant_Tracking_System/.ai-context/testing_conventions/02_integration_test.md] @[Applicant_Tracking_System/.ai-context/testing_conventions.md] 

Requirements:
- Viết test case thực hiện Intergration Testing giữa Controller và Service Layer cho feature "Job Management"

Output:
- Điền các test case vào sheet "Job Management" đảm bảo đúng cấu trúc, format (font size, colum, backgound color, ..) tại file @[Applicant_Tracking_System/Testing/02_IT/ATS_Integration Test_v0.1.xlsx] 
- Cấu trúc mỗi test case đầy đủ các thông tin chính xác như mẫu sau: 



### User Input Promt 6 - Viết code test Integration

[Step 2 - Test Code] Intergration Test
@[Applicant_Tracking_System/.ai-context/testing_conventions/02_integration_test.md] @[Applicant_Tracking_System/.ai-context/testing_conventions.md] 

Context:
- Sau khi đã có bộ test case trong file @[Applicant_Tracking_System/Testing/02_IT/ATS_Integration Test_v0.1.xlsx] 

Requirements:
- Hãy thực hiện viết Test code cho tất cả các test case được định nghĩa như trong file 
