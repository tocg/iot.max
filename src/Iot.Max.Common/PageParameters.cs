using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Common
{
    public class PageParameters<T>
    {
        private int _page;
        public int Page
        {
            get
            {
                if (_page == 0)
                    _page = 1;
                return _page;
            }
            set => _page = value;
        }

        private int _limit;
        public int Limit
        {
            get
            {
                if (_limit == 0)
                    _limit = 20;
                return _limit;
            }
            set { _limit = value; }
        }

        public T Model { get; set; }

        public PageProc Proc { get; set; } = null;
    }

    public class PageProc
    {
        public string ProcName { get; set; }
        public string ProcOutName { get; set; }
        public dynamic ProcParm { get; set; }
    }
}
