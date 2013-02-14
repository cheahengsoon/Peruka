namespace Peruka.Phone.Client.Core.Route
{
    using System.Device.Location;

    using ESRI.ArcGIS.Client;

    using Knet.Phone.Client.ArcGIS.Graphics;

    public static class RouteFactory
    {
        public static Graphic Create(GeoPosition<GeoCoordinate> geoPosition, string username, int spatialReference = 4326)
        {
            var route = GraphicFactory.Create(geoPosition.Location);
            route.Attributes.Add("Latitude", geoPosition.Location.Latitude);
            route.Attributes.Add("Longitude", geoPosition.Location.Longitude);
            route.Attributes.Add("Speed", geoPosition.Location.Speed);
            route.Attributes.Add("Altitude", geoPosition.Location.Altitude);
            route.Attributes.Add("HorizontalAccuracy", geoPosition.Location.HorizontalAccuracy);
            route.Attributes.Add("VertialAccuracy", geoPosition.Location.VerticalAccuracy);
            route.Attributes.Add("Course", geoPosition.Location.Course);
            route.Attributes.Add("RecordTime", geoPosition.Timestamp);
            route.Attributes.Add("UserName", username);

            return route;
        }
    }
}
