using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CarRecyclingWeb.Models
{
    public static class EnumExtensions
    {
        public static string GetEnumMemberValue<TEnum>(this TEnum value)
            where TEnum : Enum
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo?.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                                     .OfType<EnumMemberAttribute>()
                                     .FirstOrDefault();
            return attribute == null ? value.ToString() : attribute.Value;
        }

        public static TEnum ToEnum<TEnum>(this string str)
            where TEnum : Enum
        {
            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) is EnumMemberAttribute enumMemberAttribute)
                {
                    if (enumMemberAttribute.Value == str)
                    {
                        return (TEnum)field.GetValue(null);
                    }
                }
                else if (field.Name.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    return (TEnum)field.GetValue(null);
                }
            }
            throw new ArgumentException($"Cannot convert '{str}' to enum '{typeof(TEnum).Name}'. No matching EnumMemberAttribute.Value or enum member name found.");
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name ?? enumValue.ToString();
        }
    }
}