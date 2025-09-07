using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNworking.Models.ObjectBase
{
    interface IObjectBase
    {
        void SetDefaultValue(ref object model);
        //C242_ErrorItemNotify_New SetDefaultValue();
    }
}
