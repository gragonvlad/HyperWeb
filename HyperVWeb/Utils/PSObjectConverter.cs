using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class PSObjectConverter
    {
        public static dynamic ToDynamic(this PSObject @object)
        {
            Dictionary<string, object> arrayDictionary = new Dictionary<string, object>();
            foreach (var property in @object.Properties)
            {
                object value = null;
                if (property.Value is PSObject)
                {
                    value = (property.Value as PSObject).ToDynamic();
                }
                else
                {
                    value = property.Value;
                }

                arrayDictionary.Add(property.Name, value);
            }
            return arrayDictionary.ToExpando();
        }

        public static dynamic ToDynamic(this ManagementObject @object)
        {
            Dictionary<string, object> arrayDictionary = new Dictionary<string, object>();
            foreach (var property in @object.Properties)
            {
                object value = null;
                if (property.Value is ManagementObject)
                {
                    value = (property.Value as ManagementObject).ToDynamic();
                }
                else
                {
                    value = property.Value;
                }

                arrayDictionary.Add(property.Name, value);
            }
            return arrayDictionary.ToExpando();
        }

        /// <summary>
        /// Extension method that turns a dictionary of string and object to an ExpandoObject
        /// </summary>
        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = ((IDictionary<string, object>)kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = ((IDictionary<string, object>)item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
        }
    }
}
