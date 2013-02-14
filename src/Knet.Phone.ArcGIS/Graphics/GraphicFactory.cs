namespace Knet.Phone.Client.ArcGIS.Graphics
{
    using System.Device.Location;

    using ESRI.ArcGIS.Client;
    using ESRI.ArcGIS.Client.Geometry;

    public static class GraphicFactory
    {
        /// <summary>
        /// Creates new <see cref="Graphic"/> from GeoCoorinate.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// Returns new <see cref="Graphic"/> with provided MapPoint geoometry.
        /// </returns>
        public static Graphic Create(GeoCoordinate location)
        {
              var graphic = new Graphic();
              var point = new MapPoint(
                  location.Longitude,
                  location.Latitude, 
                  new SpatialReference(4326));

              graphic.Geometry = point;
              return graphic;
        }
    }
}
