using Iot.Max.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.ExtenModels
{
    /*
     * 同时插入邀约、公司信息
     * **/
    public class InsertInterInviteModels
    {
        public InterCompany Company { get; set; }
        public InterInvite Invite { get; set; }
    }
}
