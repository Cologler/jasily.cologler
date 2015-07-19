namespace System
{
    public class RequestActionEventArgs<TArg> : EventArgs
    {
        public RequestActionEventArgs(TArg arg)
        {
            this.Arg = arg;
        }

        public TArg Arg { get; private set; }

        /// <summary>
        /// get or set if request should be accept. default value was false.
        /// </summary>
        public bool IsAccept { get; set; }
    }

    public class RequestActionEventArgs<TAction, TArg> : RequestActionEventArgs<TArg>
    {
        public RequestActionEventArgs(TAction action, TArg arg)
            : base(arg)
        {
            this.Action = action;
        }

        public TAction Action { get; private set; }
    }
}