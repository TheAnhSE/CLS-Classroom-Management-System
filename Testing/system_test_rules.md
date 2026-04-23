# System Test (ST) Rules - CLS Classroom Management System

## Overview
System Tests verify the entire system functionality end-to-end. They test complete user workflows, system behavior, UI interactions, and business processes in an environment that closely mimics production.

<tech-stack>
    <ui-automation>Selenium WebDriver 4.x, Appium (if mobile)</ui-automation>
    <api-testing>Postman, REST Assured, RestSharp</api-testing>
    <framework>SpecFlow for BDD, NUnit with Selenium</framework>
    <language>C# or Python</language>
    <environment>Staging or UAT environment mirroring production</environment>
    <data-setup>Database scripts, API automation for data preparation</data-setup>
    <performance-tools>JMeter, LoadRunner (if load testing)</performance-tools>
    <reporting>Allure, ExtentReports, Custom dashboards</reporting>
</tech-stack>

## Testing Rules

| Rule | Description |
|------|-------------|
| Scope | Test complete user workflows and system behavior end-to-end |
| Environment | Execute in staging/UAT that mirrors production exactly |
| Validation | Include UI, API, and database validations |
| Use Cases | Follow actual user stories and business scenarios |
| Test Data | Use realistic production-like data |
| Performance | Monitor response times and system behavior under load |
| Edge Cases | Test boundary conditions, errors, and failure scenarios |
| Automation | Automate repeatable tests; manual testing for exploratory |
| Regression | Maintain regression test suite for major features |
| Evidence | Capture screenshots, logs, and videos for failed tests |

## Naming Conventions

### Test Class Names
- Pattern: `[Feature]SystemTests` or `[UserStory]Tests`
- Examples:
  - `ClassroomSystemTests`
  - `TeacherLearnerManagementTests`
  - `AdminAcademicProcessTests`

### Test Method Names
- Pattern: `Test_[UserStory]_[Scenario]` or `Should_[CompleteWorkflow]_[Outcome]`
- Examples:
  - `Test_TeacherCreatesClassroom_AndAssignsStudents`
  - `Test_ParentViewsChildProgress_AndDownloadsReport`
  - `Test_AdminManagesAcademicTerms_AndPublishesSchedule`
  - `Should_FailGracefully_WhenDatabaseConnectionLost`

### Variable Names
- Page objects: `loginPage`, `classroomPage`, `reportPage`
- WebDriver elements: `driver`, `element`, `webDriverWait`
- Test data: `testUser`, `testClassroom`, `testData`

## Test Structure Template

### UI Automation Test Example (Selenium)
```csharp
[TestFixture]
public class TeacherLearnerManagementTests
{
    private IWebDriver _driver;
    private WebDriverWait _wait;
    private LoginPage _loginPage;
    private ClassroomPage _classroomPage;

    [SetUp]
    public void Setup()
    {
        _driver = new ChromeDriver();
        _driver.Navigate().GoToUrl("https://staging-cls.example.com");
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        _loginPage = new LoginPage(_driver);
        _classroomPage = new ClassroomPage(_driver);
    }

    [Test]
    public void Test_TeacherCreatesClassroom_AndAssignsLearners()
    {
        // Authenticate
        _loginPage.Login("teacher@example.com", "password");
        
        // Create Classroom
        _classroomPage.ClickCreateClassroom();
        _classroomPage.EnterClassroomName("Grade 10 - Math");
        _classroomPage.EnterCapacity("30");
        _classroomPage.ClickSave();
        
        // Verify Classroom Created
        Assert.That(_classroomPage.GetSuccessMessage(), 
            Contains.Substring("Classroom created successfully"));
        
        // Add Learners
        _classroomPage.ClickAddLearners();
        _classroomPage.SelectLearners(new[] { "John Doe", "Jane Smith" });
        _classroomPage.ClickAssign();
        
        // Verify Learners Added
        var addedLearners = _classroomPage.GetAssignedLearners();
        Assert.That(addedLearners, Contains.Item("John Doe"));
        Assert.That(addedLearners, Contains.Item("Jane Smith"));
    }

    [TearDown]
    public void TearDown()
    {
        _driver?.Quit();
    }
}
```

### Page Object Model
```csharp
public class ClassroomPage
{
    private IWebDriver _driver;
    private IWebElement _createButton => _driver.FindElement(By.Id("btnCreateClassroom"));
    private IWebElement _nameInput => _driver.FindElement(By.Name("classroomName"));

    public ClassroomPage(IWebDriver driver)
    {
        _driver = driver;
    }

    public void ClickCreateClassroom() => _createButton.Click();
    public void EnterClassroomName(string name) => _nameInput.SendKeys(name);
}
```

### API-based System Test Example
```csharp
[Test]
public async Task Test_CompleteStudentEnrollmentWorkflow()
{
    // Step 1: Admin creates classroom
    var classroomResponse = await _apiClient.PostAsync(
        "/api/classrooms",
        new { name = "Grade 10 - Science", capacity = 30 }
    );
    Assert.That(classroomResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    var classroomId = classroomResponse.Data.Id;

    // Step 2: Teacher assigns learners
    var assignResponse = await _apiClient.PostAsync(
        $"/api/classrooms/{classroomId}/learners",
        new { learnerIds = new[] { 1, 2, 3 } }
    );
    Assert.That(assignResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    // Step 3: Verify database persistence
    using var dbContext = new ApplicationDbContext();
    var classroom = await dbContext.Classrooms
        .Include(c => c.Learners)
        .FirstAsync(c => c.Id == classroomId);
    Assert.That(classroom.Learners.Count, Is.EqualTo(3));
}
```

## Output Rules

### Report Format
- Generate reports with detailed screenshots and logs
- Export results to **Excel** format for traceability and sign-off
- Template: **Report5.3_System Test.xlsx**
- Include video recordings of failed tests (optional)

### Report Columns
| Column | Description |
|--------|-------------|
| Test Case ID | Unique identifier for the test |
| Scenario | Complete user workflow being tested |
| Steps | Step-by-step actions performed |
| Expected Result | What should happen on successful execution |
| Actual Result | What actually occurred |
| Status | Pass/Fail/Blocked |
| Execution Time | Duration in seconds |
| Screenshots | Links to screenshot evidence |
| Defects | Associated bug IDs if failed |
| Remarks | Additional notes or observations |

### Report Generation Commands
```bash
# Using ExtentReports
dotnet test --logger "extentreports;LogFileName=st-report.html"

# Using Allure
dotnet test
allure generate --clean -o allure-report

# CSV Export
Export-TestResults -Path "st-results.xlsx" -Format Excel
```

## Test Scenarios & Coverage

### User Story Example
**User Story**: Teacher manages classroom attendance

**System Test Scenarios**:
1. `ST001`: Teacher marks single learner present
2. `ST002`: Teacher marks multiple learners absent
3. `ST003`: Teacher adds late learner after attendance closed
4. `ST004`: System prevents duplicate attendance records
5. `ST005`: Attendance report generates correctly
6. `ST006`: System handles network disconnect during attendance

## BDD Approach with SpecFlow

```gherkin
Feature: Teacher Manages Classroom
  As a Teacher
  I want to manage my classroom roster
  So that I can track student attendance and performance

  Scenario: Teacher views classroom roster
    Given I am logged in as a teacher
    When I navigate to my classroom
    Then I should see all assigned learners
    And the roster should show learner status
```

## Best Practices

1. **Test Independence**: Each test should be independent and runnable in any order
2. **Explicit Waits**: Use explicit waits instead of sleeps
3. **Page Objects**: Organize UI interactions into page object classes
4. **Test Data**: Use factories or builders for complex test data
5. **Error Handling**: Test error scenarios and user error handling
6. **Accessibility**: Include basic accessibility testing (WCAG compliance)
7. **Performance**: Monitor system performance during test execution
8. **Stability**: Use reliable locators; avoid brittle UI selectors
9. **Maintenance**: Keep tests simple and maintainable
10. **Documentation**: Document complex workflows and test logic

## Test Execution Schedule

| Trigger | Tests to Run | Frequency |
|---------|--------------|-----------|
| Every Commit | Smoke tests (critical paths) | Continuous |
| Daily | Full regression suite | Nightly |
| Release | Complete system tests + performance | As needed |
| UAT | Full suite with client scenarios | 1-2 weeks before release |

## CI/CD Integration

- Execute smoke tests on every commit
- Run full regression suite nightly
- Execute system tests in parallel where possible
- Generate detailed HTML reports with screenshots
- Archive test artifacts (logs, videos) for failed tests
- Fail build if critical system tests fail
- Notify team about test results via Slack/Email

## Performance Testing Guidelines

If performance tests are included:
- Define acceptable response times for each API
- Set load thresholds (concurrent users, requests/second)
- Monitor CPU, memory, and database metrics
- Document baseline performance metrics
- Run performance tests in staging environment
- Compare results across releases to detect regressions

