using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasenTest.SchoolModels;

public partial class Proffesion
{
    public int ProffesionId { get; set; }

    public string? ProffesionName { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
