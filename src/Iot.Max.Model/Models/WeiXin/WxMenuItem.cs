using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models.WeiXin
{
    [Table("wx_menu_item")]
    public class WxMenuItem
    {
        public string ID { get; set; }
        public string MenuID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Sort { get; set; }
        public string State { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
