using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2Polygons
{
    public class Polygon
    {
        public Vertex3D p1, p2, p3;
        public Color color;
        public int specular;
        public double reflective;

        public Polygon(Vertex3D p1, Vertex3D p2, Vertex3D p3, Color color, int specular = -1, double reflective = 0)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.color = color;
            this.specular = specular;
            this.reflective = reflective;
        }

        public Vertex3D Normal
        {
            get { return Vertex3D.CrossProduct(Vertex3D.Substract(p2, p1), Vertex3D.Substract(p3, p1)); }
        }

        public Vertex3D NormalAToCam(Vertex3D cameraPosition)
        {
            Vertex3D n = Vertex3D.CrossProduct(Vertex3D.Substract(p2, p1), Vertex3D.Substract(p3, p1));

            if (Vertex3D.Substract(p1, cameraPosition).findAngle(n) < Math.PI / 2)
            {
                n = Vertex3D.CrossProduct(Vertex3D.Substract(p3, p1), Vertex3D.Substract(p2, p1));
            }

            return n;
        }

        public Vertex3D NormalBToCam(Vertex3D cameraPosition)
        {
            Vertex3D n = Vertex3D.CrossProduct(Vertex3D.Substract(p3, p2), Vertex3D.Substract(p1, p2));

            if (Vertex3D.Substract(p2, cameraPosition).findAngle(n) < Math.PI / 2)
            {
                n = Vertex3D.CrossProduct(Vertex3D.Substract(p1, p2), Vertex3D.Substract(p3, p2));
            }

            return n;
        }

        public Vertex3D NormalCToCam(Vertex3D cameraPosition)
        {
            Vertex3D n = Vertex3D.CrossProduct(Vertex3D.Substract(p1, p3), Vertex3D.Substract(p2, p3));

            if (Vertex3D.Substract(p3, cameraPosition).findAngle(n) < Math.PI / 2)
            {
                n = Vertex3D.CrossProduct(Vertex3D.Substract(p2, p3), Vertex3D.Substract(p1, p3));
            }


            return n;
        }
    }

}
