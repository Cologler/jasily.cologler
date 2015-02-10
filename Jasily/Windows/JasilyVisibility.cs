
namespace System.Windows
{
    public static class JasilyVisibility
    {
        /// <summary>
        /// 如果 True，返回 Visible，否则返回 Collapsed
        /// </summary>
        /// <param name="value"></param>
        /// <returns>value ? Visibility.Visible : Visibility.Collapsed;</returns>
        public static Visibility VisibleIfTrue(this bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 如果 True，返回 Collapsed，否则返回 Visible
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Visibility CollapsedIfTrue(this bool value)
        {
            return (!value) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
