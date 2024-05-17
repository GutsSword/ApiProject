using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public record BookDtoForInsertion : BookDtoForManipulation
    {
        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; init; }
    }
}
