using Autodesk.Revit.DB;
using System;
using System.Linq;

namespace UOL.Revit.SampleAddin.Utilities
{
    internal class ParameterHelper
    {
        public double ConvertFromAPI(DisplayUnitType displayUnitTypeTo, double value)
        {
            if (UnitUtils.IsValidDisplayUnit(displayUnitTypeTo))
            {
                return UnitUtils.ConvertFromInternalUnits(value, displayUnitTypeTo);
            }
            else
            {
                return value;
            }
        }

        public double ConvertToAPI(double value, DisplayUnitType displayUnitTypeFrom)
        {
            if (UnitUtils.IsValidDisplayUnit(displayUnitTypeFrom))
            {
                return UnitUtils.ConvertToInternalUnits(value, displayUnitTypeFrom);
            }
            else
            {
                return value;
            }
        }

        public Parameter GetParameter(Element element, BuiltInParameter builtInParameter)
        {
            return element.get_Parameter(builtInParameter);
        }

        public Parameter GetParameter(Element element, Guid parameterGuid)
        {
            return element.get_Parameter(parameterGuid);
        }

        public Parameter GetParameter(Element element, string parameterName)
        {
            return element.LookupParameter(parameterName);
        }

        public Parameter GetParameterBySearchString(Element element, string searchString, bool ignoreCase = false)
        {
            if (ignoreCase)
            {
                var foundParameter = element.GetOrderedParameters().FirstOrDefault(parameter => parameter.Definition.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);
                if (foundParameter == null)
                {
                    foreach (Parameter parameter in element.Parameters)
                    {
                        if (parameter.Definition.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundParameter = parameter;
                            break;
                        }
                    }
                }

                return foundParameter;
            }
            else
            {
                var foundParameter = element.GetOrderedParameters().FirstOrDefault(parameter => parameter.Definition.Name.Contains(searchString));
                if (foundParameter == null)
                {
                    foreach (Parameter parameter in element.Parameters)
                    {
                        if (parameter.Definition.Name.Contains(searchString))
                        {
                            foundParameter = parameter;
                            break;
                        }
                    }
                }

                return foundParameter;
            }
        }

        public object GetParameterValue(Element element, BuiltInParameter builtInParameter)
        {
            return GetParameterValue(GetParameter(element, builtInParameter));
        }

        public object GetParameterValue(Element element, Guid parameterGuid)
        {
            return GetParameterValue(GetParameter(element, parameterGuid));
        }

        public object GetParameterValue(Element element, string parameterName)
        {
            return GetParameterValue(GetParameter(element, parameterName));
        }

        public object GetParameterValue(Parameter parameter)
        {
            object returnValue = null;
            if (parameter == null || !parameter.HasValue)
            {
                return returnValue;
            }

            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    returnValue = parameter.AsDouble();
                    break;
                case StorageType.Integer:
                    returnValue = parameter.AsInteger();
                    break;
                case StorageType.String:
                    returnValue = parameter.AsString();
                    break;
                case StorageType.ElementId:
                    returnValue = parameter.AsElementId();
                    break;
            }

            return returnValue;
        }

        public bool SetParameterValue(Element element, BuiltInParameter builtInParameter, object value)
        {
            return SetParameterValue(GetParameter(element, builtInParameter), value);
        }

        public bool SetParameterValue(Element element, Guid parameterGuid, object value)
        {
            return SetParameterValue(GetParameter(element, parameterGuid), value);
        }

        public bool SetParameterValue(Element element, string parameterName, object value)
        {
            return SetParameterValue(GetParameter(element, parameterName), value);
        }

        public bool SetParameterValue(Parameter parameter, object value)
        {
            var returnValue = false;
            if (parameter == null || parameter.IsReadOnly)
            {
                return returnValue;
            }

            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    var doubleValue = ValueToDouble(value);
                    if (double.IsNaN(doubleValue))
                    {
                        return returnValue;
                    }

                    returnValue = parameter.Set(doubleValue);
                    break;
                case StorageType.Integer:
                    returnValue = parameter.Set(ValueToInt(value));
                    break;
                case StorageType.String:
                    if (value == null)
                    {
                        value = string.Empty;
                    }

                    returnValue = parameter.Set(value.ToString());
                    break;
                case StorageType.ElementId:
                    var valueInt = ValueToInt(value);
                    if (valueInt != 0 && valueInt != ElementId.InvalidElementId.IntegerValue)
                    {
                        returnValue = parameter.Set(new ElementId(valueInt));
                    }

                    break;
            }

            return returnValue;
        }

        private double ValueToDouble(object value)
        {
            var returnValue = 0.0;
            if (value is double)
            {
                returnValue = (double)value;
            }
            else if (value is string)
            {
                var valueString = value as string;
                valueString = valueString.Replace(".", ",");
                double.TryParse(valueString, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("nl-NL"), out returnValue);
            }
            else
            {
                returnValue = Convert.ToDouble(value);
            }

            return returnValue;
        }

        private int ValueToInt(object value)
        {
            var returnValue = 0;

            if (value is int)
            {
                returnValue = (int)value;
            }
            else if (value is bool)
            {
                returnValue = ((bool)value) ? 1 : 0;
            }
            else if (value is double)
            {
                returnValue = Convert.ToInt32(value);
            }
            else if (value is string)
            {
                int.TryParse(value.ToString(), out returnValue);
            }
            else
            {
                returnValue = value is ElementId ? (value as ElementId).IntegerValue : value == null ? 0 : Convert.ToInt32(value);
            }

            return returnValue;
        }
    }
}
