using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Jasily.Windows.Interactivity.Behaviors
{
    public class TextBlockHighLightBehavior : Behavior<TextBlock>
    {
        #region DependencyProperty

        public static readonly DependencyProperty PlainTextProperty = DependencyProperty.Register(
            nameof(PlainText), typeof(string), typeof(TextBlockHighLightBehavior),
            new PropertyMetadata(default(string), OnDependencyPropertyChanged));

        public static readonly DependencyProperty HighLightContentProperty = DependencyProperty.Register(
            nameof(HighLightContent), typeof(string), typeof(TextBlockHighLightBehavior),
            new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty HighLightForegroundProperty = DependencyProperty.Register(
            nameof(HighLightForeground), typeof(Brush), typeof(TextBlockHighLightBehavior),
            new PropertyMetadata(Brushes.OrangeRed, OnDependencyPropertyChanged));

        public static readonly DependencyProperty HighLightBackgroundProperty = DependencyProperty.Register(
            nameof(HighLightBackground), typeof(Brush), typeof(TextBlockHighLightBehavior),
            new PropertyMetadata(default(Brush), OnDependencyPropertyChanged));

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var changed = false;
            if (e.Property.PropertyType == typeof(string))
            {
                changed = !string.Equals((string)e.NewValue, (string)e.OldValue);
            }

            if (!changed) return;
            (d as TextBlockHighLightBehavior)?.ApplyChanged();
        }

        #endregion

        public string PlainText
        {
            get { return (string)this.GetValue(PlainTextProperty); }
            set { this.SetValue(PlainTextProperty, value); }
        }

        public string HighLightContent
        {
            get { return (string)this.GetValue(HighLightContentProperty); }
            set { this.SetValue(HighLightContentProperty, value); }
        }

        public Brush HighLightForeground
        {
            get { return (Brush)this.GetValue(HighLightForegroundProperty); }
            set { this.SetValue(HighLightForegroundProperty, value); }
        }

        public Brush HighLightBackground
        {
            get { return (Brush)this.GetValue(HighLightBackgroundProperty); }
            set { this.SetValue(HighLightBackgroundProperty, value); }
        }

        /// <summary>
        /// 在行为附加到 AssociatedObject 后调用。
        /// </summary>
        /// <remarks>
        /// 替代它以便将功能挂钩到 AssociatedObject。
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.ApplyChanged();
        }

        private void ApplyChanged()
        {
            var textBlock = this.AssociatedObject;
            if (textBlock == null) return;

            var plaintext = this.PlainText ?? string.Empty;
            var highlight = this.HighLightContent ?? string.Empty;
            textBlock.Inlines.Clear();
            if (plaintext.Length == 0 || plaintext.Length < highlight.Length)
            {
                textBlock.Inlines.Add(new Run(plaintext));
            }
            else
            {
                var highlightForeground = this.HighLightForeground ??
                    (Brush)HighLightForegroundProperty.DefaultMetadata.DefaultValue;
                var highlightBackground = this.HighLightBackground ??
                    (Brush)HighLightBackgroundProperty.DefaultMetadata.DefaultValue;

                var splited = plaintext.Split(highlight)
                    .Select(z => new Run(z))
                    .JoinWith(() => new Run(highlight)
                    {
                        Foreground = highlightForeground,
                        Background = highlightBackground
                    })
                    .ToArray();
                foreach (var run in splited)
                {
                    textBlock.Inlines.Add(run);
                }
            }
        }
    }
}