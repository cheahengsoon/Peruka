using System.Threading.Tasks;

namespace Peruka.Phone.Client.Core.Services
{
    using System.Device.Location;

    using Knet.Phone.Client.ArcGIS.Tasks;

    using Peruka.Phone.Client.Core.Route;

    public class RouteService : GeoService
    {
        private const string ServiceUrl = "http://services.arcgis.com/4PuGhqdWG1FwH2Yk/ArcGIS/rest/services/PerukaLocations/FeatureServer/0";

        public RouteService(SettingsService settingsService)
            : base(settingsService)
        {
        }

        public override async Task InitializeAsync()
        {
            base.InitializeAsync();
        }

        public override void HandlePostionChanged(GeoPosition<GeoCoordinate> coordinate)
        {
            var route = RouteFactory.Create(coordinate,  SettingsService.Username);

            var addRouteParameters = new AddFeatureParameters();
            addRouteParameters.AddGraphics.Add(route);
 
            var addRoutePointTask = new AddFeatureTask(ServiceUrl);
            addRoutePointTask.ExecuteAsync(addRouteParameters);
        }
    }
}
