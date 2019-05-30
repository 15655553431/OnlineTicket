using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.other
{
    public class PagingInfo
    {
        //总数量
        public int TotalItems { get; set; }
        //当前索引
        public int PageIndex { get; set; }
        //分页大小
        public int PageSize { get; set; }
        //页数
        public int PageCount
        {
            get
            {
                return PageSize == 0 ? 0 : (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
        }
    }
}
