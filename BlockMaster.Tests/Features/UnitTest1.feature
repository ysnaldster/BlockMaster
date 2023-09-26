Feature: TestTest
Enable and disable account IP Validation

    Scenario: Enable Account IP Validation
        Given the account id 9
        When ip validation is enabled
        Then the ip validation result should be 204