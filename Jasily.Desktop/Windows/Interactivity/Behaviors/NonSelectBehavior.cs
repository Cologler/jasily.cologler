using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Jasily.Windows.Interactivity.Behaviors
{
    public class NonSelectBehavior : Behavior<Selector>
    {
        #region Overrides of Behavior

        /// <summary>
        /// 在行为附加到 AssociatedObject 后调用。
        /// </summary>
        /// <remarks>
        /// 替代它以便将功能挂钩到 AssociatedObject。
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += this.AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.AssociatedObject.SelectedItem != null) this.AssociatedObject.SelectedItem = null;
        }

        /// <summary>
        /// 在行为与其 AssociatedObject 分离时（但在它实际发生之前）调用。
        /// </summary>
        /// <remarks>
        /// 替代它以便将功能从 AssociatedObject 中解除挂钩。
        /// </remarks>
        protected override void OnDetaching()
        {
            this.AssociatedObject.SelectionChanged -= this.AssociatedObject_SelectionChanged;
            base.OnDetaching();
        }

        #endregion
    }
}