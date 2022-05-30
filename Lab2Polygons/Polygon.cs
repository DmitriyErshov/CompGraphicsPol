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

            n  =  (1.0 / n.GetLength) * n;

            return n;
        }

        public Vertex3D NormalBToCam(Vertex3D cameraPosition)
        {
            Vertex3D n = Vertex3D.CrossProduct(Vertex3D.Substract(p3, p2), Vertex3D.Substract(p1, p2));

            if (Vertex3D.Substract(p2, cameraPosition).findAngle(n) < Math.PI / 2)
            {
                n = Vertex3D.CrossProduct(Vertex3D.Substract(p1, p2), Vertex3D.Substract(p3, p2));
            }

            n = (1.0 / n.GetLength) * n;

            return n;
        }

        public Vertex3D NormalCToCam(Vertex3D cameraPosition)
        {
            Vertex3D n = Vertex3D.CrossProduct(Vertex3D.Substract(p1, p3), Vertex3D.Substract(p2, p3));

            if (Vertex3D.Substract(p3, cameraPosition).findAngle(n) < Math.PI / 2)
            {
                n = Vertex3D.CrossProduct(Vertex3D.Substract(p2, p3), Vertex3D.Substract(p1, p3));
            }

            n = (1.0 / n.GetLength) * n;

            return n;
        }

        //public double alpha()
        //{
        //    return 0;
        //}

        //public double beta()
        //{
        //    return 0;
        //}

        public double alpha(Vertex3D cameraPosition, Vertex3D q)
        {
            Vertex3D normal = this.NormalAToCam(cameraPosition);

            double result = 0;

            Vertex3D n = (p2 - p1) * (p3 - p1);


            if ((p1 - cameraPosition).findAngle(n) < Math.PI / 2)
            {
                n = (p3 - p1) * (p2 - p1);

                result = Vertex3D.DotProduct((q - p2) * (p3 - p2), normal) /
                         Vertex3D.DotProduct(n, normal);
            }
            else
            {
                result = Vertex3D.DotProduct((p3 - p2) * (q - p2), normal) /
                         Vertex3D.DotProduct(n, normal);
            }

            return result;
        }

        public double beta(Vertex3D cameraPosition, Vertex3D q)
        {
            Vertex3D normal = this.NormalAToCam(cameraPosition);

            double result = 0;

            Vertex3D n = (p2 - p1) * (p3 - p1);


            if ((p1 - cameraPosition).findAngle(n) < Math.PI / 2)
            {
                n = (p3 - p1) * (p2 - p1);

                result = Vertex3D.DotProduct((q - p3) * (p1 - p3), normal) /
                         Vertex3D.DotProduct(n, normal);
            }
            else
            {
                result = Vertex3D.DotProduct((p1 - p3) * (q - p3), normal) /
                         Vertex3D.DotProduct(n, normal);
            }

            return result;
        }

        public double gamma(Vertex3D cameraPosition, Vertex3D q)
            {
                Vertex3D normal = this.NormalAToCam(cameraPosition);

                double result = 0;

                Vertex3D n = (p2 - p1) * (p3 - p1);

           
                if ((p1 - cameraPosition).findAngle(n) < Math.PI / 2)
                {
                    n = (p3 - p1) * (p2 - p1);

                    result = Vertex3D.DotProduct((q - p1) * (p2 - p1), normal) /
                             Vertex3D.DotProduct(n, normal);
                }
                else
                {
                    result = Vertex3D.DotProduct((p2 - p1) * (q - p1), normal) /
                             Vertex3D.DotProduct(n, normal);
                }

                return result;
            }

        }

}
