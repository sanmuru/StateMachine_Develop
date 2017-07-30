using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine.EventDriven
{
    /// <summary>
    /// 表示将处理与事件驱动的有限状态机的状态的事件引发相关的方法。
    /// </summary>
    /// <param name="sender">事件处理程序附加到的对象。</param>
    /// <param name="e">事件数据。</param>
    public delegate void EventInvokeEventHandler(object sender, EventInvokeEventArgs e);
}
