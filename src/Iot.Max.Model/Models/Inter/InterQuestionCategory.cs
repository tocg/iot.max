using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("inter_question_category")]
    public class InterQuestionCategory
    {
        public string ID { get; set; }
        public string SupID { get; set; }
        public string CategoryName { get; set; }
        public string Sort { get; set; }
        public string State { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
