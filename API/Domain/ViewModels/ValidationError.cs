namespace MinimalAPI.Domain.ViewModels;

public struct ValidationErrors
{
    public ValidationErrors(List<string> messages) => this.Messages = messages;

    public List<string> Messages { get; set; }

    public bool IsError { get => Messages.Any(); }    
}