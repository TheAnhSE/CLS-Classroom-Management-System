# Testing Conventions - CLS Classroom Management System

As a Senior QA for the CLS project, this document provides central context for AI-assisted testing activities and references the detailed testing rules for each level.

## Testing Levels Overview

The CLS project has three testing levels, each with dedicated rule documents:

### 1. Unit Test (UT) - [unit_test_rules.md](unit_test_rules.md)
**Purpose**: Test individual components in isolation  
**Scope**: Services, repositories, controllers, utilities  
**Focus**: Single behavior verification, mocking dependencies

### 2. Integration Test (IT) - [integration_test_rules.md](integration_test_rules.md)
**Purpose**: Test interactions between components  
**Scope**: API endpoints, database interactions, service integration  
**Focus**: Data flow validation, end-to-end component communication

### 3. System Test (ST) - [system_test_rules.md](system_test_rules.md)
**Purpose**: Test complete system functionality end-to-end  
**Scope**: User workflows, UI interactions, business processes  
**Focus**: Production-like scenario validation, cross-module workflows

## Quick Reference - Technology Stack by Level

| Aspect | Unit Test | Integration Test | System Test |
|--------|-----------|------------------|-------------|
| **Framework** | xUnit.net / NUnit | xUnit.net + TestServer | Selenium / NUnit + SpecFlow |
| **Language** | C# | C# | C# / Python |
| **Database** | In-memory / Mocks | In-memory / Test DB | Staging DB (production-like) |
| **Dependencies** | Mocked | Mixed (real + mocked) | Mostly real |
| **Execution Time** | Fast (< 1s) | Medium (< 5s) | Slow (> 5s) |
| **Report Template** | Report5.1_Unit Test.xls | Report5.2_Integration Test.xlsx | Report5.3_System Test.xlsx |

## General Guidelines (All Levels)

### Repository & Version Control
- All tests must be version-controlled alongside code
- Follow naming conventions consistently across all levels
- Keep tests synchronized with feature implementations
- Document complex test logic with comments

### CI/CD Integration
- Execute unit tests on every commit (fast feedback)
- Run integration tests on pull requests
- Execute system tests before release
- Fail build if critical tests fail
- Generate and archive test reports
- Monitor test coverage trends

### Documentation & Traceability
- Link tests to Jira issues/user stories
- Document test scenarios and expected outcomes
- Maintain test case descriptions for AI context
- Update tests when requirements change
- Remove obsolete tests regularly

### Test Data Management
- Use builder patterns or factories for complex objects
- Keep test data realistic (production-like)
- Isolate test data to prevent cross-contamination
- Clean up test data after execution
- Document sensitive test data handling

### Performance & Optimization
- Measure test execution times
- Optimize slow tests (reduce I/O, parallelize where possible)
- Set acceptable performance thresholds
- Monitor resource usage during test runs
- Archive performance metrics for trend analysis

## File Structure

```
Testing/
├── testing_conventions.md (this file)
├── unit_test_rules.md
├── integration_test_rules.md
├── system_test_rules.md
└── Reports/
    └── (generated test reports and results)
```

## Best Practices Summary

✓ **Write tests BEFORE implementation** (TDD approach)  
✓ **Keep tests independent** (no dependencies between tests)  
✓ **Use clear, descriptive test names** (self-documenting)  
✓ **Mock external dependencies** appropriately for each level  
✓ **Verify both happy path and error scenarios**  
✓ **Maintain test suite health** (remove flaky tests)  
✓ **Automate what's repeatable** (manual for exploratory)  
✓ **Document complex test logic** with comments  
✓ **Generate reports** for stakeholder visibility  
✓ **Review and update conventions** quarterly

## Review & Update Policy

- **Quarterly Review**: Assess test coverage and effectiveness
- **As-Needed Updates**: Adjust conventions for new technologies/frameworks
- **Best Practices**: Incorporate lessons learned from test execution
- **Team Discussion**: Discuss improvements during sprint retrospectives

For detailed rules, examples, and best practices for each testing level, refer to the dedicated rule files:
- [Unit Test Rules](unit_test_rules.md)
- [Integration Test Rules](integration_test_rules.md)
- [System Test Rules](system_test_rules.md)