Feature: Delete a movie
Delete movie feature

    Background:
        Given the data for create a token for delete movie is
          | UserId                               | Email         | CustomClaims |
          | 8a3bbfd4-5bbd-4f8e-8b22-6a066d471bf6 | user2@mail.com | true        |
        When the token for delete a movie is created

    Scenario: Delete movie by name
        Given the movie name for deleted is In Time
        When the movie is deleted
        Then the movie deleted is equal to expected
        Then the result for http response should be 200

    Scenario: Delete movie doesn't exist by name
        Given the movie name for deleted is Tron
        When the movie is deleted
        Then the result for http response should be 404