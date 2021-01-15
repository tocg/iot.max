using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("wx_user")]
    public class WxUser
    {
        public string ID { get; set; }
        public string OpendID { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public string Privilege { get; set; }
        public string UnionID { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
