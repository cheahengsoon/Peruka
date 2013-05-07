namespace Peruka.Phone.Client.Presentation.Map
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media;

    using ESRI.ArcGIS.Client;
    using ESRI.ArcGIS.Client.FeatureService.Symbols;
    using ESRI.ArcGIS.Client.Symbols;

    using Knet.Phone.Client.ArcGIS.Layers;
    using Knet.Phone.Client.ViewModels;

    using Peruka.Phone.Client.Core.Services;

    using SimpleMarkerSymbol = ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol;

    public class RouteMapViewModel : ScreenViewModel
    {
        private readonly RouteService _routeService;
        private LayerCollection _layers;

        public RouteMapViewModel(RouteService routeService)
        {
            _routeService = routeService;
        }

        public LayerCollection Layers
        {
            get
            {
                return _layers;
            }
            set
            {
                if (Equals(value, _layers))
                {
                    return;
                }
                _layers = value;
                NotifyOfPropertyChange(() => Layers);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            CreateLayersAsync();
        }

        private async Task CreateLayersAsync()
        {
            var layerTaskResult = await LayerFactory.CreateLayersAsync<CreateRouteLayersTask>();
            if (layerTaskResult.FailedLayerInitializations.Any())
            {
                Debug.WriteLine("CreateLayers had some errors...");
            }

            var routeLayer = layerTaskResult.Layers.OfType<GraphicsLayer>().FirstOrDefault(x => x.ID == "Route");
            if (routeLayer != null)
            {
                var symbol = new SimpleMarkerSymbol();
                symbol.Color = new SolidColorBrush(Colors.Red);
                symbol.Size = 10;
                _routeService.Route.Symbol = symbol;

                routeLayer.Graphics.Add(_routeService.Route);
            }

            this.Layers = layerTaskResult.Layers;
        }
    }
}
