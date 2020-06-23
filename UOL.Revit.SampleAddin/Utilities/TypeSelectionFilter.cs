using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace UOL.Revit.SampleAddin.Utilities
{
    internal class TypeSelectionFilter : ISelectionFilter
    {
        private readonly Type type;

        public TypeSelectionFilter(Type type)
        {
            this.type = type;
        }

        public bool AllowElement(Element element)
        {
            return type.IsInstanceOfType(element);
        }

        public bool AllowReference(Reference reference, XYZ xyz)
        {
            return true;
        }
    }
}
