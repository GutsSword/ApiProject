using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        //Ref : Collection Navigation Property
        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }
    }
}
