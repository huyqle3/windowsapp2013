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

namespace OnTimePhoneApp
{
    public static class CoordinateConverterOrange
    {
        public static GeoCoordinate ConvertGeocoordinateOrange(Geocoordinate geocoordinate)
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

    public partial class OrangeLine : PhoneApplicationPage
    {

        public OrangeLine()
        {
            InitializeComponent();
            map.SetView(new GeoCoordinate(42.3487, -71.0956, 200), 12);
        }

        private async void ShowMyLocationOnTheMap()
        {
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate = CoordinateConverterOrange.ConvertGeocoordinateOrange(myGeocoordinate);
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