namespace Knet.Phone.Client.ArcGIS.Layers
{
    using System;
    using System.Collections.Generic;

    using ESRI.ArcGIS.Client;

    /// <summary>
    /// Event handler for layers created.
    /// </summary>
    public class LayersCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Layers.
        /// </summary>
        public LayerCollection Layers { get; set; }

        /// <summary>
        /// Gets or sets the failed layer initializations.
        /// </summary>
        public List<Tuple<Layer, Exception>> FailedLayerInitializations { get; set; }
    }
}
