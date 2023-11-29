using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Entities.Models;
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

        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entites, string fieldsString)
        {
            var requiredFields = GetRequiredProperties(fieldsString);
            return FetchData(entites, requiredFields);
        }

        public ShapedEntity ShapeData(T entities, string fieldsString)
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
        private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)  
        {
            var shapedObject = new ShapedEntity();

            foreach(var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapedObject.Entity.TryAdd(property.Name, objectPropertyValue);
            }
            var objectProperty = entity.GetType().GetProperty("Id");
            shapedObject.Id = (int)objectProperty.GetValue(entity);

            return shapedObject;
        }

        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ShapedEntity>();
            foreach(var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }

    }
}
