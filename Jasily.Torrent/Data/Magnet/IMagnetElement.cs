
namespace System.Data.Magnet
{
    interface IMagnetElement
    {
        string NodeName { get; }

        string NodeValue { get; }

        string AsMagnetElement();
    }
}
