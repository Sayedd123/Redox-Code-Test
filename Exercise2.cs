using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace Redox_Code_Test
{
    
    public class Event
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }

        public Event(string name, string location, DateTime dateTime)
        {
            Name = name;
            Location = location;
            DateTime = dateTime;
        }

     
        public Event() { }

        public override string ToString()
        {
            return $"{Name} at {Location} on {DateTime:yyyy-MM-dd HH:mm}";
        }
    }

    
    public class EventScheduler
    {
        private List<Event> events;
        private const string StorageFile = "events.json";

        public EventScheduler()
        {
            events = new List<Event>();
            LoadEvents();
        }

       
        public bool ScheduleEvent(Event newEvent)
        {
            
            if (IsDoubleBooked(newEvent.DateTime))
            {
                Console.WriteLine($"Cannot schedule '{newEvent.Name}': Time slot already booked at {newEvent.DateTime:yyyy-MM-dd HH:mm}");
                return false;
            }

            events.Add(newEvent);
            SaveEvents();
            Console.WriteLine($"Event scheduled: {newEvent}");
            return true;
        }

     
        public bool CancelEvent(string eventName)
        {
            var eventToRemove = events.FirstOrDefault(e => e.Name.Equals(eventName, StringComparison.OrdinalIgnoreCase));
            
            if (eventToRemove != null)
            {
                events.Remove(eventToRemove);
                SaveEvents();
                Console.WriteLine($"Event cancelled: {eventToRemove}");
                return true;
            }

            Console.WriteLine($"Event '{eventName}' not found.");
            return false;
        }

        
        public List<Event> GetUpcomingEvents()
        {
            return events.Where(e => e.DateTime > DateTime.Now)
                        .OrderBy(e => e.DateTime)
                        .ToList();
        }

      
        private bool IsDoubleBooked(DateTime dateTime)
        {
            return events.Any(e => e.DateTime == dateTime);
        }

       
        private void SaveEvents()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(events, options);
                File.WriteAllText(StorageFile, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving events: {ex.Message}");
            }
        }

     
        private void LoadEvents()
        {
            try
            {
                if (File.Exists(StorageFile))
                {
                    string jsonString = File.ReadAllText(StorageFile);
                    events = JsonSerializer.Deserialize<List<Event>>(jsonString) ?? new List<Event>();
                    Console.WriteLine($"Loaded {events.Count} event(s) from storage.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading events: {ex.Message}");
                events = new List<Event>();
            }
        }

     
        public void DisplayAllEvents()
        {
            if (events.Count == 0)
            {
                Console.WriteLine("No events scheduled.");
                return;
            }

            Console.WriteLine("\nAll Events:");
            foreach (var evt in events.OrderBy(e => e.DateTime))
            {
                Console.WriteLine($"  - {evt}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Exercise 2: Event Scheduler ===\n");
            
            EventScheduler scheduler = new EventScheduler();

        
            scheduler.ScheduleEvent(new Event("Team Meeting", "Conference Room A", new DateTime(2025, 11, 10, 10, 0, 0)));
            scheduler.ScheduleEvent(new Event("Lunch with Client", "Downtown Restaurant", new DateTime(2025, 11, 10, 12, 30, 0)));
            scheduler.ScheduleEvent(new Event("Project Review", "Conference Room B", new DateTime(2025, 11, 15, 14, 0, 0)));
            
            scheduler.ScheduleEvent(new Event("Another Meeting", "Conference Room C", new DateTime(2025, 11, 10, 10, 0, 0)));

            scheduler.DisplayAllEvents();

            Console.WriteLine("\nUpcoming Events:");
            var upcomingEvents = scheduler.GetUpcomingEvents();
            foreach (var evt in upcomingEvents)
            {
                Console.WriteLine($"  - {evt}");
            }

            Console.WriteLine();
            scheduler.CancelEvent("Lunch with Client");

            scheduler.DisplayAllEvents();
        }
    }
}
