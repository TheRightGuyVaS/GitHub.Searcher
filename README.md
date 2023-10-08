# GitHub Searcher

The GitHub Searcher is a solution designed to provide information about pull requests in a specific repository. It allows users to query pull requests based on criteria such as repository owner, repository name, pull request label, and keywords for title filtration.

## Contents

- [Components](#components)
- [How to Run](#how-to-run)
- [Usage](#usage)
- [Tests](#tests)

---

## Components

### Console

The Console project serves as a command-line interface for executing the business logic. It provides a way to interact with the core functionalities of the solution.

### API

The API project contains the REST API with a SearchController responsible for handling requests related to pull request information.

### Business

The Business project houses the main logic for fetching and processing pull request data. It acts as the core engine for handling API requests.

### Business.UnitTests

The Business.UnitTests project consists of test cases for the Business logic. These tests ensure the correctness and reliability of the core functionalities.

---

## How to Run

1. Clone the repository to your local machine.

```bash
git clone https://github.com/TheRightGuyVaS/GitHub.Searcher.git
```

2. Open the solution in your preferred IDE (e.g., Visual Studio).

3. Build the solution to ensure all dependencies are resolved.

4. Set the desired startup project to `API` and run the application.

---

## Usage

1. Launch the API application.

2. Send a GET request to the API endpoint with the required query parameters:
- `owner` (Repository owner)
- `repo` (Repository name)
- `label` (Pull request label)
- `keywords` (Collection of keywords for title filtration)

Example API Request:

```
GET https://localhost:5001/api/search?owner=YourOwner&repo=YourRepo&label=YourLabel&keywords=Keyword1,Keyword2
```

3. The API will respond with the relevant pull request information.

---

## Response Type

The `SearchController` in the API project returns data in JSON format. The response type is represented by the `GitHubPullRequestResponse` class, which contains information about pull requests based on the provided criteria.

### GitHubPullRequestResponse

This class serves as a response model for the `SearchController` in the API project. It is responsible for encapsulating data related to a GitHub pull request. The purpose of this class is to provide a structured representation of pull request information that can be sent as a response to client requests.

**Data Contained:**

- `UrlToPage` (Type: Uri): Represents the URL to the pull request page on GitHub.
- `Title` (Type: string): Holds the title of the pull request.
- `ShortenedDescription` (Type: string): Contains a shortened description of the pull request.
- `NumberOfComments` (Type: int): Indicates the total number of comments on the pull request.
- `CreatedAt` (Type: DateTime): Represents the date and time when the pull request was created.
- `CreatorName` (Type: string): Contains the name of the pull request creator.
- `CreatorEmail` (Type: string): Stores the email address of the pull request creator.
- `CreatorAvatarUrl` (Type: Uri): Holds the URL to the avatar of the pull request creator.

**Purpose:**

The purpose of this response class is to package relevant pull request information in a structured format. It enables the API to communicate pull request details to the client application in a standardized manner. This facilitates easy consumption of data and allows client applications to utilize the received information for their intended purposes.

## Tests

The solution includes a suite of unit tests to verify the correctness of the Business logic. These tests can be found in the Business.UnitTests project. You can run the tests to ensure the stability of the core functionalities.

---

Feel free to reach out if you have any questions or need further assistance!
