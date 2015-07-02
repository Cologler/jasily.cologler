using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace System.Windows.Controls
{
    public static class ValidationExtensions
    {
        public static bool IsValid(this DependencyObject node, bool isFocusErrorControl = false)
        {
            if (node == null)
                return true;
            
            bool hasError = Validation.GetHasError(node);

            if (hasError)
            {
                if (isFocusErrorControl)
                {
                    node.IfType<IInputElement, IInputElement>(Keyboard.Focus);
                }

                return false;
            }

            return LogicalTreeHelper.GetChildren(node).OfType<DependencyObject>().All(subnode => IsValid(subnode, isFocusErrorControl));
        }
    }
}