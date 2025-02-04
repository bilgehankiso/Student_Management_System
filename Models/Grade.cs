namespace StudentManagementSystem.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Midterm { get; set; }
        public int Final { get; set; }
    }
}
