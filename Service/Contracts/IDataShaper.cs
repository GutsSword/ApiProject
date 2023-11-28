using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IDataShaper<T> 
    {
        IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entites, string fieldsString);  // For List

        ExpandoObject ShapeData(T entites, string fieldsString);   // For Singular object
    }
}
