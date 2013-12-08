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
    public class StopInfo
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

    
    public class ScheduedETA
    {
        public string route_id { get; set; }
        public string route_name { get; set; }
        public Direction[] direction { get; set; }
    }

    public class Trip
    {
        public string trip_id { get; set; }
        public string trip_name { get; set; }
        public Stop[] stop { get; set; }
    }

    public static class CoordinateConverterRed
    {
        public static GeoCoordinate ConvertGeocoordinateRed(Geocoordinate geocoordinate)
        {
            return new GeoCoordinate
                (
                geocoordinate.Latitude,
                geocoordinate.Longitude,
                geocoordinate.Altitude ?? Double.NaN,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }
    }

    public partial class RedLine : PhoneApplicationPage
    {
        const string red931 = "http://realtime.mbta.com/developer/api/v1/stopsbyroute?api_key=wX9NwuHnZU2ToO7GmGR9uw&route=931_";
        StopInfo stops = new StopInfo();

        public RedLine()
        {
            InitializeComponent();
            map.SetView(new GeoCoordinate(42.3487, -71.0956, 200), 12);
            Loaded += RedLine_Loaded;
        }

        private async void ShowMyLocationOnTheMap()
        {
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate = CoordinateConverterRed.ConvertGeocoordinateRed(myGeocoordinate);
            this.map.Center = myGeoCoordinate;
            this.map.ZoomLevel = 13;
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Red);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = myGeoCoordinate;
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);
            map.Layers.Add(myLocationLayer);
        }
        private async void RedLine_Loaded(object sender, RoutedEventArgs e)
        {
            string json;
            object stops;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                json = await httpClient.GetStringAsync(red931);
                stops = JsonConvert.DeserializeObject(json);
            }
            catch (JsonSerializationException jsonner) {
                Console.WriteLine("Jsonner Exception");
            }
            catch (HttpRequestException httpe)
            {
                Console.WriteLine("Http exception");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowMyLocationOnTheMap();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            map.ZoomLevel++;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            map.ZoomLevel--;
        }

    }
}