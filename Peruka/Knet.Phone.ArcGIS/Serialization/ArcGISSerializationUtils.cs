namespace Knet.Phone.Client.ArcGIS.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using ESRI.ArcGIS.Client;

    public class ArcGISSerializationUtils
    {
        private const string GraphicTemplate = "[{\"geometry\" : <geometry>,\"attributes\" : {<attributes>}}]";

        /// <summary>
        /// Returns Graphic JSON for that is in format that can be used when stored new features directly via REST endpoint.
        /// Using REST specification from http://servicesbeta2.esri.com/arcgis/sdk/rest/index.html
        /// // TODO create more sophisticated version of serializatioin...
        /// </summary>
        /// <param name="graphic">The graphic that is serialized</param>
        /// <returns>Geometry and Attribute information as JSON</returns>
        /// <remarks>Notice that attributes are not handled as a collection.</remarks>
        public string ToJson(Graphic graphic)
        {
            var attributesList = new List<string>();

            foreach (var attribute in graphic.Attributes)
            {
                if (attribute.Key.ToLowerInvariant() == "objectid")
                {
                    continue;
                }

                if (attribute.Value.GetType() != typeof(DateTime))
                {
                    if (attribute.Value == null)
                    {
                        attributesList.Add("\"" + attribute.Key + "\":\"\"");
                    }
                    else
                    {
                        if (attribute.Value is string)
                        {
                            attributesList.Add("\"" + attribute.Key + "\":\"" + attribute.Value + "\"");
                        }

                        if (attribute.Value is double)
                        {
                            var value = 0d;
                            var result = double.TryParse(attribute.Value.ToString(), out value);

                            if (result)
                            {
                                if (double.IsNaN(value))
                                {
                                    attributesList.Add("\"" + attribute.Key + "\": null");
                                }
                                else
                                {
                                    attributesList.Add("\"" + attribute.Key + "\":" + value.ToString(CultureInfo.InvariantCulture));
                                }
                            }
                            else
                            {
                                attributesList.Add("\"" + attribute.Key + "\": null");
                            }
                        }
                    }
                }
            }

            var attributes = string.Empty;
            for (int i = 0; i < attributesList.Count; i++)
            {
                if (i > 0)
                {
                    attributes += ",";
                }

                attributes += attributesList[i];
            }

            var graphicJson = GraphicTemplate.Replace("<geometry>", graphic.Geometry.ToJson());
            graphicJson = graphicJson.Replace("<attributes>", attributes);

            return graphicJson;
        }
    }
}
