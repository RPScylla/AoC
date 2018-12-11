using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace RPSonline.AoC
{
    class Program
    {
        private readonly CompositionContainer _container;

        static void Main(string[] args)
        {
            Program p = new Program();
        }

        private Program()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            if (Directory.Exists("Years"))
            {
                catalog.Catalogs.Add(new DirectoryCatalog("Years"));
            }
            _container = new CompositionContainer(catalog);

            try
            {
                AoCRunner aoCRunner = new AoCRunner();

                _container.ComposeParts(aoCRunner);

                aoCRunner.Run();
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

            Console.ReadKey();
        }
    }
}
