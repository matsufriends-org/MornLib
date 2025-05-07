using System.Collections.Generic;
using MornLib.Pools;

namespace MornLib.Cores
{
    public class MornHashSet<T> : HashSet<T>, IMornPoolItem
    {
        void IMornPoolItem.Clear()
        {
            Clear();
        }
    }
}