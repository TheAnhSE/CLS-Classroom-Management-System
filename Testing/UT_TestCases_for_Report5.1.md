# Unit Test Cases - Report5.1_Unit Test.xls (methodName1 sheet)

## Test Case Data to Fill in Sheet methodName1

### Test Case Structure:
```
Columns: Condition | Confirm | Result | Executed By | Total Test Cases
```

---

## TEST CASE 1: Should_CreateLearner_WhenValidDataProvided

| Column | Value |
|--------|-------|
| **Condition** | Create new learner with valid data (FirstName, LastName, Email, PhoneNumber), Parent email is valid format |
| **Confirm** | LearnerService.CreateLearnerAsync(learnerDto) is called with valid LearnerDto object |
| **Result** | PASS - Learner entity is created in database, Returns learner ID > 0, LearnerStatus = Pending |
| **Executed By** | QA_Unit_Test_01 |
| **Total Test Cases** | 1 |

---

## TEST CASE 2: Should_ThrowValidationException_WhenFirstNameIsEmpty

| Column | Value |
|--------|-------|
| **Condition** | Create learner with empty FirstName field, other fields are valid |
| **Confirm** | LearnerService.CreateLearnerAsync(learnerDto) is called with empty FirstName |
| **Result** | FAIL/EXCEPTION - ValidationException thrown with message "First name is required", Learner NOT created in database, Transaction rollback |
| **Executed By** | QA_Unit_Test_02 |
| **Total Test Cases** | 1 |

---

## TEST CASE 3: Should_ThrowValidationException_WhenEmailFormatIsInvalid

| Column | Value |
|--------|-------|
| **Condition** | Create learner with invalid email format (e.g., "invalid.email"), other fields valid |
| **Confirm** | LearnerService.CreateLearnerAsync(learnerDto) is called, Email validation is triggered |
| **Result** | FAIL/EXCEPTION - ValidationException thrown with message "Invalid email format", Learner NOT created, Transaction rollback |
| **Executed By** | QA_Unit_Test_03 |
| **Total Test Cases** | 1 |

---

## TEST CASE 4: Should_ReturnLearner_WhenValidIdProvided

| Column | Value |
|--------|-------|
| **Condition** | Get learner by ID that exists in database (ID = 1), Learner status = Active |
| **Confirm** | LearnerService.GetLearnerByIdAsync(1) is called, Mock repository returns valid learner |
| **Result** | PASS - Returns LearnerDto with matching ID, Name, Email populated correctly, Status = Active |
| **Executed By** | QA_Unit_Test_04 |
| **Total Test Cases** | 1 |

---

## TEST CASE 5: Should_ThrowNotFoundException_WhenLearnerIdNotFound

| Column | Value |
|--------|-------|
| **Condition** | Get learner by ID that does NOT exist (ID = 999), Database is mocked to return null |
| **Confirm** | LearnerService.GetLearnerByIdAsync(999) is called, Mock repository returns null |
| **Result** | FAIL/EXCEPTION - NotFoundException thrown with message "Learner with ID 999 not found", Returns null, No exception propagated |
| **Executed By** | QA_Unit_Test_05 |
| **Total Test Cases** | 1 |

---

## TEST CASE 6: Should_UpdateLearnerStatus_WhenValidStatusProvided

| Column | Value |
|--------|-------|
| **Condition** | Update learner status from Pending to Active, Learner exists in database, UpdateLearnerAsync(id, status) called |
| **Confirm** | LearnerService.UpdateLearnerStatusAsync(learnerId, LearnerStatus.Active) is called, Mock repository updates entity |
| **Result** | PASS - Learner status updated to Active, SaveChanges() called, Returns true, Updated timestamp recorded |
| **Executed By** | QA_Unit_Test_06 |
| **Total Test Cases** | 1 |

---

## TEST CASE 7: Should_DeleteLearner_WhenLearnerExists

| Column | Value |
|--------|-------|
| **Condition** | Delete learner record from database, Learner ID = 1 exists, No active sessions assigned |
| **Confirm** | LearnerService.DeleteLearnerAsync(1) is called, Mock repository removes entity |
| **Result** | PASS - Learner deleted successfully, SaveChanges() called, Returns true, Verify(mockRepository.DeleteAsync) called once |
| **Executed By** | QA_Unit_Test_07 |
| **Total Test Cases** | 1 |

---

## TEST CASE 8: Should_ReturnAll_WhenGetAllLearnersInvoked

| Column | Value |
|--------|-------|
| **Condition** | Get all learners from database, Database contains 5 active learners, No filter applied |
| **Confirm** | LearnerService.GetAllLearnersAsync() is called, Mock repository returns IEnumerable of 5 learners |
| **Result** | PASS - Returns list with count = 5, All learner data populated correctly, No null values in collection |
| **Executed By** | QA_Unit_Test_08 |
| **Total Test Cases** | 1 |

---

## Summary Statistics
- **Total Unit Test Cases**: 8
- **Positive Test Cases**: 4 (PASS results)
- **Negative Test Cases**: 4 (FAIL/EXCEPTION results)
- **Feature Coverage**: LearnerService CRUD operations
- **Coverage Target**: 80%+ for critical business logic paths

---

## How to Fill in Excel Template:

1. Open Report5.1_Unit Test.xls
2. Go to sheet "methodName1"
3. Copy each test case row into the corresponding columns:
   - Column A: Condition
   - Column B: Confirm
   - Column C: Result
   - Column D: Executed By
   - Column E: Total Test Cases

4. Fill test case numbers in appropriate cells
5. Save file

**Note**: Ensure you maintain the same formatting and structure as the existing template to preserve test report integrity.
