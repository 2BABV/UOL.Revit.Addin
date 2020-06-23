using Autodesk.Revit.DB;
using Cadac.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UOL.Models;
using UOL.Revit.SampleAddin.Extensions;
using UOL.Revit.SampleAddin.Models;
using UOL.Revit.SampleAddin.Utilities;
using UOL.SDK;

namespace UOL.Revit.SampleAddin
{
    internal class RevitHost : ICADHost
    {
        private readonly IMonitoredExecutionContext monitoredExecutionContext = ApplicationGlobals.ApplicationContext.GetService<IMonitoredExecutionContext>();
        private readonly FamilyHelper familyHelper = new FamilyHelper();
        private readonly ParameterHelper parameterHelper = new ParameterHelper();
        private Dictionary<string, DisplayUnitType> typeCatelogDisplayUnitTypeDictionary = null;

        public CADHostResult InsertCadContent(string contentPath, string name, IEnumerable<CADProperty> properties, bool placeInstance, CADMetadata cadMetadata)
        {
            using (var monitoredExecutionBlock = monitoredExecutionContext
                .MonitorMethod<RevitHost>(nameof(InsertCadContent))
                .WithParameter(nameof(contentPath), contentPath)
                .WithParameter(nameof(name), name)
                .WithParameter(nameof(properties), properties)
                .WithParameter(nameof(placeInstance), placeInstance)
                .WithTiming())
            {
                var parameters = new Dictionary<string, ParameterInfo>();
                try
                {
                    parameters = ConvertProperties(properties);
                }
                catch
                {
                    MessageBox.Show(Properties.Resources.MessageBoxCadPropertiesIncomplete_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new CADHostResult { State = CADHostResultState.Failed };
                }

                ElementType loadedElementType;
                using (var transaction = new Transaction(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document, "Insert Family"))
                {
                    transaction.Start();

                    var typeDataList = new List<TypeData>()
                    {
                        new TypeData()
                        {
                            Path = contentPath,
                            Parameters = new Dictionary<string, ParameterInfo>(),
                            TypeName = name
                        }
                    };

                    loadedElementType = familyHelper.LoadTypes(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document, typeDataList);

                    SetDefaultParameterValues(loadedElementType, properties, cadMetadata);

                    foreach (var parameter in parameters)
                    {
                        var revitParameter = parameterHelper.GetParameterBySearchString(loadedElementType, parameter.Value.Name);

                        if (revitParameter == null)
                        {
                            revitParameter = parameterHelper.GetParameterBySearchString(loadedElementType, parameter.Value.Name.Replace("_0_", "_"));
                        }

                        parameterHelper.SetParameterValue(revitParameter, parameter.Value.Value);
                    }

                    transaction.Commit();
                }

                if (!loadedElementType.IsValidObject)
                {
                    return new CADHostResult { State = CADHostResultState.Failed };
                }

                UOLAddInUtilities.UpdatePropertiesWithValuesFromFamily(loadedElementType, properties, false);

                if (placeInstance)
                {
                    try
                    {
                        if ((FamilySymbol)loadedElementType != null && ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.ActiveView.ViewType != ViewType.Elevation && ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.ActiveView.ViewType != ViewType.Section)
                        {
                            new ElementHelper().PlaceFamilyInstance(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument, (FamilySymbol)loadedElementType);
                        }
                        else
                        {
                            MessageBox.Show(Properties.Resources.MessageBoxCadContentLoadedCantPlace_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Autodesk.Revit.Exceptions.InvalidOperationException invalidOperationException)
                    {
                        monitoredExecutionBlock.LogException(invalidOperationException);
                        MessageBox.Show(Properties.Resources.MessageBoxCadContentLoadedCantPlace_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return new CADHostResult { CADObject = loadedElementType, State = CADHostResultState.Succeeded };
                    }
                }

                return new CADHostResult { CADObject = loadedElementType, State = CADHostResultState.Succeeded };
            }
        }

        public CADHostResult SetPropertyValue(object cadObject, string propertyName, object value)
        {
            using (var monitoredExecutionBlock = monitoredExecutionContext
                .MonitorMethod<RevitHost>(nameof(SetPropertyValue))
                .WithParameter(nameof(cadObject), cadObject)
                .WithParameter(nameof(propertyName), propertyName)
                .WithParameter(nameof(value), value)
                .WithTiming())
            {
                var element = cadObject as Element;

                var result = parameterHelper.SetParameterValue(element, propertyName, value);

                return result == true ?
                    new CADHostResult { CADObject = cadObject, State = CADHostResultState.Succeeded } :
                    new CADHostResult { CADObject = cadObject, State = CADHostResultState.Failed };
            }
        }

        public CADHostResult WriteAdditionalData(object cadObject, string data)
        {
            using (var monitoredExecutionBlock = monitoredExecutionContext
                .MonitorMethod<RevitHost>(nameof(WriteAdditionalData))
                .WithParameter(nameof(cadObject), cadObject)
                .WithParameter(nameof(data), data)
                .WithTiming())
            {
                try
                {
                    var element = cadObject as Element;

                    var path = element.Document.PathName;

                    if (!string.IsNullOrEmpty(path))
                    {
                        File.WriteAllText(path.Replace(".rvt", $"_{element.Id.IntegerValue.ToString()}.met"), data);
                    }

                    return new CADHostResult { CADObject = cadObject, State = CADHostResultState.Succeeded };
                }
                catch (System.Exception exception)
                {
                    monitoredExecutionBlock.LogException(exception);
                    return new CADHostResult { CADObject = cadObject, State = CADHostResultState.Failed };
                }
            }
        }

        private Dictionary<string, ParameterInfo> ConvertProperties(IEnumerable<CADProperty> properties)
        {
            using (var monitoredExecutionBlock = monitoredExecutionContext
                .MonitorMethod<RevitHost>(nameof(ConvertProperties))
                .WithParameter(nameof(properties), properties)
                .WithTiming())
            {
                var parameters = new Dictionary<string, ParameterInfo>();
                var type = Models.ParameterType.Type;
                var valueType = ParameterValueType.String;

                foreach (var property in properties)
                {
                    if (property.Info.TryGetValue("InjectionType", out var typeValue))
                    {
                        type = typeValue.ToEnum(Models.ParameterType.Type);
                    }

                    object value = null;
                    if (property.Selected && (property.Value.LogicalValue != null || property.Value.NumericValue != null || property.Value.Description != null))
                    {
                        switch (property.Type)
                        {
                            case "N":
                                valueType = ParameterValueType.Numeric;
                                var etimDisplayUnitType = UOLAddInUtilities.ConvertEUCode(property.UnitCode);
                                value = parameterHelper.ConvertToAPI((double)property.Value.NumericValue, etimDisplayUnitType);
                                break;
                            case "L":
                                valueType = ParameterValueType.Boolean;
                                value = property.Value.LogicalValue;
                                break;
                            default:
                                value = property.Value.Code ?? property.Value.Description;
                                valueType = ParameterValueType.String;
                                break;
                        }

                        var propertyName = $"{property.Code}_{property.PortCode}_";

                        if (!parameters.ContainsKey(propertyName))
                        {
                            parameters.Add(propertyName, new ParameterInfo() { Name = propertyName, Type = type, Value = value, ValueType = valueType });
                        }
                    }
                }

                return parameters;
            }
        }

        private Dictionary<string, DisplayUnitType> GetDisplayUnitTypeDictionary()
        {
            if (typeCatelogDisplayUnitTypeDictionary == null)
            {
                typeCatelogDisplayUnitTypeDictionary = new Dictionary<string, DisplayUnitType>();
                foreach (var displayUnitType in (DisplayUnitType[])Enum.GetValues(typeof(DisplayUnitType)))
                {
                    var typeCatalogString = string.Empty;
                    try
                    {
                        typeCatalogString = UnitUtils.GetTypeCatalogString(displayUnitType);
                    }
                    catch
                    {
                    }

                    if (!string.IsNullOrEmpty(typeCatalogString))
                    {
                        typeCatelogDisplayUnitTypeDictionary.Add(typeCatalogString, displayUnitType);
                    }
                }
            }

            return typeCatelogDisplayUnitTypeDictionary;
        }

        private void SetDefaultParameterValues(ElementType loadedElementType, IEnumerable<CADProperty> properties, CADMetadata cadMetadata)
        {
            using (var monitoredExecutionBlock = monitoredExecutionContext
                .MonitorMethod<RevitHost>(nameof(SetDefaultParameterValues))
                .WithParameter(nameof(loadedElementType), loadedElementType)
                .WithParameter(nameof(properties), properties)
                .WithParameter(nameof(cadMetadata), cadMetadata)
                .WithTiming())
            {
                if (!string.IsNullOrEmpty(cadMetadata?.ParameterInfo))
                {
                    var parameterInfoList = JsonConvert.DeserializeObject<List<CADObjectParameter>>(cadMetadata.ParameterInfo);

                    var parameters = loadedElementType.GetOrderedParameters().Where(p => p.Definition.Name.StartsWith("EC_EF") || p.Definition.Name.StartsWith("MC_EF") || p.Definition.Name.StartsWith("uob_prod_"));

                    foreach (Parameter parameter in loadedElementType.Parameters)
                    {
                        if (parameter != null && parameter.HasValue && (parameter.Definition.Name.StartsWith("EC_EF") || parameter.Definition.Name.StartsWith("MC_EF") || parameter.Definition.Name.StartsWith("uob_prod_")))
                        {
                            var cadObjectParameter = parameterInfoList.FirstOrDefault(parameterInfo => parameterInfo.Name.Equals(parameter.Definition.Name));
                            if (cadObjectParameter != null && !string.IsNullOrEmpty(cadObjectParameter.Value))
                            {
                                switch (cadObjectParameter.ValueType)
                                {
                                    case "N":
                                        if (GetDisplayUnitTypeDictionary().ContainsKey(cadObjectParameter.Unit))
                                        {
                                            var displayUnitType = GetDisplayUnitTypeDictionary()[cadObjectParameter.Unit];
                                            var valueString = cadObjectParameter.Value.Replace(".", ",");

                                            double.TryParse(valueString, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("nl-NL"), out var doubleValue);
                                            var value = parameterHelper.ConvertToAPI(doubleValue, displayUnitType);

                                            parameterHelper.SetParameterValue(parameter, value);
                                        }
                                        else
                                        {
                                            parameterHelper.SetParameterValue(parameter, string.Empty);
                                        }

                                        break;
                                    default:
                                        parameterHelper.SetParameterValue(parameter, cadObjectParameter.Value);
                                        break;
                                }
                            }
                            else
                            {
                                if (!parameter.Definition.Name.ToLower().Contains("uob_prod_") ||
                                    (parameter.Definition.Name.StartsWith("uob_prod_", StringComparison.OrdinalIgnoreCase) &&
                                    properties.FirstOrDefault(cadProperty => parameter.Definition.Name.StartsWith(cadProperty.Code, StringComparison.OrdinalIgnoreCase)) != null))
                                {
                                    parameterHelper.SetParameterValue(parameter, string.Empty);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}