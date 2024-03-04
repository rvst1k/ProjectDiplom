using System;
using System.Collections.Generic;

namespace ExerciseComplex;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public int? Height { get; set; }

    public int? Weight { get; set; }

    public bool Gender { get; set; }

    public int RolesId { get; set; }

    public string? ProfilePicture { get; set; }

    public virtual ICollection<Complex> Complexes { get; } = new List<Complex>();

    public virtual Role Roles { get; set; } = null!;
}
