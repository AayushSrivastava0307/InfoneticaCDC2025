namespace WorkflowEngine.Models;

// represents a running instance of a workflow created from a workflow definition
// workflow instances track the current state and execution history of a specific workflow run
public class WorkflowInstance
{
    // auto-generated unique identifier for this workflow instance
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // reference to the workflow definition this instance was created from
    public Guid DefinitionId { get; set; }
    
    // current state id where this workflow instance is positioned
    public string CurrentStateId { get; set; } = string.Empty;
    
    // chronological log of all actions executed on this instance
    public List<HistoryItem> History { get; set; } = new();
}

// represents a single entry in the workflow instance execution history
// tracks what action was performed and when it occurred
public record HistoryItem(
    string ActionId,      // id of the action that was executed
    DateTime Timestamp    // exact time when the action was performed
);