using System;
using System.Collections.Generic;

namespace ExerciseComplex;

public partial class Exercise
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Link { get; set; }

    public string? Preview { get; set; }

    public int TypeId { get; set; }

    public int DifficultyId { get; set; }

    public int AimId { get; set; }

    public string? Description { get; set; }

    public double? CaloriesNumber { get; set; }

    public virtual ExerciseAim Aim { get; set; } = null!;

    public virtual ExerciseDifficulty Difficulty { get; set; } = null!;

    public virtual ExerciseComplex? ExerciseComplex { get; set; }

    public virtual ExerciseType Type { get; set; } = null!;
}
