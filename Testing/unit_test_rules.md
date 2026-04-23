# Unit Test (UT) Rules - CLS Classroom Management System

## Overview
Unit Tests focus on testing individual components in isolation. All parts should work correctly before moving to integration testing.

<tech-stack>
    <framework>xUnit.net or NUnit</framework>
    <language>C#</language>
    <mocking-tool>Moq</mocking-tool>
    <test-runner>dotnet test</test-runner>
    <coverage-tool>Coverlet or OpenCover</coverage-tool>
</tech-stack>

## Testing Rules

| Rule | Description |
|------|-------------|
| Single Responsibility | Each test method should verify one behavior only |
| Pattern | Use Arrange-Act-Assert (AAA) pattern |
| Dependencies | Mock all external dependencies (database, APIs, services) |
| Isolation | Tests must run independently without affecting each other |
| Parallelization | Run tests in parallel where possible for speed |
| Coverage Target | Aim for 80%+ coverage on critical business logic paths |
| Performance | Unit tests should complete in < 1 second |
| No I/O | Avoid file system, database, or network calls |
| Deterministic | Tests must produce consistent results every run |
| Readability | Test names should clearly describe what is being tested |

## Naming Conventions

### Test Class Names
- Pattern: `[ClassName]Tests`
- Examples:
  - `LearnerServiceTests`
  - `LearnerControllerTests`
  - `ApplicationDbContextTests`

### Test Method Names
- Pattern: `Should_[ExpectedBehavior]_When[Condition]` or `[MethodName]_[Scenario]_[ExpectedResult]`
- Examples:
  - `Should_ReturnLearner_WhenValidIdProvided`
  - `Should_ThrowNotFoundException_WhenLearnerNotFound`
  - `Should_UpdateLearnerStatus_WhenValidDataProvided`

### Variable & Mock Names
- Arrange variables: `testData`, `input`, `expectedResult`
- Mock variables: `mock[InterfaceName]`
  - Examples: `mockILearnerRepository`, `mockILearnerService`, `mockHttpClient`
- System Under Test: `sut` or `service`

## Test Structure Template

```csharp
[Fact]
public void Should_ReturnLearner_WhenValidIdProvided()
{
    // Arrange
    var learnerId = 1;
    var expectedLearner = new Learner { Id = learnerId, Name = "John Doe" };
    var mockRepository = new Mock<ILearnerRepository>();
    mockRepository.Setup(r => r.GetByIdAsync(learnerId))
        .ReturnsAsync(expectedLearner);
    var service = new LearnerService(mockRepository.Object);

    // Act
    var result = await service.GetLearnerAsync(learnerId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedLearner.Id, result.Id);
    Assert.Equal(expectedLearner.Name, result.Name);
    mockRepository.Verify(r => r.GetByIdAsync(learnerId), Times.Once);
}
```

## Output Rules

### Report Format
- Generate reports in **XML** or **JSON** format using `dotnet test`
- Export results to **Excel** format for stakeholder review
- Template: **Report5.1_Unit Test.xls**

### Report Columns
| Column | Description |
|--------|-------------|
| Test Case ID | Unique identifier for the test |
| Test Method | Full name of the test method |
| Description | What behavior is being tested |
| Expected Result | What should happen |
| Actual Result | What actually happened |
| Status | Pass/Fail/Skipped |
| Execution Time | Duration in milliseconds |
| Comments | Any notes or issues |

### Report Generation Command
```bash
dotnet test --logger "xunit;LogFileName=test-results.xml"
dotnet test --logger "json;LogFileName=test-results.json"
dotnet test /p:CollectCoverageMetrics=true /p:CoverageThreshold=80
```

## Best Practices

1. **Test-Driven Development**: Write tests before implementation
2. **Red-Green-Refactor**: Cycle through failing test → passing test → refactoring
3. **Mocking Strategy**: Mock interfaces, not concrete classes
4. **Assertions**: Use specific, meaningful assertions (not just Assert.True)
5. **Test Data**: Use builders or factories for complex objects
6. **Edge Cases**: Test boundary conditions, nulls, empty collections
7. **Cleanup**: Use setup/teardown methods where needed
8. **Documentation**: Comment complex test logic

## Categorization

Tests should be organized by category using traits/tags:

```csharp
[Trait("Category", "Unit")]
[Trait("Feature", "Learner")]
public void Should_ReturnLearner_WhenValidIdProvided()
{
    // Test implementation
}
```

Run specific tests by category:
```bash
dotnet test --filter "Category=Unit & Feature=Learner"
```

## CI/CD Integration

- Run unit tests on every commit
- Fail build if coverage drops below 80%
- Run tests in parallel to speed up pipeline
- Generate reports and upload to repository

