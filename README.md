# Configurable Workflow Engine

A minimal backend service that implements a configurable state-machine API, built with .NET 8 and C#.

## [cite_start]Quick-Start Instructions [cite: 40]

1.  Ensure you have the .NET 8 SDK installed.
2.  Clone the repository.
3.  Navigate to the project directory and run the application:
    ```sh
    dotnet run
    ```
4.  The API will be running on a local host (e.g., `https://localhost:7123`).
5.  Access the interactive API documentation (Swagger UI) at the `/swagger` endpoint to test the API.

## [cite_start]Assumptions & Limitations [cite: 41]

* [cite_start]**Persistence**: Per the guidelines, data is stored in-memory[cite: 24]. All workflow definitions and running instances will be lost when the application stops.
* [cite_start]**Validation**: The service implements all core validation rules specified in the requirements[cite: 21, 22]. More complex validation (e.g., ensuring all states are reachable) was considered out of scope for the time-boxed exercise.
* **API Design**: The API uses a minimal design with Data Transfer Objects (DTOs) for clear request payloads. Error handling is done via `400 Bad Request` or `404 Not Found` responses with a descriptive message.

## [cite_start]Core Concepts [cite: 10]

The engine is built around four main concepts:
* **State**: A single status in a workflow. [cite_start]Must have an `id`, `name`, and boolean flags for `isInitial`, `isFinal`, and `enabled`[cite: 11].
* [cite_start]**Action**: A transition that moves an instance from one or more `fromStates` to a single `toState`[cite: 11].
* **Workflow Definition**: A template consisting of a collection of states and actions. [cite_start]It must contain exactly one initial state[cite: 11].
* [cite_start]**Workflow Instance**: A running copy of a workflow definition that tracks its `currentState` and a basic history of actions taken[cite: 11].