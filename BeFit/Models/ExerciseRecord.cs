using System.ComponentModel.DataAnnotations;

namespace BeFit.Models;

public class ExerciseRecord
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Sesja treningowa jest wymagana")]
    [Display(Name = "Sesja treningowa")]
    public int TrainingSessionId { get; set; }
    
    [Required(ErrorMessage = "Typ ćwiczenia jest wymagany")]
    [Display(Name = "Typ ćwiczenia")]
    public int ExerciseTypeId { get; set; }
    
    [Required(ErrorMessage = "Obciążenie jest wymagane")]
    [Range(0, 1000, ErrorMessage = "Obciążenie musi być w zakresie od 0 do 1000 kg")]
    [Display(Name = "Obciążenie (kg)")]
    public decimal Weight { get; set; }
    
    [Required(ErrorMessage = "Liczba serii jest wymagana")]
    [Range(1, 1000, ErrorMessage = "Liczba serii musi być w zakresie od 1 do 1000")]
    [Display(Name = "Liczba serii")]
    public int Sets { get; set; }
    
    [Required(ErrorMessage = "Liczba powtórzeń jest wymagana")]
    [Range(1, 1000, ErrorMessage = "Liczba powtórzeń musi być w zakresie od 1 do 1000")]
    [Display(Name = "Liczba powtórzeń w serii")]
    public int Repetitions { get; set; }
    
    [StringLength(500, ErrorMessage = "Notatki nie mogą przekraczać 500 znaków")]
    [Display(Name = "Notatki")]
    public string? Notes { get; set; }
    
    public TrainingSession TrainingSession { get; set; } = null!;
    public ExerciseType ExerciseType { get; set; } = null!;
}

