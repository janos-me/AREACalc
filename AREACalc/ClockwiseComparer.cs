using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AREACalc
{
    public class ClockwiseComparer : IComparer
    {
        public int Compare(object A, object B)
        {
            //  Variables to Store the atans
            double aTanA, aTanB;
            var pointA = (PointF)A;
            var pointB = (PointF)B;
            //  Reference Point
            var reference = Program.center;

            //  Fetch the atans
            aTanA = Math.Atan2(pointA.Y - reference.Y, pointA.X - reference.X);
            aTanB = Math.Atan2(pointB.Y - reference.Y, pointB.X - reference.X);

            //  Determine next point in Clockwise rotation
            if (aTanA < aTanB) return -1;
            else if (aTanA > aTanB) return 1;
            return 0;
        }
    }
}
