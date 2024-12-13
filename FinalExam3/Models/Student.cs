using System;
using System.Collections.Generic;

namespace FinalExam3.Models;

public partial class Student
{
    public int StudentNumber { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Registeration> Registerations { get; set; } = new List<Registeration>();

    public virtual User? User { get; set; }
}
