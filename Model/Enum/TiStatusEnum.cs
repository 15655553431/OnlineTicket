using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    /// <summary>
    /// 车票状态枚举
    /// </summary>
    public enum TiStatusEnum
    {
        /// <summary>
        /// 订单座位号已经产生，但是没有付款
        /// </summary>
        NoPay = 5,

        /// <summary>
        /// 已经付款，还没有票，预约票状态
        /// </summary>
        Order = 4,

        /// <summary>
        /// 退票后，座位号已经卖出去了
        /// </summary>
        Over = 3,

        /// <summary>
        /// 正常购买付款后，退票了,座位号还可以继续卖的
        /// </summary>
        Quit = 2,

        /// <summary>
        /// 正常购买，已经完成付款
        /// </summary>
        Done = 1
    }
}
