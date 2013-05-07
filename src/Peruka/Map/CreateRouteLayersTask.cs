namespace Peruka.Phone.Client.Presentation.Map
{
    using Knet.Phone.Client.ArcGIS.Layers;

    public class CreateRouteLayersTask : CreateLayersTask
    {
        protected override void CreateLayers()
        {
            Layers.Add(
                LayerFactory.CreateTiledLayer(
                    @"http://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer", "Basemap"));

            Layers.Add(
                LayerFactory.CreateGraphicsLayer("Route", "Route"));

            //Layers.Add(
            //    LayerFactory.CreateFeatureLayer(
            //        @"http://services.arcgis.com/4PuGhqdWG1FwH2Yk/ArcGIS/rest/services/PerukaLocations/FeatureServer/0", "Route"));
            
        }
    }
}