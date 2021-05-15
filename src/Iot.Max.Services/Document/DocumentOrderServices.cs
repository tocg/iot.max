using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Services.Document
{
    public class DocumentOrderServices : BaseServices
    {
        public int Insert(Model.ExtenModels.InsertDocumentAnnexModels model)
        {
            List<Tuple<string, object>> list = new List<Tuple<string, object>>();

            string sql;
            //主表（文档）
            sql = $"insert into document_order(id,title,context,category,author,isopen,createdate,createby) " +
                $"values(@id,@title,@context,@category,@author,@isopen,@createdate,@createby)";
            list.Add(Tuple.Create(sql, (object)model.Document));

            if (model.Annex != null)
            {
                foreach (var item in model.Annex)
                {
                    //明细表（附件）
                    var t = Tuple.Create("insert into document_annex(id,documentid,path) values(@id,@documentid,@path)",
                        (object)item);
                    list.Add(t);
                }
            }
            base.DapperClient.ExecuteTransaction(list);
            return 0;
        }
    }
}
