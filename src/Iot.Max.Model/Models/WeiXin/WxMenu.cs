using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("wx_menu")]
    public class WxMenu
    {
        public string ID { get; set; }
        public string SupID { get; set; }
        public string Name { get; set; }
        public string MenuType { get; set; }
        public string MenuTypeValue { get; set; }
        public string IsReg { get; set; }
        public string IsConfig { get; set; }
        public string Sort { get; set; }
        public string State { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
