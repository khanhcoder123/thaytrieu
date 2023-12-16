using System.ComponentModel.DataAnnotations;
using Tranning.DataDBContext;
using Tranning.Validations;

namespace Tranning.Models
{
    public class TraineeCourseModel
    {
        public List<TraineeCourseDetail> TraineeCourseDetailLists { get; set; }
    }

    public class TraineeCourseDetail
    {
        [Required(ErrorMessage = "Choose Course, please")]
        public int course_id { get; set; }
        [Required(ErrorMessage = "Choose Trainee, please")]

        public int trainee_id { get; set; }
        

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }
        public string? CourseName { get; set; }
        public string? TraineeName { get; set; }

    }
}
