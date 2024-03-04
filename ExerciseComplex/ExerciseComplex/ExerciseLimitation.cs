using System;
using System.Collections.Generic;

namespace ExerciseComplex;

public partial class ExerciseLimitation
{
    public int ExerciseId { get; set; }

    public int LimitationId { get; set; }

    public virtual Exercise Exercise { get; set; } = null!;

    public virtual Limitation Limitation { get; set; } = null!;
}
