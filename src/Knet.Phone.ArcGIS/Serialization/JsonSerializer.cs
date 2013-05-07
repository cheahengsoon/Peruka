namespace Knet.Phone.Client.ArcGIS.Serialization
{
    using Newtonsoft.Json;

    /// <summary>
    /// Helper class for Json serialization.
    /// </summary>
    public static class JsonSerializer
    {
        /// <summary>
        /// Serialize obect to JSON.
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>Serialized object.</returns>
        public static string SerializeObject(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        /// <summary>
        /// Deserialize JSON object to CLR object.
        /// </summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="stringToDeserialize">JSON to deserialize.</param>
        /// <returns>Deserialized object as a target type.</returns>
        public static T DeserializeObject<T>(string stringToDeserialize)
        {
            return JsonConvert.DeserializeObject<T>(stringToDeserialize);
        }
    }
}