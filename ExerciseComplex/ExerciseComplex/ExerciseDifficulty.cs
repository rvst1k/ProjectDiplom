﻿using System;
using System.Collections.Generic;

namespace ExerciseComplex;

public partial class ExerciseDifficulty
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Exercise> Exercises { get; } = new List<Exercise>();
}
