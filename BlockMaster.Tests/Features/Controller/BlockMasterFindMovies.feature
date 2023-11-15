Feature: Find Movies
Find movies feature
    
    Background:
        Given the data for create a token for find movies is
          | UserId                               | Email         | CustomClaims |
          | 8a3bbfd4-5bbd-4f8e-8b22-6a066d471bf9 | user4@mail.com | true        |
        When the token for find movies is created
    
    Scenario: Find movies
        When movies are wanted
        Then the http status code should be 200
        Then the movies returned by FindMovies are asserted
        Then the movies count is equal 3
        