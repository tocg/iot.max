using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("rbac_function")]
    public class RbacFunction
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public string Sort { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
