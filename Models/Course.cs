using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public User Teacher { get; set; }
    }
}
