<h1>Configurable Workflow Engine</h1>
<img src="https://img.shields.io/badge/.NET-9-blue.svg" alt=".NET">

<br><br>

<p>
A minimal backend service built with .NET 9/C# that implements a configurable state-machine API. It allows clients to define workflows, create instances, and execute actions to transition between states with full validation.
<br><br>
The project is designed with clarity, correctness, and maintainability in mind, focusing on a pragmatic level of abstraction suitable for the scope.
</p>

<hr>

<h2>Key Features</h2>
<ul>
    <li><b>Dynamic Definitions</b>: Define workflows with custom states and actions on the fly.</li>
    <li><b>Stateful Instances</b>: Start new workflow instances from any chosen definition.</li>
    <li><b>Validated Transitions</b>: Execute actions with robust validation against the state machine's rules.</li>
    <li><b>Inspection</b>: Retrieve the current state and basic history of any running instance.</li>
    <li><b>Simple Persistence</b>: Utilizes a simple in-memory data store for all definitions and instances.</li>
</ul>

<hr>

<h2>Getting Started</h2>
<p>These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.</p>

<h3>Prerequisites</h3>
<p>.NET 8 SDK</p>

<h3>Installation & Running</h3>
<ol>
    <li>
        Clone the repository to your local machine:
        <pre><code>git clone https://github.com/AayushSrivastava0307/Infonetica23EE10080.git</code></pre>
    </li>
    <li>
        Navigate to the project directory:
        <pre><code>cd Infonetica23EE10080/WorkflowEngine</code></pre>
    </li>
    <li>
        Run the application:
        <pre><code>dotnet run</code></pre>
    </li>
    <li>The API will now be running on a local port (e.g., https://localhost:7123).</li>
    <li>Access the interactive Swagger UI at <code>/swagger</code> to test the endpoints.</li>
</ol>

<hr>

<h2>API Endpoints</h2>
<p>The API provides endpoints for workflow configuration and runtime execution.</p>

<h3>1. Create a Workflow Definition</h3>
<ul>
    <li><b>Endpoint</b>: <code>POST /workflows/definitions</code></li>
    <li><b>Description</b>: Creates a new workflow definition from a collection of states and actions.</li>
    <li><b>Example Payload</b>:
        <pre><code>{
  "states": [
    { "id": "draft", "name": "Draft", "isInitial": true, "isFinal": false, "enabled": true },
    { "id": "in_review", "name": "In Review", "isInitial": false, "isFinal": false, "enabled": true },
    { "id": "approved", "name": "Approved", "isInitial": false, "isFinal": true, "enabled": true }
  ],
  "actions": [
    { "id": "submit", "name": "Submit for Review", "enabled": true, "fromStates": ["draft"], "toState": "in_review" },
    { "id": "approve", "name": "Approve", "enabled": true, "fromStates": ["in_review"], "toState": "approved" }
  ]
}</code></pre>
    </li>
</ul>

<h3>2. Start a Workflow Instance</h3>
<ul>
    <li><b>Endpoint</b>: <code>POST /workflows/instances</code></li>
    <li><b>Description</b>: Starts a new instance of a specified workflow definition.</li>
    <li><b>Example Payload</b>:
        <pre><code>{
  "definitionId": "YOUR_WORKFLOW_DEFINITION_ID"
}</code></pre>
    </li>
</ul>

<h3>3. Execute an Action</h3>
<ul>
    <li><b>Endpoint</b>: <code>POST /workflows/instances/{id}/execute</code></li>
    <li><b>Description</b>: Executes an action on a given instance, moving it to the next state if the transition is valid.</li>
    <li><b>Example Payload</b>:
        <pre><code>{
  "actionId": "submit"
}</code></pre>
    </li>
</ul>

<hr>

<h2>Assumptions & Limitations</h2>
<ul>
    <li><b>Persistence</b>: Data is stored <b>in-memory</b> and will be lost when the application stops, as permitted for this exercise.</li>
    <li><b>Scope</b>: The focus is on the core state-machine logic. Production features like authentication, authorization, and detailed logging are not implemented.</li>
    <li><b>Validation</b>: The service implements all core validation rules required, such as rejecting actions on final states or from incorrect source states. It does not perform advanced validation, like checking if all states in a definition are reachable.</li>
</ul>

<hr>

<h2>Project Structure</h2>
<p>The project is organized into a clean and maintainable structure to promote clear boundaries.</p>
<ul>
    <li><code>/Data</code>: Contains the in-memory data store.</li>
    <li><code>/Models</code>: Holds the core concept classes: State, Action, WorkflowDefinition, and WorkflowInstance.</li>
    <li><code>/Services</code>: Contains the main business logic and validation service.</li>
    <li><code>/Endpoints</code>: Defines the API routes and DTOs.</li>
    <li><code>Program.cs</code>: The entry point for the application, responsible for service registration and middleware configuration.</li>
</ul>
