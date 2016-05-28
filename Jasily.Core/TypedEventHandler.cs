namespace System
{
    public delegate void TypedEventHandler<in T>(T sender);

    public delegate void TypedEventHandler<in T, in TEventArgs>(T sender, TEventArgs e);
}