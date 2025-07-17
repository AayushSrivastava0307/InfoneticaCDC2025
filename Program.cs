using WorkflowEngine.Data;
using WorkflowEngine.Endpoints;
using WorkflowEngine.Services;

// main entry point for the workflow engine web application
var builder = WebApplication.CreateBuilder(args);

// register the in-memory data store as a singleton to persist data across requests
builder.Services.AddSingleton<InMemoryDataStore>();
// register the workflow service with scoped lifetime for request handling
builder.Services.AddScoped<IWorkflowService, WorkflowService>();

// enable openapi documentation generation for api testing and exploration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
var app = builder.Build();

// configure the http pipeline for development environment
if (app.Environment.IsDevelopment())
{
    // enable swagger json endpoint for api documentation      
    app.UseSwagger();      
    // enable swagger ui for interactive api testing
    app.UseSwaggerUI();    
}

// enforce https redirection for security
app.UseHttpsRedirection();

// map all workflow-related api endpoints
app.MapWorkflowApi(); 

// start the web application
app.Run();