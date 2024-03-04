using System;
using System.Collections.Generic;

namespace ExerciseComplex;

public partial class Complex
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<ExerciseComplex> ExerciseComplexes { get; } = new List<ExerciseComplex>();

    public virtual User? User { get; set; }
}
