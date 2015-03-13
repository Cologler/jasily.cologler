
namespace System.Collections.Generic
{
    public struct NameValuePair<TName, TValue>
    {
        private TName _name;
        private TValue _value;
        
        public NameValuePair(TName name, TValue value)
        {
            _name = name;
            _value = value;
        }

        public TName Name { get { return _name; } }

        public TValue Value { get { return _value; } }

        public override string ToString()
        {
            if (this.Name != null)
                return this.Name.ToString();
            else
                return this.ToString();
        }
    }
}
