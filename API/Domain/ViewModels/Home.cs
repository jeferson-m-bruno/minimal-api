namespace MinimalAPI.Domain.ViewModels;

public struct Home
{
    public string Doc { get => "/swagger"; }
    public string Message { get => "Bem vindo a API de veículos - Minimal API";  }
}