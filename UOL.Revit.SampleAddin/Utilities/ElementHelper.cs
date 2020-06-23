using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Cadac.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using UOL.Revit.SampleAddin.Utilities;
using UOL.SDK;
using UOL.SDK.Authorization;

namespace UOL.Revit.SampleAddin
{
    internal class ElementHelper
    {
        private readonly IUOLClient uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
        private readonly IMonitoredExecutionContext monitoredExecutionContext = ApplicationGlobals.ApplicationContext.GetService<IMonitoredExecutionContext>();

        public ElementHelper()
        {
            uolClient.Initialize();
            var authorize = (IAuthorizationService)ApplicationGlobals.ApplicationContext.GetService(typeof(IAuthorizationService));
            authorize.GetAccessToken();
        }

        /// <summary>
        /// Place a <paramref name="familySymbol"/> in <paramref name="uiDocument"/>.
        /// </summary>
        /// <param name="uiDocument">Specifies the document to place the family symbol in.</param>
        /// <param name="familySymbol">Specifies the symbol that must be placed.</param>
        public void PlaceFamilyInstance(UIDocument uiDocument, FamilySymbol familySymbol)
        {
            using (var executionBlock = monitoredExecutionContext
                .MonitorMethod<ElementHelper>(nameof(PlaceFamilyInstance))
                .WithParameter(nameof(uiDocument), uiDocument)
                .WithParameter(nameof(familySymbol), familySymbol)
                .WithTiming())
            {
                try
                {
                    uiDocument.PromptForFamilyInstancePlacement(familySymbol);
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                }
                catch (Exception exception)
                {
                    executionBlock.LogException(exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Select elements in the document.
        /// </summary>
        /// <param name="uiDocument">The document to select elements from.</param>
        /// <param name="selectionType">The type of elements to select.</param>
        /// <param name="message">The message to show in the status bar.</param>
        /// <param name="skipPreselection">A value indicating whether to skip preselected elements.</param>
        /// <returns>A list of selected elements.</returns>
        public IList<Element> SelectElements(UIDocument uiDocument, Type selectionType, string message, bool skipPreselection = false)
        {
            using (var executionBlock = monitoredExecutionContext
                .MonitorMethod<ElementHelper>(nameof(SelectElements))
                .WithParameter(nameof(uiDocument), uiDocument)
                .WithParameter(nameof(selectionType), selectionType)
                .WithParameter(nameof(message), message)
                .WithParameter(nameof(skipPreselection), skipPreselection)
                .WithTiming())
            {
                var selectedElementsList = new List<Element>();

                try
                {
                    var preselectedElementIdCollection = skipPreselection ? null : uiDocument.Selection.GetElementIds();
                    if (preselectedElementIdCollection == null || preselectedElementIdCollection.Count == 0)
                    {
                        var selectedObjectReferenceList = uiDocument.Selection.PickObjects(ObjectType.Element, new TypeSelectionFilter(selectionType), message);
                        foreach (var reference in selectedObjectReferenceList)
                        {
                            var element = uiDocument.Document.GetElement(reference);
                            if (element != null)
                            {
                                selectedElementsList.Add(element);
                            }
                        }
                    }
                    else
                    {
                        var filteredElementCollector = new FilteredElementCollector(uiDocument.Document, preselectedElementIdCollection);
                        filteredElementCollector.WherePasses(new ElementClassFilter(selectionType));

                        selectedElementsList = filteredElementCollector.ToList<Element>();
                        if (selectedElementsList.Count == 0)
                        {
                            uiDocument.Selection.SetElementIds(new List<ElementId>());
                            selectedElementsList = SelectElements(uiDocument, selectionType, message, true).ToList();
                        }
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    selectedElementsList.Clear();
                }
                catch (Exception exception)
                {
                    executionBlock.LogException(exception);
                    selectedElementsList.Clear();
                    throw;
                }

                return selectedElementsList;
            }
        }

        public IList<Element> SelectElements(UIDocument uiDocument, Type selectionType, string message, IList<Element> preSelectedElements = null)
        {
            using (var executionBlock = monitoredExecutionContext
                .MonitorMethod<ElementHelper>(nameof(SelectElements))
                .WithParameter(nameof(uiDocument), uiDocument)
                .WithParameter(nameof(selectionType), selectionType)
                .WithParameter(nameof(message), message)
                .WithParameter(nameof(preSelectedElements), preSelectedElements)
                .WithTiming())
            {
                var selectedElementsList = new List<Element>();

                try
                {
                    var preSelectedReferences = new List<Reference>();

                    if (preSelectedElements != null)
                    {
                        preSelectedReferences.AddRange(preSelectedElements.Select(e => new Reference(e)));
                    }

                    var selectedObjectReferenceList = uiDocument.Selection.PickObjects(ObjectType.Element, new TypeSelectionFilter(selectionType), message, preSelectedReferences);
                    foreach (var reference in selectedObjectReferenceList)
                    {
                        var element = uiDocument.Document.GetElement(reference);
                        if (element != null)
                        {
                            selectedElementsList.Add(element);
                        }
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    selectedElementsList.Clear();
                }
                catch (Exception exception)
                {
                    executionBlock.LogException(exception);
                    selectedElementsList.Clear();
                    throw;
                }

                return selectedElementsList;
            }
        }
    }
}
