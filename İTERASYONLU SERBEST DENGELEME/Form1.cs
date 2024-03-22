using CsvHelper.Configuration;
using CsvHelper;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.Xml;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Diagnostics;

namespace İTERASYONLU_SERBEST_DENGELEME
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public string dogrultu_yol;
        public string kenar_yol;
        public string koord_yol;
        public string Location_yol;
        public string excel_yol;


        // DOĞRULTU DOSYASI
        public void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();   // OpenFileDialog oluştur
            openFileDialog.Filter = "CSV Dosyaları|*.csv";         // Sadece CSV dosyalarını filtrele
            openFileDialog.Title = "CSV Dosyası Seç";             // Dialog penceresinin başlığını ayarla
            DialogResult result = openFileDialog.ShowDialog();   // Dosya seçim penceresini göster ve kullanıcının seçim yapmasını bekle
            if (result == DialogResult.OK)                      // Kullanıcı bir dosya seçtiyse ve işlemi onayladıysa
            {
                string selectedFilePath = openFileDialog.FileName; // Seçilen dosyanın yolunu al
                MessageBox.Show("Seçilen dosya yolu: \n\n " + selectedFilePath);
                dogrultu_yol = selectedFilePath;
            }
        }




        // KENAR DOSYASI
        public void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();   // OpenFileDialog oluştur
            openFileDialog.Filter = "CSV Dosyaları|*.csv";         // Sadece CSV dosyalarını filtrele
            openFileDialog.Title = "CSV Dosyası Seç";             // Dialog penceresinin başlığını ayarla
            DialogResult result = openFileDialog.ShowDialog();   // Dosya seçim penceresini göster ve kullanıcının seçim yapmasını bekle
            if (result == DialogResult.OK)                      // Kullanıcı bir dosya seçtiyse ve işlemi onayladıysa
            {
                string selectedFilePath = openFileDialog.FileName; // Seçilen dosyanın yolunu al
                MessageBox.Show("Seçilen dosya yolu: \n\n " + selectedFilePath);
                kenar_yol = selectedFilePath;
            }
        }



        // LOCATİON
        public void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog(); // Klasör seçme işlemi için FolderBrowserDialog oluştur
            folderBrowserDialog.Description = "Klasör Seç";                     // Dialog penceresinin başlığını ayarla
            DialogResult result = folderBrowserDialog.ShowDialog();            // Kullanıcı bir klasör seçene kadar işlemi beklet
            if (result == DialogResult.OK)                                    // Kullanıcı bir klasör seçtiyse ve işlemi onayladıysa
            {
                // Seçilen klasörün yolunu al
                string selectedFolderPath = folderBrowserDialog.SelectedPath;

                MessageBox.Show("Seçilen klasör yolu: \n\n " + selectedFolderPath);
                Location_yol = selectedFolderPath + @"\"+"Sonuçlar"+@"\";
                textBox1.Text = Location_yol;
                if (!Directory.Exists(Location_yol))
                {
                    // Klasör yoksa oluşturma
                    Directory.CreateDirectory(Location_yol);
                    Console.WriteLine("Klasör oluşturuldu.");
                }
                else
                {
                    Console.WriteLine("Klasör zaten mevcut.");
                }
            }
        }
    

        // KOORD DOSYASI
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();   // OpenFileDialog oluştur
            openFileDialog.Filter = "CSV Dosyaları|*.csv";         // Sadece CSV dosyalarını filtrele
            openFileDialog.Title = "CSV Dosyası Seç";             // Dialog penceresinin başlığını ayarla
            DialogResult result = openFileDialog.ShowDialog();   // Dosya seçim penceresini göster ve kullanıcının seçim yapmasını bekle
            if (result == DialogResult.OK)                      // Kullanıcı bir dosya seçtiyse ve işlemi onayladıysa
            {
                string selectedFilePath = openFileDialog.FileName; // Seçilen dosyanın yolunu al
                MessageBox.Show("Seçilen dosya yolu: \n\n " + selectedFilePath);
                koord_yol = selectedFilePath;
            }
        }








       // EXCEL
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Dosyaları|*.xlsx;*.xls"; // Yalnızca Excel dosyalarını seçmek için filtreleme
            openFileDialog.Title = "Excel Dosyası Seç"; // Açılacak pencerenin başlığı

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK) // Eğer kullanıcı bir dosya seçerse
            {
                string selectedFilePath = openFileDialog.FileName;
                excel_yol = selectedFilePath;
                MessageBox.Show($"Seçilen dosya: {selectedFilePath}", "Dosya Seçildi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Dosyayı işleme devam edebilirsiniz...
            }
            else
            {
                MessageBox.Show("Dosya seçilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // CSV-EXCEL BUTONU
        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Text == "Excel")
                button7.Text = "CSV";
            else if (button7.Text == "CSV")
                button7.Text = "Excel";
        }






        // PROGRAM ÇALIŞTIRMA
        public void button1_Click(object sender, EventArgs e)
        {

            //string yol = "C:\\Users\\zubey\\OneDrive\\Masaüstü\\Proje_test\\Kaydedilen_veriler\\CCC\\";
            string yol = Location_yol;
            string yol_veri = excel_yol;

            int[] DN;
            int[] BN;
            double[] DOD;
            double[] Pvek_d;
            int[] DNK;
            int[] BNK;
            double[] KENAR;
            double[] Pvek_k;
            int[] NN;
            double[] Y1;
            double[] X1;
            int nk;
            int nd;
            int u;
            int m;

            if (button7.Text == "Excel")
            {

                DN = ReadExcelColumn_int(yol_veri, 0, 1);
                BN = ReadExcelColumn_int(yol_veri, 0, 2);
                DOD = ReadExcelColumn_double(yol_veri, 0, 3);
                Pvek_d = ReadExcelColumn_double(yol_veri, 0, 8);
                DNK = ReadExcelColumn_int(yol_veri, 1, 1);
                BNK = ReadExcelColumn_int(yol_veri, 1, 2);
                KENAR = ReadExcelColumn_double(yol_veri, 1, 3);
                Pvek_k = ReadExcelColumn_double(yol_veri, 1, 8);
                NN = ReadExcelColumn_int(yol_veri, 2, 1);
                Y1 = ReadExcelColumn_double(yol_veri, 2, 2);
                X1 = ReadExcelColumn_double(yol_veri, 2, 3);


                nk = GetSheetRowCount(yol_veri, 1);
                m = GetSheetColumnCount(yol_veri, 0);
                nd = GetSheetRowCount(yol_veri, 0);
                u = NN.Length;
            }
            else // "CSV"
            {
                string csvFilePath_dogrultu = dogrultu_yol;
                string csvFilePath_kenar = kenar_yol;
                string csvFilePath_koord = koord_yol;

                // CSV dosyasından sadece sütunu çekme
                var records1 = ReadCsvColumn1<int>(csvFilePath_dogrultu, 0);         // DN
                var records2 = ReadCsvColumn1<int>(csvFilePath_dogrultu, 1);        // BN
                var records3 = ReadCsvColumn(csvFilePath_dogrultu, 2);             // DOD 
                var records4 = ReadCsvColumn(csvFilePath_dogrultu, 7);            // Pvek_d
                var records5 = ReadCsvColumn1<int>(csvFilePath_kenar, 0);        // DNK
                var records6 = ReadCsvColumn1<int>(csvFilePath_kenar, 1);       // BNK
                var records7 = ReadCsvColumn(csvFilePath_kenar, 2);            // KENAR
                var records8 = ReadCsvColumn(csvFilePath_kenar, 7);           // Pvek_k
                var records9 = ReadCsvColumn1<int>(csvFilePath_koord, 0);    // NN
                var records10 = ReadCsvColumn(csvFilePath_koord, 1);        // Y1
                var records11 = ReadCsvColumn(csvFilePath_koord, 2);       // X1


                DN = records1.ToArray();
                BN = records2.ToArray();
                DOD = records3.ToArray();
                Pvek_d = records4.ToArray();
                DNK = records5.ToArray();
                BNK = records6.ToArray();
                KENAR = records7.ToArray();
                Pvek_k = records8.ToArray();
                NN = records9.ToArray();
                Y1 = records10.ToArray();
                X1 = records11.ToArray();




                nk = GetRowCount(csvFilePath_kenar);                  // nk
                m = GetColumnCount(csvFilePath_dogrultu);            // m
                nd = GetRowCount(csvFilePath_dogrultu);             // nd
                m = GetColumnCount(csvFilePath_dogrultu);          // m
                u = NN.Length;                                    // [u,~]=size(NN);
            }

            


            double top_y1 = Y1.Sum();                           // toplam X1
            double top_x1 = X1.Sum();                          // toplam Y1
            double mean_y1 = Y1.Average();                    // ortalama X1
            double mean_x1 = X1.Average();                   // ortalama Y1
            double[] koor_ort1X = new double[NN.Length];    // %1.per koor-ort X
            double[] koor_ort1Y = new double[NN.Length];   // %1.per koor-ort Y

            for (int i = 0; i < NN.Length; i++)
            {
                koor_ort1X[i] = X1[i] - mean_x1;
            }

            for (int i = 0; i < NN.Length; i++)
            {
                koor_ort1Y[i] = Y1[i] - mean_y1;
            }



            //////////////////////////////////////////////////////////
            ////////  YÖNELTME BİLİNMEYENİ SAYISININ HESABI  /////////
            //////////////////////////////////////////////////////////

            List<int> BNS_list = new List<int>();
            int aa = 0;
            int sayac = 1;

            for (int i = 0; i < nd - 1; i++)
            {
                if (DN[i + 1] == DN[i])
                {
                    sayac = sayac + 1;
                }
                else
                {
                    BNS_list.Insert(aa, sayac);
                    sayac = 1;
                    aa = aa + 1;
                }
            }
            BNS_list.Insert(aa, sayac);
            int[] BNS = BNS_list.ToArray();

            int YBS = aa + 1;      // YÖNELTME BİLİNMEYENİ SAYISI
            int BS = 2 * u + YBS; // BİLİNMEYEN SAYISI

            ///////////////////////////////////////////////////////////////
            //////  DOĞRULTU İÇİN SEMT VE MESAFELERİN HESAPLANMASI  ///////
            ///////////////////////////////////////////////////////////////

            int uzunluk = NN.Max();

            double[] kx1 = new double[uzunluk];
            double[] ky1 = new double[uzunluk];

            for (int j = 0; j < NN.Length; j++)
            {
                kx1[NN[j] - 1] = X1[j];
                ky1[NN[j] - 1] = Y1[j];
            }


            double[] dx = new double[nd];
            double[] dy = new double[nd];
            double[] kenar = new double[nd];
            double[] alfa = new double[nd];
            double[] Semt = new double[nd];

            for (int k = 0; k < nd; k++)
            {
                dx[k] = kx1[BN[k] - 1] - kx1[DN[k] - 1];
                dy[k] = ky1[BN[k] - 1] - ky1[DN[k] - 1];
                kenar[k] = Math.Sqrt(dx[k] * dx[k] + dy[k] * dy[k]);
                alfa[k] = Math.Atan(dy[k] / dx[k]);
                alfa[k] = alfa[k] * 200 / Math.PI;
                if (dy[k] > 0 && dx[k] > 0)
                    Semt[k] = alfa[k];
                else if (dy[k] > 0 && dx[k] < 0)
                    Semt[k] = alfa[k] + 200;
                else if (dy[k] < 0 && dx[k] < 0)
                    Semt[k] = alfa[k] + 200;
                else if (dy[k] < 0 && dx[k] > 0)
                    Semt[k] = alfa[k] + 400;

            }

            // Semt=Semt';
            // Kenar = Kenar';


            ////////////////////////////////////////     
            ///// YÖNELTME BİLİNMEYENİ HESABI  /////
            ////////////////////////////////////////  

            double[] zy = new double[nd];
            double[] sabit = new double[BNS.Sum()];
            double[] z = new double[BNS.Sum()];
            double[] z0 = new double[YBS];

            for (int i = 0; i < nd; i++)
            {

                zy[i] = Semt[i] - DOD[i];
            }

            int sayac2 = 0;

            for (aa = 0; aa < YBS; aa++)
            {
                double sayac1 = 0;
                double k1 = BNS[aa];
                double k = k1 + sayac2;
                for (int i = sayac2; i < k; i++)
                {

                    z[i] = Semt[i] - DOD[i];
                    if (z[i] < 0)
                    {
                        z[i] = z[i] + 400;
                    }
                    sayac1 = sayac1 + z[i];
                    sayac2 = sayac2 + 1;

                }

                z0[aa] = sayac1 / BNS[aa];
            }
            sayac2 = 0;
            for (aa = 0; aa < YBS; aa++)
            {
                double k1 = BNS[aa];
                double k = k1 + sayac2;
                for (int i = sayac2; i < k; i++)
                {

                    sabit[i] = z[i] - z0[aa];
                    sabit[i] = -1 * (sabit[i] * 10000);
                    sayac2 = sayac2 + 1;
                }
            }


            double ro = 200 / Math.PI;
            double ros = ro * 10000;


            ///////////////////////////////////////////////////////////////////////
            ///////   DOĞRULTU İÇİN AİK ve BİK KATSAYILARIN HESAPLANMASI   ////////
            ///////////////////////////////////////////////////////////////////////


            double[] Aik = new double[nd];
            double[] Bik = new double[nd];

            for (int p = 0; p < nd; p++)
            {

                Semt[p] = Semt[p] / ro;
                Aik[p] = -Math.Sin(Semt[p]) * ros / (kenar[p] * 100);   // cc/cm
                Bik[p] = Math.Cos(Semt[p]) * ros / (kenar[p] * 100);   // cc/cm
            }

            ////////////////////////////////////////////
            /////// DOĞRULTU KATSAYILAR MATRİSİ ////////
            ////////////////////////////////////////////

            double[,] Katsayi = new double[nd, u * 2];   // Katsayi=zeros(nd,u*2); // sıfırları otomatik atıyo

            for (int i = 0; i < nd; i++)
            {
                int i1 = 1 + Array.IndexOf(NN, DN[i]);   //  i1=find(ismember(NN,DN(i)));
                int i2 = 1 + Array.IndexOf(NN, BN[i]);  //  i2=find(ismember(NN,BN(i)));

                Katsayi[i, (2 * i1) - 2] = -1 * Aik[i];     // Katsayi(i,2*i1-1)=-Aik(i);
                Katsayi[i, (2 * i1) - 1] = -1 * Bik[i];    // Katsayi(i,2*i1)=-Bik(i);
                Katsayi[i, (2 * i2) - 2] = Aik[i];        // Katsayi(i,2*i2-1)=Aik(i);
                Katsayi[i, (2 * i2) - 1] = Bik[i];       // Katsayi(i,2*i2) = Bik(i);
            }

            ///////////////////////////////////////////////////////////////////////////////
            ///////  DOĞRULTU KATSAYILAR MATRİSİ İÇİN YÖNELTME BİLİNMEYENİ DENKLEMİ  //////
            ///////////////////////////////////////////////////////////////////////////////

            double[,] AIK = new double[nd, u * 2];
            double[,] takat = new double[BNS.Length, u * 2];
            double s;
            int jl;
            int nz;
            int il = 0;
            for (int k = 0; k < YBS; k++)
            {
                nz = BNS[k];
                jl = il;
                for (int j = 0; j < 2 * u; j++)
                {
                    s = 0;
                    for (int i = 0; i < nz; i++)
                    {
                        s = s + Katsayi[jl, j];
                        takat[k, j] = s / nz;
                        jl = jl + 1;
                    }
                    jl = il;
                    for (int i = 0; i < nz; i++)
                    {
                        AIK[jl, j] = Katsayi[jl, j] - takat[k, j];
                        jl = jl + 1;
                    }
                    jl = il;
                }
                il = il + nz;
            }


            ///////////////////////////////////////////////////////////////////////////
            /////////  DOĞRULTU L VEKTÖRÜ İÇİN YÖNELTME BİLİNMEYENİ DENKLEMİ  /////////
            ///////////////////////////////////////////////////////////////////////////


            double[] tsabit = new double[YBS];
            double[] sabiti = new double[BNS.Sum()];

            il = 0;
            for (int k = 0; k < YBS; k++)
            {

                nz = BNS[k];
                jl = il;
                s = 0.0;

                for (int i = 0; i < nz; i++)
                {

                    s = s + sabit[jl];
                    tsabit[k] = s / nz;
                    jl = jl + 1;
                }
                jl = il;
                for (int i = 0; i < nz; i++)
                {

                    sabiti[jl] = sabit[jl] - tsabit[k];
                    jl = jl + 1;
                }
                il = il + nz;
            }

            //////////////////////////////////////////////////////////////////
            //////  KENAR İÇİN AİKK ve BİKK KATSAYILARIN HESAPLANMASI  ///////
            //////////////////////////////////////////////////////////////////
            ///
            double[] Aikk = new double[nd];
            double[] Bikk = new double[nd];
            double[] dxk = new double[nd];
            double[] dyk = new double[nd];
            double[] Kenar1 = new double[nd];
            double[] Semt1 = new double[nd];
            double[] alfa_ = new double[nd];

            for (int i = 0; i < nk; i++)
            {

                dxk[i] = kx1[BNK[i] - 1] - kx1[DNK[i] - 1];
                dyk[i] = ky1[BNK[i] - 1] - ky1[DNK[i] - 1];
                Kenar1[i] = Math.Sqrt(dxk[i] * dxk[i] + dyk[i] * dyk[i]);
                alfa_[i] = Math.Atan(dyk[i] / dxk[i]);
                alfa_[i] = alfa_[i] * (200 / Math.PI);

                if (dyk[i] > 0 && dxk[i] > 0)
                    Semt1[i] = alfa_[i];
                else if (dyk[i] > 0 && dxk[i] < 0)
                    Semt1[i] = alfa_[i] + 200;
                else if (dyk[i] < 0 && dxk[i] < 0)
                    Semt1[i] = alfa_[i] + 200;
                else if (dyk[i] < 0 && dx[i] > 0)
                    Semt1[i] = alfa_[i] + 400;

                Aikk[i] = dxk[i] / Kenar1[i];
                Bikk[i] = dyk[i] / Kenar1[i];

            }
            // Semt= Semt'
            // Kenar = Kenar1'


            ///////////////////////////////////////////////////////////
            /////  FARKLARIN HESAPLANMASI (l DEĞERLERİ)--KENAR-- //////
            ///////////////////////////////////////////////////////////

            double[] lk1 = new double[nk];
            for (int i = 0; i < nk; i++)
            {
                lk1[i] = (KENAR[i] - Kenar1[i]) * 100;  // lk1(p)=(KENAR(p)-Kenar(p))*100;
            }
            double[] kenar_yazdırma_için = Kenar1.ToArray();


            ///////////////////////////////////////////////////
            //////   KENARLAR İÇİN KATSAYILAR MATRİSİ  ////////
            ///////////////////////////////////////////////////

            double[,] KatsayiK = new double[nk, u * 2];

            for (int i = 0; i < nk; i++)
            {
                int i1 = 1 + Array.IndexOf(NN, DNK[i]);
                int i2 = 1 + Array.IndexOf(NN, BNK[i]);

                KatsayiK[i, (2 * i1) - 2] = -1 * Aikk[i];
                KatsayiK[i, (2 * i1) - 1] = -1 * Bikk[i];
                KatsayiK[i, (2 * i2) - 2] = Aikk[i];
                KatsayiK[i, (2 * i2) - 1] = Bikk[i];
            }

            /////////////////////////////////////////////////////////////
            ///////   DOĞRULTU VE KENAR MATRİSLERİNİ BİRLEŞTİRME  ///////
            /////////////////////////////////////////////////////////////

            int ndk = nd + nk;
            int d = 3;

            double[,] AMAT = new double[ndk, u * 2];   // AMAT=[AIK;KatsayiK];

            for (int i = 0; i < nd; i++)
                for (int j = 0; j < u * 2; j++)
                    AMAT[i, j] = AIK[i, j];
            for (int i = nd; i < ndk; i++)
                for (int j = 0; j < u * 2; j++)
                    AMAT[i, j] = KatsayiK[i - nd, j];



            double[] LMAT = new double[ndk];   // LMAT=[sabiti;lk1];
            for (int i = 0; i < nd; i++)
                LMAT[i] = sabiti[i];
            for (int i = nd; i < ndk; i++)
                LMAT[i] = lk1[i - nd];



            double[] AGIRLIK = new double[ndk];     // AGIRLIK=[Pvek_d;Pvek_k];
            for (int i = 0; i < nd; i++)
                AGIRLIK[i] = Pvek_d[i];
            for (int i = nd; i < ndk; i++)
                AGIRLIK[i] = Pvek_k[i - nd];




            double[,] AGIRLIK_diag = new double[ndk, ndk];
            for (int i = 0; i < ndk; i++)
                for (int j = 0; j < ndk; j++)
                    if (i == j)
                        AGIRLIK_diag[i, j] = AGIRLIK[i];



            ///////////////////////////////////////
            //////////// DENGELEME ////////////////
            ///////////////////////////////////////


            double[,] AMAT_Transpose = TransposeMatrix(AMAT);                                                   // AMAT' 
            double[,] cevap1 = MultiplyMatrices(MultiplyMatrices(AMAT_Transpose, AGIRLIK_diag), AMAT);         // AMAT'*AGIRLIK*AMAT
            double[] cevap2 = MultiplyMatrixAndVector(MultiplyMatrices(AMAT_Transpose, AGIRLIK_diag), LMAT);  // AMAT'*AGIRLIK*LMAT
            double[,] deneme = CalculatePseudoInverse(cevap1);                                               // pinv(AMAT'*AGIRLIK*AMAT)
            double[] xbilinmeyen_old = MultiplyMatrixAndVector(deneme, cevap2);                              // xbilinmeyen_old=pinv(AMAT'*AGIRLIK*AMAT)*(AMAT'*AGIRLIK*LMAT)


            int f = ndk - BS + d;                                                                 // SERBEST DERECESİ 
            double[] V = SubtractVectors(MultiplyMatrixAndVector(AMAT, xbilinmeyen_old), LMAT);  // V=(AMAT*xbilinmeyen_old)-LMAT;  - DÜZELTMELERİN HESABI
            double ABC = MultiplyVectors(MultiplyMatrixAndVector(AGIRLIK_diag, V), V);           // V'*AGIRLIK*V 
            double M0 = Math.Sqrt(ABC / f);                                                    // M0=sqrt((V'*AGIRLIK*V)/(f))       - BİRİM ÖLÇÜNÜN KARASEL ORTALAMA HATASI


            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            double alfa1 = 0.95;
            double tdist = MathNet.Numerics.Distributions.StudentT.InvCDF(0, 1, f, alfa1);// tdist = tinv(alfa1, f);  // double tdist = 1.699127026533497; (yapamadım)


            double[,] QVV = SubtractMatrices(CalculatePseudoInverse(AGIRLIK_diag), MultiplyMatrices(MultiplyMatrices(AMAT, deneme), AMAT_Transpose));  // QVV=pinv(AGIRLIK)-(AMAT*pinv(AMAT'*AGIRLIK*AMAT)*AMAT');
            // QVV=pinv(AGIRLIK)-(AMAT*pinv(AMAT'*AGIRLIK*AMAT)*AMAT');                                                                                                                                                                                                                                                                                

            double[,] sqrt_QVV = matriskarekok(QVV);                         // sqrt(QVV)
            double[] diag_sqrt_QVV = DiagMatrix(sqrt_QVV);                  // diag(sqrt(QVV))
            double[] cevap7 = MultiplyVectorByScalar(diag_sqrt_QVV, M0);   // M0.*diag(sqrt(QVV)

            double[,] sqrt_AGIRLIK = matriskarekok(AGIRLIK_diag);                  // sqrt(AGIRLIK) 
            double[] sqrt_AGIRLIK_diag = DiagMatrix(sqrt_AGIRLIK);                // diag(sqrt(AGIRLIK))
            double[] cevap8 = MultiplyVectorByScalar(sqrt_AGIRLIK_diag, tdist);  // diag(sqrt(AGIRLIK))*tdist

            double[] c_sinir = ElementwiseMultiplyVectors(cevap7, cevap8); //  M0.*diag(sqrt(QVV)).*diag(sqrt(AGIRLIK))*tdist



            double sum_c_sinir = c_sinir.Sum();               // sum(c_sinir) 
            double c_sinir1 = sum_c_sinir / (nd + nk);       // c_sinir1=sum(c_sinir)/(nd+nk); 

            ///////////////////////////////////////////
            ///////////    DENGELEME //////////////////
            ///////////////////////////////////////////

            List<int> zz1 = new List<int>(); // zz=find(abs(V)>c_sinir*pi) 
            List<double> xbilinmeyen_new1 = new List<double>();
            int iterasyon = 0;
            double[,] W = new double[ndk, ndk];
            int iii = 0;
            ; for (iii = 0; iii < 1000; iii++)
            {
                iterasyon = iterasyon + 1;
                ndk = nd + nk;
                d = 3;

                for (int i = 0; i < nd; i++)           // AMAT=[AIK;KatsayiK];
                    for (int j = 0; j < u * 2; j++)
                        AMAT[i, j] = AIK[i, j];

                for (int k = nd; k < ndk; k++)         // AMAT=[AIK;KatsayiK];
                    for (int l = 0; l < u * 2; l++)
                        AMAT[k, l] = KatsayiK[k - nd, l];

                for (int ii = 0; ii < nd; ii++)        // LMAT=[sabiti;lk1];
                    LMAT[ii] = sabiti[ii];
                for (int ii = nd; ii < ndk; ii++)
                    LMAT[ii] = lk1[ii - nd];


                AMAT_Transpose = TransposeMatrix(AMAT);                                                    //  AMAT' 
                cevap1 = MultiplyMatrices(MultiplyMatrices(AMAT_Transpose, AGIRLIK_diag), AMAT);          // AMAT'*AGIRLIK*AMAT
                cevap2 = MultiplyMatrixAndVector(MultiplyMatrices(AMAT_Transpose, AGIRLIK_diag), LMAT);  // AMAT'*AGIRLIK*LMAT
                deneme = CalculatePseudoInverse(cevap1);                                                // pinv(AMAT'*AGIRLIK*AMAT)
                double[] xbilinmeyen_oldD = MultiplyMatrixAndVector(deneme, cevap2);                    // xbilinmeyen_oldD=pinv(AMAT'*AGIRLIK*AMAT)*(AMAT'*AGIRLIK*LMAT)

                V = SubtractVectors(MultiplyMatrixAndVector(AMAT, xbilinmeyen_oldD), LMAT); // V=(AMAT*xbilinmeyen_oldD)-LMAT; - DÜZELTMELERİN HESABI
                f = ndk - BS + d; // SERBEST DERECESİ


                ABC = MultiplyVectors(MultiplyMatrixAndVector(AGIRLIK_diag, V), V);  // V'*AGIRLIK*V 
                M0 = Math.Sqrt(ABC / f);                                             // M0=sqrt((V'*AGIRLIK*V)/(f))

                // andrew UYUŞUMSUZ ÖLÇÜLERİN TEST BÜYÜKLÜKLERİ
                // güven aralığı değeri
                // double c = 1.5;

                double[] ortalama = CalculateAbsoluteValues(V); // abs(V)
                double result = c_sinir1 * Math.PI;            // c_sinir*pi

                double[] result1 = DivideVectorByScalar(ortalama, c_sinir1);        // abs(V)/c_sinir
                double[] result2 = CalculateSinusOfVector(result1);                // sin(abs(V)/c_sinir))
                double[] result3 = CalculateNegativePower(result1, -1);           // (abs(V)/c_sinir).^(-1)
                double[] result4 = ElementwiseMultiplyVectors(result3, result2); // (((abs(V)/c_sinir).^(-1))).*sin(abs(V)/c_sinir)
                W = DiagVector(result4);                                        // W = diag((((abs(V) / c_sinir).^ (-1))).* sin(abs(V) / c_sinir));

                zz1 = FindIndices(ortalama, result); // zz=find(abs(V)>c_sinir*pi)
                int[] zz = zz1.ToArray();           // Döngü dışında kullancam zz1 i

                for (int kk = 0; kk < zz.Length; kk++)
                {
                    W[zz[kk], zz[kk]] = 0;  // W(zz,zz)=0;
                }

                double[,] AGIRLIK_1 = CopyMatrix(W);                             // AGIRLIK_1=W;
                AGIRLIK_diag = MultiplyMatrices(AGIRLIK_diag, AGIRLIK_1);       // AGIRLIK=AGIRLIK*AGIRLIK_1;

                AMAT_Transpose = TransposeMatrix(AMAT);                                                       // AMAT' 
                cevap1 = MultiplyMatrices(MultiplyMatrices(AMAT_Transpose, AGIRLIK_diag), AMAT);             // AMAT'*AGIRLIK*AMAT
                cevap2 = MultiplyMatrixAndVector(MultiplyMatrices(AMAT_Transpose, AGIRLIK_diag), LMAT);     // AMAT'*AGIRLIK*LMAT
                deneme = CalculatePseudoInverse(cevap1);                                                   // pinv(AMAT'*AGIRLIK*AMAT)
                double[] xbilinmeyen_new = MultiplyMatrixAndVector(deneme, cevap2);                 // xbilinmeyen_new=pinv(AMAT'*AGIRLIK*AMAT)*(AMAT'*AGIRLIK*LMAT)


                double[] pp = SubtractVectors(xbilinmeyen_new, xbilinmeyen_oldD);  // xbilinmeyen_new-xbilinmeyen_oldD
                double[] abs_new_eksi_old = CalculateAbsoluteValues(pp);          // abs(xbilinmeyen_new-xbilinmeyen_oldD)

                int sayac_abs_icin = 0;
                for (int i = 0; i < abs_new_eksi_old.Length; i++) // abs(xbilinmeyen_new-xbilinmeyen_oldD) 
                {
                    if (abs_new_eksi_old[i] <= 0.001)
                    {
                        sayac_abs_icin = sayac_abs_icin + 1;
                    }

                }

                if (sayac_abs_icin >= abs_new_eksi_old.Length)
                {
                    break;
                }

            }
            int[] zz2 = zz1.ToArray(); //  zz=find(abs(V)>c_sinir*pi)




            //////////////////////////////////
            /////////// DENETİM  /////////////
            //////////////////////////////////


            double[] xbilinmeyen_new_orjinal = MultiplyMatrixAndVector(deneme, cevap2);      // xbilinmeyen_new=pinv(AMAT'*AGIRLIK*AMAT)*(AMAT'*AGIRLIK*LMAT)
            double[] cevap33 = MultiplyMatrixAndVector(AMAT, xbilinmeyen_new_orjinal);      // AMAT*xbilinmeyen_new
            double[] V_son = SubtractVectors(cevap33, LMAT);                               // V_son=(AMAT*xbilinmeyen_new)-LMAT;  - DÜZELTMELERİN HESABI


            double[] V_son_transpose = V_son; // V_son' (tek boyutlu olduğunu için transpoze işlemi yapmamıza gerek yok)

            double VtPV = MultiplyVectors(MultiplyMatrixAndVector(AGIRLIK_diag, V_son_transpose), V_son);                             // VtPV=V_son'*AGIRLIK*V_son;
            double VtPL = MultiplyVectors(MultiplyMatrixAndVector(AGIRLIK_diag, MultiplyArrayByScalar(V_son_transpose, -1)), LMAT);  // VtPL=-V_son'*AGIRLIK*LMAT;


            double cevap34 = MultiplyVectors(MultiplyMatrixAndVector(AGIRLIK_diag, LMAT), LMAT);     // LMAT'*AGIRLIK*LMAT
            double cevap35 = MultiplyVectors(cevap2, xbilinmeyen_new_orjinal);                     // xbilinmeyen_new'*AMAT'*AGIRLIK*LMAT;
            // cevap2 = AMAT'*AGIRLIK*LMAT;
            double LPLt_xtAPLt = cevap34 - cevap35; // (LMAT'*AGIRLIK*LMAT)-xbilinmeyen_new'*AMAT'*AGIRLIK*LMAT;


            // BİLİNMEYENLERİN TERS AĞIRLIK MATRİSİ
            double[,] N = cevap1;                       //  cevap1=AMAT'*AGIRLIK*AMAT
            double[,] Qxx = CalculatePseudoInverse(N); //  Qxx=pinv(N);



            //////////////////////////////////////////////////////////////////////////////////////////
            //////  DENGELİ ÖLÇÜLERİN VARYANS - KOVARYANS MATRİSİ Kxx = mo ^ 2 * Qxx;   //////////////        
            //////////////////////////////////////////////////////////////////////////////////////////

            double M0_karesi = Math.Pow(M0, 2);
            double[,] Kxx = MultiplyMatrixByScalar(Qxx, M0_karesi); // Kxx=M0^2*Qxx;


            /////////////////////////////////////////////////////////////////////////////////////////
            ///////////////  DENGELİ ÖLÇÜLERİN TERS AĞIRLIKLARI Qll=(akati*Qxx*akati');   ///////////
            /////////////////////////////////////////////////////////////////////////////////////////

            double[,] Qll = MultiplyMatrices(MultiplyMatrices(AMAT, Qxx), AMAT_Transpose); // Qll=AMAT*Qxx*AMAT';


            /////////////////////////////////////////////////////////////////////////////////////////
            ///////////////  DENGELİ ÖLÇÜLERİN VARYANS-KOVARYANS MATRİSİ Kll=mo^2*Qll;   ////////////
            /////////////////////////////////////////////////////////////////////////////////////////

            double[,] Kll = MultiplyMatrixByScalar(Qll, M0_karesi); // Kll=M0^2*Qll;



            //////////////////////////////////////////////////
            /////  DÜZELTMELERİN TERS AĞIRLIK MATRİSİ   //////
            //////////////////////////////////////////////////


            double[,] Qvv = SubtractMatrices(CalculatePseudoInverse(AGIRLIK_diag), Qll);  // Qvv=pinv(AGIRLIK)-Qll;;

            // REDUNDANZ PAYI
            double[] Rdnz = DiagMatrix(MultiplyMatrices(Qvv, AGIRLIK_diag)); // Rdnz=diag(Qvv*AGIRLIK);
            double[] Rdnz_dog = new double[nd];
            double[] Rdnz_ken = new double[nk];
            double[] Redundanz_dog = new double[DN.Length * 3];
            double[] Redundanz_ken = new double[DNK.Length * 3];

            for (int i = 0; i < nd; i++)
            {
                Rdnz_dog[i] = Rdnz[i];       // Rdnz_dog(i)=Rdnz(i);
            }

            for (int i = 0; i < nk; i++)
            {
                Rdnz_ken[i] = Rdnz[i + nd];   //  Rdnz_ken(j)=Rdnz(j+nd); 
            }


            for (int i = 0; i < DN.Length; i++)
            {
                Redundanz_dog[(i + 1) * 3 - 3] = DN[i];            //  Redundanz_dog(3*i-2,1)=DN(i);
                Redundanz_dog[(i + 1) * 3 - 2] = BN[i];           //  Redundanz_dog(3*i-1,1)=BN(i);
                Redundanz_dog[(i + 1) * 3 - 1] = Rdnz_dog[i];    //  Redundanz_dog(3*i,1)=Rdnz_dog(i);
            }
            for (int i = 0; i < DNK.Length; i++)
            {
                Redundanz_ken[(i + 1) * 3 - 3] = DNK[i];         // Redundanz_ken(3*i-2,1)=DNK(i);
                Redundanz_ken[(i + 1) * 3 - 2] = BNK[i];        // Redundanz_ken(3*i-1,1)=BNK(i);
                Redundanz_ken[(i + 1) * 3 - 1] = Rdnz_ken[i];  // Redundanz_ken(3*i,1)=Rdnz_ken(i);
            }



            /////////////////////////////////////////////////////////////
            /////   x ve y koordinatların karesel ortalama hatası  //////
            /////////////////////////////////////////////////////////////

            double[] mx = new double[u];
            double[] my = new double[u];

            for (int i = 0; i < u; i++)
            {
                mx[i] = Math.Sqrt(Kxx[(i + 1) * 2 - 2, (i + 1) * 2 - 2]); // mx(i)=(sqrt(Kxx((i*2-1),(i*2-1)))); 
                my[i] = Math.Sqrt(Kxx[(i + 1) * 2 - 1, (i + 1) * 2 - 1]); // my(i)=(sqrt(Kxx((i*2),(i*2))));
            }


            ///////////////////////////////////////////////////////////////
            /////////   NOKTALARIN ORTALAMA KOORDİNAT DUYARLIĞI   /////////
            ///////////////////////////////////////////////////////////////

            double mxy = M0 * Math.Sqrt(Trace(MultiplyMatrixByScalar(MatrixBölInt(Qxx, 2), u)));
            // mxy=M0*sqrt(trace(pinv(AMAT'*AGIRLIK*AMAT))/2*u);



            /////////////////////////////////////////////
            ////////   ORTALAMA KONUM DUYARLIĞI  ////////
            /////////////////////////////////////////////

            double[] mp = new double[u];

            for (int i = 0; i < u; i++)
            {
                mp[i] = Math.Sqrt(Math.Pow(mx[i], 2) + Math.Pow(my[i], 2)); // mp(i)=sqrt(mx(i)^2+my(i)^2);  
            }


            ////////////////////////////////////////
            /////   DÜZELTMELERİN SEMT HESABI  /////
            ////////////////////////////////////////

            int nnn = AMAT.GetLength(0);
            int uuu = AMAT.GetLength(1);
            double[] dendog = new double[nd];
            double[] YXMAT = new double[uuu / 2];
            double[] XXMAT = new double[uuu / 2];

            for (int i = 0; i < nd; i++)
            {
                dendog[i] = DOD[i] + V[i] / 10000;  // dendog(i)=DOD(i)+V(i)./10000;
            }
            for (int i = 0; i < uuu / 2; i++)
            {
                YXMAT[i] = xbilinmeyen_new_orjinal[(i + 1) * 2 - 1];  // YXMAT(i,1)=xbilinmeyen_new(2*i,1);
            }
            for (int i = 0; i < uuu / 2; i++)
            {
                XXMAT[i] = xbilinmeyen_new_orjinal[(i + 1) * 2 - 2]; // XXMAT(i,1)=xbilinmeyen_new(2*i-1,1);
            }

            double[] Dengky1 = PlusVectors(Y1, DivideVectorByScalar(YXMAT, 100)); // Dengky1=KOORD(:,2)+YXMAT./100;
            double[] Dengkx1 = PlusVectors(X1, DivideVectorByScalar(XXMAT, 100)); // Dengkx1=KOORD(:,3)+XXMAT./100;


            //////////////////////////////
            //////////////////////////////
            //////////////////////////////
            //////////////////////////////
            //////////////////////////////
            //////////////////////////////



            /////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////



            BASLIK(yol + "BAŞLIK.csv");
            BİLİNMEYEN_NOKTALARIN_YAKLAŞIK_KOORDİNATLARININ_YAZDIRILMASI(NN, Y1, X1, yol + "BİLİNMEYEN_NOKTALARIN_YAKLAŞIK_KOORDİNATLARININ_YAZDIRILMASI.csv");
            KENAR_YAZDIRMA(DNK, BNK, KENAR, kenar_yazdırma_için, lk1, yol + "KENAR_YAZDIRMA.csv");
            AĞA_İLİŞKİN_BİLGİLER(iii, nd, YBS, nk, ndk, u, d, yol + "AĞA_İLİŞKİN_BİLGİLER.csv");
            SONUÇ_DENETİMLERİ(VtPV, VtPL, LPLt_xtAPLt, yol + "SONUÇ_DENETİMLERİ.csv");
            REDUNDANZLARR_DOG(DN, BN, Rdnz_dog, yol + "REDUNDANZLARR_DOG.csv");
            REDUNDANZLARR_KEN(DNK, BNK, Rdnz_ken, yol + "REDUNDANZLARR_KEN.csv");
            DOĞRULTU_DÜZELTMELERİN_YAZDIRILMASI(nd, DN, BN, V, yol + "DOĞRULTU_DÜZELTMELERİN_YAZDIRILMASI.csv");
            KENAR_DÜZELTMELERİN_YAZDIRILMASI(nk, DNK, BNK, V, nd, yol + "KENAR_DÜZELTMELERİN_YAZDIRILMASI.csv");
            DENGELİ_NOKTA_KOORDİNATLARIN_YAZDIRILMASI(u, NN, Dengky1, Dengkx1, @yol + "DENGELİ_NOKTA_KOORDİNATLARIN_YAZDIRILMASI.csv");
            BİRİM_ÖLÇÜNÜN_KARESEL_ORTALAMA_HATASI(M0, yol + "BİRİM_ÖLÇÜNÜN_KARESEL_ORTALAMA_HATASI.csv");
            KOORDİNATLARA_AİT_KARASAL_ORTALAMA_HATALARI_VE_NOKTA_KONUM_DUYARLIKLARIN_YAZDIRILMASI(u, NN, mx, my, mp, yol + "KOORDİNATLARA_AİT_KARASAL_ORTALAMA_HATALARI_VE_NOKTA_KONUM_DUYARLIKLARIN_YAZDIRILMASI.csv");


            REDUNDANZ(Redundanz_dog, yol + "Redundanz_dog.csv");
            REDUNDANZ(Redundanz_ken, yol + "Redundanz_ken.csv");
            Qxx_YAZDIRMA(Qxx, yol + "Qxx.csv");
            N_YAZDIRMA(N, yol + "N.csv");
            Dengkx1_Dengky1(Dengkx1, yol + "Dengkx1.csv");
            Dengkx1_Dengky1(Dengky1, yol + "Dengky1.csv");


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////  TOPLU YAZDIRMA //////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string outputDirectory = yol; // Çıktı dosyalarının bulunduğu klasör

            // Tüm işlevlerin çağrılması ve çıktılarının birleştirilmesi
            List<string> allData = new List<string>();
            allData.Add(ReadFile(Path.Combine(outputDirectory, "BAŞLIK.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "BİLİNMEYEN_NOKTALARIN_YAKLAŞIK_KOORDİNATLARININ_YAZDIRILMASI.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "KENAR_YAZDIRMA.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "AĞA_İLİŞKİN_BİLGİLER.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "SONUÇ_DENETİMLERİ.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "REDUNDANZLARR_DOG.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "REDUNDANZLARR_KEN.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "DOĞRULTU_DÜZELTMELERİN_YAZDIRILMASI.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "KENAR_DÜZELTMELERİN_YAZDIRILMASI.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "DENGELİ_NOKTA_KOORDİNATLARIN_YAZDIRILMASI.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "BİRİM_ÖLÇÜNÜN_KARESEL_ORTALAMA_HATASI.csv")));
            allData.Add(ReadFile(Path.Combine(outputDirectory, "KOORDİNATLARA_AİT_KARASAL_ORTALAMA_HATALARI_VE_NOKTA_KONUM_DUYARLIKLARIN_YAZDIRILMASI.csv")));

            if (File.Exists(yol + "İTERASYONLU SERBEST DENGELEME SONUÇ RAPORU.csv"))
            {
                // Dosyayı silme
                File.Delete(yol + "İTERASYONLU SERBEST DENGELEME SONUÇ RAPORU.csv");
            }
            // Tüm verileri birleştirerek tek bir string haline getirme
            string combinedData = string.Join(Environment.NewLine, allData);

            // Birleştirilmiş verileri tek bir dosyaya yazma
            string combinedFilePath = Path.Combine(outputDirectory, "İTERASYONLU SERBEST DENGELEME SONUÇ RAPORU.csv");
            WriteToFile(combinedData, combinedFilePath);


            MessageBox.Show(
            "Dosya Oluşturuldu: " + "BİLİNMEYEN_NOKTALARIN_YAKLAŞIK_KOORDİNATLARININ_YAZDIRILMASI.csv" + "\n" +
            "Dosya Oluşturuldu: " + "KENAR_YAZDIRMA.csv" + "\n" +
            "Dosya Oluşturuldu: " + "AĞA_İLİŞKİN_BİLGİLER.csv" + "\n" +
            "Dosya Oluşturuldu: " + "SONUÇ_DENETİMLERİ.csv" + "\n" +
            "Dosya Oluşturuldu: " + "REDUNDANZLARR_DOG.csv" + "\n" +
            "Dosya Oluşturuldu: " + "REDUNDANZLARR_DOG.csv" + "\n" +
            "Dosya Oluşturuldu: " + "DOĞRULTU_DÜZELTMELERİN_YAZDIRILMASI.csv" + "\n" +
            "Dosya Oluşturuldu: " + "KENAR_DÜZELTMELERİN_YAZDIRILMASI.csv" + "\n" +
            "Dosya Oluşturuldu: " + "DENGELİ_NOKTA_KOORDİNATLARIN_YAZDIRILMASI.csv" + "\n" +
            "Dosya Oluşturuldu: " + "BİRİM_ÖLÇÜNÜN_KARESEL_ORTALAMA_HATASI.csv" + "\n" +
            "Dosya Oluşturuldu: " + "KOORDİNATLARA_AİT_KARASAL_ORTALAMA_HATALARI_VE_NOKTA_KONUM_DUYARLIKLARIN_YAZDIRILMASI.csv" + "\n"
            );

            Console.WriteLine("****************************************");
            Console.WriteLine(" ");
            MessageBox.Show("Tüm veriler tek bir CSV dosyasına kaydedildi: \n\n " + combinedFilePath);
            Console.WriteLine(" ");
            Console.WriteLine("*****************************************");

       

            X1_form2 = X1;
            Y1_form2 = Y1;
            NN_form2 = NN;
            DN_form2 = DN;
            BN_form2 = BN;


            Form2 form2 = new Form2(X1_form2, Y1_form2, NN_form2, DN_form2, BN_form2,Location_yol);
            form2.Show();

        }

        public double[] X1_form2;
        public double[] Y1_form2;
        public int[] NN_form2;
        public int[] DN_form2;
        public int[] BN_form2;
       



        // vektör ÇIKARMA //
        public static double[] SubtractVectors(double[] v1, double[] v2)
        {
            if (v1.Length != v2.Length)
            {
                throw new ArgumentException("Vektörlerin uzunlukları eşit olmalıdır.");
            }

            int length = v1.Length;
            double[] result = new double[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = v1[i] - v2[i];
            }

            return result;
        }
        // vektör TOPLAMA //
        public static double[] PlusVectors(double[] v1, double[] v2)
        {
            if (v1.Length != v2.Length)
            {
                throw new ArgumentException("Vektörlerin uzunlukları eşit olmalıdır.");
            }

            int length = v1.Length;
            double[] result = new double[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = v1[i] + v2[i];
            }

            return result;
        }


   
        // Matris Transpose
        public static double[,] TransposeMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[,] transposeMatrix = new double[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    transposeMatrix[j, i] = matrix[i, j];
                }
            }

            return transposeMatrix;
        }

        // 2 tane matrisin çarpımı
        public static double[,] MultiplyMatrices(double[,] matrix1, double[,] matrix2)
        {
            int row1 = matrix1.GetLength(0);
            int col1 = matrix1.GetLength(1);
            int row2 = matrix2.GetLength(0);
            int col2 = matrix2.GetLength(1);

            if (col1 != row2)
            {
                throw new ArgumentException("Matrislerin çarpımı için uyumsuz boyutlar.");
            }

            // Sonuç matrisini tanımlayın
            double[,] resultMatrix = new double[row1, col2];

            // Çarpma işlemi
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col2; j++)
                {
                    for (int k = 0; k < col1; k++)
                    {
                        resultMatrix[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }

            return resultMatrix;
        }

        // MATRİS İLE VEKTORUN ÇARPIMI 
        public static double[] MultiplyMatrixAndVector(double[,] matrix, double[] vector)
        {
            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);
            double[] result = new double[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < colCount; j++)
                {
                    sum += matrix[i, j] * vector[j];
                }
                result[i] = sum;
            }

            return result;
        }
      
        // PİNV
        public static double[,] CalculatePseudoInverse(double[,] array)
        {
            // İki boyutlu diziyi MathNet Numerics kütüphanesindeki Matrix sınıfına dönüştürün
            Matrix<double> matrix = Matrix<double>.Build.DenseOfArray(array);

            // Genelleştirilmiş tersi hesaplayın
            Matrix<double> pinvA = matrix.PseudoInverse();

            // Genelleştirilmiş ters matrisi iki boyutlu diziye dönüştürün
            double[,] result = new double[pinvA.RowCount, pinvA.ColumnCount];
            for (int i = 0; i < pinvA.RowCount; i++)
            {
                for (int j = 0; j < pinvA.ColumnCount; j++)
                {
                    result[i, j] = pinvA[i, j];
                }
            }

            return result;
        }


        // VEKTÖR ÇARPIMI 
        public static double MultiplyVectors(double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new ArgumentException("Vektörlerin boyutları aynı olmalıdır.");
            }

            // Tek boyutlu dizileri MathNet Numerics kütüphanesindeki Vector sınıfına dönüştürün
            var v1 = Vector<double>.Build.DenseOfArray(vector1);
            var v2 = Vector<double>.Build.DenseOfArray(vector2);

            // İki vektörün iç çarpımını hesaplayın
            double result = v1.DotProduct(v2);

            return result;
        }

        // matrisden matris çıkarma
        public static double[,] SubtractMatrices(double[,] matrix1, double[,] matrix2)
        {
            int rows = matrix1.GetLength(0);
            int columns = matrix1.GetLength(1);

            // Sonuç matrisi oluştur
            double[,] resultMatrix = new double[rows, columns];

            // Matris çıkarma işlemi
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    resultMatrix[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }

            return resultMatrix;
        }

        // MATRİX KAREKÖK ALMA
        public static double[,] matriskarekok(double[,] inputMatrix)
        {
            int rows = inputMatrix.GetLength(0);
            int columns = inputMatrix.GetLength(1);

            double[,] resultMatrix = new double[rows, columns];

            // Her bir elemanın karekökünü al
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    resultMatrix[i, j] = Math.Sqrt(inputMatrix[i, j]);
                }
            }

            return resultMatrix;
        }

        // DİAG MATRİX
         public static double[] DiagMatrix(double[,] array)
        {
            int rowCount = array.GetLength(0);
            int colCount = array.GetLength(1);

            if (rowCount != colCount)
            {
                throw new ArgumentException("Girdi matrisi kare olmalıdır.");
            }

            double[] diagonal = new double[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                diagonal[i] = array[i, i];
            }

            return diagonal;
        }

        // DİAG VECTOR  
        public static double[,] DiagVector(double[] array)
        {
            int n = array.Length;
            double[,] result = new double[n, n];

            for (int i = 0; i < n; i++)
            {

                result[i, i] = array[i];
            }

            return result;
        }

        // Vektörü bir double sayısı ile çarp
        public static double[] MultiplyVectorByScalar(double[] vector, double scalar)
        {

            double[] resultVector = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                resultVector[i] = vector[i] * scalar;
            }

            return resultVector;
        }

        // Matrisi bir double sayısı ile çarp
        public static double[,] MultiplyMatrixByScalar(double[,] matrix, double scalar)
        {

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);
            double[,] resultMatrix = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    resultMatrix[i, j] = matrix[i, j] * scalar;
                }
            }

            return resultMatrix;
        }


        // Matrisi bir int sayısı ile böl
        public static double[,] MatrixBölInt(double[,] array, int divisor)
        {
            // Dizinin boyutlarını alalım
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            // Sonuç için yeni bir dizi oluşturalım
            double[,] result = new double[rows, cols];

            // Her bir elemanı tam sayıya bölerek yeni diziye yerleştirelim
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = array[i, j] / divisor;
                }
            }

            return result;
        }



        // İki vektörün element-wise çarpımını hesapla
        public static double[] ElementwiseMultiplyVectors(double[] vectorA, double[] vectorB)
        {

            if (vectorA.Length != vectorB.Length)
            {
                throw new ArgumentException("Vectors must have the same length for element-wise multiplication.");
            }

            double[] resultVector = new double[vectorA.Length];

            for (int i = 0; i < vectorA.Length; i++)
            {
                resultVector[i] = vectorA[i] * vectorB[i];
            }

            return resultVector;
        }

        // trace fonskiyonu
        public static double Trace(double[,] matrix)
        {
            // Matris boyutunu alalım
            int n = matrix.GetLength(0);
            // Köşegen elemanların toplamını başlat
            double sum = 0;

            // Köşegen elemanları topla
            for (int i = 0; i < n; i++)
            {
                sum += matrix[i, i];
            }

            return sum;
        }

      
        // vector abs alma //
        public static double[] CalculateAbsoluteValues(double[] array)
        {
            double[] absoluteValues = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                // Math.Abs fonksiyonunu kullanarak mutlak değeri hesapla
                absoluteValues[i] = Math.Abs(array[i]);
            }

            return absoluteValues;
        }


        // Gerekli indeksleri bulan LINQ ifadesi
        public static List<int> FindIndices(double[] a, double b)
        {
            var result = Enumerable.Range(0, a.Length)
                                   .Where(i => a[i] > b)
                                   .ToList();

            return result;
        }



        // VEKTÖRÜ DOUBLEYE BÖL //
        public static double[] DivideVectorByScalar(double[] array, double scalar)
        {
            if (scalar == 0)
            {
                // Sıfıra bölmeyi önlemek için uygun bir işlem yapabilirsiniz.
                throw new ArgumentException("Sıfıra bölme hatası.");
            }

            double[] result = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                // Her vektör elemanını belirtilen sayıya böl
                result[i] = array[i] / scalar;
            }

            return result;
        }

        // VEKTÖR SİN ALMA //
        public static double[] CalculateSinusOfVector(double[] array)
        {
            double[] result = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                // Her vektör elemanının sinüsünü hesapla
                result[i] = Math.Sin(array[i]);
            }

            return result;
        }

        // vektör eksi kuvvet //
        public static double[] CalculateNegativePower(double[] array, int power)
        {
            if (power == 0)
            {
                // 0. kuvvet her zaman 1'dir.
                throw new ArgumentException("0. kuvvet alınamaz.");
            }

            double[] result = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                // Her vektör elemanının -1. kuvvetini hesapla
                result[i] = Math.Pow(array[i], power);
            }

            return result;
        }


        // VEKTÖRÜ GİRİLEN DOUBLE DEĞERİ İLE ÇARP (1 KEZ KULLANUYOZ ODA -1)
        public static double[] MultiplyArrayByScalar(double[] array, double scalar)
        {
            int length = array.Length;
            double[] resultArray = new double[length];

            for (int i = 0; i < length; i++)
            {
                resultArray[i] = array[i] * scalar;
            }

            return resultArray;
        }

        // matrisi kopyaLA //
        public static double[,] CopyMatrix(double[,] source)
        {
            int numRows = source.GetLength(0);
            int numColumns = source.GetLength(1);

            double[,] destination = new double[numRows, numColumns];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    destination[i, j] = source[i, j];
                }
            }

            return destination;
        }


        // double tipinde belirli sutunları çekme
        // double tipinde belirli sutunları çekme
        // double tipinde belirli sutunları çekme
        public static List<double> ReadCsvColumn(string filePath, int columnIndex)
        {
            List<double> columnData = new List<double>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                // CSV dosyasındaki satırları okuma
                while (csv.Read())
                {
                    // Belirli sütundaki veriyi çekme
                    string columnValueStr = csv.GetField(columnIndex);

                    // Virgülle ayrılmış veriyi ondalık sayıya çevirme
                    if (double.TryParse(columnValueStr.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double columnValue))
                    {
                        columnData.Add(columnValue);
                    }
                }
            }

            return columnData;
        }


        // int tipinde belirli sutunları çekme
        // int tipinde belirli sutunları çekme
        // int tipinde belirli sutunları çekme
        public static List<T> ReadCsvColumn1<T>(string filePath, int columnIndex)
        {
            List<T> columnData = new List<T>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                // CSV dosyasındaki satırları okuma
                while (csv.Read())
                {
                    // Belirli sütundaki veriyi çekme
                    string columnValueStr = csv.GetField(columnIndex);

                    // Veriyi çevirme
                    if (int.TryParse(columnValueStr, out var intValue))
                    {
                        columnData.Add((T)(object)intValue);
                    }
                }
            }

            return columnData;
        }




        static int[] ReadExcelColumn_int(string filePath, int sheetIndex, int columnIndex)
        {
            List<int> dataList = new List<int>();

            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetIndex]; // Belirtilen sayfa indeksi üzerinde işlem yapma

                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        if (int.TryParse(Convert.ToString(worksheet.Cells[row, columnIndex].Value), out int value))
                        {
                            dataList.Add(value); // Belirtilen sütun indeksi üzerindeki sayısal veriyi al ve int'e dönüştür
                        }
                        else
                        {
                            Console.WriteLine($"Hücre ({row}, {columnIndex}) değeri bir tam sayıya dönüştürülemedi.");
                        }
                    }
                }

                // List<int> nesnesini int dizisine dönüştürme
                int[] dataArray = dataList.ToArray();
                return dataArray;
            }
            else
            {
                Console.WriteLine("Belirtilen Excel dosyası bulunamadı.");
                return new int[0]; // Boş bir dizi döndür
            }
        }

        static double[] ReadExcelColumn_double(string filePath, int sheetIndex, int columnIndex)
        {
            List<double> dataList = new List<double>();

            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetIndex]; // Belirtilen sayfa indeksi üzerinde işlem yapma

                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        if (double.TryParse(Convert.ToString(worksheet.Cells[row, columnIndex].Value), out double value))
                        {
                            dataList.Add(value); // Belirtilen sütun indeksi üzerindeki sayısal veriyi al ve double'a dönüştür
                        }
                        else
                        {
                            Console.WriteLine($"Hücre ({row}, {columnIndex}) değeri bir ondalıklı sayıya dönüştürülemedi.");
                        }
                    }
                }

                // List<double> nesnesini double dizisine dönüştürme
                double[] dataArray = dataList.ToArray();
                return dataArray;
            }
            else
            {
                Console.WriteLine("Belirtilen Excel dosyası bulunamadı.");
                return new double[0]; // Boş bir dizi döndür
            }
        }



        static int GetSheetRowCount(string filePath, int sheetIndex)
        {
            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetIndex];
                    return worksheet.Dimension.Rows;
                }
            }
            else
            {
                Console.WriteLine("Belirtilen Excel dosyası bulunamadı.");
                return -1; // Dosya yoksa -1 döndür
            }
        }


        static int GetSheetColumnCount(string filePath, int sheetIndex)
        {
            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetIndex];
                    return worksheet.Dimension.Columns;
                }
            }
            else
            {
                Console.WriteLine("Belirtilen Excel dosyası bulunamadı.");
                return -1; // Dosya yoksa -1 döndür
            }
        }









        //matrix_tablo_double_yazdırma
        //matrix_tablo_double_yazdırma
        public static void matrix_tablo_double_yazdırma(double[,] array)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            // Sütun başlıklarını yazdırma
            Console.Write("       ");
            for (int j = 0; j < columns; j++)
            {
                Console.Write($"|{"Column " + (j + 1),-8}");
            }
            Console.WriteLine("|");
            Console.WriteLine("-------------------------------------------------");

            // Dizi elemanlarını yazdırma
            for (int i = 0; i < rows; i++)
            {
                Console.Write($"Row {i + 1,-3} |");
                for (int j = 0; j < columns; j++)
                {
                    Console.Write($"{array[i, j],-8:F2}|");
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------");
            }
        }


        //vektor_tablo_int_yazdırma
        //vektor_tablo_int_yazdırma
        public static void vektor_tablo_int_yazdırma(int[] vector)
        {
            int length = vector.Length;

            // Başlık sırasını yazdırma
            Console.Write("| Index | Value |\n");
            Console.WriteLine("----------------");

            // Vektör elemanlarını yazdırma
            for (int i = 0; i < length; i++)
            {
                Console.Write($"| {i + 1,-6} | {vector[i],-6} |\n");
                Console.WriteLine("----------------");
            }
        }


        //vektor_tablo_double_yazdırma
        //vektor_tablo_double_yazdırma
        public static void vektor_tablo_double_yazdırma(double[] vector)
        {
            int length = vector.Length;

            // Başlık sırasını yazdırma
            Console.Write("| Index |  Value |\n");
            Console.WriteLine("----------------");

            // Vektör elemanlarını yazdırma
            for (int i = 0; i < length; i++)
            {
                Console.Write($"| {i + 1,-6} | {vector[i],-6:F2} |\n");
                Console.WriteLine("----------------");
            }
        }




        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        ////////  DOSYA BOYUTU OGRENME //////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////


        public static int GetRowCount(string filePath)
        {
            // CSV dosyasındaki satır sayısını alın
            var lines = File.ReadAllLines(filePath);
            return lines.Length;
        }

        public static int GetColumnCount(string filePath)
        {
            // CSV dosyasındaki sütun sayısını alın
            var lines = File.ReadAllLines(filePath);
            var firstLine = lines.FirstOrDefault();

            if (firstLine != null)
            {
                var columns = ParseColumns(firstLine);
                return columns.Length;
            }

            return 0; // Dosya boşsa sütun sayısını 0 olarak döndürün
        }

        public static string[] ParseColumns(string line)
        {
            // Virgülle ayrılmış sütunları ayrıştırırken, çift tırnak içindeki virgülleri saymayın
            var columns = new System.Collections.Generic.List<string>();
            var insideQuotes = false;
            var currentColumn = "";

            foreach (char c in line)
            {
                if (c == '"')
                {
                    insideQuotes = !insideQuotes;
                }
                else if (c == ',' && !insideQuotes)
                {
                    columns.Add(currentColumn.Trim());
                    currentColumn = "";
                }
                else
                {
                    currentColumn += c;
                }
            }

            columns.Add(currentColumn.Trim()); // Son sütunu ekleyin

            return columns.ToArray();
        }

        ///////////////////      YAZDIRMA İŞLEMLERİ    ////////////////////////////
      



        // "N" YAZDIRMA

        public static void N_YAZDIRMA(double[,] array, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 2 boyutlu diziyi CSV dosyasına yazma
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        string formattedValue = array[i, j].ToString("0.#####").Replace(",", ".");
                        if (formattedValue.Length > 7)
                            formattedValue = formattedValue.Substring(0, 7); // En fazla 5 basamağı al

                        writer.Write(formattedValue.PadRight(8)); // Onaylamak için 8 karakterlik bir alan bırakıyoruz
                                                                  //writer.Write(array[i, j].ToString());

                        if (j < array.GetLength(1) - 1)
                        {
                            writer.Write("|"); // Değerler arasına virgül ekleme
                        }
                    }
                    writer.WriteLine(); // Satır sonu
                }
            }
        }

        // "Qxx" YAZDIRMA
        public static void Qxx_YAZDIRMA(double[,] array, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 2 boyutlu diziyi CSV dosyasına yazma
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        string formattedValue = array[i, j].ToString("N8").Replace(",", ".");
                        if (formattedValue.Length > 10)
                            formattedValue = formattedValue.Substring(0, 10);

                        writer.Write(formattedValue.PadRight(12));

                        if (j < array.GetLength(1) - 1)
                        {
                            writer.Write("|"); // Değerler arasına virgül ekleme
                        }
                    }
                    writer.WriteLine(); // Satır sonu
                }
            }
        }




        // "Dengky1" VE "Dengkx1"
        public static void Dengkx1_Dengky1(double[] array, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Tek boyutlu diziyi CSV dosyasına yazma
                for (int i = 0; i < array.Length; i++)
                {
                    // Virgülle ayrılmış değerleri yazarken, double değerlerin string'e dönüşümünde nokta kullanılmalıdır
                    // writer.Write(array[i].ToString().Replace(",", "."));
                    writer.Write(array[i].ToString("N2").Replace(",", "."));
                    if (i < array.Length - 1)
                    {
                        // writer.Write(","); // Değerler arasına virgül ekleme
                        writer.WriteLine();
                    }
                }
            }
        }


        // YAZDIRMA  "DOĞRULTU DÜZELTMELERİN YAZDIRILMASI"

        public static void DOĞRULTU_DÜZELTMELERİN_YAZDIRILMASI(int nd, int[] DN, int[] BN, double[] V, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Başlık ve çizgi
                writer.WriteLine("++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("++ DÜZELTME DEĞERLERİ(V) (DOĞRULTU) ++");
                writer.WriteLine("++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("DN         BN        Düzeltme(cc)");
                writer.WriteLine("---------------------------------");


                // Verilerin yazılması
                for (int i = 0; i < nd; i++)
                {
                    writer.WriteLine($"{DN[i],-10} {BN[i],-10} {V[i],-15:0.#####}");
                }
            }
        }



        //YAZDIRMA   "KENAR DÜZELTMELERİN YAZDIRILMASI"
        public static void KENAR_DÜZELTMELERİN_YAZDIRILMASI(int nk, int[] DNK, int[] BNK, double[] V, int nd, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Başlık ve çizgi
                writer.WriteLine("++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("++ DÜZELTME DEĞERLERİ(V) (KENAR)    ++");
                writer.WriteLine("++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("DN         BN        Düzeltme(cc)");
                writer.WriteLine("---------------------------------");

                // Verilerin yazılması
                for (int i = 0; i < nk; i++)
                {
                    writer.WriteLine($"{DNK[i],-10} {BNK[i],-10} {V[i + nd],-15:0.#####}");
                }
            }
        }


        //YAZDIRMA "DENGELİ NOKTA KOORDİNATLARIN YAZDIRILMASI"
        public static void DENGELİ_NOKTA_KOORDİNATLARIN_YAZDIRILMASI(int u, int[] NN, double[] Dengky1, double[] Dengkx1, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Başlık ve çizgi
                writer.WriteLine("++++++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("++      DENGELİ NOKTA KOORDİNATLARI     ++");
                writer.WriteLine("++++++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("NNO            Y(m)            X(m)");
                writer.WriteLine("------------------------------------------");

                // Verilerin yazılması
                for (int i = 0; i < u; i++)
                {
                    writer.WriteLine($"{NN[i]}   {Dengky1[i],18:0.#####}  {Dengkx1[i],0:0.#####}");
                }
            }
        }


        //YAZDIRMA  "BİRİM ÖLÇÜNÜN KARESEL ORTALAMA HATASI"
        public static void BİRİM_ÖLÇÜNÜN_KARESEL_ORTALAMA_HATASI(double M0, string filePath)

        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Başlık ve çizgi
                writer.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
                writer.WriteLine($"++ Birim Ölçünün Karesel Ortalama Hatasi(cc) = {M0,5:0.00000} ++");
                writer.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
                writer.WriteLine();
            }
        }



        //YAZDIRMA "KOORDİNATLARA AİT KAR. ORT. HATALAR VE NOKTA KONUM DUYARLIKLARIN YAZDIRILMASI"

        public static void KOORDİNATLARA_AİT_KARASAL_ORTALAMA_HATALARI_VE_NOKTA_KONUM_DUYARLIKLARIN_YAZDIRILMASI(int u, int[] NN, double[] mx, double[] my, double[] mp, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Başlık ve çizgi
                writer.WriteLine("Koordinatlara Ait Karesel Ortalama Hatalar ve Nokta Konum Duyarlılıkları:");
                writer.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
                writer.WriteLine("NNO     mx     my    mp(cm)");
                writer.WriteLine("--------------------------------------------");

                // Verilerin yazılması
                for (int i = 0; i < u; i++)
                {
                    writer.WriteLine($"{NN[i],-3}   {mx[i],5:0.0000}  {my[i],5:0.0000} {mp[i],5:0.0000}");
                }
            }
        }





        // YAZDIRMA      "SONUÇ DENETİMLERİ"
        public static void SONUÇ_DENETİMLERİ(double VtPV, double VtPL, double LPLt_xtAPLt, string filePath)
        {

            if (File.Exists(filePath))
            {
                // Dosya varsa üzerine yazma
                File.Delete(filePath);
            }

            // Sonuçları dosyaya yazma
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("++++++++++++++++++++++++++++++++");
                writer.WriteLine("++     SONUÇ DENETİMLERİ      ++");
                writer.WriteLine("++     -----------------      ++");
                writer.WriteLine($"++   VtPV        = {VtPV,5:0.#####}     ++");
                writer.WriteLine($"++   VtPL        = {VtPL,5:0.#####}     ++");
                writer.WriteLine($"++   LPLt_xtAPLt = {LPLt_xtAPLt,5:0.#####}     ++");
                writer.WriteLine("++++++++++++++++++++++++++++++++");
                writer.WriteLine();
            }
        }


        // YAZDIRMA   "AĞA İLİŞKİN BİLGİLER"
        public static void AĞA_İLİŞKİN_BİLGİLER(int it, int nd, int YBS, int nk, int ndk, int u, int d, string filePath)
        {

            if (File.Exists(filePath))
            {
                // Dosya varsa üzerine yazma
                File.Delete(filePath);
            }

            // Sonuçları dosyaya yazma
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
                writer.WriteLine($"+ İterasyon Sayısı =\t {it}  +");
                writer.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
                writer.WriteLine(" ");
                writer.WriteLine("+++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("AĞA İLİŞKİN BİLGİLER");
                writer.WriteLine($"Doğrultu Ölçü Sayısı           nd = {nd,3} ");
                writer.WriteLine($"Yöneltme Bilinmeyeni Sayisi   YBS = {YBS,3} ");
                writer.WriteLine($"Kenar Ölçü Sayısı              nk = {nk,3} ");
                writer.WriteLine($"Bilinmeyen Nokta Sayısı         u = {u,3} ");
                writer.WriteLine($"Datum Defekti                   d = {d,3} ");
                writer.WriteLine($"Serbestlik Derecesi         n-u+d = {ndk - u * 2 - YBS + d,3} ");
                writer.WriteLine("+++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine();
            }
        }



        // YAZDIRMA "KENAR YAZDIRMA"
        public static void KENAR_YAZDIRMA(int[] DNK, int[] BNK, double[] KENAR, double[] Kenar, double[] lk1, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosya varsa üzerine yazma
                File.Delete(filePath);
            }
            using (StreamWriter writer = new StreamWriter(filePath, append: true)) // Dosyaya ekleme modunda açılır
            {
                writer.WriteLine("++++++++++++++++");
                writer.WriteLine("++ Verilenler ++");
                writer.WriteLine("++++++++++++++++");
                writer.WriteLine();
                writer.WriteLine("---------------------------------------------------------------------------");
                writer.WriteLine("  DN         BN      Ölç. Kenar(m)    Hes. Kenar(m)      lvek(cm)");
                writer.WriteLine("---------------------------------------------------------------------------");

                for (int i = 0; i < DNK.Length; i++)
                {
                    writer.WriteLine($"{DNK[i],5} {BNK[i],10} {KENAR[i],15:F4} {Kenar[i],15:F4} {lk1[i],12:F2}");
                }
            }
        }


        // YAZDIRMA  "BİLİNMEYEN NOKTALARIN YAKLAŞIK KOORDİNATLARININ YAZDIRILMASI"
        public static void BİLİNMEYEN_NOKTALARIN_YAKLAŞIK_KOORDİNATLARININ_YAZDIRILMASI(int[] NN, double[] Y1, double[] X1, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosya varsa üzerine yazma
                File.Delete(filePath);
            }
            using (StreamWriter writer = new StreamWriter(filePath, append: true)) // Dosyaya ekleme modunda açılır
            {
                writer.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("++ BİLİNMEYEN NOKTALARIN YAKLAŞIK KOORDİNATLARI ++");
                writer.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                writer.WriteLine("  NNO              Y(m)               X(m)");

                for (int i = 0; i < NN.Length; i++)
                {
                    writer.WriteLine($"{NN[i],5}   {Y1[i],18:F5}  {X1[i],18:F5}");
                }

                writer.WriteLine("--------------------------------------------------");
                writer.WriteLine();
            }
        }


        // BASLIK YAZDIRMA
        public static void BASLIK(string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosya varsa üzerine yazma
                File.Delete(filePath);
            }
            using (StreamWriter writer = new StreamWriter(filePath, append: true)) // Dosyaya ekleme modunda açılır
            {
                writer.WriteLine("+İTERASYONLU SERBEST DENGELEME SONUÇ RAPORU (DOĞRULTU-KENAR AĞI)+");
                writer.WriteLine("+Mehmed Zübeyir GÜNAYDIN+");
                writer.WriteLine("*************************************");
            }
        }





        // TOPLU YAZMA
        public static string ReadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                Console.WriteLine($"Dosya bulunamadı: {filePath}");
                return string.Empty;
            }
        }
        // TOPLU YAZMA
        public static void WriteToFile(string data, string filePath)
        {
            File.WriteAllText(filePath, data);
        }




        // Redundanz YAZMA

        public static void REDUNDANZ(double[] data, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, append: true)) // Dosyaya ekleme modunda açılır
            {
                for (int i = 0; i < data.Length; i++)
                {
                    writer.WriteLine($"{data[i],10:F4}");
                }

                writer.WriteLine();
            }
        }


        //  REDUNDANZ_DOG YAZDIRMA

        public static void REDUNDANZLARR_DOG(int[] DN, int[] BN, double[]Rdnz_dog, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Başlık ve çizgi
                writer.WriteLine(" ");
                writer.WriteLine("------------------------------------------------------------");
                writer.WriteLine("DN         BN         Redundanz_dog          Sınır Değeri ");
                writer.WriteLine("                                             0.3 veya 0.5");
                writer.WriteLine("------------------------------------------------------------");

                // Verilerin yazılması
                for (int i = 0; i < Rdnz_dog.Length; i++)
                {
                    writer.WriteLine($"{DN[i],-10} {BN[i],-10} {Rdnz_dog[i],-15:0.#####}");
                }
            }
        }


        //  REDUNDANZ_KEN YAZDIRMA
        public static void REDUNDANZLARR_KEN(int[] DNK, int[] BNK, double[] Rdnz_ken, string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı silme
                File.Delete(filePath);
            }
            // Dosyayı oluşturma veya varsa üzerine yazma
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Başlık ve çizgi
                writer.WriteLine(" ");
                writer.WriteLine("------------------------------------------------------------");
                writer.WriteLine("DNK         BNK         Redundanz_ken          Sınır Değeri ");
                writer.WriteLine("                                               0.3 veya 0.5");
                writer.WriteLine("------------------------------------------------------------");

                // Verilerin yazılması
                for (int i = 0; i < Rdnz_ken.Length; i++)
                {
                    writer.WriteLine($"{DNK[i],-10} {BNK[i],-10} {Rdnz_ken[i],-15:0.#####}");
                }
            }
        }









        private void Form1_Load(object sender, EventArgs e)
        {
             
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        
    }


    }

