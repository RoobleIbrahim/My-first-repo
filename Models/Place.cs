using System.ComponentModel.DataAnnotations;

namespace Visitors.Models
{
    public class Place
    {
        [Key]
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
    }
}
