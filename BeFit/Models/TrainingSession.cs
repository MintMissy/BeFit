using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models;

public class TrainingSession
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Data i czas rozpoczęcia są wymagane")]
    [Display(Name = "Data i czas rozpoczęcia")]
    [DataType(DataType.DateTime)]
    public DateTime StartDateTime { get; set; }
    
    [Required(ErrorMessage = "Data i czas zakończenia są wymagane")]
    [Display(Name = "Data i czas zakończenia")]
    [DataType(DataType.DateTime)]
    public DateTime EndDateTime { get; set; }
    
    [StringLength(500, ErrorMessage = "Notatki nie mogą przekraczać 500 znaków")]
    [Display(Name = "Notatki")]
    public string? Notes { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public IdentityUser User { get; set; } = null!;
    public ICollection<ExerciseRecord> ExerciseRecords { get; set; } = new List<ExerciseRecord>();
}

