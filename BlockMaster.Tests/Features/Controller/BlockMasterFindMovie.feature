Feature: Find Movie
Find movie feature
    
    Background:
        Given the data for create a token for find movie is
          | UserId                               | Email         | CustomClaims |
          | 8a3bbfd4-5bbd-4f8e-8b22-6a066d471bf1 | user3@mail.com | true        |
        When the token for find movie is created
    
    Scenario: Find movie exist by name
        Given the movie name is E.T
        When the movie is found
        Then the movie returned by FindMovie is asserted
        Then the result should be 200
        
    Scenario: Find movie doesn't exist by name
        Given the movie name is other
        When the movie is found
        Then the result should be 404