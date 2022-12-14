using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Utility{
    public static class EnumUtility{
        public static T GetValue<T>(string description) where T : Enum{
            foreach (var field in typeof(T).GetFields())
                if (Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) is DescriptionAttribute attribute){
                    if (attribute.Description == description) return (T)field.GetValue(null);
                } else{
                    if (field.Name == description) return (T)field.GetValue(null);
                }

            throw new ArgumentException("Not found.", nameof(description));
        }

        public static IEnumerable<T> GetValues<T>(){
            foreach (T value in Enum.GetValues(typeof(T))) yield return value;
        }
    }
}