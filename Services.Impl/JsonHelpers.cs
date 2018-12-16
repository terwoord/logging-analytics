using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace TerWoord.LoggingAnalytics.Server.Services
{
    public static class JsonHelpers
    {
        public static void FlattenNestedObjects([NotNull] JObject message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var propertiesToAdd    = new Dictionary<string, JToken>();
            var propertiesToRemove = new List<string>();
            foreach (var prop in message.Properties())
            {
                if (prop.Value.Type == JTokenType.Object)
                {
                    var nestedObject = (JObject)prop.Value;
                    FlattenNestedObjects(nestedObject);
                    foreach (var nestedProp in prop.Value.Children<JProperty>())
                    {
                        propertiesToAdd.Add(prop.Name + "." + nestedProp.Name, nestedProp.Value);
                    }
                    propertiesToRemove.Add(prop.Name);
                    continue;
                }

                if (prop.Value.Type == JTokenType.Array)
                {
                    propertiesToRemove.Add(prop.Name);

                    var arrayValue = (JArray)prop.Value;
                    propertiesToAdd.Add(prop.Name + ".@length", arrayValue.Count);

                    var countLength = Math.Max(arrayValue.Count.ToString().Length, 4); 
                    for (int i = 0; i < arrayValue.Count; i++)
                    {
                        var item = arrayValue[i];
                        if (item.Type == JTokenType.Object)
                        {
                            var objItem = (JObject)item;

                            foreach (var property in objItem.Properties())
                            {
                                propertiesToAdd.Add(prop.Name + "." + i.ToString().PadLeft(countLength, '0') + "." + property.Name, property.Value);
                            }

                            continue;
                        }
                        throw new Exception($"Array element of type '{item.Type}' not supported!");
                    }
                }
            }

            foreach (var property in propertiesToAdd)
            {
                message[property.Key] = property.Value;
            }
            propertiesToRemove.ForEach(i => message.Remove(i));
        }
    }
}