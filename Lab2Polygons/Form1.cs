using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2Polygons
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;

        int Cw, Ch;
        int Vw, Vh;
        double d;

        Vertex3D CameraPosition;
        Vertex3D CameraDir;
        Matrix toRotate;

        int gradY, gradX, gradZ;

        List<Polygon> polygons;
        List<Light> lights;

        Color backgroundColor;


        StreamWriter str;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cw = pictureBox.Width;
            Ch = pictureBox.Height;

            Vw = 1;
            Vh = 1;

            bitmap = new Bitmap(Cw, Ch);

            textBoxCameraPosition.Text = "2 2 0";
            CameraPosition = new Vertex3D(textBoxCameraPosition.Text);
            CameraDir = new Vertex3D(1, 1, 1);
            CameraDir = Vertex3D.Substract(CameraDir, CameraPosition);

            d = 1;

            backgroundColor = Color.Black;

            polygons = new List<Polygon>();
            //polygons.Add(new Polygon(new Vertex3D(0, -1, 4),
            //                         new Vertex3D(2, 0, 4),
            //                         new Vertex3D(-2, 0, 4),
            //                         Color.Green));

            polygons.Add(new Polygon(new Vertex3D(0, 0, 4),
                                     new Vertex3D(1, 0, 4),
                                     new Vertex3D(0, 1, 4),
                                     Color.Green, 500, 0.2));

            polygons.Add(new Polygon(new Vertex3D(1, 1, 4),
                                     new Vertex3D(1, 0, 4),
                                     new Vertex3D(0, 1, 4),
                                     Color.Green, 500, 0.2));

            polygons.Add(new Polygon(new Vertex3D(1, 1, 4),
                                     new Vertex3D(1, 0, 4),
                                     new Vertex3D(1, 1, 5),
                                     Color.Green, 500, 0.2));

            polygons.Add(new Polygon(new Vertex3D(1, 1, 5),
                                     new Vertex3D(1, 0, 5),
                                     new Vertex3D(1, 0, 4),
                                     Color.Green, 500, 0.2));

            polygons.Add(new Polygon(new Vertex3D(0, 1, 4),
                                     new Vertex3D(1, 1, 4),
                                     new Vertex3D(1, 1, 5),
                                     Color.Green, 500, 0.2));

            polygons.Add(new Polygon(new Vertex3D(0, 1, 4),
                                     new Vertex3D(1, 1, 5),
                                     new Vertex3D(0, 1, 5),
                                     Color.Green, 500, 0.2));



            //polygons.Add(new Polygon(new Vertex3D(0, 0, 5),
            //                         new Vertex3D(1, 0, 5),
            //                         new Vertex3D(0, 1, 5),
            //                         Color.Green));

            //polygons.Add(new Polygon(new Vertex3D(1, 1, 5),
            //                         new Vertex3D(1, 0, 5),
            //                         new Vertex3D(0, 1, 5),
            //                         Color.Green));

            //polygons.Add(new Polygon(new Vertex3D(1, 1, 4),
            //                         new Vertex3D(1, 0, 4),
            //                         new Vertex3D(1, 0, 5),
            //                         Color.Green));

            polygons.Add(new Polygon(new Vertex3D(5, 0, 6),
                                     new Vertex3D(-5, 0, 6),
                                     new Vertex3D(0, 0, 0),
                                     Color.Yellow, 500, 0.2));

            lights = new List<Light>();
            lights.Add(new Light(LightType.Ambient, 0.2));



            Light lightDir = new Light(LightType.Directional, 0.8);
            lightDir.Direction = new Vertex3D(1, 1, 1);
            lights.Add(lightDir);           
            Light lightPoint = new Light(LightType.Point, 0.6);
            lightPoint.Position = new Vertex3D(1, 1, 1);
            lights.Add(lightPoint);

            textBoxY.Text = "-10";
            gradY = int.Parse(textBoxY.Text);
            textBoxX.Text = "0";
            gradX = int.Parse(textBoxX.Text);
            textBoxZ.Text = "0";
            gradZ = int.Parse(textBoxZ.Text);

            str = new StreamWriter("output.txt");

            Draw();
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
                            vec_l = Vertex3D.Substract(light.Position, point);
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
                        Vertex3D R = Vertex3D.Multiply(2.0 * Vertex3D.DotProduct(normal, vec_l), normal);
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

            N = Vertex3D.Multiply(1.0 / N.GetLength, N);

            double d = Vertex3D.DotProduct(polygon.p1, N);

            double t = double.MaxValue;

            double nd = Vertex3D.DotProduct(N, D);

            if (nd != 0)
            {
                t = (d - Vertex3D.DotProduct(N, O)) / nd;

                //пересечение
                Vertex3D Q = Vertex3D.Add(O, Vertex3D.Multiply(t, D));
               // Q.printToFile(str);

                bool first = Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(polygon.p2, polygon.p1), Vertex3D.Substract(Q, polygon.p1)), N) >= 0;
                bool second = Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(polygon.p3, polygon.p2), Vertex3D.Substract(Q, polygon.p2)), N) >= 0;
                bool third = Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(polygon.p1, polygon.p3), Vertex3D.Substract(Q, polygon.p3)), N) >= 0;

                if (!(first && second && third))
                {
                    t = double.MaxValue;
                }
            }

            
            return t;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CameraPosition = new Vertex3D(textBoxCameraPosition.Text);

            int temp;
            if (int.TryParse(textBoxY.Text, out temp))
            {
                gradY = int.Parse(textBoxY.Text);
            }
            else
            {
                gradY = 0;
            }

            if (int.TryParse(textBoxX.Text, out temp))
            {
                gradX = int.Parse(textBoxX.Text);
            }
            else
            {
                gradX = 0;
            }

            if (int.TryParse(textBoxZ.Text, out temp))
            {
                gradZ = int.Parse(textBoxZ.Text);
            }
            else
            {
                gradZ = 0;
            }

            Draw();
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
            return Vertex3D.Substract(Vertex3D.Multiply(2.0 * Vertex3D.DotProduct(N, R), N), R);
        }

        Color TraceRay(Vertex3D O, Vertex3D D, double t_min, double t_max, int recursion_depth)
        {
            double closest_t = 0;
            Polygon closest_pol = null;

            ClosestIntersection(O, D, t_min, t_max, ref closest_pol, ref closest_t);

            //str.WriteLine(closest_t.ToString());

            if (closest_pol == null)
            {
                return backgroundColor;
            }

            Vertex3D point = Vertex3D.Add(O, Vertex3D.Multiply(closest_t, D));
            Vertex3D polNormal = closest_pol.NormalAToCam(CameraPosition);
            polNormal = Vertex3D.Multiply(1.0 / polNormal.GetLength, polNormal);

            double alpha = Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(closest_pol.p3, closest_pol.p2), Vertex3D.Substract(point, closest_pol.p2)), polNormal) /
                           Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(closest_pol.p2, closest_pol.p1), Vertex3D.Substract(closest_pol.p3, closest_pol.p1)), polNormal);

            double beta = Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(closest_pol.p1, closest_pol.p3), Vertex3D.Substract(point, closest_pol.p3)), polNormal) /
                           Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(closest_pol.p2, closest_pol.p1), Vertex3D.Substract(closest_pol.p3, closest_pol.p1)), polNormal);

            double gamma = Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(closest_pol.p2, closest_pol.p1), Vertex3D.Substract(point, closest_pol.p1)), polNormal) /
                           Vertex3D.DotProduct(Vertex3D.CrossProduct(Vertex3D.Substract(closest_pol.p2, closest_pol.p1), Vertex3D.Substract(closest_pol.p3, closest_pol.p1)), polNormal);

            Vertex3D na = closest_pol.NormalAToCam(CameraPosition);
            na = Vertex3D.Multiply(1.0 / na.GetLength, na);
            Vertex3D nb = closest_pol.NormalBToCam(CameraPosition);
            nb = Vertex3D.Multiply(1.0 / nb.GetLength, nb);
            Vertex3D nc = closest_pol.NormalCToCam(CameraPosition);
            nc = Vertex3D.Multiply(1.0 / nc.GetLength, nc);

            Vertex3D nq = Vertex3D.Add(Vertex3D.Multiply(alpha, na), Vertex3D.Multiply(beta, nb));
            nq = Vertex3D.Add(nq, Vertex3D.Multiply(gamma, nc));

            nq = Vertex3D.Multiply(1.0 / nq.GetLength, nq);

            double intensity = ComputeLighting(point, nq, Vertex3D.Multiply(-1, D), closest_pol.specular);

            Color localColor = MultiplyColor(closest_pol.color, intensity);

            double r = closest_pol.reflective;
            if (recursion_depth <= 0 || r <= 0)
            {
                return localColor;
            }

            Vertex3D R = ReflectRay(Vertex3D.Multiply(-1, D), nq);
            Color reflectedColor = TraceRay(point, R, 0.001, double.MaxValue, recursion_depth - 1);

            localColor = MultiplyColor(localColor, (1 - r));
            reflectedColor = MultiplyColor(reflectedColor, r);

            return Color.FromArgb(localColor.R + reflectedColor.R, localColor.G + reflectedColor.G, localColor.B + reflectedColor.B);


            //return localColor;
        }

        void Draw()
        {
            int recursion_depth = 3;


            for (int x = -Cw / 2; x < Cw / 2; x++)
            {
                for (int y = -Ch / 2; y < Ch / 2; y++)
                {
                    //определить область viewport которая соответствует данному пикселю
                    Vertex3D D = CanvasToViewport(x, y);
                    //D.printToFile(str);

                    //D = D.MultiplyMatr(calcRotateYMatr(gradY));
                    //D = D.MultiplyMatr(Matrix.getRotateYMatr(gradY));
                    //D = D.MultiplyMatr(Matrix.getRotateXMatr(gradX));
                    //D = D.MultiplyMatr(Matrix.getRotateZMatr(gradZ));

                    //определить цвет
                    D = D.MultiplyMatr(Matrix.getRotateYMatr(gradY));
                    D = D.MultiplyMatr(Matrix.getRotateXMatr(gradX));
                    D = D.MultiplyMatr(Matrix.getRotateZMatr(gradZ));

                    Color color = TraceRay(CameraPosition, D, 1, double.MaxValue, recursion_depth);

                    PutPixel(x, y, color);
                }
            }


            pictureBox.Image = bitmap;
        }
    }
}
