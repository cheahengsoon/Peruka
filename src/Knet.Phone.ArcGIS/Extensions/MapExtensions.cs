namespace Knet.Phone.Client.ArcGIS.Extensions
{
    using System.Windows;

    using ESRI.ArcGIS.Client;
    using ESRI.ArcGIS.Client.Geometry;

    public class MapExtensions : DependencyObject
    {
        #region Extent

        /// <summary>
        /// <see cref="Map.Extent"/> as a dependency object for binding.
        /// </summary>
        public static readonly DependencyProperty ExtentProperty = DependencyProperty.RegisterAttached(
            "Extent", typeof(Envelope), typeof(MapExtensions), new PropertyMetadata(PropertyChangedCallback));

        /// <summary>
        /// Sets extent property to the element
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">Extent as a <see cref="Envelope"/></param>
        public static void SetExtent(UIElement element, Envelope value)
        {
            element.SetValue(ExtentProperty, value);
        }

        /// <summary>
        /// Gets extent from the element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>Returns extent as a <see cref="Envelope"/></returns>
        public static Envelope GetExtent(UIElement element)
        {
            return (Envelope)element.GetValue(ExtentProperty);
        }

        /// <summary>
        /// Changes the <see cref="Map.Extent"/> with the given value.
        /// </summary>
        /// <param name="dependencyObject">The map.</param>
        /// <param name="dependencyPropertyChangedEventArgs">Value arguments.</param>
        private static async void PropertyChangedCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var map = dependencyObject as Map;
            if (map == null)
            {
                return;
            }

            if (map.SpatialReference == null)
            {
                return;
            }

            var newExtent = dependencyPropertyChangedEventArgs.NewValue as Envelope;

            // If spatial reference differs, reproject it.
            if (newExtent.SpatialReference != map.SpatialReference)
            {
                var projectedExtent = await GeometryServiceExtensions.ProjectAsync(newExtent, map.SpatialReference);
                map.ZoomTo(projectedExtent as Envelope);
            }
            else
            {
                map.Extent = newExtent;
            }
        }

        #endregion // Extent
    }
}