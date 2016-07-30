using System.Linq;
using System.Windows;

namespace Jasily.Windows
{
    public class WindowStatusCached
    {
        public WindowStatusCached(Window window)
        {
            this.IsClosed = !Application.Current.Windows.OfType<Window>().Contains(window);
            if (!this.IsClosed)
            {
                window.Closed += (_, __) => this.IsClosed = true;
            }
        }

        public bool IsClosed { get; private set; }
    }
}