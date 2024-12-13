using System;
using System.Collections.Generic;

namespace FinalExam3.Models;

public partial class User
{
    public int UserCode { get; set; }

    public string Password { get; set; } = null!;

    public virtual Student UserCodeNavigation { get; set; } = null!;
}
