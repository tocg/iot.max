using Iot.Max.Model.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.ExtenModels
{
    public class InsertDocumentAnnexModels
    {
        public DocumentOrder Document  { get; set; }
        public List<DocumentAnnex> Annex { get; set; }
    }
}
