namespace WorkflowEngine.Models;

// defines a workflow action that can transition instances between states
// actions represent the operations/events that trigger state changes
public record Action(
    string Id,                 // unique identifier for this action within the workflow
    string Name,               // human-readable name describing what this action does
    bool Enabled,              // controls whether this action can currently be executed
    List<string> FromStates,   // list of state ids from which this action can be triggered
    string ToState             // destination state id where instances move after action execution
);