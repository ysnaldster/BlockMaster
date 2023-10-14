Feature: Create a movie
Create movie feature
    
    Scenario: Movie created successfully
        Given The details for creating the movie are
        | Name | Description     | CountryCode | Score | Category |
        | Ghosted  | is simply dummy | 57          | 3.2   | Action   |
        When The movie is created
        Then the response should be 200
        Then the result returned by CreateMovie is asserted