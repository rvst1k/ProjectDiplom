using System;
using System.Collections.Generic;

namespace ExerciseComplex;

public partial class ExerciseComplex
{
    public int ExerciseId { get; set; }

    public int ComplexId { get; set; }

    public virtual Complex? Complex { get; set; }

    public virtual Exercise Exercise { get; set; } = null!;
}
