Feature: Update a movie
Update movie feature
    
    Background:
        Given the data for create a token for update is
          | UserId                               | Email         | CustomClaims |
          | 8a3bbfd4-5bbd-4f8e-8b22-6a066d471bf5 | user@mail.com | true        |
        When the token for update is created
    
    Scenario: Update a movie with valid information
        Given the movie name for update is Avengers
        Given The details for updating the movie are
          | Name    | Description      | CountryCode | Score | Category |
          | Born | is simply dummy6 | 51          | 4.3   | Action   |
        When The movie is updated
        Then the result returned by UpdateMovie is asserted
        Then the response for UpdateMovie should be 200
    
    Scenario: Update a movie with invalid name
        Given the movie name for update is In Time
        Given The details for updating the movie are
          | Name        | Description | CountryCode | Score | Category |
          |  C@rdig@12/ | is simply dummy6 | 51          | 4.3   | Action   |
        When The movie is updated
        Then the response for UpdateMovie should be 400
        
    Scenario: Update a movie with invalid score
        Given the movie name for update is E.T
        Given The details for updating the movie are
          | Name | Description      | CountryCode | Score | Category |
          |  ELP | is simply dummy6 | 51          | 7.3   | Drama    |
        When The movie is updated
        Then the response for UpdateMovie should be 400
        
    Scenario: Update a movie with invalid country code
        Given the movie name for update is E.T
        Given The details for updating the movie are
          | Name  | Description      | CountryCode | Score | Category |
          |  Hots | is simply dummy6 | 52          | 3.3   | Fantasy  |
        When The movie is updated
        Then the response for UpdateMovie should be 400
    
    Scenario: Create a movie with a name longer than 30 characters
        Given the movie name for update is In Time
        Given The details for updating the movie are
          | Name  | Description      | CountryCode | Score | Category |
          |  TestTestTestTestTestTestTestTestTest | is simply dummy6 | 51          | 3.3   | Fantasy  |
        When The movie is updated
        Then the response for UpdateMovie should be 400            