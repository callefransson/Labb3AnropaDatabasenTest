namespace Labb3AnropaDatabasenTest.SchoolModels
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SchoolContext context = new SchoolContext();
            context.ShowMenu();
        }
    }
}
