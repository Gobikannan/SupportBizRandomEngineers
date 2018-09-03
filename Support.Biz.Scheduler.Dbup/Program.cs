using DbUp;
using System;
using System.Reflection;

namespace Support.Biz.Scheduler.Dbup
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initiating DB setup...");

            //For development purpose, I kept the connection string in the Application Arguments => Project settings -> Debug -> Application Arguments
            //this way, we can provide this variable while autoamting the builds through Octopus or VSTS
            if(args.Length == 0)
            {
                throw new ArgumentNullException("ConnectionString");
            }
            var connectionString = args[0];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            Console.WriteLine(connectionString);

            EnsureDatabase.For.SqlDatabase(connectionString);
            var upgrader = DeployChanges.To.SqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()).LogToConsole().Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ReadLine();
            Console.ResetColor();
        }
    }
}
