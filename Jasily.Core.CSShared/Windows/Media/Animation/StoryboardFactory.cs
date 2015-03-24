using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Media.Animation
{
#if WINDOWS_PHONE_80
    public class StoryboardFactory
    {
        

        public static class PropertyPathResourceStrings
        {
            public const string UIElement_Opacity = "(UIElement.Opacity)";
            public const string UIElement_IsHitTestVisible = "UIElement.IsHitTestVisible";
            public const string UIElement_RenderTransform_CompositeTransform_TranslateX = "(UIElement.RenderTransform).(CompositeTransform.TranslateX)";
            public const string UIElement_RenderTransform_CompositeTransform_TranslateY = "(UIElement.RenderTransform).(CompositeTransform.TranslateY)";
        }
    }
#endif
}
