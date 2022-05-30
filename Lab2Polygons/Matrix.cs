using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2Polygons
{
    public class Matrix
    {
        public double[,] fields;

        public Matrix()
        {
            fields = new double[3, 3];
        }
        public static Matrix getShiftMatr(double shiftX, double shiftY, double shiftZ)
        {
            Matrix result = new Matrix();
            result.fields[0, 0] = 1;
            result.fields[1, 1] = 1;
            result.fields[2, 2] = 1;
            result.fields[3, 3] = 1;
            result.fields[3, 0] = shiftX;
            result.fields[3, 1] = shiftY;
            result.fields[3, 2] = shiftZ;
            return result;
        }

        public static Matrix getRotateXMatr(double angle)
        {
            Matrix result = new Matrix();

            angle = angle * Math.PI / 180.0;

            result.fields[0, 0] = 1;
            result.fields[1, 1] = Math.Cos(angle);
            result.fields[2, 2] = Math.Cos(angle);
            result.fields[1, 2] = -Math.Sin(angle);
            result.fields[2, 1] = Math.Sin(angle);

            return result;
        }

        public static Matrix getRotateYMatr(double angle)
        {
            Matrix result = new Matrix();

            angle = angle * Math.PI / 180.0;

            result.fields[0, 0] = Math.Cos(angle);
            result.fields[1, 1] = 1;
            result.fields[2, 2] = Math.Cos(angle);
            result.fields[0, 2] = Math.Sin(angle);
            result.fields[2, 0] = -Math.Sin(angle);

            return result;
        }

        public static Matrix getRotateZMatr(double angle)
        {
            Matrix result = new Matrix();

            angle = angle * Math.PI / 180.0;

            result.fields[0, 0] = Math.Cos(angle);
            result.fields[1, 1] = Math.Cos(angle);
            result.fields[2, 2] = 1;
            result.fields[0, 1] = -Math.Sin(angle);
            result.fields[1, 0] = Math.Sin(angle);
            
            return result;
        }

        public Matrix mulMatrs(Matrix m2)
        {
            Matrix result = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        result.fields[i, j] += this.fields[i, k] * m2.fields[k, j];
                    }
                }
            }
            return result;
        }
    }
}
