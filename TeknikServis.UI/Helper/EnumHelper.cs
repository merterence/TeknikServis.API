namespace TeknikServis.UI.Helper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class EnumHelper // Enum verilerimizin Description değerlerini kullanıcı arayüzlerinde select veya başka html elemanlarında kullanmak amacıyla yardımcı sınıf
    {
        public static List<(int Value, string Description)> GetEnumDescriptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => (
                    Convert.ToInt32(e),
                    GetDescription(e)
                )).ToList();
        }

        private static string GetDescription<T>(T enumValue) where T : Enum
        {
            var field = typeof(T).GetField(enumValue.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? enumValue.ToString();
        }
    }
}
