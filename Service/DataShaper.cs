using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {      

        public PropertyInfo[] Properties { get; set; }

        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entites, string fieldsString)
        {
            var requiredFields = GetRequiredProperties(fieldsString);
            return FetchData(entites, requiredFields);
        }

        public ExpandoObject ShapeData(T entities, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entities, requiredProperties);
        }


        // Get Prop Method
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldString)
        {
            var requiredFields = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fieldString))
            {
                requiredFields = Properties.ToList();
            }
            else
            {
               var fields = fieldString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach(var field in fields)
                {
                    var property = Properties.FirstOrDefault(x => 
                        x.Name.Equals(field.Trim(),   
                        StringComparison.InvariantCultureIgnoreCase));
                    if (property is null)
                        continue;

                    requiredFields.Add(property);
                }
            }          
            return requiredFields;
        }

        // Work During the Runtime
        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)  
        {
            var shapedObject = new ExpandoObject();

            foreach(var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapedObject.TryAdd(property.Name, objectPropertyValue);
            }

            return shapedObject;
        }

        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ExpandoObject>();
            foreach(var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }

    }
}
