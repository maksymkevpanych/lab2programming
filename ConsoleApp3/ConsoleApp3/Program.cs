using System;
using System.Collections.Generic;
using System.Linq;

public class AviaFlight
{
    public int FlightNumber { get; set; }
    public string DepartureCity { get; set; }
    public DateTime DepartureTime { get; set; }
    public int TotalSeats { get; set; }
    public string Destination { get; set; }
}

public class Ticket
{
    public int TicketNumber { get; set; }
    public int FlightNumber { get; set; }
    public string PassengerName { get; set; }
    public int Seat { get; set; }
    public DateTime DepartureDate { get; set; }
    public string Destination { get; set; }
    public DateTime ArrivalDateTime { get; set; }
    public double Price { get; set; }
}

public class Program
{
    public static List<AviaFlight> aviaFlights = new List<AviaFlight>
    {
        new AviaFlight { FlightNumber = 1, DepartureCity = "Kyiv", DepartureTime = DateTime.Parse("14:00"), TotalSeats = 100, Destination = "New York" },
        new AviaFlight { FlightNumber = 2, DepartureCity = "New York", DepartureTime = DateTime.Parse("16:00"), TotalSeats = 150, Destination = "Paris" },
        
    };

    public static List<Ticket> tickets = new List<Ticket>
    {
        new Ticket { TicketNumber = 101, FlightNumber = 1, PassengerName = "John Doe", Seat = 5, DepartureDate = DateTime.Parse("2023-11-01"), Destination = "New York", ArrivalDateTime = DateTime.Parse("2023-11-01 20:00"), Price = 500.00 },
        new Ticket { TicketNumber = 102, FlightNumber = 2, PassengerName = "Misha", Seat = 7, DepartureDate = DateTime.Parse("2023-11-01"), Destination = "New York", ArrivalDateTime = DateTime.Parse("2023-11-01 20:00"), Price = 550.00 },
        new Ticket { TicketNumber = 103, FlightNumber = 2, PassengerName = "Bob Smith", Seat = 10, DepartureDate = DateTime.Parse("2023-11-02"), Destination = "Paris", ArrivalDateTime = DateTime.Parse("2023-11-02 22:00"), Price = 600.00 },
        
    };

    static void Main(string[] args)
    {
        
        List<string> destinationsWithNoTickets = GetDestinationsWithNoTickets("Kyiv");
        Console.WriteLine("Destinations with no tickets:");
        foreach (var destination in destinationsWithNoTickets)
        {
            Console.WriteLine(destination);
        }

        
        double averageAvailableSeats = GetAverageAvailableSeats("Kyiv", 2022);
        Console.WriteLine($"Average available seats: {averageAvailableSeats}");

        
        Dictionary<string, double> totalSpentByPassenger = GetTotalSpentByPassenger("Misha", "Paris", 2023);
        Console.WriteLine("Total spent by passenger:");

        foreach (var destination in totalSpentByPassenger)
        {
            Console.WriteLine($"{destination.Key}: {destination.Value}");
        }
        Console.ReadLine();
    }

   
    public static List<string> GetDestinationsWithNoTickets(string departureCity)
    {
        DateTime afterNoon = DateTime.Parse("12:00 PM");

        var destinations = aviaFlights
            .Where(flight => flight.DepartureCity == departureCity && flight.DepartureTime > afterNoon)
            .Select(flight => flight.Destination)
            .ToList();

        var destinationsWithNoTickets = destinations
            .Where(destination => !tickets.Any(ticket => aviaFlights.Any(flight => flight.Destination == destination && flight.FlightNumber == ticket.FlightNumber)))
            .Distinct()
            .ToList();

        return destinationsWithNoTickets;
    }

    
    public static double GetAverageAvailableSeats(string departureCity, int year)
    {
        var matchingFlights = aviaFlights
            .Where(flight => flight.DepartureCity == departureCity && flight.DepartureTime.Year == year);

        if (matchingFlights.Any())
        {
            var averageSeats = matchingFlights.Average(flight => flight.TotalSeats);
            return averageSeats;
        }
        else
        {
            return 0; 
        }
    }
    
    public static Dictionary<string, double> GetTotalSpentByPassenger(string passengerName, string destination, int currentYear)
    {
        var passengerTickets = tickets
            .Where(ticket => ticket.PassengerName == passengerName && ticket.DepartureDate.Year == currentYear)
            .ToList();

        var otherDestinations = passengerTickets
            .Where(ticket => ticket.Destination != destination)
            .Select(ticket => ticket.Destination)
            .Distinct()
            .ToList();

        var totalSpentByDestination = new Dictionary<string, double>();

        foreach (var otherDestination in otherDestinations)
        {
            var totalSpent = passengerTickets
                .Where(ticket => ticket.Destination == otherDestination)
                .Sum(ticket => ticket.Price);

            totalSpentByDestination.Add(otherDestination, totalSpent);
        }

        return totalSpentByDestination;
    }
}
