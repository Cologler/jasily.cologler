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
        /// </summary>
        public Type Converter { get; set; }

        public bool IsSubEditableViewModel { get; set; }
    }
}