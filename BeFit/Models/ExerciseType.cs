using System.ComponentModel.DataAnnotations;

namespace BeFit.Models;

public class ExerciseType
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Nazwa ćwiczenia musi mieć od 2 do 150 znaków")]
    [Display(Name = "Nazwa ćwiczenia")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Opis nie może przekraczać 500 znaków")]
    [Display(Name = "Opis")]
    public string? Description { get; set; }
    
    public ICollection<ExerciseRecord> ExerciseRecords { get; set; } = new List<ExerciseRecord>();
}

