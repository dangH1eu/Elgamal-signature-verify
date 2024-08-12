using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace ElgamalSign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public int gcd(int a, int b)
        {
            while (a * b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }
            return a + b;
        }
        public bool isPrime(int n)
        {
            if (n <= 1)
                return false;

            // Check if n=2 or n=3
            if (n == 2 || n == 3)
                return true;

            // Check whether n is divisible by 2 or 3
            if (n % 2 == 0 || n % 3 == 0)
                return false;

            // Check from 5 to square root of n
            // Iterate i by (i+6)
            for (int i = 5; i <= Math.Sqrt(n); i = i + 6)
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;

            return true;
        }

        public BigInteger modInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m;
            BigInteger y = 0, x = 1;
            if (m == 1)
            {
                return 0;
            }
            while (a > 1)
            {
                // q is quotient
                BigInteger q = a / m;
                BigInteger t = m;

                // m is remainder now, process same as Euclid's Algorithm
                m = a % m;
                a = t;
                t = y;
                // update y and x
                y = x - q * y;
                x = t;
            }
            // make x positive
            if (x < 0)
            {
                x += m0;
            }
            return x;
        }

        public long exponentiation(long alpha, long a, long p)
        {
            long t = 1L;
            while (a > 0)
            {
                // for cases where exponent is not an even value
                if (a % 2 != 0)
                {
                    t = (t * alpha) % p;
                }
                alpha = (alpha * alpha) % p;
                a /= 2;
            }
            return t % p;
        }

        public bool validate()
        {
            if (txtSoP.Text == "" || txtSoAlpha.Text == "" || txtSoA.Text == "" || txtSoK.Text == "")
            {
                MessageBox.Show("Can nhap so P, so alpha, khoa bi mat a, so k", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            StringBuilder sb = new StringBuilder();


            try
            {
                int p = Convert.ToInt32(txtSoP.Text);
                int alpha = Convert.ToInt32(txtSoAlpha.Text);
                int a = Convert.ToInt32(txtSoA.Text);
                int k = Convert.ToInt32(txtSoK.Text);
                if (!isPrime(p))
                {
                    sb.Append("p phai la so nguyen to\n");
                }
                if (gcd(alpha, p) != 1 || alpha > p)
                {
                    sb.Append("alpha phai nguyen to cung nhau voi p va alpha < p\n");
                }
                if (a > p)
                {
                    sb.Append("Khoa bi mat a phai nho hon P!\n");
                }
                if (gcd(k, p - 1) != 1 || k > p)
                {
                    sb.Append("k phai nguyen to cung nhau voi p - 1 va k < p\n");
                }
            }
            catch
            {
                sb.Append("So nhap vao cac truong phai so so nguyen\n");
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public static string ComputeSha256Hash(string input)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input.Replace("\r\n", "\n")));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnTaoKNN_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int p;
            int alpha;
            int a;
            int k;
            while (true)
            {
                p = rnd.Next(Int32.MaxValue) + 1;
                if (isPrime(p))
                {
                    break;
                }
            }
            txtSoP.Text = p.ToString();
            while (true)
            {
                alpha = rnd.Next(p - 1) + 1;
                if (gcd(alpha, p) == 1)
                {
                    break;
                }
            }
            txtSoAlpha.Text = alpha.ToString();
            a = rnd.Next(p - 2) + 1;
            txtSoA.Text = a.ToString();
            long beta = exponentiation(alpha, a, p);
            txtSoBeta.Text = beta.ToString();
            while (true)
            {
                k = rnd.Next(p - 1) + 1;
                if (gcd(k, p - 1) == 1)
                {
                    break;
                }
            }
            txtSoK.Text = k.ToString();


        }

        private void btnTaoKTC_Click(object sender, RoutedEventArgs e)
        {
            if (!validate())
            {
                return;
            }
            int p = Convert.ToInt32(txtSoP.Text);
            int alpha = Convert.ToInt32(txtSoAlpha.Text);
            int a = Convert.ToInt32(txtSoA.Text);
            long beta = exponentiation(alpha, a, p);
            txtSoBeta.Text = beta.ToString();
            MessageBox.Show("Tao khoa thanh cong", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSoP.Text = "";
            txtSoAlpha.Text = "";
            txtSoA.Text = "";
            txtSoBeta.Text = "";
            txtSoK.Text = "";
            txtFileKy.Text = "";
            txt_read_P.Text = "";
            txt_read_R.Text = "";
            txt_read_S.Text = "";
            txt_read_Alpha.Text = "";
            txt_read_Beta.Text = "";
            txtSoS.Text = "";
            txtSoR.Text = "";
            txtSoR1.Text = "";
            txtNdHashKtra.Text = "";
            txtFileKtra.Text = "";
        }

        private void btnChonFileKy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                txtFileKy.Text = File.ReadAllText(filename);
            }
        }

        private void btnTaoChuKy_Click(object sender, RoutedEventArgs e)
        {
            if (!validate())
            {
                return;
            }
            if (txtFileKy.Text == "")
            {
                MessageBox.Show("Chon file can ky", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            BigInteger p = BigInteger.Parse(txtSoP.Text);
            BigInteger alpha = BigInteger.Parse(txtSoAlpha.Text);
            BigInteger a = BigInteger.Parse(txtSoA.Text);
            BigInteger k = BigInteger.Parse(txtSoK.Text);
            BigInteger soR = BigInteger.ModPow(alpha, k, p);
            BigInteger soNdK = modInverse(k, p - 1);


            String m = ComputeSha256Hash(txtFileKy.Text);

            // thêm bit đầu là 0 để quy định dấu
            BigInteger soM = BigInteger.Parse("0" + m, NumberStyles.AllowHexSpecifier);
            txtNdHash.Text = m.ToString();

            BigInteger SoS = (soNdK * (soM - a * soR)) % (p - 1);

            txtSoS.Text = SoS.ToString();
            txtSoR.Text = soR.ToString();
            txtSoR1.Text = soR.ToString();
            MessageBox.Show("Ky thanh cong", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void txtChonFileKtra_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                txtFileKtra.Text = File.ReadAllText(filename);
            }
        }


        private void btnLuuChuKy_Click(object sender, RoutedEventArgs e)
        {
            string r = txtSoR.Text;
            string s = txtSoS.Text;
            string soAlpha = txtSoAlpha.Text;
            string soBeta = txtSoBeta.Text;
            string soP = txtSoP.Text;
            string fileName;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                fileName = saveFileDialog.FileName;

                try
                {
                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        writer.WriteLine(r);
                        writer.WriteLine(s);
                        writer.WriteLine(soAlpha);
                        writer.WriteLine(soBeta);
                        writer.WriteLine(soP);
                    }
                    MessageBox.Show("Lưu thành công vào tệp tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu tệp tin: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnChonFileChuKy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                string[] lines = File.ReadAllLines(filename);
                if (lines.Length >= 2)
                {
                    txt_read_R.Text = lines[0];
                    txt_read_S.Text = lines[1];
                    txt_read_Alpha.Text = lines[2];
                    txt_read_Beta.Text = lines[3];
                    txt_read_P.Text = lines[4];
                }
                else
                {
                    MessageBox.Show("Tệp tin không đúng định dạng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool validateKtraChuKy()
        {
            if (string.IsNullOrEmpty(txt_read_R.Text) || string.IsNullOrEmpty(txt_read_S.Text) || string.IsNullOrEmpty(txt_read_Alpha.Text) || string.IsNullOrEmpty(txt_read_Beta.Text) || string.IsNullOrEmpty(txt_read_P.Text))
            {
                MessageBox.Show("Chưa nhập đủ thông tin chữ ký và khoá", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            try
            {
                BigInteger r = Convert.ToInt32(txt_read_R.Text);
                BigInteger s = BigInteger.Parse(txt_read_S.Text);
                BigInteger alpha = BigInteger.Parse(txt_read_Alpha.Text);
                BigInteger beta = BigInteger.Parse(txt_read_Beta.Text);
                BigInteger p = BigInteger.Parse(txt_read_P.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("khoá và chữ ký không đúng đinh dạng", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }

        private void KtraChuKy_Click(object sender, RoutedEventArgs e)
        {
            if (validateKtraChuKy() == true)
            {
                String m = ComputeSha256Hash(txtFileKtra.Text);
                txtNdHashKtra.Text = m.ToString();
                BigInteger SoM = BigInteger.Parse("0" + m, NumberStyles.HexNumber);


                BigInteger r = BigInteger.Parse(txt_read_R.Text);
                BigInteger s = BigInteger.Parse(txt_read_S.Text);
                BigInteger alpha = BigInteger.Parse(txt_read_Alpha.Text);
                BigInteger beta = BigInteger.Parse(txt_read_Beta.Text);
                BigInteger p = BigInteger.Parse(txt_read_P.Text);

                BigInteger left = BigInteger.ModPow(beta, r, p) * BigInteger.ModPow(r, s, p) % p;

                BigInteger right = BigInteger.ModPow(alpha, SoM, p);

                if (left.Equals(right) && r < p)
                {
                    MessageBox.Show("văn bản chưa bị sửa đổi", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("văn bản đã bị sửa đổi", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

    }
}
