using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Common
{
    public class Pages<T>
    {
        private int _pageindex;//当前页码
        private int _count;//总记录数
        private int _pagecount;//每页记录数
        private string _url;//请求路径和参数，例如：/page/?method=findXXX&cid=1&bname=2
        private List<T> _pageList;

        // 计算总页数
        public int GetPages()
        {
            return (int)Math.Ceiling((decimal)_count % _pagecount);
            //int tp = _count / _pagecount;
            // _count % _pagecount == 0 ? tp : tp + 1;
        }

        public int GetPageIndex()
        {
            return _pageindex;
        }
        public void SetPageIndex(int _pageindex)
        {
            this._pageindex = _pageindex;
        }
        public int GetCount()
        {
            return _count;
        }
        public void SetCount(int _count)
        {
            this._count = _count;
        }
        public int GetPageCount()
        {
            return _pagecount;
        }
        public void SetPageCount(int _pagecount)
        {
            this._pagecount = _pagecount;
        }
        public string GetUrl()
        {
            return _url;
        }
        public void SetUrl(string _url)
        {
            this._url = _url;
        }
        public List<T> GetPageList()
        {
            return _pageList;
        }
        public void SetPageList(List<T> pageList)
        {
            this._pageList = pageList;
        }
    }
}
