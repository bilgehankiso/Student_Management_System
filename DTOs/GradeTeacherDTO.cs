namespace StudentManagementSystem.DTOs
{
    public class GradeTeacherDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Midterm { get; set; }
        public int Final { get; set; }
    }
}
