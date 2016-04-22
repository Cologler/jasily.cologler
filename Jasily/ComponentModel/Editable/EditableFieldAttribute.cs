using System;

namespace Jasily.ComponentModel.Editable
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EditableFieldAttribute : Attribute
    {
        /// <summary>
        /// Converter should implemented IConverter.
        /// Convert() will be call on object to view model.
        /// ConvertBack() will be call on view model to object.
        /// Whatever, IPropertyContainer will auto wrap and unwrap.
        /// </summary>
        public Type Converter { get; set; }

        /// <summary>
        /// this will ignore Converter because of XXX -_-.
        /// </summary>
        public bool IsSubEditableViewModel { get; set; }
    }
}