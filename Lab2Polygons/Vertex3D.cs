using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2Polygons
{
    public class Vertex3D
    {
        //Используем обобщенные координаты с возможностью задать бесконечно удаленные точки
        //(X, Y, Z, W)об = (Xдек, Yдек, Zдек, 1)
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        //вектор камеры
        public Vertex3D cameraPosition;

        public Vertex3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vertex3D(String str)
        {
            string[] coords = str.Split(new char[] { ' ' }, 3);
            X = Double.Parse(coords[0]);
            Y = Double.Parse(coords[1]);
            Z = Double.Parse(coords[2]);
        }

        public double this[int index]
        {
            get {
                switch (index)
                {
                    case 0:
                        return X;
                        break;
                    case 1:
                        return Y;
                        break;
                    case 2:
                        return Z;
                        break;
                    default:
                        return 0;
                        break;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public static double DotProduct(Vertex3D v1, Vertex3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vertex3D CrossProduct(Vertex3D v1, Vertex3D v2)
        {
            return new Vertex3D(v1.Y * v2.Z - v1.Z * v2.Y,
                                v1.Z * v2.X - v1.X * v2.Z,
                                v1.X * v2.Y - v1.Y * v2.X);
        }

        public static Vertex3D operator *(Vertex3D v1, Vertex3D v2)
        {
            double ax, ay, az;
            ax = v1.Y * v2.Z - v1.X * v2.Y;
            ay = v1.X * v2.X - v1.X * v2.Z;
            az = v1.X * v2.Y - v1.Y * v2.X;
            return new Vertex3D(ax, ay, az);
        }

        public static Vertex3D Add(Vertex3D v1, Vertex3D v2)
        {
            return new Vertex3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static Vertex3D Substract(Vertex3D v1, Vertex3D v2)
        {
            return new Vertex3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z); 
        }

        public static Vertex3D Multiply(double k, Vertex3D v)
        {
            return new Vertex3D(k * v.X, k * v.Y, k * v.Z);
        }

        public Vertex3D MultiplyMatr(double[,] matr)
        {
            Vertex3D res = new Vertex3D(0, 0, 0);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    res[i] += this[j] * matr[i, j];
                }
            }

            return res;
        }

        public Vertex3D MultiplyMatr(Matrix matr)
        {
            Vertex3D res = new Vertex3D(0, 0, 0);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    res[i] += this[j] * matr.fields[i, j];
                }
            }

            return res;
        }

        public double GetLength { get => Math.Sqrt(X * X + Y * Y + Z * Z); }


        public double findAngle(Vertex3D v2)
        {
            if (this.GetLength == 0 || v2.GetLength == 0)
                return 0;
            double cos = (X * v2.X + Y * v2.Y + Z * v2.Z) / (this.GetLength * v2.GetLength);
            return Math.Acos(cos);
        }

        

        public void printToFile(StreamWriter str)
        {
            str.Write(X.ToString());
            str.Write(" ");
            str.Write(Y.ToString());
            str.Write(" ");
            str.Write(Z.ToString());
            str.WriteLine();
        }
    }
}
