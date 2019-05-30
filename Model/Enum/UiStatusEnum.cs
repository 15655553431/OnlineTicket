using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    /// <summary>
    /// 用户账户状态信息的枚举
    /// </summary>
    public enum UiStatusEnum
    {
        /// <summary>
        /// 旅客账户：未通过核验的状态。其他账户：停用状态
        /// </summary>
        Block = 2,

        /// <summary>
        /// 旅客账户：通过核验的状态。其他账户：正常使用状态
        /// </summary>
        Pass = 1
    }
}
