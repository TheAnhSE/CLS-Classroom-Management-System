# Integration Test (IT) Rules - CLS Classroom Management System

## Overview
Integration Tests verify that different components work together correctly. They test the interaction between services, repositories, databases, and external systems.

<tech-stack>
    <framework>xUnit.net with TestServer or NUnit</framework>
    <tools>ASP.NET Core Test Host, WebApplicationFactory</tools>
    <testing-apis>Postman, REST Assured, HttpClient</testing-apis>
    <database>In-memory database (SQLite) or test database</database>
    <language>C#</language>
    <mocking-external-services>Moq for third-party APIs</mocking-external-services>
</tech-stack>

## Testing Rules

| Rule | Description |
|------|-------------|
| Scope | Test API endpoints, database interactions, and service integrations |
| Dependencies | Use real dependencies (database, file system) where safe; mock external APIs |
| Test Cases | Include both positive (happy path) and negative (error) scenarios |
| Data Flow | Validate complete data flow from request → processing → database → response |
| Error Handling | Verify error handling and exception propagation |
| Isolation | Use fresh database state for each test (setup/teardown) |
| Environment | Run in isolated environment to prevent side effects |
| Performance | Each test should complete in < 5 seconds |
| Transactions | Use transaction rollback or database reset after each test |
| Logging | Verify appropriate logging and error messages |

## Naming Conventions

### Test Class Names
- Pattern: `[Feature]IntegrationTests`
- Examples:
  - `LearnerIntegrationTests`
  - `ClassroomIntegrationTests`
  - `TeacherLearnerIntegrationTests`

### Test Method Names
- Pattern: `Should_[Action]_[Scenario]` or `Test_[UserStory]_[Scenario]`
- Examples:
  - `Should_CreateLearner_WhenValidDataProvided`
  - `Should_ReturnBadRequest_WhenInvalidDataProvided`
  - `Should_UpdateLearnerStatus_AndPersistToDatabase`
  - `Should_ThrowException_WhenDatabaseConnectionFails`

### Variable Names
- Use descriptive names: `requestData`, `response`, `createdEntity`
- Database context: `dbContext`, `testDb`
- HTTP client: `httpClient`, `apiClient`

## Test Structure Template

### API Endpoint Test Example
```csharp
[Fact]
public async Task Should_CreateLearner_WhenValidDataProvided()
{
    // Arrange
    using var client = _factory.CreateClient();
    var learnerDto = new LearnerDto 
    { 
        FirstName = "John", 
        LastName = "Doe", 
        Email = "john@example.com" 
    };
    var content = new StringContent(
        JsonSerializer.Serialize(learnerDto),
        Encoding.UTF8,
        "application/json"
    );

    // Act
    var response = await client.PostAsync("/api/learners", content);

    // Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    var responseData = await response.Content.ReadAsAsync<LearnerDto>();
    Assert.NotNull(responseData.Id);
    await using var dbContext = _factory.Services.GetRequiredService<ApplicationDbContext>();
    var createdLearner = await dbContext.Learners.FindAsync(responseData.Id);
    Assert.NotNull(createdLearner);
}
```

### Service Integration Test Example
```csharp
[Fact]
public async Task Should_UpdateLearnerStatus_WhenApprovedByAdmin()
{
    // Arrange
    var learner = new Learner { Id = 1, Name = "Jane", Status = LearnerStatus.Pending };
    _dbContext.Learners.Add(learner);
    await _dbContext.SaveChangesAsync();

    // Act
    var result = await _learnerService.ApproveLearnerAsync(1);

    // Assert
    Assert.True(result.IsSuccess);
    var updatedLearner = await _dbContext.Learners.FindAsync(1);
    Assert.Equal(LearnerStatus.Active, updatedLearner.Status);
}
```

## Output Rules

### Report Format
- Generate reports in **JSON** or **HTML** format from test runners
- Export results to **Excel** format for stakeholder review
- Template: **Report5.2_Integration Test.xlsx**

### Report Columns
| Column | Description |
|--------|-------------|
| Test Case ID | Unique identifier for the test |
| Module | Which module/feature is being tested |
| API Endpoint | The endpoint being tested (if applicable) |
| Input Data | Request data or parameters |
| Expected Output | What should be returned/changed |
| Actual Output | What was actually returned |
| Status | Pass/Fail/Skipped |
| Execution Time | Duration in milliseconds |
| Issues/Defects | Any bugs identified |

### Report Generation Command
```bash
dotnet test --logger "xunit;LogFileName=it-results.xml"
dotnet test --logger "json;LogFileName=it-results.json"
```

## Test Setup & Teardown

### Database Fixture Example
```csharp
public class IntegrationTestsBase : IAsyncLifetime
{
    protected WebApplicationFactory<Program> _factory;
    protected ApplicationDbContext _dbContext;

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(s => 
                        s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    services.Remove(descriptor);
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb"));
                });
            });
        _dbContext = _factory.Services.GetRequiredService<ApplicationDbContext>();
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        _factory?.Dispose();
    }
}
```

## Best Practices

1. **Database Isolation**: Use transactions or database reset between tests
2. **Realistic Data**: Use test data that closely mimics production scenarios
3. **Error Scenarios**: Test both happy path and error cases
4. **Timing**: Add minimal delays for async operations if needed
5. **Assertions**: Verify both response and database state changes
6. **External Services**: Mock third-party APIs to ensure test reliability
7. **Logging**: Capture logs for failed tests for debugging
8. **Documentation**: Document complex test scenarios

## Test Categories

Organize tests by feature or endpoint:

```csharp
[Collection("Learner Integration Tests")]
public class LearnerIntegrationTests : IntegrationTestsBase
{
    [Fact]
    [Trait("Feature", "Learner")]
    [Trait("Endpoint", "POST /api/learners")]
    public async Task Should_CreateLearner_WhenValidDataProvided() { }
}
```

Run specific tests by category:
```bash
dotnet test --filter "Feature=Learner"
```

## CI/CD Integration

- Run integration tests on every commit to develop/main
- Use isolated test database to avoid data pollution
- Generate reports and upload to repository
- Run tests sequentially if database conflicts occur
- Fail build if critical integration tests fail

