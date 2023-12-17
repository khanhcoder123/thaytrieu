using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    public class Roles
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }
        public string description { get; set; }

        public int status { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        
        public DateTime deleted_at { get; set; }

    }
}
