using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    public class TrainerTopic
    {
        public int topic_id { get; set; }
        public int trainer_id { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual User User { get; set; }



        [AllowNull]
        public DateTime? created_at { get; set; }
        [AllowNull]
        public DateTime? updated_at { get; set; }
        [AllowNull]
        public DateTime? deleted_at { get; set; }
    }
}
