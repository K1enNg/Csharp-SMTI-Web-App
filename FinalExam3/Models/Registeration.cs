using System;
using System.Collections.Generic;

namespace FinalExam3.Models;

public partial class Registeration
{
    public int RegisterationId { get; set; }

    public int StudentsNumber { get; set; }

    public string CourseNumber { get; set; } = null!;

    public virtual Course CourseNumberNavigation { get; set; } = null!;

    public virtual Student StudentsNumberNavigation { get; set; } = null!;
}
