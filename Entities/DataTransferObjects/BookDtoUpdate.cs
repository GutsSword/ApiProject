using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    // init can provide immutable of variable

    public record BookDtoUpdate 
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public decimal Price { get; init; }
    }
}
