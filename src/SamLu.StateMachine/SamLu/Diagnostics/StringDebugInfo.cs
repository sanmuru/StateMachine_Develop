using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    /// <summary>
    /// 为字符串提供调试信息。
    /// </summary>
    public class StringDebugInfo : IDebugInfo
    {
        private string s;
        
        /// <summary>
        /// 获取调试信息。
        /// </summary>
        public string DebugInfo => s == null ? "{Null}" : $"\"{s}\"";

        /// <summary>
        /// 此为支持获取调试信息的类型的必要约定。初始化 <see cref="StringDebugInfo"/> 的新实例。
        /// </summary>
        /// <param name="s">要获取调试信息的字符串对象。</param>
        /// <param name="args">获取调试信息时的可选参数。</param>
        public StringDebugInfo(string s, params object[] args)
        {
            this.s = s;
        }
    }
}
