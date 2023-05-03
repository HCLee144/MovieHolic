using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeAccount { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Access { get; set; }
}
