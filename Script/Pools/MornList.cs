using System.Collections.Generic;
using MornLib.Pools;

namespace MornLib.Cores
{
    public class MornList<T> : List<T>, IMornPoolItem
    {
        void IMornPoolItem.Clear()
        {
            Clear();
        }
    }
}