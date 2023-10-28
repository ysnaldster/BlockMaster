Feature: Delete a movie
Delete movie feature

    Scenario: Delete movie by name
        Given the movie name for deleted is In Time
        When the movie is deleted
        Then the movie deleted is equal to expected
        Then the result for http response should be 200
        
    Scenario: Delete movie doesn't exist by name
        Given the movie name for deleted is Tron
        When the movie is deleted
        Then the result for http response should be 404