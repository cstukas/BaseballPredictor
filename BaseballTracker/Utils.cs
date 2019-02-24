using AutoMapper;
using BaseballTracker.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker
{
    public static class Utils
    {
        public static T Map<W, T>(W source)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<W, T>(); });
            IMapper mapper = config.CreateMapper();
            var dest = mapper.Map<W, T>(source);
            return dest;
        }

        // Compares properties of 2 objects of the same class. Returns a list of changed properties
        public static List<Property> GetChangedProperties(object obj1, object obj2)
        {
            List<Property> result = new List<Property>();

            if (obj1 == null || obj2 == null)
                // just return empty result
                return result;

            if (obj1.GetType() != obj2.GetType())
                throw new InvalidOperationException("Two objects should be from the same type");

            Type objectType = obj1.GetType();
            // check if the objects are primitive types
            if (objectType.IsPrimitive || objectType == typeof(Decimal) || objectType == typeof(String))
            {
                return result;
            }

            var properties = objectType.GetProperties();
            foreach (var property in properties)
            {
                if (!object.Equals(property.GetValue(obj1), property.GetValue(obj2)))
                {
                    Property newProperty = new Property();
                    newProperty.Name = property.Name;
                    newProperty.Value = property.GetValue(obj1);
                    newProperty.OldValue = property.GetValue(obj2);

                    result.Add(newProperty);
                }
            }

            return result;
        }

        public static List<Property> GetAllValidProperties(object obj1)
        {
            List<Property> result = new List<Property>();

            if (obj1 == null)
                // just return empty result
                return result;

            Type objectType = obj1.GetType();
            var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var property in properties)
            {
                Property newProperty = new Property();
                newProperty.Name = property.Name;
                newProperty.Value = property.GetValue(obj1);

                if (property.PropertyType == typeof(DateTime))
                {
                    DateTime date = (DateTime)newProperty.Value;
                    if (date.Year == 1) continue;  // Datetime default value is "1/1/0001", year 1 means it was never set
                }

                if(property.PropertyType == typeof(Models.Team))
                {
                    continue;
                }
                

                result.Add(newProperty);
            }

            return result;
        }


        public static List<Property> GetEmptyDateTimeProperties(object obj1)
        {
            List<Property> result = new List<Property>();

            if (obj1 == null)
                // just return empty result
                return result;

            Type objectType = obj1.GetType();
            var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var property in properties)
            {
                Property newProperty = new Property();
                newProperty.Name = property.Name;
                newProperty.Value = property.GetValue(obj1);

                if (property.PropertyType == typeof(DateTime))
                {

                    DateTime date = (DateTime)newProperty.Value;
                    if (date.Year == 1)  // Datetime default value is "1/1/0001", year 1 means it was never set
                        result.Add(newProperty); // Only get DateTimes with a year of 1 (aka not set)
                }
            }

            return result;
        }

        public static List<Property> GetPrivateProperties(object obj1)
        {
            List<Property> result = new List<Property>();

            if (obj1 == null)
                // just return empty result
                return result;

            Type objectType = obj1.GetType();
            var properties = objectType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var property in properties)
            {
                Property newProperty = new Property();
                newProperty.Name = property.Name;
                newProperty.Value = property.GetValue(obj1);

                if (property.PropertyType == typeof(DateTime))
                {
                    DateTime date = (DateTime)newProperty.Value;
                    if (date.Year == 1) continue;  // Datetime default value is "1/1/0001", year 1 means it was never set
                }

                result.Add(newProperty);
            }

            return result;
        }

        //gets a single object property based off the property Name
        public static Property GetPropertyInfo(object obj, string name)
        {
            Type objectType = obj.GetType();
            if (objectType.IsPrimitive || objectType == typeof(Decimal) || objectType == typeof(String))
                return null;

            var properties = objectType.GetProperties();
            PropertyInfo prop = properties.SingleOrDefault(x => x.Name == name);

            Property newProperty = new Property();
            newProperty.Name = prop.Name;
            newProperty.Value = prop.GetValue(obj);
            return newProperty;

        }



    }
}
