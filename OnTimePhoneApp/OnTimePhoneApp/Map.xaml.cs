using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace OnTimePhoneApp
{
    public static class CoordinateConverterMap
    {
        public static GeoCoordinate ConvertGeocoordinateBlue(Geocoordinate geocoordinate)
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
    public partial class Map : PhoneApplicationPage
    {
        const string blue946 = "http://realtime.mbta.com/developer/api/v1/stopsbyroute?api_key=wX9NwuHnZU2ToO7GmGR9uw&route=946_";
        const string red931 = "http://realtime.mbta.com/developer/api/v1/stopsbyroute?api_key=wX9NwuHnZU2ToO7GmGR9uw&route=931_";
        const string orange903 = "http://realtime.mbta.com/developer/api/v1/stopsbyroute?api_key=wX9NwuHnZU2ToO7GmGR9uw&route=903_";
        const string green810 = "http://realtime.mbta.com/developer/api/v1/stopsbyroute?api_key=wX9NwuHnZU2ToO7GmGR9uw&route=810_";
        const string green830 = "http://realtime.mbta.com/developer/api/v1/stopsbyroute?api_key=wX9NwuHnZU2ToO7GmGR9uw&route=830_";
        const string green852 = "http://realtime.mbta.com/developer/api/v1/stopsbyroute?api_key=wX9NwuHnZU2ToO7GmGR9uw&route=852_";

        public Map()
        {
            InitializeComponent();
            map.SetView(new GeoCoordinate(42.3487, -71.0956, 200), 12);
            Loaded += Map_Loaded;
        }
        private async void ShowMyLocationOnTheMap()
        {
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate = CoordinateConverterBlue.ConvertGeocoordinateBlue(myGeocoordinate);
            this.map.Center = myGeoCoordinate;
            this.map.ZoomLevel = 13;
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Magenta);
            myCircle.Height = 15;
            myCircle.Width = 15;
            myCircle.Opacity = 90;
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = myGeoCoordinate;
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);
            map.Layers.Add(myLocationLayer);
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Ellipse myCircle = new Ellipse();
            MapPolyline line = new MapPolyline();

            WebClient blue = new WebClient();
            line.StrokeColor = Colors.Blue;
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            blue.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            blue.DownloadStringAsync(new Uri(blue946));
            WebClient green1 = new WebClient();
            line.StrokeColor = Colors.Green;
            myCircle.Fill = new SolidColorBrush(Colors.Green);
            green1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            green1.DownloadStringAsync(new Uri(green810));
            WebClient green2 = new WebClient();
            line.StrokeColor = Colors.Green;
            green2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            green2.DownloadStringAsync(new Uri(green830));
                    line.StrokeColor = Colors.Green;;
            WebClient green3 = new WebClient();
            green3.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            green3.DownloadStringAsync(new Uri(green852));
                    line.StrokeColor = Colors.Green;
            WebClient orange = new WebClient();
            line.StrokeColor = Colors.Orange;
            myCircle.Fill = new SolidColorBrush(Colors.Orange);
            orange.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            orange.DownloadStringAsync(new Uri(orange903));
            WebClient red = new WebClient();
            myCircle.Fill = new SolidColorBrush(Colors.Red);
            line.StrokeColor = Colors.Red;
            red.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            red.DownloadStringAsync(new Uri(red931));
        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rootObject = JsonConvert.DeserializeObject<Rootobject>(e.Result);
            double lat, longitude;
            MapPolyline line = new MapPolyline();
            
            line.StrokeThickness = 2;

            double[] coord = new double[2 * rootObject.direction[0].stop.Length];
            for (int i = 0; i < rootObject.direction[0].stop.Length; i++)
            {
                lat = Convert.ToDouble(rootObject.direction[0].stop[i].stop_lat);
                longitude = Convert.ToDouble(rootObject.direction[0].stop[i].stop_lon);

                line.Path.Add(new GeoCoordinate(lat, longitude));

                Ellipse myCircle = new Ellipse();
    
                myCircle.Height = 10;
                myCircle.Width = 10;
                myCircle.Opacity = 60;
                MapOverlay myLocationOverlay = new MapOverlay();
                myLocationOverlay.Content = myCircle;
                myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                myLocationOverlay.GeoCoordinate = new GeoCoordinate(lat, longitude, 200);
                MapLayer myLocationLayer = new MapLayer();
                myLocationLayer.Add(myLocationOverlay);
                map.Layers.Add(myLocationLayer);
            }
            map.MapElements.Add(line);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowMyLocationOnTheMap();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (map.ZoomLevel < 19)
            {
                map.ZoomLevel++;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (map.ZoomLevel > 2)
            {
                map.ZoomLevel--;
            }
        }
    }
}