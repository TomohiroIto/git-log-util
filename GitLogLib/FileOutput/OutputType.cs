using System;
using System.ComponentModel;
using System.Reflection;

namespace GitLogLib.FileOutput
{
    public enum OutputType
    {
        [Description("Commit Count")]
        CommitCount,

        [Description("Modified Rows")]
        ModifiedRows,

        [Description("Added Rows")]
        AddedRows,

        [Description("Deleted Rows")]
        DeletedRows
    }

    public static class OutputTypeExtension
    {
        public static string GetDescription(this OutputType value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
