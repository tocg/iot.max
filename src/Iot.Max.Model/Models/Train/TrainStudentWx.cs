using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Iot.Max.Model.Models.Train
{
    [Table("train_student_wx")]
    public partial class TrainStudentWx
    {
        public string Id { get; set; }
        public string Stuid { get; set; }
        public string Openid { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string State { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Updatedate { get; set; }
    }
}
