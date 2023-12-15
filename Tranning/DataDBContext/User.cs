using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    public class User
    {
        [Key]
        public int id { get; set; }

        [Column("role_id", TypeName = "Integer"), Required]
        public int role_id { get; set; }

        [Column("extra_code", TypeName = "Varchar(50)"), Required]
        public string extra_code { get; set; }

        [Column("username", TypeName = "Varchar(50)"), Required]
        public string username { get; set; }

        [Column("password", TypeName = "Varchar(50)"), Required]
        public string password { get; set; }

        [Column("email", TypeName = "Varchar(50)"), Required]
        public string email { get; set; }

        [Column("phone", TypeName = "Varchar(20)"), Required]
        public string phone { get; set; }

        [Column("address", TypeName = "Varchar(100)"), AllowNull]
        public string? address { get; set; }

        [Column("gender", TypeName = "Varchar(50)"), Required]
        public string gender { get; set; }


        [Column("last_login", TypeName = "Datetime"), AllowNull]
        public DateTime? last_login { get; set; }

        [Column("last_logout", TypeName = "Datetime"), AllowNull]
        public DateTime? last_logout { get; set; }

        [Column("status", TypeName = "Varchar(50)"), Required]
        public string status { get; set; }

        public DateTime? created_at { get; set; }
        [AllowNull]
        public DateTime? updated_at { get; set; }
        [AllowNull]
        public DateTime? deleted_at { get; set; }

        [Column("full_name", TypeName = "Varchar(50)"), Required]
        public string full_name { get; set; }

        [Column("education", TypeName = "Varchar(50)"), AllowNull]
        public string? education { get; set; }

        [Column("programming_language", TypeName = "Varchar(50)"), AllowNull]
        public string? programming_language { get; set; }

        [Column("toeic_score", TypeName = "Integer"), AllowNull]
        public int? toeic_score { get; set; }

        [Column("experience", TypeName = "Varchar(50)"), AllowNull]
        public string? experience { get; set; }

        [Column("department", TypeName = "Varchar(50)"), AllowNull]
        public string? department { get; set; }

        public virtual ICollection<TraineeCourse> TraineeCourses { get; set; }
        public virtual ICollection<TrainerTopic> TrainerTopics { get; set; }
    }
}
