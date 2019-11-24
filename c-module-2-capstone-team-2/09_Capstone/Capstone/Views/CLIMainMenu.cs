using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Console = Colorful.Console;

namespace Capstone.Views
{
    public class CLIMainMenu
    {
        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;

        public CLIMainMenu(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

       
        //startup for first menu
        public void Run()
        {
            while (true)
            {
            PrintHeader();
            PrintMainMenu();

            string command = Console.ReadLine();

            if (command.ToUpper() == "Q")
            {
                return;
            }
            try
                {
                    Console.Clear();
                    int parkID = int.Parse(command);
                    IList<Park> parks = parkDAO.GetAllParks();

                    Park selectedPark = parkDAO.GetInfo(parkID);

                    PrintSelectedPark(selectedPark);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Please select valid park");
                }

            }

        }
        //prints information for park selected off first menu
        private void PrintSelectedPark(Park selectedPark)
        {
            while (true)
            {
                List<string> wrappedDesc = Wrap(selectedPark.Description, 100);

                PrintHeader();
                System.Console.WriteLine();
                System.Console.WriteLine("                        " + selectedPark.Name.PadRight(50));
                System.Console.WriteLine();
                System.Console.WriteLine("                        " + "Location: ".PadRight(50) + selectedPark.Location.PadRight(10));
                System.Console.WriteLine($"                        Established:".PadRight(74) + $"{selectedPark.EstDate:d}".PadRight(10));
                System.Console.WriteLine("                        " + "Area: ".PadRight(50) + $"{selectedPark.Area:n0}" + " sq km");
                System.Console.WriteLine("                        " + "Annual Visitors: ".PadRight(50) + $"{selectedPark.Visitors:n0}");
                System.Console.WriteLine();
                System.Console.WriteLine();
                for (int i = 0; i < wrappedDesc.Count; i++)
                {
                    System.Console.WriteLine("       " + wrappedDesc[i]);
                }
                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine("       " + "Select a Command");
                System.Console.WriteLine("          " + "1) View Campgrounds");
                System.Console.WriteLine("          " + "2) Search For Reservations");
                System.Console.WriteLine("          " + "3) Return to Previous Screen");

                string command = Console.ReadLine();

                switch (command.ToLower())
                {
                    case "1":
                        ViewCampgroundMenu(selectedPark);
                        break;
                    case "2":
                        ParkWideReservation(selectedPark);
                        break;
                    case "3":
                        Console.Clear();
                        return;
                    default:
                        System.Console.WriteLine("Type the right thing dumbie!!!");
                        Thread.Sleep(2000);
                        Console.Clear();
                        break;
                }
            }

        }

        //first menu displaying park names
        private void PrintMainMenu()
        {
            IList<Park> parks = parkDAO.GetAllParks();

            if (parks.Count > 0)
            {
                System.Console.WriteLine("                        " + "Select a Park for Further Details");
                System.Console.WriteLine("                      " + "======================================");
                foreach (Park park in parks)
                {
                    Console.WriteLine("                            " + park.Id.ToString() + ")".PadRight(10) + park.Name.PadRight(40));
                }

            }
            else
            {
                Console.WriteLine("                        " + "NO RESULTS");
            }
            System.Console.WriteLine("                            " + "Q)".PadRight(10) + " Quit");
        }

        public void ViewCampgroundMenu(Park selectedPark)
        {
            while (true)
            {
                IList<Campground> campgrounds = campgroundDAO.GetAllCampgrounds(selectedPark);

                Console.Clear();
                PrintHeader();

                Console.WriteLine($"{selectedPark.Name} National Park Campgrounds".PadLeft(10));
                System.Console.WriteLine();
                System.Console.WriteLine();

                System.Console.WriteLine("Camp ID".PadRight(15) + "Name".PadRight(40) + "Open".PadRight(10) + "Close".PadRight(20) + "Daily Fee".PadRight(20));

                Dictionary<int, string> Months = new Dictionary<int, string>() { { 01, "January" }, { 02, "February" }, { 03, "March" }, { 04, "April" }, { 05, "May" }, { 06, "June" }, { 07, "July" }, { 08, "August" }, { 09, "September" }, { 10, "October" }, { 11, "November" }, { 12, "December" } };

                for (int i = 0; i < campgrounds.Count; i++)
                {
                    System.Console.WriteLine($"#{campgrounds[i].CampgroundId}".PadRight(15) + $"{campgrounds[i].CampName}".PadRight(40) + $"{Months[campgrounds[i].Open_From_MM]}".PadRight(10) + $"{Months[campgrounds[i].Open_To_MM]}".PadRight(20) + $"{campgrounds[i].Daily_Fee:c}".PadRight(20));
                }
                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine("Select a Command");
                System.Console.WriteLine("".PadRight(3) + "1) Search for Available Reservation");
                System.Console.WriteLine("".PadRight(3) + "2) Return to Previous Screen");

                string command = Console.ReadLine();

                switch (command.ToLower())
                {
                    case "1":
                        FindAvailableSitesMenu();
                        break;
                    case "2":
                        Console.Clear();
                        return;
                    default:
                        System.Console.WriteLine("Type the right thing dumbie!!!");
                        Thread.Sleep(2000);
                        Console.Clear();
                        break;
                }
            }
        }

        public void ParkWideReservation(Park selectedPark)
        {
            //TODO make it promt user for new dates when no sites are available within dates specified            
            System.Console.WriteLine("What is the arrival date? yyyy/mm/dd");
            DateTime desiredArrival = DateTime.Parse(Console.ReadLine());
            System.Console.WriteLine("What is the departure date? yyyy/mm/dd");
            DateTime desiredDeparture = DateTime.Parse(Console.ReadLine());
            System.Console.WriteLine();
            System.Console.WriteLine();
            int duration = (int)(desiredDeparture.Subtract(desiredArrival).TotalDays) + 1;
            IList<Site> sites = siteDAO.ListAvailableSitesInPark(selectedPark, desiredArrival, desiredDeparture);

            printSiteListHeadingParkWide();

            foreach (var site in sites)
            {
                printSiteListingParkWide(site, duration);
            }
            System.Console.Write("Which site should be reserved (enter 0 to cancel)? ");
            int selectedSiteNum = int.Parse(Console.ReadLine());
            int selectedSiteId = 0;
            foreach (Site site in sites)
            {
                if (site.SiteNumber == selectedSiteNum)
                {
                    selectedSiteId = site.SiteId;
                }
            }
            if (selectedSiteId == 0)
            {
                Console.Clear();
                return;
            }

            System.Console.Write("What name should the reservation be made under? ");
            string reservationName = Console.ReadLine();

            int conformationNumber = reservationDAO.MakeReservation(selectedSiteId, reservationName, desiredArrival, desiredDeparture);

            System.Console.WriteLine($"The reservation has been made and the conformation id is {conformationNumber}");
            Console.ReadLine();
        }

        private void FindAvailableSitesMenu()
        {
            int duration;
            IList<Site> sites;
            DateTime desiredArrival;
            DateTime desiredDeparture;
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("Enter ID# of desired campground (enter 0 to cancel):");
            int desiredCampgroundId = int.Parse(Console.ReadLine());
            if (desiredCampgroundId == 0)
            {
                return;
            }
            while (true)
            {
                System.Console.WriteLine("What is the arrival date? yyyy/mm/dd");
                desiredArrival = DateTime.Parse(Console.ReadLine());
                System.Console.WriteLine("What is the departure date? yyyy/mm/dd");
                desiredDeparture = DateTime.Parse(Console.ReadLine());
                System.Console.WriteLine();
                System.Console.WriteLine();
                duration = (int)(desiredDeparture.Subtract(desiredArrival).TotalDays) + 1;
                sites = siteDAO.ListAvailableSites(desiredCampgroundId, desiredArrival, desiredDeparture);
                if (sites.Count == 0)
                {
                    System.Console.WriteLine("We're sorry, no sites were available within those dates, please enter another date range and try again");
                    Thread.Sleep(4000);
                    continue;
                }
                else
                {
                    break;
                }
            }

            printSiteListHeading();
            
            foreach (var site in sites)
            {
                printSiteListing(site, duration);
            }
            System.Console.Write("Which site should be reserved (enter 0 to cancel)? ");
            int selectedSiteNum = int.Parse(Console.ReadLine()); 
            int selectedSiteId = 0;
            foreach (Site site in sites)
            {
                if (site.SiteNumber == selectedSiteNum)
                {
                    selectedSiteId = site.SiteId;
                }
            }
            if (selectedSiteId == 0)
            {
                return;
            }

            System.Console.Write("What name should the reservation be made under? ");
            string reservationName = Console.ReadLine();

            int conformationNumber = reservationDAO.MakeReservation(selectedSiteId, reservationName, desiredArrival, desiredDeparture);

            System.Console.WriteLine($"The reservation has been made and the conformation id is {conformationNumber}");
            Console.ReadLine();
        }

        private static void printSiteListingParkWide(Site site, int duration)
        {
            System.Console.WriteLine(site.printListingForParkWide(duration));
        }

        private static void printSiteListHeadingParkWide()
        {
            foreach (string headingLine in Site.HeadingForParkWide())
            {
                System.Console.WriteLine(headingLine);
            }
        }

        private static void printSiteListing(Site site, int duration)
        {
          // maybe clear Console.Clear(); 
            System.Console.WriteLine(site.printListing(duration));
        }

        private static void printSiteListHeading()
        {
            foreach(string headingLine in Site.Heading())
            {
                System.Console.WriteLine(headingLine);
            }
        }

        //wraps description (CREDIT Mike-Ward.net) counts back returns lines
        static List<string> Wrap(string text, int margin)
        {
            int start = 0;
            int end;
            List<string> lines = new List<string>();
            text = Regex.Replace(text, @"\s", " ").Trim();

            while ((end = start + margin) < text.Length)
            {
                while (text[end] != ' ' && end > start)
                {
                    end -= 1;
                }

                if (end == start)
                {
                    end = start + margin;
                }

                lines.Add(text.Substring(start, end - start));
                start = end + 1;
            }

            if (start < text.Length)
                lines.Add(text.Substring(start));

            return lines;
        }

        //Prints gradiate header image and font
        private void PrintHeader()
        {

            //DONE make it print one tree with console height/width when small screne : )
            int redTree = 164;
            int greenTree = 20;
            int blueTree = 24;

            int redTrunk = 137;
            int greenTrunk = 39;
            int blueTrunk = 39;

            string[] headerFont = new string[]
            {
                @"                                                                                                                                                       ",
                @"                                                                                                                                                       ",
                @"                                                                                                                                                       ",
                @"                                                                                                                                                       ",
                @"   ___                _         ___                                            _                        ___                 _                          ",
                @"  (  _`\             ( )       |  _`\                                         ( )_  _                  (  _`\              ( )_                        ",
                @"  | |_) )  _ _  _ __ | |/')    | (_) )   __    ___    __   _ __  _   _    _ _ | ,_)(_)   _     ___     | (_(_) _   _   ___ | ,_)   __    ___ ___       ",
                @"  | ,__/'/'_` )( '__)| , <     | ,  /  /'__`\/',__) /'__`\( '__)( ) ( ) /'_` )| |  | | /'_`\ /' _ `\   `\__ \ ( ) ( )/',__)| |   /'__`\/' _ ` _ `\     ",
                @"  | |   ( (_| || |   | |\`\    | |\ \ (  ___/\__, \(  ___/| |   | \_/ |( (_| || |_ | |( (_) )| ( ) |   ( )_) || (_) |\__, \| |_ (  ___/| ( ) ( ) |     ",
                @"  (_)   `\__,_)(_)   (_) (_)   (_) (_)`\____)(____/`\____)(_)   `\___/'`\__,_)`\__)(_)`\___/'(_) (_)   `\____)`\__, |(____/`\__)`\____)(_) (_) (_)     ",
                @"                                                                                                              ( )_| |                                  ",
                @"                                                                                                              `\___/'                                  ",
                @"                                                                                                                                                       ",
                @"                                                                                                                                                       ",
                @"                                                                                                                                                       ",
                @"                                                                                                                                                       "
            };

            string[] headerTree = new string[]
            {
                @"                    ",
                @"         A          ",
                @"        d$b         ",
                @"      .d\$$b.       ",
                @"    .d$i$$\$$b.     ",
                @"       d$$@b        ",
                @"      d\$$$ib       ",
                @"    .d$$$\$$$b      ",
                @"  .d$$@$$$$\$$ib.   ",
                @"      d$$i$$b       ",
                @"     d\$$$$@$b      ",
                @"  .d$@$$\$$$$$@b.   ",
                @".d$$$$i$$$\$$$$$$b. ",
                @"        ###         ",
                @"        ###         ",
                @"        ###         "
            };

            //if on small screen
            if (Console.WindowWidth < 173)
            {
                for (int i = 0; i < headerFont.Length; i++)
                {
                    while (i < 13)
                    {
                        Console.Write(headerTree[i], Color.FromArgb(redTree, greenTree, blueTree));
                        Console.WriteLine(headerFont[i], Color.FromArgb(redTree, greenTree, blueTree));

                        greenTree += 14;
                        redTree -= 8;
                        blueTree += 3;
                        i++;
                    }

                    Console.Write(headerTree[i], Color.FromArgb(redTrunk, greenTrunk, blueTrunk));
                    System.Console.WriteLine(headerFont[i], Color.FromArgb(redTrunk, greenTrunk, blueTrunk));
                }

                Console.WriteLine("                                                                                                                                                                            ");
                Console.WriteLine("===========================================================================================================================================================================");
                Console.WriteLine("===========================================================================================================================================================================");
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
            else //if on bigger screen
            {
                for (int i = 0; i < headerFont.Length; i++)
                {
                    while (i < 13)
                    {
                        Console.Write("   " + headerTree[i], Color.FromArgb(redTree, greenTree, blueTree));
                        Console.Write(headerFont[i], Color.FromArgb(redTree, greenTree, blueTree));
                        Console.WriteLine(headerTree[i], Color.FromArgb(redTree, greenTree, blueTree));

                        greenTree += 14;
                        redTree -= 8;
                        blueTree += 3;
                        i++;
                    }

                    Console.Write("   " + headerTree[i], Color.FromArgb(redTrunk, greenTrunk, blueTrunk));
                    System.Console.Write(headerFont[i], Color.FromArgb(redTrunk, greenTrunk, blueTrunk));
                    Console.WriteLine(headerTree[i], Color.FromArgb(redTrunk, greenTrunk, blueTrunk));
                }

                Console.WriteLine("                                                                                                                                                                            ");
                Console.WriteLine("==================================================================================================================================================================================================");
                Console.WriteLine("==================================================================================================================================================================================================");
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
        }
    }
}
