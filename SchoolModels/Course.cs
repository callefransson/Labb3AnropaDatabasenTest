using System;
using System.Collections.Generic;

namespace Labb3AnropaDatabasenTest.SchoolModels;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
