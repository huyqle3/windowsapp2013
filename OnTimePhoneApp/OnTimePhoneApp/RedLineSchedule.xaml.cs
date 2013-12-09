using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Windows.Devices.Geolocation;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace OnTimePhoneApp
{
    /// <summary>
    /// test
    /// </summary>
    ///

    namespace Schedule
    {

        public class schedule
        {
            public Direction[] direction { get; set; }
            public string route_id { get; set; }
            public string route_name { get; set; }
        }

        public class Direction
        {
            public string direction_id { get; set; }
            public string direction_name { get; set; }
            public Trip[] trip { get; set; }
        }

        public class Trip
        {
            public Stop[] Stop { get; set; }
            public string trip_id { get; set; }
            public string trip_name { get; set; }
        }

        public class Stop
        {
            public int sch_arr_dt { get; set; }
            public int sch_dep_dt { get; set; }
            public string stop_id { get; set; }
            public string stop_name { get; set; }
            public string stop_sequence { get; set; }
        }
    }

    public partial class RedLineSchedule : PhoneApplicationPage
    {
        const string RedLine931 = "http://realtime.mbta.com/developer/api/v1/schedulebyroute?api_key=ePbnXz1wU0Oru5ApeL_mqA&route=931_";
        const string RedLine933 = "http://realtime.mbta.com/developer/api/v1/schedulebyroute?api_key=ePbnXz1wU0Oru5ApeL_mqA&route=933_";
        List<TrainInfo> source = new List<TrainInfo>();

        public RedLineSchedule()
        {
            InitializeComponent();
            Loaded += RedLineSchedule_Loaded;
            List<AlphaKeyGroup<TrainInfo>> DataSource = AlphaKeyGroup<TrainInfo>.CreateGroups(source,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (TrainInfo s) => { return s.TripName; }, true);
            //List<string>DataSource = new List<string>();
            List.ItemsSource = DataSource;

        }
        private void RedLineSchedule_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(RedLine931));
            WebClient webClient1 = new WebClient();
            webClient1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient1.DownloadStringAsync(new Uri(RedLine933));
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rootObject = JsonConvert.DeserializeObject<Schedule.schedule>(e.Result);
            string arrival;
            string departure;
            string tripname;
            if (rootObject != null)
            {
                for (int i = 0; i < rootObject.direction[0].trip.Length; i++)
                {
                    for (int j = 0; j < rootObject.direction[0].trip[i].Stop.Length; j++)
                    {
                        tripname = rootObject.direction[0].trip[i].trip_name;
                        arrival = Convert.ToString(rootObject.direction[0].trip[i].Stop[j].sch_arr_dt);
                        departure = Convert.ToString(rootObject.direction[0].trip[i].Stop[j].sch_dep_dt);
                        source.Add(new TrainInfo(tripname, arrival, departure));
                    }
                }
            }
            Console.WriteLine("k");

        }
    }

    public class TrainInfo
    {
        public string TripName
        {
            get;
            set;
        }
        public string Arrival
        {
            get;
            set;
        }
        public string Departure
        {
            get;
            set;
        }

        public TrainInfo(string tripname, string arrival, string departure)
        {
            this.TripName = tripname;
            this.Arrival = arrival;
            this.Departure = departure;
        }
    }
}