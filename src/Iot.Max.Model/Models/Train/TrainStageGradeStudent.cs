using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Iot.Max.Model.Models.Train
{
    [Table("train_stage_grade_student")]
    public partial class TrainStageGradeStudent
    {
        public string Id { get; set; }
        public string Stagegradeid { get; set; }
        public string Stuid { get; set; }
        public string State { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Updatedate { get; set; }
    }
}
