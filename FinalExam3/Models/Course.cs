using System;
using System.Collections.Generic;

namespace FinalExam3.Models;

public partial class Course
{
    public string CourseNumber { get; set; } = null!;

    public string CourseTitle { get; set; } = null!;

    public int TotalHour { get; set; }

    public virtual ICollection<Registeration> Registerations { get; set; } = new List<Registeration>();
}
