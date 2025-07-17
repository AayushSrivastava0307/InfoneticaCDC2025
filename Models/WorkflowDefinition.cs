namespace WorkflowEngine.Models;

// represents a complete workflow template/blueprint that defines the structure and rules
// workflow definitions serve as templates for creating multiple workflow instances
public class WorkflowDefinition
{
    // auto-generated unique identifier for this workflow definition
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // collection of all possible states in this workflow
    public List<State> States { get; set; } = new();
    
    // collection of all possible actions/transitions in this workflow
    public List<Action> Actions { get; set; } = new();
}