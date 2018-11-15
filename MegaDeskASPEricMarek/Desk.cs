using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MegaDeskASPEricMarek
{
    public struct Desk
    {
        //properties
        public float Width;
        public float Depth;
        public int NumOfDraws;
        public float SurfaceArea;

        //method for sending Width and Depth
        public Tuple<float, float> GetMeasurements()
        {
            var Measurements = Tuple.Create(Width, Depth);
            return Measurements;
        }

    }
}