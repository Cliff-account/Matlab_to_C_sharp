using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace İTERASYONLU_SERBEST_DENGELEME
{
    public partial class Form2 : Form
    {

        private double[] X1;
        private double[] Y1;
        private int[] NN;
        private int[] DN;
        private int[] BN;
        private string locationYol;


        public Form2(double[] X1, double[] Y1, int[] NN, int[] DN, int[] BN, string locationYol)
        {
            InitializeComponent();

            this.X1 = X1;
            this.Y1 = Y1;
            this.NN = NN;
            this.DN = DN;
            this.BN = BN;
            this.locationYol = locationYol;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DrawGraph(X1, Y1, NN, DN, BN, 1.0f);

        }

        // çizdirme 1
        private void DrawGraph(double[] X1, double[] Y1, int[] NN, int[] DN, int[] BN, float zoomFactor)
        {
            double enkucukX = X1.Min();
            double enkucukY = Y1.Min();


            for (int i=0; i<X1.Length; i++)
            {
                X1[i] = ((float)X1[i] - enkucukX) *zoomFactor+50;
                Y1[i] = ((float)Y1[i] - enkucukY) *zoomFactor+50;
            }

            double enbuyukX = X1.Max();
            double enbuyukY = Y1.Max();

            Bitmap bitmap = new Bitmap((int)enbuyukX+100, (int)enbuyukY+100);    // bitmap size
            this.Size = new Size((int)enbuyukX + 100, (int)enbuyukY + 100);   // form size

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White); // SİL VE ARKAYI BEYAZ YAP
                Font titleFont = new Font("Arial", 10, FontStyle.Bold);
                g.DrawString("DOĞRULTU-KENAR AĞI", titleFont, Brushes.Black, new PointF(10, 10));


                for (int i = 0; i < X1.Length; i++)
                {
                    // Noktanın koordinatları
                    float x = (float)X1[i]-10;
                    float y = (float)Y1[i]-10;

                    // Nokta numarası ve üçgen simgesi metni
                    string pointText = $" N.{NN[i]}";
                    string triangleSymbol = "▲"; // Üçgen simgesi Unicode karakteri

                    // Nokta metnini ve üçgen simgesini birleştirme
                    string combinedText = $"{triangleSymbol}{pointText} ";

                    // Metni çizim yapma noktasına yerleştirme
                    g.DrawString(combinedText, new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new PointF(x, y));
                }


                for (int i = 0; i < DN.Length; i++)
                {
                    int Durulan = Array.IndexOf(NN, DN[i]);
                    int Bakilan = Array.IndexOf(NN, BN[i]);
                    float x1 = (float)X1[Durulan] ;
                    float y1 = (float)Y1[Durulan] ;
                    float x2 = (float)X1[Bakilan] ;
                    float y2 = (float)Y1[Bakilan] ;
                    g.DrawLine(Pens.Black, x1, y1, x2, y2);
                }
            }
                
            // PictureBox'a resmi ekleme
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.BackColor = Color.White;
            // bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            // bitmap.RotateFlip(RotateFlipType.Rotate90FlipY);
          
            pictureBox.Image = bitmap;
            Controls.Add(pictureBox);
            bitmap.Save(locationYol+"DOĞRULTU KENAR AĞI.png", System.Drawing.Imaging.ImageFormat.Png); //locationYol
        }
    }
}
