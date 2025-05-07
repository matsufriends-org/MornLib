using System.Text;
using MornLib.Pools;

namespace MornLib.Cores
{
    public sealed class MornStringBuilder : IMornPoolItem
    {
        private readonly StringBuilder _builder = new();
        private bool _isFirst;
        private char _split;

        public void Clear()
        {
            _builder.Clear();
        }

        public void Init(char split)
        {
            _builder.Clear();
            _split = split;
            _isFirst = true;
        }

        public void Append(string message)
        {
            if (_isFirst == false) _builder.Append(_split);

            _builder.Append(message);
            _isFirst = false;
        }

        public string Get()
        {
            return _builder.ToString();
        }
    }
}