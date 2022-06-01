using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2Polygons
{
    public class BoxDrawer
    {
        Bitmap bitmap;

        int Cw, Ch;
        int Vw, Vh;
        double d;

        Vertex3D CameraDir;

        public int gradX { get; set; }
        public int gradY { get; set; }
        public int gradZ { get; set; }
        public Vertex3D CameraPosition { get; set; }
        

        List<Polygon> polygons;
        List<Vertex3D> verts;
        List<Light> lights;

        Color backgroundColor;

        int recursionDepth;
        public int RecursionDepth
        {
            get { return recursionDepth; }
            set
            {
                if (value < 0)
                {
                    recursionDepth = 0;
                }
                else
                {
                    recursionDepth = value;
                }
            }
        }

        public BoxDrawer(int width, int height, Vertex3D cameraPosition,
                         int gradX, int gradY, int gradZ, int recursionDepth)
        {
            Cw = width;
            Ch = height;

            Vw = 1;
            Vh = 1;

            bitmap = new Bitmap(Cw, Ch);

            CameraPosition = cameraPosition;
            CameraDir = new Vertex3D(1, 1, 1);
            CameraDir = Vertex3D.Substract(CameraDir, CameraPosition);

            d = 1;

            this.RecursionDepth = recursionDepth;
            backgroundColor = Color.Black;

            polygons = new List<Polygon>();
            verts = new List<Vertex3D>();




            polygons.Add(new Polygon(new Vertex3D(5, 0, 8),
                                     new Vertex3D(-5, 0, 8),
                                     new Vertex3D(0, 0, -2),
                                     Color.Aquamarine, 500, 0.2));


            AddBox(new Vertex3D(0, 0, 4), 1, 2, 1, Color.Green, 700, 0.2);
            AddBox(new Vertex3D(2, 0, 4), 1, 1, 1, Color.Blue, 100, 0.4);


            lights = new List<Light>();
            lights.Add(new Light(LightType.Ambient, 0.2));


            Light lightDir = new Light(LightType.Directional, 0.8);
            lightDir.Direction = new Vertex3D(1, 1, 1);
            lights.Add(lightDir);
            Light lightPoint = new Light(LightType.Point, 0.6);
            lightPoint.Position = new Vertex3D(1, 1, 1);
            lights.Add(lightPoint);
        }


        void PutPixel(int x, int y, Color color)
        {
            int sx = Cw / 2 + x;
            int sy = Ch / 2 - y;

            if (!(sx < 0 || sx >= Cw || sy < 0 || sy >= Ch))
            {
                bitmap.SetPixel(sx, sy, color);
            }
        }

        Vertex3D CanvasToViewport(int x, int y)
        {
            return new Vertex3D((double)x * Vw / Cw, (double)y * Vh / Ch, d);
        }

        Color MultiplyColor(Color color, double k)
        {
            int R = (int)(color.R * k);
            if (R < 0)
            {
                R = 0;
            }
            else if (R > 255)
            {
                R = 255;
            }

            int G = (int)(color.G * k);
            if (G < 0)
            {
                G = 0;
            }
            else if (G > 255)
            {
                G = 255;
            }

            int B = (int)(color.B * k);
            if (B < 0)
            {
                B = 0;
            }
            else if (B > 255)
            {
                B = 255;
            }

            return Color.FromArgb(R, G, B);
        }


        void AddBox(Vertex3D startPoint, double ax, double ay, double az, Color color, int specular = -1, double reflective = 0)
        {
            
            int first = verts.Count;

            verts.Add(new Vertex3D(0, 0, az) + startPoint);
            verts.Add(new Vertex3D(0, ay, az) + startPoint);
            verts.Add(new Vertex3D(ax, ay, az) + startPoint);
            verts.Add(new Vertex3D(ax, 0, az) + startPoint);
            verts.Add(new Vertex3D(0, 0, 0) + startPoint);
            verts.Add(new Vertex3D(0, ay, 0) + startPoint);
            verts.Add(new Vertex3D(ax, ay, 0) + startPoint);
            verts.Add(new Vertex3D(ax, 0, 0) + startPoint);

            polygons.Add(new Polygon(verts[5 + first], verts[4 + first], verts[0 + first], color, specular, reflective));
            polygons.Add(new Polygon(verts[0 + first], verts[1 + first], verts[5 + first], color, specular, reflective));

            polygons.Add(new Polygon(verts[0 + first], verts[4 + first], verts[7 + first], color, specular, reflective));
            polygons.Add(new Polygon(verts[7 + first], verts[3 + first], verts[0 + first], color, specular, reflective));

            polygons.Add(new Polygon(verts[7 + first], verts[4 + first], verts[5 + first], color, specular, reflective));
            polygons.Add(new Polygon(verts[7 + first], verts[5 + first], verts[6 + first], color, specular, reflective));

            polygons.Add(new Polygon(verts[2 + first], verts[1 + first], verts[0 + first], color, specular, reflective)); ;
            polygons.Add(new Polygon(verts[0 + first], verts[3 + first], verts[2 + first], color, specular, reflective));

            polygons.Add(new Polygon(verts[2 + first], verts[3 + first], verts[7 + first], color, specular, reflective));
            polygons.Add(new Polygon(verts[2 + first], verts[7 + first], verts[6 + first], color, specular, reflective));

            polygons.Add(new Polygon(verts[1 + first], verts[2 + first], verts[5 + first], color, specular, reflective));
            polygons.Add(new Polygon(verts[2 + first], verts[6 + first], verts[5 + first], color, specular, reflective));

        }
        double ComputeLighting(Vertex3D point, Vertex3D normal, Vertex3D V, int s)
        {
            double intensity = 0;
            double length = normal.GetLength;

            double tMax = 0;

            foreach (Light light in lights)
            {
                if (light.Type == LightType.Ambient)
                {
                    intensity += light.Intensity;
                }
                else
                {
                    Vertex3D vec_l = null;
                    switch (light.Type)
                    {
                        case LightType.Point:
                            vec_l = light.Position - point;
                            tMax = 1;
                            break;
                        case LightType.Directional:
                            vec_l = light.Direction;
                            tMax = double.MaxValue;
                            break;
                        default:
                            break;
                    }

                    //shadows
                    double closest_t = 0;
                    Polygon shadowPolygon = null;
                    ClosestIntersection(point, vec_l, 0.001, tMax, ref shadowPolygon, ref closest_t);
                    if (shadowPolygon != null)
                    {
                        continue;
                    }

                    //diffuse
                    double n_dot_l = Vertex3D.DotProduct(normal, vec_l);
                    if (n_dot_l > 0)
                    {
                        intensity += light.Intensity * n_dot_l / (length * vec_l.GetLength);
                    }

                    //specular
                    if (s != -1)
                    {
                        Vertex3D R = (2.0 * Vertex3D.DotProduct(normal, vec_l)) * normal;
                        R = Vertex3D.Substract(R, vec_l);
                        double r_dot_v = Vertex3D.DotProduct(R, V);
                        if (r_dot_v > 0)
                        {
                            intensity += light.Intensity * Math.Pow(r_dot_v / (R.GetLength * V.GetLength), s);
                        }
                    }
                }

            }

            return intensity;
        }

        //переписать 
        double IntersectRayPol(Vertex3D O, Vertex3D D, Polygon polygon)
        {
            Vertex3D N = Vertex3D.CrossProduct(Vertex3D.Substract(polygon.p2, polygon.p1), Vertex3D.Substract(polygon.p3, polygon.p1));
            N = (1.0 / N.GetLength) * N;

            double d = Vertex3D.DotProduct(polygon.p1, N);

            double t = double.MaxValue;

            double nd = Vertex3D.DotProduct(N, D);

            if (nd != 0)
            {
                t = (d - Vertex3D.DotProduct(N, O)) / nd;

                //пересечение
                Vertex3D Q = O + (t * D);

                bool first = Vertex3D.DotProduct((polygon.p2 - polygon.p1) * (Q - polygon.p1), N) >= 0;
                bool second = Vertex3D.DotProduct((polygon.p3 - polygon.p2) * (Q - polygon.p2), N) >= 0;
                bool third = Vertex3D.DotProduct((polygon.p1 - polygon.p3) * (Q - polygon.p3), N) >= 0;

                if (!(first && second && third))
                {
                    t = double.MaxValue;
                }
            }


            return t;
        }

        public void ClosestIntersection(Vertex3D O, Vertex3D D, double t_min, double t_max, ref Polygon closestPolygon, ref double closestT)
        {
            closestT = double.MaxValue;
            closestPolygon = null;

            foreach (Polygon polygon in polygons)
            {
                double ts = IntersectRayPol(O, D, polygon);

                if (t_min < ts && ts < t_max && ts < closestT)
                {
                    closestT = ts;
                    closestPolygon = polygon;
                }
            }
        }

        Vertex3D ReflectRay(Vertex3D R, Vertex3D N)
        {
            return 2.0 * Vertex3D.DotProduct(N, R) * N - R;
        }

        Color TraceRay(Vertex3D O, Vertex3D D, double t_min, double t_max, int recursion_depth)
        {
            double closest_t = 0;
            Polygon closest_pol = null;

            ClosestIntersection(O, D, t_min, t_max, ref closest_pol, ref closest_t);


            if (closest_pol == null)
            {
                return backgroundColor;
            }

            Vertex3D point = O + (closest_t * D);

            Vertex3D polNormal = closest_pol.NormalAToCam(CameraPosition);

            double alpha = closest_pol.alpha(CameraPosition, point);
            double beta = closest_pol.beta(CameraPosition, point);
            double gamma = closest_pol.gamma(CameraPosition, point);

            Vertex3D na = closest_pol.NormalAToCam(CameraPosition);
            Vertex3D nb = closest_pol.NormalBToCam(CameraPosition);
            Vertex3D nc = closest_pol.NormalCToCam(CameraPosition);

            Vertex3D nq = (alpha * na) + (beta * nb) + (gamma * nc);
            nq = (1.0 / nq.GetLength) * nq;

            double intensity = ComputeLighting(point, nq, -1 * D, closest_pol.specular);

            Color localColor = MultiplyColor(closest_pol.color, intensity);

            double r = closest_pol.reflective;
            if (recursion_depth <= 0 || r <= 0)
            {
                return localColor;
            }

            Vertex3D R = ReflectRay(-1 * D, nq);
            Color reflectedColor = TraceRay(point, R, 0.001, double.MaxValue, recursion_depth - 1);

            localColor = MultiplyColor(localColor, (1 - r));
            reflectedColor = MultiplyColor(reflectedColor, r);

            return Color.FromArgb(localColor.R + reflectedColor.R, localColor.G + reflectedColor.G, localColor.B + reflectedColor.B);


            //return localColor;
        }

        public Bitmap Draw()
        {
            for (int x = -Cw / 2; x < Cw / 2; x++)
            {
                for (int y = -Ch / 2; y < Ch / 2; y++)
                {
                    //определить область viewport которая соответствует данному пикселю
                    Vertex3D D = CanvasToViewport(x, y);

                    //D = D.MultiplyMatr(calcRotateYMatr(gradY));

                    //определить цвет
                    D = D.MultiplyMatr(Matrix.getRotateYMatr(gradY));
                    D = D.MultiplyMatr(Matrix.getRotateXMatr(gradX));
                    D = D.MultiplyMatr(Matrix.getRotateZMatr(gradZ));

                    Color color = TraceRay(CameraPosition, D, 1, double.MaxValue, recursionDepth);

                    PutPixel(x, y, color);
                }
            }


            return bitmap;
        }
    }
}



//polygons.Add(new Polygon(new Vertex3D(0, 0, 4),
//                         new Vertex3D(1, 0, 4),
//                         new Vertex3D(0, 1, 4),
//                         Color.Green, 500, 0.2));

//polygons.Add(new Polygon(new Vertex3D(1, 1, 4),
//                         new Vertex3D(1, 0, 4),
//                         new Vertex3D(0, 1, 4),
//                         Color.Green, 500, 0.2));

//polygons.Add(new Polygon(new Vertex3D(1, 1, 4),
//                         new Vertex3D(1, 0, 4),
//                         new Vertex3D(1, 1, 5),
//                         Color.Green, 500, 0.2));

//polygons.Add(new Polygon(new Vertex3D(1, 1, 5),
//                         new Vertex3D(1, 0, 5),
//                         new Vertex3D(1, 0, 4),
//                         Color.Green, 500, 0.2));

//polygons.Add(new Polygon(new Vertex3D(0, 1, 4),
//                         new Vertex3D(1, 1, 4),
//                         new Vertex3D(1, 1, 5),
//                         Color.Green, 500, 0.2));

//polygons.Add(new Polygon(new Vertex3D(0, 1, 4),
//                         new Vertex3D(1, 1, 5),
//                         new Vertex3D(0, 1, 5),
//                         Color.Green, 500, 0.2));

////polygons.Add(new Polygon(new Vertex3D(0, 0, 4),
////                         new Vertex3D(0, 1, 4),
////                         new Vertex3D(0, 1, 5),
////                         Color.Green, 500, 0.2));

////polygons.Add(new Polygon(new Vertex3D(0, 0, 4),
////                         new Vertex3D(0, 0, 5),
////                         new Vertex3D(0, 1, 5),
////                         Color.Green, 500, 0.2));