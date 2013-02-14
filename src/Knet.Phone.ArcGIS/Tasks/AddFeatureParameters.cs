using System.Collections.Generic;

namespace Knet.Phone.Client.ArcGIS.Tasks
{
    using ESRI.ArcGIS.Client;

    public class AddFeatureParameters
    {
        public AddFeatureParameters()
        {
            this.Graphics = new List<Graphic>();
        }

        public IList<Graphic> Graphics { get; set; } 
    }
}
