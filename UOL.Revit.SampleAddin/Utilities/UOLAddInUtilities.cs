using Autodesk.Revit.DB;
using Cadac.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UOL.Models;
using UOL.Revit.SampleAddin.Utilities;

namespace UOL.Revit.SampleAddin
{
    internal class UOLAddInUtilities
    {
        public static DisplayUnitType ConvertEUCode(string etimUnitCode)
        {
            switch (etimUnitCode)
            {
                case "EU000018": return DisplayUnitType.DUT_KILOVOLT_AMPERES;
                case "EU000022": return DisplayUnitType.DUT_VOLT_AMPERES;
                case "EU570011": return DisplayUnitType.DUT_JOULES;
                case "EU570020": return DisplayUnitType.DUT_NEWTONS;
                case "EU570028": return DisplayUnitType.DUT_HERTZ;
                case "EU570036": return DisplayUnitType.DUT_METERS;
                case "EU570054": return DisplayUnitType.DUT_WATTS;
                case "EU570056": return DisplayUnitType.DUT_BARS;
                case "EU570073": return DisplayUnitType.DUT_CELSIUS;
                case "EU570076": return DisplayUnitType.DUT_KELVIN;
                case "EU570079": return DisplayUnitType.DUT_VOLTS;
                case "EU570097": return DisplayUnitType.DUT_SQUARE_METERS;
                case "EU570106": return DisplayUnitType.DUT_LUX;
                case "EU570108": return DisplayUnitType.DUT_LUMENS;
                case "EU570109": return DisplayUnitType.DUT_CANDELAS;
                case "EU570121": return DisplayUnitType.DUT_SQUARE_CENTIMETERS;
                case "EU570126": return DisplayUnitType.DUT_SQUARE_MILLIMETERS;
                case "EU570170": return DisplayUnitType.DUT_KILOAMPERES;
                case "EU570174": return DisplayUnitType.DUT_MILLIAMPERES;
                case "EU570193": return DisplayUnitType.DUT_KILOVOLTS;
                case "EU570235": return DisplayUnitType.DUT_WATTS_PER_SQUARE_METER;
                case "EU570245": return DisplayUnitType.DUT_NEWTON_METERS;
                case "EU570257": return DisplayUnitType.DUT_CANDELAS_PER_SQUARE_METER;
                case "EU570376": return DisplayUnitType.DUT_METERS_PER_SECOND;
                case "EU570378": return DisplayUnitType.DUT_CUBIC_CENTIMETERS;
                case "EU570383": return DisplayUnitType.DUT_DECIMAL_DEGREES;
                case "EU570399": return DisplayUnitType.DUT_CENTIMETERS;
                case "EU570403": return DisplayUnitType.DUT_LITERS;
                case "EU570416": return DisplayUnitType.DUT_KILOPASCALS;
                case "EU570418": return DisplayUnitType.DUT_KILOWATTS;
                case "EU570419": return DisplayUnitType.DUT_KILOWATT_HOURS;
                case "EU570420": return DisplayUnitType.DUT_POUNDS_FORCE;
                case "EU570428": return DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_INCH;
                case "EU570448": return DisplayUnitType.DUT_MILLIMETERS;
                case "EU570449": return DisplayUnitType.DUT_LITERS_PER_SECOND;
                case "EU570451": return DisplayUnitType.DUT_CUBIC_METERS_PER_HOUR;
                case "EU570459": return DisplayUnitType.DUT_AMPERES;
                case "EU570492": return DisplayUnitType.DUT_PASCALS;
                case "EU570508": return DisplayUnitType.DUT_KILONEWTONS;
                case "EU570515": return DisplayUnitType.DUT_MILLIVOLTS;
                case "EU571032": return DisplayUnitType.DUT_KILONEWTONS_PER_SQUARE_METER;
                case "EU571039": return DisplayUnitType.DUT_CUBIC_MILLIMETERS;
                case "EU571050": return DisplayUnitType.DUT_CUBIC_METERS_PER_SECOND;
                case "EU571052": return DisplayUnitType.DUT_PASCAL_SECONDS;
                case "EU571055": return DisplayUnitType.DUT_KILONEWTONS_PER_METER;
                case "EU571057": return DisplayUnitType.DUT_NEWTONS_PER_SQUARE_METER;
                case "EU571085": return DisplayUnitType.DUT_POUNDS_MASS_PER_CUBIC_FOOT;
                case "EU571087": return DisplayUnitType.DUT_FAHRENHEIT;
                case "EU571088": return DisplayUnitType.DUT_CUBIC_FEET;
                case "EU571089": return DisplayUnitType.DUT_SQUARE_FEET;
                case "EU571090": return DisplayUnitType.DUT_SQUARE_INCHES;
                case "EU571097": return DisplayUnitType.DUT_CUBIC_INCHES;
                case "EU571100": return DisplayUnitType.DUT_FEET_PER_MINUTE;
                case "EU571101": return DisplayUnitType.DUT_FEET_PER_SECOND;
                case "EU571116": return DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT;
                case "EU571119": return DisplayUnitType.DUT_POUNDS_MASS_PER_CUBIC_INCH;
                case "EU571126": return DisplayUnitType.DUT_POUNDS_FORCE_PER_FOOT;
                case "EU571133": return DisplayUnitType.DUT_WATTS_PER_SQUARE_FOOT;
                case "EU571137": return DisplayUnitType.DUT_CUBIC_FEET_PER_MINUTE;
                default: return DisplayUnitType.DUT_UNDEFINED;
            }
        }

        public static CADMetadataCriteria GetCADMetadataCriteria(CADProduct cadProduct)
        {
            return cadProduct == null
                ? null
                : new CADMetadataCriteria
                {
                    Identifier = new CADMetadataIdentifier
                    {
                        Application = new CADApplication
                        {
                            Name = "Revit",
                            Version = ApplicationGlobals.GetRevitVersion()
                        },
                        ETIM = new ETIM
                        {
                            ClassCode = cadProduct.EtimClassCode,
                            ClassCodeVersion = cadProduct.EtimClassCodeVersion == null || cadProduct.EtimClassCodeVersion.Equals("0") ? "1" : cadProduct.EtimClassCodeVersion,
                            ModelCode = cadProduct.ModellingClassCode,
                            ModelCodeVersion = cadProduct.ModellingClassCodeVersion == null || cadProduct.ModellingClassCodeVersion.Equals("0") ? "1" : cadProduct.ModellingClassCodeVersion
                        }
                    },
                    Options = new CADMetadataIdentifierOptions
                    {
                        LanguageCode = "nl"
                    }
                };
        }

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static TResult UIThreadSafeAsync<TResult>(Func<TResult> function)
        {
            return Task.Run(function).Result;
        }

        public static bool UpdatePropertiesWithValuesFromFamily(Element loadedElementOrType, IEnumerable<CADProperty> properties, bool setUserDefinedValues = false, bool deleteUnknownProperties = false)
        {
            var hasUserDefinedValues = false;
            var monitoredExecutionContext = ApplicationGlobals.ApplicationContext.GetService<IMonitoredExecutionContext>();
            var parameterHelper = new ParameterHelper();

            using (var monitoredExecutionBlock = monitoredExecutionContext
                .MonitorMethod<UOLAddInUtilities>(nameof(UpdatePropertiesWithValuesFromFamily))
                .WithParameter(nameof(loadedElementOrType), loadedElementOrType)
                .WithParameter(nameof(properties), properties)
                .WithParameter(nameof(setUserDefinedValues), setUserDefinedValues)
                .WithTiming())
            {
                var propertiesToDelete = new List<CADProperty>();

                foreach (var cadProperty in properties)
                {
                    if (setUserDefinedValues || (!setUserDefinedValues && !cadProperty.Selected) || (!setUserDefinedValues && deleteUnknownProperties))
                    {
                        var propertyName = cadProperty.Code.StartsWith("uob_prod_", StringComparison.OrdinalIgnoreCase) ? $"{cadProperty.Code}" : $"{cadProperty.Code}_{cadProperty.PortCode}_";
                        var revitParameter = parameterHelper.GetParameterBySearchString(loadedElementOrType, propertyName, true);

                        if (revitParameter == null || !revitParameter.HasValue)
                        {
                            if (revitParameter == null && deleteUnknownProperties)
                            {
                                propertiesToDelete.Add(cadProperty);
                            }

                            continue;
                        }

                        switch (revitParameter.StorageType)
                        {
                            case StorageType.Double:
                                var etimDisplayUnitType = UOLAddInUtilities.ConvertEUCode(cadProperty.UnitCode);
                                double? parameterDoubleValue = Math.Round(parameterHelper.ConvertFromAPI(etimDisplayUnitType, revitParameter.AsDouble()), 6);

                                if (parameterDoubleValue == 0 && cadProperty.Value.NumericValue == null)
                                {
                                    parameterDoubleValue = cadProperty.Value.NumericValue;
                                }

                                if (setUserDefinedValues && !parameterDoubleValue.Equals(cadProperty.Value.NumericValue))
                                {
                                    cadProperty.UserDefinedValue = new CADPropertyValue { NumericValue = parameterDoubleValue };
                                    cadProperty.Selected = true;
                                    hasUserDefinedValues = true;
                                }
                                else if (!setUserDefinedValues)
                                {
                                    cadProperty.Value.NumericValue = parameterDoubleValue;
                                }

                                break;
                            case StorageType.Integer:
                                if (cadProperty.Type == "L")
                                {
                                    var parameterBooleanValue = Convert.ToBoolean(revitParameter.AsInteger());
                                    if (setUserDefinedValues && !parameterBooleanValue.Equals(cadProperty.Value.LogicalValue))
                                    {
                                        cadProperty.UserDefinedValue = new CADPropertyValue { LogicalValue = parameterBooleanValue };
                                        cadProperty.Selected = true;
                                        hasUserDefinedValues = true;
                                    }
                                    else if (!setUserDefinedValues)
                                    {
                                        cadProperty.Value.LogicalValue = parameterBooleanValue;
                                    }
                                }
                                else
                                {
                                    double? parameterIntegerValue = revitParameter.AsInteger();
                                    if (parameterIntegerValue == 0 && cadProperty.Value.NumericValue == null)
                                    {
                                        parameterIntegerValue = cadProperty.Value.NumericValue;
                                    }

                                    if (setUserDefinedValues && !parameterIntegerValue.Equals(cadProperty.Value.NumericValue))
                                    {
                                        cadProperty.UserDefinedValue = new CADPropertyValue { NumericValue = parameterIntegerValue };
                                        cadProperty.Selected = true;
                                        hasUserDefinedValues = true;
                                    }
                                    else if (!setUserDefinedValues)
                                    {
                                        cadProperty.Value.NumericValue = parameterIntegerValue;
                                    }
                                }

                                break;
                            case StorageType.String:
                                var parameterStringValue = revitParameter.AsString();
                                if (parameterStringValue.StartsWith("EV", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (setUserDefinedValues && !parameterStringValue.Equals(cadProperty.Value.Code))
                                    {
                                        cadProperty.UserDefinedValue = new CADPropertyValue { Code = parameterStringValue, Description = null };
                                        cadProperty.Selected = true;
                                        hasUserDefinedValues = true;
                                    }
                                    else if (!setUserDefinedValues && (string.IsNullOrEmpty(cadProperty.Value.Code) || !cadProperty.Value.Code.Equals(parameterStringValue, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        cadProperty.Value.Code = parameterStringValue;
                                        cadProperty.Value.Description = null;
                                    }
                                }
                                else
                                {
                                    if (setUserDefinedValues && !(parameterStringValue == string.Empty && cadProperty.Value.Description == null) && !parameterStringValue.Equals(cadProperty.Value.Description))
                                    {
                                        cadProperty.UserDefinedValue = new CADPropertyValue { Code = null, Description = string.IsNullOrEmpty(parameterStringValue) ? null : parameterStringValue };
                                        cadProperty.Selected = true;
                                        hasUserDefinedValues = true;
                                    }
                                    else if (!setUserDefinedValues)
                                    {
                                        cadProperty.Value.Code = null;
                                        cadProperty.Value.Description = string.IsNullOrEmpty(parameterStringValue) ? null : parameterStringValue;
                                    }
                                }

                                break;
                            case StorageType.ElementId:
                                var parameterElementIdIntegerValue = revitParameter.AsElementId().IntegerValue;
                                if (setUserDefinedValues && !parameterElementIdIntegerValue.Equals(cadProperty.Value.NumericValue))
                                {
                                    cadProperty.UserDefinedValue = new CADPropertyValue { NumericValue = parameterElementIdIntegerValue };
                                    cadProperty.Selected = true;
                                    hasUserDefinedValues = true;
                                }
                                else if (!setUserDefinedValues)
                                {
                                    cadProperty.Value.NumericValue = parameterElementIdIntegerValue;
                                }

                                break;
                        }
                    }
                }

                if (propertiesToDelete.Count > 0)
                {
                    var filteredProperties = new List<CADProperty>(properties);

                    foreach (var property in propertiesToDelete)
                    {
                        filteredProperties.Remove(property);
                    }

                    properties = filteredProperties;
                }
            }

            return hasUserDefinedValues;
        }
    }
}