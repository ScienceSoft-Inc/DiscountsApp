using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Web.Admin.ServiceLayer.Extensions
{
    public static class EnitiyExtensionsMapper
    {
        private static readonly string[] languages = new[] { "RU", "EN" };
        private const char langSeparator = '_';

        public static TTarget MapPublicProperties<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            var target = new TTarget();
            var sourceProperties = typeof(TSource).GetTypeInfo().GetProperties();
            var targetProperties = typeof(TTarget).GetTypeInfo().GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                var targetProperty = targetProperties.SingleOrDefault(p => p.Name == sourceProperty.Name);
                if (targetProperty != null && targetProperty.PropertyType == sourceProperty.PropertyType)
                {
                    targetProperty.SetValue(target, sourceProperty.GetValue(source));
                }
            }
            return target;
        }

        public static TTarget MapLocalizableToFlatProperties<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            var sourceProperties = typeof(TSource).GetTypeInfo().GetProperties().Where(p => p.PropertyType == typeof(IEnumerable<LocalizableText>));
            var targetProperties = typeof(TTarget).GetTypeInfo().GetProperties();
            foreach (var language in languages)
            {
                foreach (var sourceProperty in sourceProperties)
                {
                    var targetProperty = targetProperties.SingleOrDefault(p => p.Name == String.Format("{0}_{1}", sourceProperty.Name, language));
                    if (targetProperty != null)
                    {
                        var sourceValue = ((IEnumerable<LocalizableText>)sourceProperty.GetValue(source)).SingleOrDefault(lt => lt.Lan == language);
                        if (sourceValue == null)
                        {
                            continue;
                        }

                        targetProperty.SetValue(target, sourceValue.LocText ?? String.Empty);
                    }
                }
            }
            return target;
        }

        public static TTarget MapFlatToLocalizableProperties<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            var sourceProperties = typeof(TSource).GetTypeInfo().GetProperties().Where(p => p.Name.Contains("_"));
            var targetProperties = typeof(TTarget).GetTypeInfo().GetProperties().Where(p => p.PropertyType == typeof(IEnumerable<LocalizableText>));
            foreach (var targetProperty in targetProperties)
            {
                var targetName = targetProperty.Name;
                var targetValue = sourceProperties.Where(p => p.Name.Split(langSeparator)[0] == targetName).Select(sourceProperty => new LocalizableText()
                {
                    Lan = sourceProperty.Name.Split(langSeparator)[1],
                    LocText = (string)sourceProperty.GetValue(source)
                }).ToArray();

                targetProperty.SetValue(target, targetValue);
            }
            return target;
        }
    }
}