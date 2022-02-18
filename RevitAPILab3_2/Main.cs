using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPILab3_2
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipesFilter(), "Выберите трубы");
            var elementList = new List<Wall>();

            double length = 0;

            foreach (var selectedElement in selectedElementRefList)
            {
                Pipe pipe = doc.GetElement(selectedElement) as Pipe;
                Parameter lenghtPar = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                if (lenghtPar.StorageType == StorageType.Double)
                {
                    length += lenghtPar.AsDouble();
                }
            }

            double lengthMeters = UnitUtils.ConvertFromInternalUnits(length, DisplayUnitType.DUT_METERS);

            TaskDialog.Show("Длина выбранных труб", lengthMeters.ToString());

            return Result.Succeeded;
        }
    }
}
