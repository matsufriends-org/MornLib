using System.Collections.Generic;
using MornLib.Pools;

namespace MornLib.Cores
{
    public class MornQueue<T> : Queue<T>, IMornPoolItem
    {
        void IMornPoolItem.Clear()
        {
            Clear();
        }
    }
}