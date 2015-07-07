namespace System.Shell
{
    public interface ISingleInstanceApp
    {
        bool SignalExternalCommandLineArgs(string[] args);
    } 
}
