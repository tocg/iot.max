using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Iot.Max.Model.Models.Train
{
    [Table("train_student")]
    public partial class TrainStudent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Registaddress { get; set; }
        public string Workdaddress { get; set; }
        public DateTime? Workdate { get; set; }
        public decimal? Salary { get; set; }
        public string State { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Updatedate { get; set; }
        public string Createby { get; set; }
        public string Updateby { get; set; }
    }
}
