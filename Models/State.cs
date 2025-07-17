namespace WorkflowEngine.Models;

// represents a single state in a workflow definition
// states define the possible positions/conditions within a workflow process
public record State(
    string Id,        // unique identifier for this state within the workflow
    string Name,      // human-readable name describing this state
    bool IsInitial,   // indicates if this is the starting state for new workflow instances
    bool IsFinal,     // indicates if this is a terminal state where workflow execution ends
    bool Enabled      // controls whether this state is currently active/usable
);