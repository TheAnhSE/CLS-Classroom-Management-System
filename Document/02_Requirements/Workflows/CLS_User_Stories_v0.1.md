# CLS System - User Stories Specification (v0.1)

This document details the User Stories derived from the System Requirements Specification (SRS) and the established Use Cases mapping to the 3-week MVP scope.

---

## 1. Authentication & Security

### US-01: System Login
* **As a** System User (Academic Admin, Teacher, Center Director)
* **I want to** log into the CLS platform securely using my credentials
* **So that** I can access my role-specific dashboard and functions.
**Acceptance Criteria:**
* System verifies username (email) and password against the database.
* Upon success, redirects Teacher to Teacher Dashboard, Admin to Admin Dashboard, etc.
* Shows clear error message for wrong credentials.

### US-02: Password Reset
* **As a** System User
* **I want to** request a password reset link via my registered email
* **So that** I can regain access to my account if I forget my password.
**Acceptance Criteria:**
* Password reset link expires after 24 hours.
* Must enforce strong password policies on reset.

---

## 2. Learner Management

### US-03: Create Learner Profile
* **As an** Academic Admin
* **I want to** create a new learner profile and link it to a Parent/Sponsor
* **So that** the center can officially enroll the student and communicate with their family.
**Acceptance Criteria:**
* Must require mandatory fields (Name, DOB, Parent Email, Parent Phone).
* Validates that the Parent Email format is correct for notification dispatch.

### US-04: View & Search Learners
* **As an** Academic Admin or Center Director
* **I want to** search and view the list of learners
* **So that** I can quickly pull up a student's academic history or contact details.
**Acceptance Criteria:**
* Can search by Learner Name, Parent Name, or Phone Number.
* Shows active/inactive status and current package balance.

---

## 3. Learning Package Management

### US-05: Define Package Master Data
* **As an** Academic Admin
* **I want to** define new learning packages (e.g., 20 sessions for Kids Coding)
* **So that** they are available for purchase and assignment.
**Acceptance Criteria:**
* Package must have a name, subject, number of sessions, and tuition fee.

### US-06: Assign Package to Learner
* **As an** Academic Admin
* **I want to** assign a purchased learning package to a specific learner
* **So that** the system can begin deducting sessions via attendance.
**Acceptance Criteria:**
* Initializes the "remaining sessions" counter.
* Logs the assignment date and who authorized it.

### US-07: Low Session Auto-Warning
* **As a** Parent
* **I want to** receive an automated email notification when my child's package reaches a critical low (e.g., <= 2 sessions)
* **So that** I have time to process the renewal payment without interrupting my child's learning.
**Acceptance Criteria:**
* System scans package balances daily.
* Triggers Email Service integration exactly when balance hits the configured threshold.
* Notifies the Academic Admin on the dashboard simultaneously.

---

## 4. Session Scheduling & Conflict Prevention

### US-08: Schedule Class Session
* **As an** Academic Admin
* **I want to** schedule a session by selecting a Time Slot, Classroom, Teacher, and Subject
* **So that** the academic week is properly planned out.
**Acceptance Criteria:**
* Form must include Date, Start/End Time, Room, and Teacher assignment.

### US-09: Prevent Scheduling Conflicts (Double Booking)
* **As an** Academic Admin
* **I want to** be blocked or warned by the system if I schedule a Teacher or Classroom that is already booked for that specific time block
* **So that** the center guarantees zero scheduling errors.
**Acceptance Criteria:**
* `<<Include>>` relation triggers automatically on Save.
* System displays explicit error: "Conflict Detected: Teacher [Name] is already teaching Session-[ID] in Room [Name]."

### US-10: View Personal Timetable
* **As a** Teacher
* **I want to** view my upcoming teaching schedule on a calendar or list format
* **So that** I know exactly which room to go to and what subject to teach.

---

## 5. Attendance Management

### US-11: Record Session Attendance
* **As a** Teacher (or Academic Admin)
* **I want to** mark the attendance (Present/Absent/Late) for all learners enrolled in my session
* **So that** the center has accurate records of student participation.
**Acceptance Criteria:**
* Completing attendance automatically deducts 1 session from the Learner's active Learning Package.

### US-12: Attendance Notification
* **As a** Parent
* **I want to** automatically receive an email when my child is marked Absent or Late
* **So that** I can track my child's physical safety and compliance.
**Acceptance Criteria:**
* Email is dispatched immediately upon saving an "Absent" or "Late" record.

---

## 6. Academic Feedback & SLA

### US-13: Submit Post-Session Feedback
* **As a** Teacher
* **I want to** write and submit a brief academic evaluation for each student after a session
* **So that** parents are kept informed of the learning progress.
**Acceptance Criteria:**
* Form includes a performance rating and text notes.
* Submission must be mobile-friendly to support quick typing on phones.

### US-14: Feedback 12-Hour SLA Tracking
* **As an** Academic Admin / Center Director
* **I want to** the system to track whether feedback was submitted within 12 hours after the session ended
* **So that** we maintain premium service standards.
**Acceptance Criteria:**
* Feedback submitted after (Session End Time + 12h) is flagged as "Overdue" in reports.
* Teachers receive an automated reminder if SLA is approaching.

### US-15: Feedback Notification to Parent
* **As a** Parent
* **I want to** receive an automated email containing the Teacher's feedback right after it is submitted
* **So that** I have zero-touch communication regarding my child's performance.

---

## 7. Dashboard & Reporting

### US-16: View Director Dashboard
* **As a** Center Director
* **I want to** view high-level metrics including active learners, package depletion rates, and SLA compliance
* **So that** I can monitor business health and step in to prevent churn.
**Acceptance Criteria:**
* Metrics update in real-time or near real-time.
* Visual indicators (Red/Yellow/Green) for SLA violations.

### US-17: Admin Operational Dashboard
* **As an** Academic Admin
* **I want to** see a list of today's classes and pending package renewals
* **So that** I can execute daily operations efficiently and reclaim time previously spent checking Excel.

### US-18: Export Reports
* **As a** Center Director
* **I want to** export financial or attendance reports to CSV/Excel
* **So that** I can use the data for accounting and center-external analysis.
