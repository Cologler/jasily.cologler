using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Media.Animation
{
#if WINDOWS_PHONE_80
    public static class JasilyStoryboard
    {
        public static Task BeginAsync(this Storyboard storyboard)
        {
            storyboard.ThrowIfNull("storyboard");

            var tcs = new TaskCompletionSource<bool>();

            EventHandler onComplete = null;
            onComplete  = (s, e) =>
            {
                // sure ref set to null.
                storyboard.Completed -= onComplete;
                tcs.SetResult(true);
            };
            storyboard.Completed += onComplete;

            storyboard.Begin();

            return tcs.Task;
        }
    }
#endif
}
