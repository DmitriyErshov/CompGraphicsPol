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
        int gradY, gradX, gradZ;
        Vertex3D CameraPosition;
        BoxDrawer boxDrawer;
        int recursionDepth;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            textBoxCameraPosition.Text = "2 2 0";
            CameraPosition = new Vertex3D(textBoxCameraPosition.Text);

            textBoxY.Text = "-10";
            gradY = int.Parse(textBoxY.Text);
            textBoxX.Text = "0";
            gradX = int.Parse(textBoxX.Text);
            textBoxZ.Text = "0";
            gradZ = int.Parse(textBoxZ.Text);

            //recursionDepthComboBox.SelectedIndex = 0;

            //recursionDepth = int.Parse((string)recursionDepthComboBox.SelectedItem);
            recursionDepth = 0;
            boxDrawer = new BoxDrawer(pictureBox.Width, pictureBox.Height, CameraPosition,
                                      gradX, gradY, gradZ, recursionDepth);

            Draw();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            int temp;
            if (int.TryParse(textBoxY.Text, out temp))
            {
                gradY = int.Parse(textBoxY.Text);
            }
            else
            {
                textBoxY.Text = "0";
                gradY = 0;
            }

            if (int.TryParse(textBoxX.Text, out temp))
            {
                gradX = int.Parse(textBoxX.Text);
            }
            else
            {
                textBoxX.Text = "0";
                gradX = 0;
            }

            if (int.TryParse(textBoxZ.Text, out temp))
            {
                gradZ = int.Parse(textBoxZ.Text);
            }
            else
            {
                textBoxZ.Text = "0";
                gradZ = 0;
            }

            CameraPosition = new Vertex3D(textBoxCameraPosition.Text);

            recursionDepth = int.Parse((string)recursionDepthComboBox.SelectedItem);

            Draw();
        }

        

        void Draw()
        {
            boxDrawer.CameraPosition = CameraPosition;
            boxDrawer.gradX = gradX;
            boxDrawer.gradY = gradY;
            boxDrawer.gradZ = gradZ;
            //boxDrawer.RecursionDepth = recursionDepth;

            pictureBox.Image = boxDrawer.Draw();
        }
    }
}
