using System;
using System.Collections.Generic;

namespace ExerciseComplex;

public partial class Role
{
    public int Id { get; set; }

    public string NameOfRole { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
