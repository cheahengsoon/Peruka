namespace Knet.Phone.Client.ArcGIS.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    using ESRI.ArcGIS.Client;

    public class ArcGISSerializationUtils
    {
        // private const string GraphicTemplate = "[{\"geometry\" : <geometry>,\"attributes\" : {<attributes>}}]";

        private const string GraphicListTemplate = "[<geometries>]";

        private const string GraphicTemplateInList = "{\"geometry\" : <geometry>,\"attributes\" : {<attributes>}}";

        private const string GraphicTemplateInListWithoutGeometry = "{\"attributes\" : {<attributes>}}";

        private readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public string ToJson(List<Graphic> graphics, bool includeObjectId = false, bool includeGeometry = true)
        {
            var graphicsJsonList = graphics.Select(graphic => this.ToJson(graphic, includeObjectId, includeGeometry)).ToList();

            var graphicsJson = string.Empty;

            for (int i = 0; i < graphicsJsonList.Count; i++)
            {
                if (i > 0)
                {
                    graphicsJson += ",";
                }

                graphicsJson += graphicsJsonList[i];
            }

            var listJson = GraphicListTemplate.Replace("<geometries>", graphicsJson);

            return listJson;
        }

        /// <summary>
        ///     Returns Graphic JSON for that is in format that can be used when stored new features directly via REST endpoint.
        ///     Using REST specification from http://servicesbeta2.esri.com/arcgis/sdk/rest/index.html
        ///     // TODO create more sophisticated version of serializatioin...
        /// </summary>
        /// <param name="graphic">The graphic that is serialized</param>
        /// <returns>Geometry and Attribute information as JSON</returns>
        /// <remarks>Notice that attributes are not handled as a collection.</remarks>
        public string ToJson(Graphic graphic, bool includeObjectId = false, bool includeGeometry = true)
        {
            var attributesList = new List<string>();

            foreach (var attribute in graphic.Attributes)
            {
                if (includeObjectId == false)
                {
                    if (attribute.Key.ToLowerInvariant() == "objectid")
                    {
                        continue;
                    }
                }

                if (attribute.Value == null)
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
                                    attributesList.Add(
                                        "\"" + attribute.Key + "\":" + value.ToString(CultureInfo.InvariantCulture));
                                }
                            }
                            else
                            {
                                attributesList.Add("\"" + attribute.Key + "\": null");
                            }
                        }
                        else if (attribute.Value is int)
                        {
                            var value = 0;
                            var result = int.TryParse(attribute.Value.ToString(), out value);

                            if (result)
                            {
                                attributesList.Add(
                                    "\"" + attribute.Key + "\":" + value.ToString(CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                attributesList.Add("\"" + attribute.Key + "\": null");
                            }
                        }
                        else if (attribute.Value is decimal)
                        {
                            decimal value = 0;
                            var result = decimal.TryParse(attribute.Value.ToString(), out value);

                            if (result)
                            {
                                attributesList.Add(
                                    "\"" + attribute.Key + "\":" + value.ToString(CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                attributesList.Add("\"" + attribute.Key + "\": null");
                            }
                        }
                        else if (attribute.Value is float)
                        {
                            float value = 0;
                            var result = float.TryParse(attribute.Value.ToString(), out value);

                            if (result)
                            {
                                attributesList.Add(
                                    "\"" + attribute.Key + "\":" + value.ToString(CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                attributesList.Add("\"" + attribute.Key + "\": null");
                            }
                        }
                    }
                }
                else
                {
                    if (attribute.Value == null)
                    {
                        attributesList.Add("\"" + attribute.Key + "\":\"\"");
                    }
                    else
                    {
                        var dateInMilliseconds =
                            ((long)
                             Math.Round((((DateTime)attribute.Value).ToUniversalTime() - this._epoch).TotalMilliseconds));

                        attributesList.Add(
                            "\"" + attribute.Key + "\":" + dateInMilliseconds.ToString(CultureInfo.InvariantCulture));
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

            var graphicJson = string.Empty;

            if (includeGeometry)
            {
                graphicJson = GraphicTemplateInList.Replace("<geometry>", graphic.Geometry.ToJson());
            }
            else
            {
                graphicJson = GraphicTemplateInListWithoutGeometry;
            }

            graphicJson = graphicJson.Replace("<attributes>", attributes);

#if DEBUG
            Debug.WriteLine(graphicJson);
#endif
            return graphicJson;
        }
    }
}