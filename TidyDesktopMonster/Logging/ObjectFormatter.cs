using System;
using System.Collections.Generic;

namespace TidyDesktopMonster.Logging
{
    internal class ObjectFormatter
    {
        public static string Format(object obj)
        {
            return Format(obj, 1);
        }

        static string Format(object obj, int indent)
        {
            if (obj == null)
                return "null";
            else if (obj is string || obj is Exception || obj.GetType().IsValueType)
                return Convert.ToString(obj);
            else if (obj is System.Collections.IEnumerable enumerable)
                return FormatEnumerable(enumerable, indent);
            else if (indent > 1)
                return Convert.ToString(obj);
            else
                return FormatObject(obj, indent);
        }

        static string FormatEnumerable(System.Collections.IEnumerable enumerable, int indent)
        {
            var items = new List<string>();
            foreach (var item in enumerable)
                items.Add(Format(item, indent + 1));
            return FormatDataStructure(enumerable, items.ToArray(), indent);
        }

        static string FormatObject(object obj, int indent)
        {
            var properties = new List<string>();
            var publicInstance = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            foreach (var m in obj.GetType().GetMembers(publicInstance))
            {
                var p = m as System.Reflection.PropertyInfo;
                if (p != null)
                    properties.Add(string.Format("{0}: {1}", m.Name, Format(p.GetValue(obj), indent + 1)));
            }
            return FormatDataStructure(obj, properties.ToArray(), indent);
        }

        static string FormatDataStructure(object obj, string[] properties, int indentLevel)
        {
            var type = obj.GetType();
            var typeNamePrefix = type.Namespace == null ? string.Empty : type.Name + " ";
            var innerIndent = new string(' ', indentLevel * 2);
            var outerIndent = new string(' ', (indentLevel - 1) * 2);
            return typeNamePrefix + "{" + Environment.NewLine +
                innerIndent + string.Join(Environment.NewLine + innerIndent, properties) + Environment.NewLine +
                outerIndent + "}";
        }
    }
}
