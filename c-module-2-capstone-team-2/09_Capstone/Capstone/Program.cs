using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Drawing;
using Console = Colorful.Console;
using Capstone.Views;
using Capstone.DAL;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {

            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("Project");

            IParkDAO parkDAO = new ParkSqlDAO(connectionString);
            ICampgroundDAO campgroundDAO = new CampgroundSqlDAO(connectionString);
            ISiteDAO siteDAO = new SiteSqlDAO(connectionString);
            IReservationDAO reservationDAO = new ReservationSqlDAO(connectionString);


            CLIMainMenu cliMainMenu = new CLIMainMenu(parkDAO, campgroundDAO, siteDAO, reservationDAO);

            cliMainMenu.Run();

            //TODO write tests

            //Console.WriteLine(@"                                                                                                                                                             A");
            //Console.WriteLine(@"                                                                                                                                                            d$b");
            //Console.WriteLine(@"                                                                                                                                                          .d\$$b.");
            //Console.WriteLine(@" ___                _         ___                                            _                        ___                 _                            .d$i$$\$$b.");
            //Console.WriteLine(@"(  _`\             ( )       |  _`\                                         ( )_  _                  (  _`\              ( )_                             d$$@b");
            //Console.WriteLine(@"| |_) )  _ _  _ __ | |/')    | (_) )   __    ___    __   _ __  _   _    _ _ | ,_)(_)   _     ___     | (_(_) _   _   ___ | ,_)   __    ___ ___           d\$$$ib");
            //Console.WriteLine(@"| ,__/'/'_` )( '__)| , <     | ,  /  /'__`\/',__) /'__`\( '__)( ) ( ) /'_` )| |  | | /'_`\ /' _ `\   `\__ \ ( ) ( )/',__)| |   /'__`\/' _ ` _ `\       .d$$$\$$$b");
            //Console.WriteLine(@"| |   ( (_| || |   | |\`\    | |\ \ (  ___/\__, \(  ___/| |   | \_/ |( (_| || |_ | |( (_) )| ( ) |   ( )_) || (_) |\__, \| |_ (  ___/| ( ) ( ) |     .d$$@$$$$\$$ib.");
            //Console.WriteLine(@"(_)   `\__,_)(_)   (_) (_)   (_) (_)`\____)(____/`\____)(_)   `\___/'`\__,_)`\__)(_)`\___/'(_) (_)   `\____)`\__, |(____/`\__)`\____)(_) (_) (_)          d$$i$$b");
            //Console.WriteLine(@"                                                                                                            ( )_| |                                     d\$$$$@$b");
            //Console.WriteLine(@"                                                                                                            `\___/'                                  .d$@$$\$$$$$@b.");
            //Console.WriteLine(@"                                                                                                                                                   .d$$$$i$$$\$$$$$$b.");
            //Console.WriteLine(@"                                                                                                                                                           ###");
            //Console.WriteLine(@"                                                                                                                                                           ###");
            //Console.WriteLine(@"                                                                                                                                                           ###");
        }
    }
}
