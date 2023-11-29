using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace FiorelloBackend.Models
{
    public class CategoryArchive
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; }
    }
}
