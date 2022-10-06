using System;
using System.ComponentModel;

namespace Utility.Extensions{
    public static class EnumExtension{
        public static string GetDescription(this Enum value){
            var field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute
                attribute
                ? value.ToString()
                : attribute.Description;
        }
    }
}