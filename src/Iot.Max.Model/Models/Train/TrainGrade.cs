using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Iot.Max.Model.Models.Train
{
    [Table("train_grade")]
    public partial class TrainGrade
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public DateTime? Createdate { get; set; }
    }
}
