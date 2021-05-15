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
    [Table("document_annex")]
    public class DocumentAnnex
    {
        public string ID { get; set; }
        public string DocumentID { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public int DownloadNumber { get; set; }
    }
}
