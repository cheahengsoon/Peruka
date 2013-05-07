namespace Peruka.Phone.Client.Core.Services
{
    using System.Collections.Generic;
    using System.Device.Location;
    using System.Threading.Tasks;

    using Caliburn.Micro;

    using ESRI.ArcGIS.Client;
    using ESRI.ArcGIS.Client.Geometry;

    using Knet.Phone.Client.ArcGIS.Tasks;

    using Peruka.Phone.Client.Core.Route;

    public class RouteService : GeoService
    {
        private BindableCollection<Graphic> _routePoints;

        private PointCollection _points;

        private Graphic _route;

        private const string ServiceUrl =
            "http://services.arcgis.com/4PuGhqdWG1FwH2Yk/ArcGIS/rest/services/PerukaLocations/FeatureServer/0";

        public RouteService(SettingsService settingsService)
            : base(settingsService)
        {
            RoutePoints = new BindableCollection<Graphic>();
            _points = new PointCollection();
            Route = new Graphic();
            
            var pointCollection = new PointCollection();

            var polyline = new Polyline();
            polyline.Paths.Add(pointCollection);
            Route.Geometry = polyline;
        }

        public BindableCollection<Graphic> RoutePoints
        {
            get
            {
                return _routePoints;
            }
            set
            {
                if (Equals(value, _routePoints))
                {
                    return;
                }
                _routePoints = value;
                NotifyOfPropertyChange(() => RoutePoints);
            }
        }

        public Graphic Route
        {
            get
            {
                return _route;
            }
            set
            {
                if (Equals(value, _route))
                {
                    return;
                }
                _route = value;
                NotifyOfPropertyChange(() => Route);
            }
        }

        public override void HandlePositionChanged(GeoPosition<GeoCoordinate> coordinate)
        {
            var routePoint = RouteFactory.CreatePoint(coordinate, SettingsService.Username);

            this.AddPoint(routePoint);
        }

        private async Task AddPoint(Graphic point)
        {
            RoutePoints.Add(point);

            var parameters = new AddFeatureParameters();
            parameters.AddGraphics.Add(point);

            var addTask = new AddFeatureTask(ServiceUrl);
            var results = await addTask.ExecuteAsync(parameters);

            _points.Add(point.Geometry as MapPoint);
            //this.AddPointToRoute(point.Geometry as MapPoint);
        }

        private void AddPointToRoute(MapPoint point)
        {
 //           var geometry = (Route.Geometry as Polyline);
        
            

          
        }
    }
}