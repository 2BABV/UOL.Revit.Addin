using Autodesk.Revit.DB;
using Cadac.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UOL.Revit.SampleAddin.Models;

namespace UOL.Revit.SampleAddin.Utilities
{
    internal class FamilyHelper
    {
        private readonly IMonitoredExecutionContext monitoredExecutionContext = ApplicationGlobals.ApplicationContext.GetService<IMonitoredExecutionContext>();
        private readonly ParameterHelper parameterHelper = new ParameterHelper();

        public IList<Family> GetFamilies(Document document)
        {
            using (var executionBlock = monitoredExecutionContext
                .MonitorMethod<FamilyHelper>(nameof(GetFamilies))
                .WithParameter(nameof(document), document)
                .WithTiming())
            {
                try
                {
                    return new FilteredElementCollector(document)
                    .WherePasses(new ElementClassFilter(typeof(Family)))
                    .Cast<Family>()
                    .ToList();
                }
                catch (Exception exception)
                {
                    executionBlock.LogException(exception);
                    return null;
                }
            }
        }

        public ElementType LoadTypes(Document document, IEnumerable<TypeData> typeInfo)
        {
            ElementType elementType = null;
            using (var executionBlock = monitoredExecutionContext
                .MonitorMethod<FamilyHelper>(nameof(LoadTypes))
                .WithParameter(nameof(document), document)
                .WithParameter(nameof(typeInfo), typeInfo)
                .WithTiming())
            {
                var parameterNames = new List<string>();

                if (typeInfo == null || typeInfo.Count() == 0)
                {
                    executionBlock.LogException(new ArgumentNullException(nameof(typeInfo)));
                    return elementType;
                }

                try
                {
                    Family family = null;

                    try
                    {
                        var families = GetFamilies(document).ToList();
                        ElementType familySymbol = null;

                        foreach (var typeData in typeInfo)
                        {
                            var name = Path.GetFileName(typeData.Path);

                            var familyName = Path.GetFileNameWithoutExtension(typeData.Path);

                            family = families.FirstOrDefault(f => f.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase));
                            if (family == null)
                            {
                                document.LoadFamily(typeData.Path, new FamilyLoadOption(), out family);
                            }

                            if (family == null)
                            {
                                continue;
                            }

                            var types = new Dictionary<string, KeyValuePair<string, string>>();

                            var familySymbols = family.GetFamilySymbolIds().ToList();

                            familySymbol = SetType(document, typeData, familySymbols, parameterNames);
                        }

                        if (familySymbol != null)
                        {
                            elementType = familySymbol;
                        }
                    }
                    catch (Exception exception)
                    {
                        executionBlock.LogException(exception);
                    }
                }
                catch (Exception exception)
                {
                    executionBlock.LogException(exception);
                }
            }

            return elementType;
        }
        private ElementId GetElmentIdFromName(Document document, string name)
        {
            using (var executionBlock = monitoredExecutionContext
                .MonitorMethod<FamilyManager>(nameof(GetElmentIdFromName))
                .WithParameter(nameof(document), document)
                .WithParameter(nameof(name), name)
                .WithTiming())
            {
                try
                {
                    var elements = new FilteredElementCollector(document)
                    .WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Materials))
                    .ToList();

                    var element = elements.FirstOrDefault(e => e.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                    return element != null ? element.Id : new ElementId(-1);
                }
                catch (Exception exception)
                {
                    executionBlock.LogException(exception);
                    return new ElementId(-1);
                }
            }
        }

        private ElementType SetType(Document document, TypeData typeData, List<ElementId> familySymbols, List<string> parameternames)
        {
            ElementType familySymbol = null;
            using (var executionBlock = monitoredExecutionContext
                 .MonitorMethod<FamilyHelper>(nameof(SetType))
                 .WithParameter(nameof(document), document)
                 .WithParameter(nameof(familySymbols), familySymbols)
                 .WithParameter(nameof(parameternames), parameternames)
                 .WithTiming())
            {
                try
                {
                    Element element = null;

                    foreach (var fs in familySymbols)
                    {
                        element = document.GetElement(fs);
                        if (element.Name.Equals(typeData.TypeName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            familySymbol = (FamilySymbol)element;
                            break;
                        }
                    }

                    if (element == null)
                    {
                        return null;
                    }

                    if (!element.Name.Equals(typeData.TypeName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        familySymbol = ((FamilySymbol)element).Duplicate(typeData.TypeName);
                    }

                    foreach (var parameter in typeData.Parameters)
                    {
                        if (parameter.Value.Type == Models.ParameterType.Type)
                        {
                            var foundParameter = parameterHelper.GetParameter(familySymbol, parameter.Key);

                            if (foundParameter == null)
                            {
                                parameternames.Add(parameter.Key);
                                continue;
                            }

                            var value = string.Empty;

                            switch (foundParameter.StorageType)
                            {
                                case StorageType.Double:
                                case StorageType.Integer:
                                    if (double.TryParse(parameter.Value.Value.ToString(), out var dval))
                                    {
                                        value = dval.ToString();
                                    }

                                    if (string.IsNullOrEmpty(value))
                                    {
                                        value = "0";
                                    }

                                    if (value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        value = "1";
                                    }

                                    if (value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        value = "0";
                                    }

                                    parameterHelper.SetParameterValue(familySymbol, parameter.Key, value);
                                    break;
                                case StorageType.ElementId:
                                    parameterHelper.SetParameterValue(familySymbol, parameter.Key, GetElmentIdFromName(document, parameter.Value.Value.ToString()));
                                    break;
                                default:
                                    parameterHelper.SetParameterValue(familySymbol, parameter.Key, parameter.Value.Value);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    executionBlock.LogException(exception);
                }
            }

            return familySymbol;
        }
    }
}
