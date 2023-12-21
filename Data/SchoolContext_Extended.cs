using Microsoft.EntityFrameworkCore;

namespace Labb3AnropaDatabasenTest.SchoolModels;

public partial class SchoolContext : DbContext
{
    public void ShowMenu() // In this method i create a switch case to show all the options that the user can choose from.
                           // Users input have a try parse so if user accedently enters a char insted of a number the program doens't crash
                           // Also if user goes outside the range of 1-6 it also doesn't crash
                           // If user press 0 the program ends
                           // I also made a while loop on users pick so it will keep on going until user enters a valid option
    {
        int pick;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Welcome to the school! what would you like to do? \n1. Get all staffs\n2. Get all students\n3. Retrieve all students in a particular class" +
            "\n4. Retrieve all grades set in the last month \n5. Get a list of all courses and the average grade the students received in that course" +
            "\n6. Add a new student \n7. Add a new Staff \n0. Exit");
        Console.ResetColor();
        while (!int.TryParse(Console.ReadLine(), out pick) || (pick < 0 || pick > 7))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please enter a valid option (0-7)");
            Console.ResetColor();
        }

        switch (pick)
        {
            case 0:
                Console.WriteLine("Program ended");
                Environment.Exit(0);
                break;
            case 1:
                GetStaff();
                break;
            case 2:
                GetStudents();
                break;
            case 3:
                GetStudentsFromClass();
                break;
            case 4:
                GetStudentsGrade();
                break;
            case 5:
                GetListCourseAndAvarageGrade();
                break;
            case 6:
                AddStudents();
                break;
            case 7:
                AddStaff();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please pick from the options");
                Console.ResetColor();
                break;
        }
    }
    public void ReturnToMenu() // This is a simple method i created just to return to main menu
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Press any key to go back to the menu");
        Console.ReadKey(true);
        Console.Clear();
        Console.WriteLine("Returning to main menu");
        Console.ResetColor();
        Thread.Sleep(1000);
        ShowMenu();
    }
    public void GetStaff() //This method is to show all staff in the database.
                           //This method is also using a switch case for different options the user would like to se,
                           //If user would like to se all staffs or individual staffs like all teachers, admins etc.
                           //Then i create two different foreach loops one for showing all staffs with role and one for specifik staff like teacher,admin etc.
    {
        Console.Clear();
        try
        {
            using (var context = new SchoolContext()) //Here i create a new instance of SchoolContext with a name of context, so we can interact with the database
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Choose what you would like to see:");
                Console.WriteLine("1. Show all staffs");
                Console.WriteLine("2. Show all Teachers");
                Console.WriteLine("3. Show all Admins");
                Console.WriteLine("4. Show all Principals");
                Console.WriteLine("5. Show all cleaners");
                Console.WriteLine("6. Show all cooks");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    Console.ResetColor();
                    ReturnToMenu();
                }

                // Here i create two IQuaryables variables that are attached to the staffs table and also proffesion table in the database.
                // So we can create linq code to show the data from the tables
                IQueryable<Staff> query = context.Staffs;
                IQueryable<Proffesion> roleQuery = context.Proffesions;

                //Here i create a join between two tables because i want to use column ProffesionName to be able to show what type of role staff have.
                // So i take the primary key in proffesion table and match it with the foreign key in table staff
                // Then i create a new data structure with select new, there we create new objects of an anonymus type as the result of the query
                var staffWithRoles = from staff in query
                                     join role in roleQuery on staff.FkproffesionId equals role.ProffesionId
                                     select new
                                     {
                                         Staff = staff,
                                         RoleName = role.ProffesionName
                                     };
                switch (choice)
                {
                    case 1:
                        var wholeStaffList = staffWithRoles.ToList(); // converts the result of the linq query to the list so we can use it in a foreach loop
                        Console.WriteLine("All Staff:");
                        if (wholeStaffList.Any()) //I use .Any method to check if wholeStaffList contains any data
                        {
                            foreach (var staffWithRole in wholeStaffList)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("ID: {0} Name {1} {2} Role: {3}", staffWithRole.Staff.StaffId, staffWithRole.Staff.StaffFirstName, staffWithRole.Staff.StaffLastName, staffWithRole.RoleName);
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No staffs found");
                            Console.ResetColor();
                        }
                        break;
                    case 2:
                        Console.WriteLine("All teachers");
                        query = query.Where(s => s.FkproffesionId == 1); //I filter the query collection with .Where method and use lambda expression to get all staffs with FkproffesionId of 1
                        break;
                    case 3:
                        Console.WriteLine("All Admins");
                        query = query.Where(s => s.FkproffesionId == 2);
                        break;
                    case 4:
                        Console.WriteLine("All Principals");
                        query = query.Where(s => s.FkproffesionId == 3);
                        break;
                    case 5:
                        Console.WriteLine("All cleaners");
                        query = query.Where(s => s.FkproffesionId == 4);
                        break;
                    case 6:
                        Console.WriteLine("All cooks");
                        query = query.Where(s => s.FkproffesionId == 5);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                if (choice < 1 || choice > 6)
                {
                    ReturnToMenu();
                }

                var staffList = query.ToList(); //Converts the query to a list with name of staffList so we can use foreach loop for the staff

                if (query.Any() && choice != 1)
                {
                    foreach (var staffQuery in staffList)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("ID: {0} Name {1} {2} ", staffQuery.StaffId, staffQuery.StaffFirstName, staffQuery.StaffLastName);
                        Console.ResetColor();
                    }
                }
                else if (choice != 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No staffs found");
                    Console.ResetColor();
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + ex.Message);
            Console.ResetColor();
        }
        ReturnToMenu();
    }

    public void GetStudents()// This method is for showing all the students in the database
                             // User get to choose which order he wants to show all students in
                             // Users first choise is if he want to sort by first name or last name,
                             // Then if user wants it to be in asc or desc order
                             //Im using try parse so we dont crash the program if user dont type a integer
                             //Depending on if user wants it in asc or desc order we use method .OrderBy for asc order with lambda expression to sort by either first name or last name
                             // And for desc order we use method .OrderByDescending with lambda expression to sort by either first name or last name
    {
        Console.Clear();
        try
        {
            using (var context = new SchoolContext())
            {
                IQueryable<Student> studentsQuery = context.Students;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Get all students");
                Console.WriteLine("Do you want to get all students sorted by first or last name?\n1. First name \n2.Last name");
                if (int.TryParse(Console.ReadLine(), out int userInput) && userInput == 1)
                {
                    Console.WriteLine("You selected first name");
                    Console.WriteLine("Would you like it to be in ascending order or descending order\n1. ASC\n2.DESC");
                    if (int.TryParse(Console.ReadLine(), out int orderInput) && orderInput == 1)
                    {
                        Console.WriteLine("Here are all the students sorted by first name in ascending order");
                        studentsQuery = context.Students.OrderBy(x => x.StudentFirstName);
                    }
                    else if (orderInput == 2)
                    {
                        Console.WriteLine("Here are all the students sorted by first name in descending order");
                        studentsQuery = context.Students.OrderByDescending(x => x.StudentFirstName);
                    }
                }
                else if (userInput == 2)
                {
                    Console.WriteLine("You selected last name");
                    Console.WriteLine("Would you like it to be in ascending order or descending order\n1. ASC\n2.DESC");
                    if (int.TryParse(Console.ReadLine(), out int orderInput) && orderInput == 1)
                    {
                        Console.WriteLine("Here are all the students sorted by last name in ascending order");
                        studentsQuery = context.Students.OrderBy(x => x.StudentLastName);
                    }
                    else if (orderInput == 2)
                    {
                        Console.WriteLine("Here are all the students sorted by last name in descending order");
                        studentsQuery = context.Students.OrderByDescending(x => x.StudentLastName);
                    }
                }
                Console.ResetColor();

                var sortedStudents = studentsQuery.ToList(); //Converts the studentQuery to a list with name of sortedStudents so we can use foreach loop to print all students
                if (studentsQuery.Any())
                {

                    foreach (var student in sortedStudents)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Id: {0} name {1} {2}", student.StudentId, student.StudentFirstName, student.StudentLastName);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No students in the database.");
                    Console.ResetColor();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        ReturnToMenu();
    }
    public void GetStudentsFromClass()// Method to show all students in a specific class from the database
                                      // I create two IQuaryables variables that are attached to Class table and Student table in the database with name of queryClass and queryStudents.
                                      // I add query to the list and name it to classList. Then i check with if statement if there are any data in classlist
                                      // Then i make a foreach loop to display all the classes
                                      //Then I create a switch statement and depending on which class the user wants to see, it shows all the students in that class
                                      // I filter with .Where method and use lambda expression to sort by FkclassId to get specifik classname
                                      // Then i add queryStudent to list with name of studentList to create a foreach loop so we can print students in specifik class
    {
        using (var context = new SchoolContext())
        {
            IQueryable<Class> queryClass = context.Classes;
            IQueryable<Student> queryStudents = context.Students;

            var classList = queryClass.ToList();
            int count = 1;

            if (classList.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("All classes");
                Console.ResetColor();
                foreach (var allClasses in classList)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0}. {1}", count++, allClasses.ClassName);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are no classes in this school");
                Console.ResetColor();
            }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please choose one class you want to se all the students in");
                if (int.TryParse(Console.ReadLine(), out int choise) && choise >= 1 || choise <= 8)
                {
                    switch (choise)
                    {
                        case 1:
                            Console.WriteLine("All students from Te13:");
                            queryStudents = queryStudents.Where(s => s.FkclassId == 1);
                            break;
                        case 2:
                            Console.WriteLine("All students from Te14:");
                            queryStudents = queryStudents.Where(s => s.FkclassId == 2);
                            break;
                        case 3:
                            Console.WriteLine("All students from Te15:");
                            queryStudents = queryStudents.Where(s => s.FkclassId == 3);
                            break;
                        case 4:
                            Console.WriteLine("All students from Te16:");
                            queryStudents = queryStudents.Where(s => s.FkclassId == 4);
                            break;
                        case 5:
                            Console.WriteLine("All students from Na13:");
                            queryStudents = queryStudents.Where(s => s.FkclassId == 5);
                            break;
                        case 6:
                            Console.WriteLine("All students from Na14:");
                            queryStudents = queryStudents.Where(s => s.FkclassId == 6);
                            break;
                        case 7:
                            Console.WriteLine("All students from Na15:");
                            queryStudents = queryStudents.Where(s => s.FkclassId == 7);
                            break;
                        case 8:
                            Console.WriteLine("All students from Na16:");
                            queryStudents = queryStudents.Where(s => s.FkclassId == 8);
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input, pick any of the numbers");
                }

            var studentList = queryStudents.ToList();
            foreach (var students in studentList)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ID {0} Name {1} {2}", students.StudentId, students.StudentFirstName, students.StudentLastName);
                Console.ResetColor();
            }
            Console.ResetColor();
            ReturnToMenu();
        }
    }
    public void GetStudentsGrade() // This method shows all the students grade from every course they have where grades where set from last month
                                   // In this method i start of by adding a variable for todays date and one more variable where i substract one month from todays date
                                   // Then i use linq query to combine data from different table with join
                                   // Then i use "where" clause that filters based on the date range
                                   // After that i add that query to a list with name of result so i can loop with foreach loop
    {
        DateTime today = DateTime.Now;
        DateTime oneMonthBefore = today.AddMonths(-1);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Grades from all students between {0} and {1}", oneMonthBefore, today);
        Console.WriteLine();
        Console.WriteLine("Grade 1 = lowest grade, grade 6 = highest grade");
        Console.ResetColor();
        Console.WriteLine();

        using (var context = new SchoolContext())
        {
            var query = from enrollment in context.Enrollments
                        join student in context.Students on enrollment.FkstudentId equals student.StudentId
                        join course in context.Courses on enrollment.FkcourseId equals course.CourseId
                        join grade in context.Grades on enrollment.FkgradesId equals grade.GradesId
                        where enrollment.GradeSetDay >= oneMonthBefore && enrollment.GradeSetDay <= today
                        select new
                        {
                            StudentName = student.StudentFirstName,
                            CourseName = course.CourseName,
                            Grade = grade.Grade1,
                            GradeSetDay = enrollment.GradeSetDay
                        };

            var result = query.ToList();

            foreach (var item in result)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Student Name: {0} Course: {1} Grade: {2} Grade set day: {3}", item.StudentName, item.CourseName, item.Grade, item.GradeSetDay);
                Console.ResetColor();
            }
        }
        ReturnToMenu();
    }
    public void GetListCourseAndAvarageGrade() // This method shows average grade from every course, also highest grade and lowest grade from every course
                                               // First i create a different joins to combine data from tables Enrollment,Grade and Course based on their relations with foreign keys and primary keys
                                               // Then i group all the tables by FkCourseId and name it to groupedEnrollments
                                               // Inside select new i pick the information i want from the grouped result
                                               // First i want to pick CourseName so i can print out the name of the course
                                               // Then i use .Average method to calculate the average grades from every course
                                               // Then i use methods .Max and .Min to pick out the highest grade and lowest grade from every course
                                               // then i save query to a list with name of courseStatistics so i can print all the info in a foreach loop
    {
        using (var context = new SchoolContext())
        {
            var query = from enrollment in context.Enrollments
                        join grade in context.Grades on enrollment.FkgradesId equals grade.GradesId
                        join course in context.Courses on enrollment.FkcourseId equals course.CourseId
                        group new { Enrollment = enrollment, Grade = grade, Courses = course } by enrollment.FkcourseId into groupedEnrollments
                        select new
                        {
                            CourseName = groupedEnrollments.First().Courses.CourseName,
                            AverageGrade = groupedEnrollments.Select(j => j.Grade.Grade1).Average(),
                            MaxGrade = groupedEnrollments.Select(j => j.Grade.Grade1).Max(),
                            MinGrade = groupedEnrollments.Select(j => j.Grade.Grade1).Min()
                        };
            var courseStatistics = query.ToList();
            Console.WriteLine("Average grade from every course, also max/min grade from courses");
            Console.WriteLine();
            foreach (var statistics in courseStatistics)
            {
                Console.WriteLine("Course: {0}", statistics.CourseName);
                Console.WriteLine("Average grade: {0}", statistics.AverageGrade);
                Console.WriteLine("Max grade: {0}", statistics.MaxGrade);
                Console.WriteLine("Min grade: {0}", statistics.MinGrade);
                Console.WriteLine();
            }
        }
    }
    public void AddStudents() //Method to add new students in the database
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Add a student to the school");
        Console.WriteLine();
        Console.WriteLine("Type in student first name:");
        string firstName = Console.ReadLine();
        Console.WriteLine("Type in student last name:");
        string lastName = Console.ReadLine();
        Console.WriteLine("Type students social security number");
        string socialSecurityNumber = Console.ReadLine();
        Console.WriteLine("Which class is the student sarting in \n1.Te\n2.Na");
        Console.ResetColor();
        int yearOfBirth = int.Parse(socialSecurityNumber.Substring(0, 4)); //Using method .Substring to take year from social security number

        if (int.TryParse(Console.ReadLine(), out int classInput))
        {
            if (classInput == 1) // equals to class Te
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Confirm student:\nName: {0} {1} Social security number: {2} Class: Te", firstName, lastName, socialSecurityNumber);
                Console.WriteLine("1. Confirm \n2.Cancel");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out int confirmInput))
                {
                    if (yearOfBirth > 2000 || yearOfBirth < 1997) // If age of the student is over 2000 or under 1997
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Student is either too old or too young for this school");
                        Console.ResetColor();
                        ReturnToMenu();
                    }
                    else if (confirmInput == 1) //If user confirmed
                    {
                        using (var context = new SchoolContext())
                        {
                            var newStudent = new Student
                            {
                                StudentFirstName = firstName,
                                StudentLastName = lastName,
                                SocialSecurityNumber = socialSecurityNumber,
                                FkclassId = GetClassIdFromYearOfBirth(yearOfBirth, classInput), //Here i check with method GetClassIdFromYearOfBirth so student joins the right class by age and class id
                            };

                            context.Students.Add(newStudent); // Adding new student to database
                            context.SaveChanges(); // Savning changes to database

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("{0} {1} has been added to the school", firstName, lastName);
                            Console.ResetColor();
                        }
                    }
                    else if (confirmInput == 2) // Canceling confirmation
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No student has been added to the school");
                        Console.ResetColor();
                    }
                }
            }
            else if (classInput == 2) //If student will start in class Na
            {
                Console.WriteLine("Confirm student:\nName: {0} {1} Social security number: {2} Class: Na", firstName, lastName, socialSecurityNumber);
                Console.WriteLine("1. Confirm \n2.Cancel");
                if (int.TryParse(Console.ReadLine(), out int confirmInput))
                {
                    if (confirmInput == 1) //If user confirmed
                    {
                        using (var context = new SchoolContext())
                        {
                            var newStudent = new Student
                            {
                                StudentFirstName = firstName,
                                StudentLastName = lastName,
                                SocialSecurityNumber = socialSecurityNumber,
                                FkclassId = GetClassIdFromYearOfBirth(yearOfBirth, classInput),
                            };

                            context.Students.Add(newStudent); // Adding new student to database
                            context.SaveChanges(); // Savning changes to database
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("{0} {1} has been added to the school", firstName, lastName);
                            Console.ResetColor();
                        }
                    }
                    else if (confirmInput == 2) // Canceling confirmation
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No user has been added to the school");
                        Console.ResetColor();
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
        }
        ReturnToMenu();
    }
    public void AddStaff() // Method to add new staff in the database.
                           // This method is simular to AddStudent method but in this method i create a switch case depending on what role staff should have
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Add a new staff to the database!");
        Console.WriteLine();
        Console.WriteLine("Enter staff first name:");
        string staffFirstName = Console.ReadLine();
        Console.WriteLine("Enter staff Last name");
        string staffLastName = Console.ReadLine();
        Console.WriteLine("What role does the staff have?\n1.Teacher\n2.Admin\n3.Principal\n4.Cleaner\n5.Cook");
        Console.ResetColor();
        if (int.TryParse(Console.ReadLine(), out int input))
        {
            using (var context = new SchoolContext())
            {
                switch (input)
                {
                    case 1:
                        var teacher = new Staff
                        {
                            StaffFirstName = staffFirstName,
                            StaffLastName = staffLastName,
                            FkproffesionId = input
                        };
                        context.Staffs.Add(teacher);
                        context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0} {1} has been added to the database and will have role as teacher", staffFirstName, staffLastName);
                        Console.ResetColor();
                        break;
                    case 2:
                        var admin = new Staff
                        {
                            StaffFirstName = staffFirstName,
                            StaffLastName = staffLastName,
                            FkproffesionId = input
                        };
                        context.Staffs.Add(admin);
                        context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0} {1} has been added to the database and will have role as admin", staffFirstName, staffLastName);
                        Console.ResetColor();
                        break;
                    case 3:
                        var principal = new Staff
                        {
                            StaffFirstName = staffFirstName,
                            StaffLastName = staffLastName,
                            FkproffesionId = input
                        };
                        context.Staffs.Add(principal);
                        context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0} {1} has been added to the database and will have role as principal", staffFirstName, staffLastName);
                        Console.ResetColor();
                        break;
                    case 4:
                        var cleaner = new Staff
                        {
                            StaffFirstName = staffFirstName,
                            StaffLastName = staffLastName,
                            FkproffesionId = input
                        };
                        context.Staffs.Add(cleaner);
                        context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0} {1} has been added to the database and will have role as cleaner", staffFirstName, staffLastName);
                        Console.ResetColor();
                        break;
                    case 5:
                        var cook = new Staff
                        {
                            StaffFirstName = staffFirstName,
                            StaffLastName = staffLastName,
                            FkproffesionId = input
                        };
                        context.Staffs.Add(cook);
                        context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0} {1} has been added to the database and will have role as cook", staffFirstName, staffLastName);
                        Console.ResetColor();
                        break;
                    default:
                        Console.WriteLine("Invalid input. try again");
                        break;

                }
                ReturnToMenu();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid choise, please pick any of the options");
            Console.ResetColor();
        }
    }

    private int GetClassIdFromYearOfBirth(int yearOfBirth, int className)
    // Method for adding students in database that contains two parameters first the year student was born in and then which class student will start in. Method will return a integer
    // Depending on the age and class the user specifies when adding new student
    // So for example, user types in year 1999 for student and class of 1 which equals to class Te.
    // Then we go inside if statement that matches that input from user and it will return 3 that equals te15
    {
        if (yearOfBirth == 1997 && className == 1)
        {
            return 1;
        }
        if (yearOfBirth == 1998 && className == 1)
        {
            return 2;
        }
        if (yearOfBirth == 1999 && className == 1)
        {
            return 3;
        }
        if (yearOfBirth == 2000 && className == 1)
        {
            return 4;
        }
        if (yearOfBirth == 1997 && className == 2)
        {
            return 5;
        }
        if (yearOfBirth == 1998 && className == 2)
        {
            return 6;
        }
        if (yearOfBirth == 1999 && className == 2)
        {
            return 7;
        }
        if (yearOfBirth == 2000 && className == 2)
        {
            return 8;
        }
        else
        {
            return 0; // If no matches found they wont get a specifik class
        }
    }
}
