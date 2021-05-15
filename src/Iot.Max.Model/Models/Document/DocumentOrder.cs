using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models.Document
{
    /// <summary>
    /// 文档附件
    /// </summary>
    [Table("document_order")]
    public class DocumentOrder
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public int ViewNumber { get; set; }
        public string IsOpen { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
