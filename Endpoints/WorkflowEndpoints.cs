using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Models;
using WorkflowEngine.Services;

namespace WorkflowEngine.Endpoints;

// defines all http api endpoints for workflow engine operations
// provides restful interface for workflow definition and instance management
public static class WorkflowEndpoints
{
    // extension method to register all workflow api routes with the web application
    public static void MapWorkflowApi(this WebApplication app)
    {
        // create a route group for all workflow-related endpoints under /workflows path
        var workflowGroup = app.MapGroup("/workflows");

        // === workflow definition management endpoints ===

        // creates a new workflow definition from states and actions
        // post /workflows/definitions - accepts workflow definition dto in request body
        workflowGroup.MapPost("/definitions", (
            [FromBody] WorkflowDefinitionDto definitionDto,
            IWorkflowService service) =>
        {
            // delegate to service layer for business logic and validation
            var (definition, error) = service.CreateWorkflowDefinition(definitionDto.States, definitionDto.Actions);
            // return appropriate http response based on operation result
            return string.IsNullOrEmpty(error) ? Results.Ok(definition) : Results.BadRequest(new { message = error });
        });

        // retrieves an existing workflow definition by its unique identifier
        // get /workflows/definitions/{id} - id parameter from url route
        workflowGroup.MapGet("/definitions/{id:guid}", (Guid id, IWorkflowService service) =>
        {
            // delegate to service layer for data retrieval
            var definition = service.GetWorkflowDefinition(id);
            // return 200 ok with definition or 404 not found
            return definition != null ? Results.Ok(definition) : Results.NotFound();
        });


        // === workflow instance management endpoints ===

        // creates and starts a new workflow instance from a definition
        // post /workflows/instances - accepts start instance dto in request body
        workflowGroup.MapPost("/instances", (
            [FromBody] StartInstanceDto startDto,
            IWorkflowService service) =>
        {
            // delegate to service layer for instance creation and validation
            var (instance, error) = service.StartWorkflowInstance(startDto.DefinitionId);
            // return appropriate http response based on operation result
            return string.IsNullOrEmpty(error) ? Results.Ok(instance) : Results.BadRequest(new { message = error });
        });

        // retrieves an existing workflow instance by its unique identifier
        // get /workflows/instances/{id} - id parameter from url route
        workflowGroup.MapGet("/instances/{id:guid}", (Guid id, IWorkflowService service) =>
        {
            // delegate to service layer for data retrieval
            var instance = service.GetWorkflowInstance(id);
            // return 200 ok with instance or 404 not found
            return instance != null ? Results.Ok(instance) : Results.NotFound();
        });

        // executes an action on a workflow instance causing state transition
        // post /workflows/instances/{id}/execute - accepts execute action dto in request body
        workflowGroup.MapPost("/instances/{id:guid}/execute", (
            Guid id,
            [FromBody] ExecuteActionDto actionDto,
            IWorkflowService service) =>
        {
            // delegate to service layer for action execution and validation
            var (instance, error) = service.ExecuteAction(id, actionDto.ActionId);
            // return appropriate http response based on operation result
            return string.IsNullOrEmpty(error) ? Results.Ok(instance) : Results.BadRequest(new { message = error });
        });
    }
}

// === data transfer objects for api request/response payloads ===

// dto for creating workflow definitions - contains all required states and actions
public record WorkflowDefinitionDto(List<State> States, List<Models.Action> Actions);

// dto for starting workflow instances - contains reference to definition
public record StartInstanceDto(Guid DefinitionId);

// dto for executing actions - contains the action identifier to perform
public record ExecuteActionDto(string ActionId);