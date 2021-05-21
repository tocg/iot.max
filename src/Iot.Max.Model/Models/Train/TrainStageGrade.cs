using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Iot.Max.Model.Models.Train
{
    [Table("train_stage_grade")]
    public partial class TrainStageGrade
    {
        public string Id { get; set; }
        public string Stageid { get; set; }
        public string Gradeid { get; set; }
        public string State { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Updatedate { get; set; }
    }
}
