using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamLu.StateMachine
{
    public interface IInputSet : IEnumerable
    {
        bool NextTo(object x, object y);
    }

    public interface IInputSet<T> : IInputSet, IEnumerable<T>
    {
        bool NextTo(T x, T y);
    }
}
