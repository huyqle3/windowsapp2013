using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace OnTimePhoneApp
{
    public class Rootobject
    {
        public Direction[] direction { get; set; }
    }

    public class Direction
    {
        public string direction_id { get; set; }
        public string direction_name { get; set; }
        public Stop[] stop { get; set; }
    }

    public class Stop
    {
        public string parent_station { get; set; }
        public string parent_station_name { get; set; }
        public string stop_id { get; set; }
        public string stop_lat { get; set; }
        public string stop_lon { get; set; }
        public string stop_name { get; set; }
        public string stop_order { get; set; }
    }

	
    public partial class RedLine : PhoneApplicationPage
    {
        public RedLine()
        {
            InitializeComponent();
            var url = "http://realtime.mbta.com/developer/api/v1/stopsbyroute?api_key=wX9NwuHnZU2ToO7GmGR9uw&route=931_";
            var stops = JsonConvert.DeserializeObject(url);


        }
    }
}