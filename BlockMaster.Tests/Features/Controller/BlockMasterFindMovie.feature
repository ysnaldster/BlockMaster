Feature: Find Movie
Find movie feature

    Scenario: Find movie exist by name
        Given the movie name is E.T
        When the movie is found
        Then the movie returned by FindMovie is asserted
        Then the result should be 200

    Scenario: Find movie doesn't exist by name
        Given the movie name is other
        When the movie is found
        Then the result should be 404