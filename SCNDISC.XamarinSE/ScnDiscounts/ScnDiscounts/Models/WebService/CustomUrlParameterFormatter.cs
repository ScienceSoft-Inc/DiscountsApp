using Refit;
using System;
using System.Reflection;

namespace ScnDiscounts.Models.WebService
{
    public class CustomUrlParameterFormatter : DefaultUrlParameterFormatter
    {
        public override string Format(object parameterValue, ParameterInfo parameterInfo)
        {
            if (parameterValue is DateTime dateValue)
                return dateValue.ToString("O");

            return base.Format(parameterValue, parameterInfo);
        }
    }
}
