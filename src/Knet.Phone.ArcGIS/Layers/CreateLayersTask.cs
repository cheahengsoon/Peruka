namespace Knet.Phone.Client.ArcGIS.Layers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client;

    /// <summary>
    /// Task for the layer creation and initialization.
    /// </summary>
    public class CreateLayersTask
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="CreateLayersTask"/> class.
        /// </summary>
        public CreateLayersTask()
        {
            this.Layers = new LayerCollection();
        }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        protected LayerCollection Layers { get; set; }

        /// <summary>
        /// Executes layer creation and initialization. If <see cref="CreateLayersTask.ExecuteAsync"/> is already called, methods return immediately.
        /// </summary>
        public async Task<LayersCreatedEventArgs> ExecuteAsync()
        {
            // Create layers
            this.CreateLayers();

            // Initialize layers
            var failedInitializations = await this.InitializeAsync();

            var results = new LayersCreatedEventArgs()
                              {
                                  Layers = this.Layers,
                                  FailedLayerInitializations = failedInitializations
                              };

            return results;
        }

        /// <summary>
        /// Create concrete layers. 
        /// </summary>
        /// <remarks>
        /// Base implementation creates a Tiled basemap and FeatureLayer for fast testing on DEBUG configuration for testing.
        /// </remarks>
        protected virtual void CreateLayers()
        {
#if DEBUG
            this.Layers.Add(
                LayerFactory.CreateTiledLayer(
                    @"http://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer", "Basemap"));

            // Add featurelayer
            //this.Layers.Add(
            //    LayerFactory.CreateFeatureLayer(
            //        @"http://services.arcgis.com/4PuGhqdWG1FwH2Yk/arcgis/rest/services/OfficeLocationsDemo/FeatureServer/0",
            //        "Offices"));

#endif
        }

        private readonly TaskCompletionSource<List<Tuple<Layer, Exception>>> _initializationCompletionSource =
            new TaskCompletionSource<List<Tuple<Layer, Exception>>>();

        private readonly List<Tuple<Layer, Exception>> _failedLayerInitializations = new List<Tuple<Layer, Exception>>();

        public Task<List<Tuple<Layer, Exception>>> InitializeAsync()
        {
            foreach (var layer in this.Layers)
            {
                // If layer initializaiton fails, add entry to results.
                layer.InitializationFailed += this.LayerOnInitializationFailed;
            }

            // When all layers are initialized, return failed layers information
            this.Layers.LayersInitialized += this.LayersOnLayersInitialized;

            // Beging initialization
            this.Layers.ToList().ForEach(x => x.Initialize());

            return this._initializationCompletionSource.Task;
        }

        private void LayerOnInitializationFailed(object sender, EventArgs eventArgs)
        {
            var failedLayer = sender as Layer;
            this._failedLayerInitializations.Add(
                new Tuple<Layer, Exception>(failedLayer, failedLayer.InitializationFailure));
        }

        private void LayersOnLayersInitialized(object sender, EventArgs args)
        {
            this._initializationCompletionSource.SetResult(this._failedLayerInitializations);
            this.Layers.LayersInitialized -= this.LayersOnLayersInitialized;
        }
    }
}