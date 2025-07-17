using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

// contract defining the core workflow engine operations
// provides abstraction for workflow definition and instance management
public interface IWorkflowService
{
    // === workflow definition management operations ===
    
    // creates a new workflow definition with validation rules
    // returns the created definition or error message if validation fails
    (WorkflowDefinition? definition, string? error) CreateWorkflowDefinition(List<State> states, List<Models.Action> actions);
    
    // retrieves an existing workflow definition by its unique identifier
    // returns null if definition is not found
    WorkflowDefinition? GetWorkflowDefinition(Guid definitionId);

    // === workflow instance management operations ===
    
    // creates and starts a new workflow instance from a definition
    // returns the new instance or error message if creation fails
    (WorkflowInstance? instance, string? error) StartWorkflowInstance(Guid definitionId);
    
    // retrieves an existing workflow instance by its unique identifier
    // returns null if instance is not found
    WorkflowInstance? GetWorkflowInstance(Guid instanceId);
    
    // executes an action on a workflow instance causing state transition
    // returns updated instance or error message if action execution fails
    (WorkflowInstance? instance, string? error) ExecuteAction(Guid instanceId, string actionId);
}