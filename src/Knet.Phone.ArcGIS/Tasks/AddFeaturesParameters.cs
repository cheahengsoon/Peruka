namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using System.Collections.Generic;

    using ESRI.ArcGIS.Client;

    public class AddFeatureParameters
    {
        public AddFeatureParameters()
        {
            this.AddGraphics = new List<Graphic>();
            this.UpdateGraphics = new List<Graphic>();
        }

        public List<Graphic> AddGraphics { get; set; }

        public List<Graphic> UpdateGraphics { get; set; }
    }
}