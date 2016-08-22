using System.Text;

namespace System.Diagnostics
{
    public interface ICustomPrintable : IPrintable
    {
        void Print(StringBuilder builder);
    }
}