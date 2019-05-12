using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using Newtonsoft.Json;
using System.Globalization;

namespace Projekt_ILock
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 

    public class EventsPrameters
    {
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }       
    }

    public class EventsList
    {
        public List<EventsPrameters> eventsList { get; set; }
    }

    public partial class MainWindow : Window
    {
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Adrian Kubaszewski Aplikacja testowa ILock";

        CalendarService service;
        UserCredential credential;
        EventsResource.ListRequest request;

        string jsonString = "{\"EventsList\":[{\"DateStart\":\"2019-05-14 12:00:00\",\"DateEnd\":\"2019-05-14 12:30:00\",\"Summary\":\"Wydarzenie 1\",\"Description\":\"Opis wydarzenia 1\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-14 14:13:00\",\"DateEnd\":\"2019-05-14 14:32:00\",\"Summary\":\"Wydarzenie 2\",\"Description\":\"Opis wydarzenia 2\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-14 08:20:00\",\"DateEnd\":\"2019-05-14 08:35:00\",\"Summary\":\"Wydarzenie 3\",\"Description\":\"Opis wydarzenia 3\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-14 15:00:00\",\"DateEnd\":\"2019-05-14 15:30:00\",\"Summary\":\"Wydarzenie 4\",\"Description\":\"Opis wydarzenia 4\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-15 09:00:00\",\"DateEnd\":\"2019-05-15 09:30:00\",\"Summary\":\"Wydarzenie 5\",\"Description\":\"Opis wydarzenia 5\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-15 10:00:00\",\"DateEnd\":\"2019-05-15 11:30:00\",\"Summary\":\"Wydarzenie 6\",\"Description\":\"Opis wydarzenia 6\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-15 12:00:00\",\"DateEnd\":\"2019-05-15 12:10:00\",\"Summary\":\"Wydarzenie 7\",\"Description\":\"Opis wydarzenia 7\",\"Location\":\"Gniezno\"}," +
            "{\"DateStart\":\"2019-05-15 13:40:00\",\"DateEnd\":\"2019-05-15 13:59:00\",\"Summary\":\"Wydarzenie 8\",\"Description\":\"Opis wydarzenia 8\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-16 12:00:00\",\"DateEnd\":\"2019-05-16 12:30:00\",\"Summary\":\"Wydarzenie 9\",\"Description\":\"Opis wydarzenia 9\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-14 14:00:00\",\"DateEnd\":\"2019-05-14 14:05:00\",\"Summary\":\"Wydarzenie 10\",\"Description\":\"Opis wydarzenia 10\",\"Location\":\"Poznań\"}]}";

        EventsList result;

        public void ParseJSON(string jsonString)
        {
            result = JsonConvert.DeserializeObject<EventsList>(jsonString);


            var dateStart = result.eventsList.Select(p => p.DateStart).ToList();
            var dateEnd = result.eventsList.Select(p => p.DateEnd).ToList();
            var summary = result.eventsList.Select(p => p.Summary).ToList();
            var description = result.eventsList.Select(p => p.Description).ToList();
            var location = result.eventsList.Select(p => p.Location).ToList();
        }

        public void AddJSONEventsToCalendar()
        {
            Events events = request.Execute();

            if (events.Items.Count == 0)
            {
                for (int i = 0; i < result.eventsList.Count(); i++)
                {
                    AddEvent(DateTime.ParseExact(result.eventsList[i].DateStart, "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact(result.eventsList[i].DateEnd, "yyyy-MM-dd HH:mm:ss", null), result.eventsList[i].Summary.ToString(), result.eventsList[i].Description.ToString(), result.eventsList[i].Location.ToString());
                }
            }
            else
            {
                MessageBox.Show("Kalendarz nie jest pusty");
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ParseJSON(jsonString);

            Credentials();
           
            CreateGoogleService();

            DefineParametersRequest();

            AddJSONEventsToCalendar();

            ReadEventsGoogle();
            
        }

        public void ReadEventsGoogle()
        {
            Events events = request.Execute();

            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    lvCalendarEvent.Items.Add(new EventsPrameters() { Summary = eventItem.Summary, DateStart = when, DateEnd = eventItem.End.DateTime.ToString(), Description = eventItem.Description, Location = eventItem.Location });
                }
            }
            else
            {

                MessageBox.Show("Brak nadchodzących wydarzeń");

            }
        }

        public void DeleteAllEvents()
        {
            Events events = request.Execute();

            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    service.Events.Delete("primary", eventItem.Id).Execute();
                }
            }
            else
            {
                MessageBox.Show("Brak wydarzeń do usunięcia");

            }
        }

        public void Credentials()
        {
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            }
        }

        public void CreateGoogleService()
        {
            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public void DefineParametersRequest()
        {
            request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        }

        public void AddEvent(DateTime start, DateTime end, string summary, string description, string location)
        {
            var ev = new Event();
            EventDateTime startEvent = new EventDateTime();
            startEvent.DateTime = new DateTime(start.Year,start.Month,start.Day,start.Hour,start.Minute, 0);

            EventDateTime endEvent = new EventDateTime();
            endEvent.DateTime = new DateTime(end.Year,end.Month,end.Day,end.Hour,end.Minute, 0);
          
            ev.Start = startEvent;
            ev.End = endEvent;
            ev.Summary = summary;
            ev.Description = description;
            ev.Location = location;
     
            var calendarId = "primary";
            Event recurringEvent = service.Events.Insert(ev, calendarId).Execute();
            
        }

        private void BtDeleteEvents_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Usunąć wszystkie wydarzenia z kalendarza Google?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                
            }
            else
            {
                DeleteAllEvents();
                lvCalendarEvent.Items.Clear();
            }
           
            

        }

        private void BtRefreshEvents_Click(object sender, RoutedEventArgs e)
        {
            lvCalendarEvent.Items.Clear();
            ReadEventsGoogle();
        }

        private void BtAddEventsJSON_Click(object sender, RoutedEventArgs e)
        {
            AddJSONEventsToCalendar();
            lvCalendarEvent.Items.Clear();
            ReadEventsGoogle();
        }
    }

    
}
