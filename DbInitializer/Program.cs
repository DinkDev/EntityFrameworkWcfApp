namespace DbInitializer
{
    using DataAccess.EF;
    using System;
    using DataAccess;

    internal class Program
    {
        private static void Main()
        {
            using (var context = new AutoLotContext())
            using (var uow = new UnitOfWork(context))
            {
                if (!uow.CanConnect())
                {
                    Console.WriteLine($"Creating database {uow.Context.Database.Connection.Database}");
                    uow.InitializeDatabase();
                }

                var test = uow.CanConnect();

                Console.WriteLine($"Able to read database {uow.Context.Database.Connection.Database}: {test}");
                Console.WriteLine();
                Console.WriteLine(@"Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
