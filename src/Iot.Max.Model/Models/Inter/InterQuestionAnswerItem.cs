using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("inter_question_answeritem")]
    public class InterQuestionAnswerItem
    {
        public string ID { get; set; }
        public string QuestionID { get; set; }
        public string Answer { get; set; }
        public int LikeCount { get; set; }
        public string State { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
