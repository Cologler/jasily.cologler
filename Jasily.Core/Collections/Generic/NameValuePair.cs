
namespace System.Collections.Generic
{
    public struct NameValuePair<TName, TValue>
    {
        private TName _name;
        private TValue _value;
        
        public NameValuePair(TName name, TValue value)
        {
            this._name = name;
            this._value = value;
        }

        public TName Name { get { return this._name; } }

        public TValue Value { get { return this._value; } }

        public override string ToString()
        {
            if (this.Name != null)
                return this.Name.ToString();
            else
                return this.ToString();
        }
    }
}
