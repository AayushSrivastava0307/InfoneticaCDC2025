using WorkflowEngine.Data;
using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

// core implementation of workflow engine business logic and validation rules
// handles workflow definition creation, instance management, and action execution
public class WorkflowService : IWorkflowService
{
    private readonly InMemoryDataStore _store;

    // constructor injection of data store dependency
    public WorkflowService(InMemoryDataStore store)
    {
        _store = store;
    }

    // creates a new workflow definition with comprehensive validation
    public (WorkflowDefinition? definition, string? error) CreateWorkflowDefinition(List<State> states, List<Models.Action> actions)
    {
        // validation rule: workflow must have exactly one initial state for deterministic starting point
        var initialStates = states.Where(s => s.IsInitial).ToList();
        if (initialStates.Count != 1)
        {
            return (null, "A workflow definition must have exactly one initial state.");
        }

        // validation rule: state identifiers must be unique within a workflow definition
        var duplicateStateIds = states.GroupBy(s => s.Id).Any(g => g.Count() > 1);
        if (duplicateStateIds)
        {
            return (null, "State IDs must be unique within a definition.");
        }

        // additional validation could include: duplicate action ids, orphaned states, invalid transitions

        // create the workflow definition object
        var definition = new WorkflowDefinition
        {
            States = states,
            Actions = actions
        };

        // persist the definition in the data store
        _store.WorkflowDefinitions[definition.Id] = definition;
        return (definition, null);
    }

    // retrieves a workflow definition by its unique identifier
    public WorkflowDefinition? GetWorkflowDefinition(Guid definitionId)
    {
        _store.WorkflowDefinitions.TryGetValue(definitionId, out var definition);
        return definition;
    }

    // creates and starts a new workflow instance from an existing definition
    public (WorkflowInstance? instance, string? error) StartWorkflowInstance(Guid definitionId)
    {
        // verify the workflow definition exists before creating instance
        if (!_store.WorkflowDefinitions.TryGetValue(definitionId, out var definition))
        {
            return (null, "Workflow definition not found.");
        }

        // find the initial state to position the new instance
        var initialState = definition.States.FirstOrDefault(s => s.IsInitial);
        if (initialState == null)
        {
            // defensive programming: this should never happen due to creation validation
            return (null, "Cannot start instance: Definition is invalid and has no initial state.");
        }

        // create new instance positioned at the initial state
        var instance = new WorkflowInstance
        {
            DefinitionId = definitionId,
            CurrentStateId = initialState.Id
        };

        // persist the instance in the data store
        _store.WorkflowInstances[instance.Id] = instance;
        return (instance, null);
    }

    // retrieves a workflow instance by its unique identifier
    public WorkflowInstance? GetWorkflowInstance(Guid instanceId)
    {
        _store.WorkflowInstances.TryGetValue(instanceId, out var instance);
        return instance;
    }

    // executes an action on a workflow instance with comprehensive validation
    public (WorkflowInstance? instance, string? error) ExecuteAction(Guid instanceId, string actionId)
    {
        // verify the workflow instance exists
        if (!_store.WorkflowInstances.TryGetValue(instanceId, out var instance))
        {
            return (null, "Workflow instance not found.");
        }

        // verify the associated workflow definition still exists
        if (!_store.WorkflowDefinitions.TryGetValue(instance.DefinitionId, out var definition))
        {
            return (null, "Internal error: Could not find definition for this instance.");
        }

        // find the current state of the instance within the definition
        var currentState = definition.States.FirstOrDefault(s => s.Id == instance.CurrentStateId);
        if (currentState == null)
        {
            return (null, "Internal error: Current state not found in definition.");
        }

        // validation rule: final states cannot execute any actions (workflow endpoint)
        if (currentState.IsFinal)
        {
            return (null, "Action rejected: The current state is a final state.");
        }

        // find the action to execute within the workflow definition
        var actionToExecute = definition.Actions.FirstOrDefault(a => a.Id == actionId);

        // validation rule: action must be defined in the workflow
        if (actionToExecute == null)
        {
            return (null, "Action not found in this workflow definition.");
        }

        // validation rule: action must be currently enabled for execution
        if (!actionToExecute.Enabled)
        {
            return (null, "Action is disabled.");
        }

        // validation rule: action must be valid from the current state
        if (!actionToExecute.FromStates.Contains(instance.CurrentStateId))
        {
            return (null, $"Action '{actionId}' cannot be executed from the current state '{instance.CurrentStateId}'.");
        }

        // execute the state transition and record the action in history
        instance.CurrentStateId = actionToExecute.ToState;
        instance.History.Add(new HistoryItem(actionId, DateTime.UtcNow));

        return (instance, null);
    }
}