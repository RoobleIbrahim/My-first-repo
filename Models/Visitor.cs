using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Visitors.Models
{
    public class Visitor
    {
        [Key]
        public int VisitorId { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime VisitedDate { get; set; }
        public bool Status { get; set; }
    }
}
