using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Iot.Max.Model.Models.Train
{
    [Table("train_log")]
    public partial class TrainLog
    {
        public string Id { get; set; }
        public string Planid { get; set; }
        public string Stuid { get; set; }
        public string Date { get; set; }
        public string Content { get; set; }
        public string Isfinish { get; set; }
        public DateTime? Createdate { get; set; }
        public string Createby { get; set; }
        public DateTime? Updatedate { get; set; }
        public string Updateby { get; set; }
    }
}
