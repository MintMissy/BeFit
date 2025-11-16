using System.ComponentModel.DataAnnotations;

namespace BeFit.Models;

public class ExerciseStatistics
{
    [Display(Name = "Ćwiczenie")]
    public string ExerciseName { get; set; } = string.Empty;
    
    [Display(Name = "Liczba wykonań")]
    public int TimesPerformed { get; set; }
    
    [Display(Name = "Łączna liczba powtórzeń")]
    public int TotalRepetitions { get; set; }
    
    [Display(Name = "Przeciętne obciążenie (kg)")]
    public decimal AverageWeight { get; set; }
    
    [Display(Name = "Maksymalne obciążenie (kg)")]
    public decimal MaxWeight { get; set; }
}

