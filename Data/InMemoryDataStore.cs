using System.Collections.Concurrent;
using WorkflowEngine.Models;

namespace WorkflowEngine.Data;

// thread-safe in-memory storage for workflow definitions and instances
// provides concurrent access for multiple requests without external database dependency
public class InMemoryDataStore
{
    // stores all workflow definition templates indexed by their unique ids
    // concurrent dictionary ensures thread-safe operations for simultaneous access
    public ConcurrentDictionary<Guid, WorkflowDefinition> WorkflowDefinitions { get; } = new();
    
    // stores all active workflow instances indexed by their unique ids
    // concurrent dictionary ensures thread-safe operations for simultaneous access
    public ConcurrentDictionary<Guid, WorkflowInstance> WorkflowInstances { get; } = new();
}