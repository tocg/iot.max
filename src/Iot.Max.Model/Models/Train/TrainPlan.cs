using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Iot.Max.Model.Models.Train
{
    [Table("train_plan")]
    public partial class TrainPlan
    {
        public string Id { get; set; }
        public string Stagegradeid { get; set; }
        public string Name { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public DateTime? Createdate { get; set; }
        public string Createby { get; set; }
    }
}
