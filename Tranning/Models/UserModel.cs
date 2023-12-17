using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Tranning.Validations;

namespace Tranning.Models
{
    public class UserModel
    {
        public List<UserDetail> UserDetailLists { get; set; }
    }

    public class UserDetail
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Enter role, please")]
        public int role_id { get; set; }

        public string extra_code { get; set; }
        [Required(ErrorMessage = "Enter username, please")]
        public string username { get; set; }
        [Required(ErrorMessage = "Enter password, please")]
        public string password { get; set; }

        public string email { get; set; }
        public string phone { get; set; }

        public string? address { get; set; }

        public string gender { get; set; }

        public string? last_login { get; set; }

        public string? last_logout { get; set; }

        public string status { get; set; }

        public DateTime? created_at { get; set; }
        [AllowNull]
        public DateTime? updated_at { get; set; }
        [AllowNull]
        public DateTime? deleted_at { get; set; }

        public string full_name { get; set; }

        public string? education { get; set; }

        public string? programming_language { get; set; }

        public string? toeic_score { get; set; }

        public string? experience { get; set; }

        public string? department { get; set; }
    }
}
