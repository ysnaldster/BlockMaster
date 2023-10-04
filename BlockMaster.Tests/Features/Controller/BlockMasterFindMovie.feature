Feature: Find Movie
Find movie feature
    
    Scenario: Find movie exist by name
        Given the movie name is E.T
        When the movie is found
        Then the movie name should be E.T
        Then the http status code should be Ok
        Then the movie is contain in the list