using System.ComponentModel;

namespace AuthService.Common.Extensions
{
    public static class EnumerationExtensions
    {
        public static string ToDescription(this Enum enumeration)
        {
            var attribute = GetText<DescriptionAttribute>(enumeration);

            return attribute.Description;
        }

        public static T GetText<T>(Enum enumeration) where T : Attribute
        {
            var type = enumeration.GetType();

            var memberInfo = type.GetMember(enumeration.ToString());

            if (!memberInfo.Any())
                throw new ArgumentException($"No public members for the argument '{enumeration}'.");

            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            if (attributes == null || attributes.Length != 1)
                throw new ArgumentException($"Can't find an attribute matching '{typeof(T).Name}' for the argument '{enumeration}'");

            return attributes.Single() as T;
        }
    }
}
