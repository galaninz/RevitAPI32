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

namespace RevitAPI32
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, "Выберите трубы");

            double info = 0;

            for (int i = 0; i < selectedElementRefList.Count; i++)
            {
                var selectedElement = doc.GetElement(selectedElementRefList.ElementAt(i));
                if (selectedElement is Pipe)
                {
                    Parameter lengthParameter = selectedElement.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    if (lengthParameter.StorageType == StorageType.Double)
                    {
                        double lengthValue = UnitUtils.ConvertToInternalUnits(lengthParameter.AsDouble(), UnitTypeId.Meters);
                        info += lengthParameter.AsDouble();
                        
                    }
                }
            }
            
            TaskDialog.Show("Length", info.ToString());

            return Result.Succeeded;
        }
    }
}
