using System;
using System.Collections.Generic;
using System.Text;

namespace ArcticGL
{
    internal class Solutions
    {
        public static double MaxIn3Num(double value0, double value1, double value2)
        {
            if ((value0 - value1) >= 0 && (value0 - value2) >= 0)
            {
                return value0;
            }
            else if ((value1 - value0) >= 0 && (value1 - value2) >= 0)
            {
                return value1;
            }
            else
            {
                return value2;
            }
        }
        
    }
}
