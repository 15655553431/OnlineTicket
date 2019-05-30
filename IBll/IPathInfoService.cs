using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBll
{
    public partial interface IPathInfoService : IBaseService<PathInfo>
    {
        /// <summary>
        /// 设置路线的状态信息
        /// </summary>
        /// <param name="id">路线id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        bool SetStatus(int id, int status);
        
    }
}
