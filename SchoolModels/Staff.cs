using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasenTest.SchoolModels;

public partial class Staff
{
    public int StaffId { get; set; }

    public string? StaffFirstName { get; set; }

    public string? StaffLastName { get; set; }

    public int? FkproffesionId { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Proffesion? Fkproffesion { get; set; }
}
