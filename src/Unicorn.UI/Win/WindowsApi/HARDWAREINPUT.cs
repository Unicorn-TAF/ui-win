using System;

namespace Unicorn.UI.Win.WindowsApi
{
    internal struct HARDWAREINPUT
    {
        internal UInt32 Msg;
        internal UInt16 ParamL;
        internal UInt16 ParamH;

        internal HARDWAREINPUT(UInt32 msg, UInt16 paramL, UInt16 paramH)
        {
            Msg = msg;
            ParamL = paramL;
            ParamH = paramH;
        }
    }
}
