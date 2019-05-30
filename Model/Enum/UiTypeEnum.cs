using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    /// <summary>
    /// 用户账户类型的枚举
    /// </summary>
    public enum UiTypeEnum
    {
        /// <summary>
        /// 检票员
        /// </summary>
        Teller = 3,

        /// <summary>
        /// 乘客
        /// </summary>
        Guest = 2,

        /// <summary>
        /// 车站管理员
        /// </summary>
        Admin = 1
    }
}
