﻿using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasenTest.SchoolModels;

public partial class Grade
{
    public int GradesId { get; set; }

    public int Grade1 { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
