using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.Diagnostics
{
    /// <summary>
    /// 为字符提供调试信息。
    /// </summary>
    public class CharDebugInfo : IDebugInfo
    {
        private char c;

        /// <summary>
        /// 获取调试信息。
        /// </summary>
        public string DebugInfo
        {
            get
            {
                switch (this.c)
                {
                    case '\0': return "'\\0'";
                    case '\\': return "'\\'";
                    case '\'': return "'\\''";
                    case '"': return "'\"'";
                    case '\a': return "'\\a'";
                    case '\b': return "'\\b'";
                    case '\t': return "'\\t'";
                    case '\f': return "'\\f'";
                    case '\v': return "'\\v'";
                    case '\r': return "'\\r'";
                    case '\n': return "'\\n'";
                    default: return $"'{c}'";
                }
            }
        }

        /// <summary>
        /// 此为支持获取调试信息的类型的必要约定。初始化 <see cref="CharDebugInfo"/> 的新实例。
        /// </summary>
        /// <param name="c">要获取调试信息的字符对象。</param>
        /// <param name="args">获取调试信息时的可选参数。</param>
        public CharDebugInfo(char c, params object[] args)
        {
            this.c = c;
        }
    }
}
