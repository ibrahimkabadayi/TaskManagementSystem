namespace Application.DTOs;

public class TaskDetailsDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string ListName { get; set; } = string.Empty;
        
    public string CreatedByName { get; set; } = string.Empty;
    public string CreatedByInitial { get; set; } = string.Empty;
    public string CreatedByColor { get; set; } = string.Empty;

    public string? AssignedToName { get; set; } = string.Empty;
    public string? AssignedInitial { get; set; } = string.Empty;
    public string? AssignedColor { get; set; } = string.Empty;
    
    public string? FinishedByName { get; set; } = string.Empty;
    public string? FinishedByInitial { get; set; } = string.Empty;
    public string? FinishedByColor { get; set; } = string.Empty;

    public string CreatedDate { get; set; } = string.Empty;
    public string? DueDate { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}