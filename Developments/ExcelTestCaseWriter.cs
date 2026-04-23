using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;

public class ExcelTestCaseWriter
{
    public static void Main()
    {
        EPPlus.LicenseContext.SetLicenseKey("YOUR_LICENSE_KEY_HERE");
        
        string filePath = @"d:\Back up D and E\Work\Back up\NET\KI 8 _ Spring 2026\Block 5 _ SP2026\AI\Team Project\CLS-Classroom-Management-System\Report5.1_Unit Test.xls";
        
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            // Get or create sheet "methodName1"
            var worksheet = package.Workbook.Worksheets["methodName1"];
            
            if (worksheet == null)
            {
                worksheet = package.Workbook.Worksheets.Add("methodName1");
            }

            // Define test cases
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    Id = 1,
                    Condition = "Create new learner with valid data (FirstName, LastName, Email, PhoneNumber), Parent email is valid format",
                    Confirm = "LearnerService.CreateLearnerAsync(learnerDto) is called with valid LearnerDto object",
                    Result = "PASS - Learner entity is created in database, Returns learner ID > 0, LearnerStatus = Pending",
                    ExecutedBy = "QA_Unit_Test_01"
                },
                new TestCase
                {
                    Id = 2,
                    Condition = "Create learner with empty FirstName field, other fields are valid",
                    Confirm = "LearnerService.CreateLearnerAsync(learnerDto) is called with empty FirstName",
                    Result = "FAIL/EXCEPTION - ValidationException thrown with message 'First name is required', Learner NOT created in database",
                    ExecutedBy = "QA_Unit_Test_02"
                },
                new TestCase
                {
                    Id = 3,
                    Condition = "Create learner with invalid email format (e.g., 'invalid.email'), other fields valid",
                    Confirm = "LearnerService.CreateLearnerAsync(learnerDto) is called, Email validation is triggered",
                    Result = "FAIL/EXCEPTION - ValidationException thrown with message 'Invalid email format', Learner NOT created",
                    ExecutedBy = "QA_Unit_Test_03"
                },
                new TestCase
                {
                    Id = 4,
                    Condition = "Get learner by ID that exists in database (ID = 1), Learner status = Active",
                    Confirm = "LearnerService.GetLearnerByIdAsync(1) is called, Mock repository returns valid learner",
                    Result = "PASS - Returns LearnerDto with matching ID, Name, Email populated correctly, Status = Active",
                    ExecutedBy = "QA_Unit_Test_04"
                },
                new TestCase
                {
                    Id = 5,
                    Condition = "Get learner by ID that does NOT exist (ID = 999), Database is mocked to return null",
                    Confirm = "LearnerService.GetLearnerByIdAsync(999) is called, Mock repository returns null",
                    Result = "FAIL/EXCEPTION - NotFoundException thrown with message 'Learner with ID 999 not found'",
                    ExecutedBy = "QA_Unit_Test_05"
                },
                new TestCase
                {
                    Id = 6,
                    Condition = "Update learner status from Pending to Active, Learner exists in database",
                    Confirm = "LearnerService.UpdateLearnerStatusAsync(learnerId, LearnerStatus.Active) is called",
                    Result = "PASS - Learner status updated to Active, SaveChanges() called, Returns true",
                    ExecutedBy = "QA_Unit_Test_06"
                },
                new TestCase
                {
                    Id = 7,
                    Condition = "Delete learner record from database, Learner ID = 1 exists, No active sessions assigned",
                    Confirm = "LearnerService.DeleteLearnerAsync(1) is called, Mock repository removes entity",
                    Result = "PASS - Learner deleted successfully, SaveChanges() called, Returns true",
                    ExecutedBy = "QA_Unit_Test_07"
                },
                new TestCase
                {
                    Id = 8,
                    Condition = "Get all learners from database, Database contains 5 active learners, No filter applied",
                    Confirm = "LearnerService.GetAllLearnersAsync() is called, Mock repository returns IEnumerable of 5 learners",
                    Result = "PASS - Returns list with count = 5, All learner data populated correctly",
                    ExecutedBy = "QA_Unit_Test_08"
                }
            };

            // Add headers if row 1 is empty
            if (worksheet.Cells["A1"].Value == null)
            {
                worksheet.Cells["A1"].Value = "Test Case ID";
                worksheet.Cells["B1"].Value = "Condition";
                worksheet.Cells["C1"].Value = "Confirm";
                worksheet.Cells["D1"].Value = "Result";
                worksheet.Cells["E1"].Value = "Executed By";
                worksheet.Cells["F1"].Value = "Total Test Cases";
            }

            // Add test case data
            int row = 2;
            foreach (var testCase in testCases)
            {
                worksheet.Cells[$"A{row}"].Value = testCase.Id;
                worksheet.Cells[$"B{row}"].Value = testCase.Condition;
                worksheet.Cells[$"C{row}"].Value = testCase.Confirm;
                worksheet.Cells[$"D{row}"].Value = testCase.Result;
                worksheet.Cells[$"E{row}"].Value = testCase.ExecutedBy;
                worksheet.Cells[$"F{row}"].Value = testCases.Count;

                // Auto-fit columns
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit(50); // Max width 50
                worksheet.Column(3).AutoFit(50);
                worksheet.Column(4).AutoFit(50);
                worksheet.Column(5).AutoFit();
                worksheet.Column(6).AutoFit();

                row++;
            }

            package.Save();
            Console.WriteLine($"✓ Successfully added {testCases.Count} test cases to methodName1 sheet");
            Console.WriteLine($"File saved: {filePath}");
        }
    }

    public class TestCase
    {
        public int Id { get; set; }
        public string Condition { get; set; }
        public string Confirm { get; set; }
        public string Result { get; set; }
        public string ExecutedBy { get; set; }
    }
}
