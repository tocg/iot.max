using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Iot.Max.Model.Models.Train
{
    [Table("train_plan_detail")]
    public partial class TrainPlanDetail
    {
        public string Id { get; set; }
        public string Planid { get; set; }
        public string Stuid { get; set; }
        public string Content { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Updatedate { get; set; }
    }
}
