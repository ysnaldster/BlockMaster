Feature: Find Movies
Find movies feature
    
    Scenario: Find movies
        When movies are wanted
        Then the http status code should be 200
        Then the movies count is equal 3
        