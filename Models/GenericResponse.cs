namespace Web3Ai.Service;

public class GenericResponse
{
    public required bool IsSuccess { get; set; }
    
    public string? Error {get; set;}
}