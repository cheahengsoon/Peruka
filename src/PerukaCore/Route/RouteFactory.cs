namespace Peruka.Phone.Client.Core.Route
{
    using System.Device.Location;

    using ESRI.ArcGIS.Client;

    using Knet.Phone.Client.ArcGIS.Graphics;

    public static class RouteFactory
    {
        public static Graphic CreatePoint(
            GeoPosition<GeoCoordinate> geoPosition, string username, int spatialReference = 4326)
        {
            var routePoint = GraphicFactory.Create(geoPosition.Location);
            routePoint.Attributes.Add("Latitude", geoPosition.Location.Latitude);
            routePoint.Attributes.Add("Longitude", geoPosition.Location.Longitude);
            routePoint.Attributes.Add("Speed", geoPosition.Location.Speed);
            routePoint.Attributes.Add("Altitude", geoPosition.Location.Altitude);
            routePoint.Attributes.Add("HorizontalAccuracy", geoPosition.Location.HorizontalAccuracy);
            routePoint.Attributes.Add("VertialAccuracy", geoPosition.Location.VerticalAccuracy);
            routePoint.Attributes.Add("Course", geoPosition.Location.Course);
            routePoint.Attributes.Add("RecordTime", geoPosition.Timestamp);
            routePoint.Attributes.Add("UserName", username);

            return routePoint;
        }
    }
}