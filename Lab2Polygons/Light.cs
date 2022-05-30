using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2Polygons
{
    public enum LightType
    {
        Ambient,
        Point,
        Directional
    }
    public class Light
    {
        public Light (LightType type, double intensity)
        {
            Type = type;
            Intensity = intensity;
        }

        public LightType Type { get; private set; }
        public double Intensity { get; set; }
        public Vertex3D Position { get; set; }
        public Vertex3D Direction { get; set; }
    }
}
