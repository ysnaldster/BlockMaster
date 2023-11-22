Feature: Create a movie
Create movie feature

    Scenario: Movie created successfully
        Given The details for creating the movie are
          | Name    | Description      | CountryCode | Score | Category |
          | Ghosted | is simply dummy1 | 57          | 3.2   | Action   |
        When The movie is created
        Then the response should be 200
        Then the result returned by CreateMovie is asserted

    Scenario: Try create movie with special characters
        Given The details for creating the movie are
          | Name    | Description      | CountryCode | Score | Category |
          | Any&%%2 | is simply dummy2 | 51          | 2.5   | Comedy   |
        When The movie is created
        Then the response should be 400

    Scenario: Create a movie with score out of range
        Given The details for creating the movie are
          | Name | Description      | CountryCode | Score | Category |
          | Test | is simply dummy3 | 56          | 7.5   | Comedy   |
        When The movie is created
        Then the response should be 400

    Scenario: Create a movie with a name longer than 30 characters
        Given The details for creating the movie are
          | Name                                     | Description      | CountryCode | Score | Category |
          | TestTestTestTestTestTestTestTestTestTest | is simply dummy3 | 56          | 2.1   | Comedy   |
        When The movie is created
        Then the response should be 400

    Scenario: Create a movie with a non-existing category
        Given The details for creating the movie are
          | Name  | Description      | CountryCode | Score | Category |
          | Other | is simply dummy3 | 56          | 3.3   | Other    |
        When The movie is created
        Then the response should be 400

    Scenario: Create a movie with an unalloyed country
        Given The details for creating the movie are
          | Name | Description      | CountryCode | Score | Category |
          | LP   | is simply dummy3 | 55          | 3.3   | Action   |
        When The movie is created
        Then the response should be 400

    Scenario: Create a movie with an existing name
        Given The details for creating the movie are
          | Name    | Description      | CountryCode | Score | Category |
          | In Time | is simply dummy3 | 56          | 3.3   | Action   |
        When The movie is created
        Then the response should be 409