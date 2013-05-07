namespace Knet.Phone.Client.ArcGIS.Layers
{
    using System;
    using System.Device.Location;
    using System.Threading.Tasks;

    using ESRI.ArcGIS.Client;
    using ESRI.ArcGIS.Client.Portal;
    using ESRI.ArcGIS.Client.WebMap;

    /// <summary>
    /// Factory class for layer creation.
    /// </summary>
    public static class LayerFactory
    {
        /// <summary>
        /// Creates new instance of the <typeparam name="T">CreateLayersTask</typeparam> and executes it.
        /// </summary>
        /// <typeparam name="T">Type that is <see cref="CreateLayersTask"/></typeparam>
        /// <remarks>
        /// For simple use:
        /// <code>
        ///   LayerFactory.CreateLayers{CreateOfficeLayersTask>}x =>
        ///                     {
        ///                         // Set initialized layers to in use
        ///                         this.Layers = x.Layers;
        ///                         // Do other stuff...
        ///                     });
        /// </code>
        /// </remarks>
        public static async Task<LayersCreatedEventArgs> CreateLayersAsync<T>() where T : CreateLayersTask, new()
        {
            var task = new T();
            return await task.ExecuteAsync();
        }

        /// <summary>
        /// Creates new <see cref="ESRI.ArcGIS.Client.Toolkit.DataSources"/> with given parameters.
        /// </summary>
        /// <param name="id">The Id of the layer.</param>
        /// <param name="accuracy">The accuracy of the <see cref="GeoCoordinateWatcher"/>.</param>
        /// <param name="movementTreshold">The <see cref="GeoCoordinateWatcher.MovementThreshold"/>.</param>
        /// <returns></returns>
        //public static GpsLayer CreateGpsLayer(string id, GeoPositionAccuracy accuracy = GeoPositionAccuracy.Default, int movementTreshold = 5)
        //{
        //    var layer = new GpsLayer();
        //    var geoCoordinateWatcher = new GeoCoordinateWatcher(accuracy) { MovementThreshold = movementTreshold };
        //    layer.GeoPositionWatcher = geoCoordinateWatcher;
        //    return layer;
        //}

        /// <summary>
        /// Creates new <see cref="FeatureLayer"/> with given parameters
        /// </summary>
        /// <param name="url">The Url of the service.</param>
        /// <param name="id">The Id of the layer.</param>
        /// <returns>Returns new <see cref="FeatureLayer"/></returns>
        public static FeatureLayer CreateFeatureLayer(string url, string id)
        {
            var layer = new FeatureLayer { ID = id, Url = url };

            layer.InitializationFailed += LayerOnInitializationFailed;

            return layer;
        }

        /// <summary>
        /// Creates new <see cref="FeatureLayer"/> from <see cref="ArcGISPortalItem"/>
        /// </summary>
        /// <param name="portalItem"><see cref="ArcGISPortalItem"/> that is type of <see cref="ItemType.FeatureService"/></param>
        /// <returns>Returns new <see cref="FeatureLayer"/></returns>
        public static FeatureLayer CreateFeatureLayer(ArcGISPortalItem portalItem)
        {
            if (portalItem.Type != ItemType.FeatureService)
            {
                return null;
            }

            var layer = new FeatureLayer { ID = portalItem.Id, Url = portalItem.Url, DisplayName = portalItem.Title };

            layer.InitializationFailed += LayerOnInitializationFailed;

            return layer;
        }

        /// <summary>
        /// Creates new <see cref="ArcGISTiledMapServiceLayer"/> with given parameters
        /// </summary>
        /// <param name="url">The Url of the service.</param>
        /// <param name="id">The Id of the layer.</param>
        /// <returns>Returns new <see cref="ArcGISTiledMapServiceLayer"/></returns>
        public static ArcGISTiledMapServiceLayer CreateTiledLayer(string url, string id)
        {
            var layer = new ArcGISTiledMapServiceLayer { ID = id, Url = url, };

            layer.InitializationFailed += LayerOnInitializationFailed;
            layer.SetValue(Document.IsBaseMapProperty, true);

            return layer;
        }

        /// <summary>
        /// Creates new <see cref="GraphicsLayer"/> with given parameters
        /// </summary>
        /// <param name="id">The Id of the layer.</param>
        /// <param name="displayName">The display name of the layer.</param>
        /// <returns>Returns new <see cref="GraphicsLayer"/></returns>
        public static GraphicsLayer CreateGraphicsLayer(string id, string displayName)
        {
            var layer = new GraphicsLayer { ID = id, DisplayName = displayName };

            layer.InitializationFailed += LayerOnInitializationFailed;

            return layer;
        }

        private static void LayerOnInitializationFailed(object sender, EventArgs eventArgs)
        {
            //throw new Exception("Layer initialization failed", ((Layer) sender).InitializationFailure);
        }
    }
}