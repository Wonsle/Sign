using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Sign
{
    public partial class formSing : Form
    {
        private GraphicsPath mousePath = new GraphicsPath();
        private int myAlpha = 100;
        private Color myUserColor = new Color();
        private int myPenWidth = 3;
        public Bitmap savedBitmap;
        public formSing()
        {
            InitializeComponent();
        }

        private void formSing_Load(object sender, EventArgs e)
        {
            this.Text = "簽名測試";
            this.pictureBox1.BackColor = Color.White;
        }

        /// <summary>
        /// 開始簽名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePath.StartFigure();
            }
        }

        /// <summary>
        /// 移動畫筆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    mousePath.AddLine(e.X, e.Y, e.X, e.Y);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //重新繪製控制項
            pictureBox1.Invalidate();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                myUserColor = Color.Black;
                myAlpha = 255;
                Pen p = new Pen(Color.FromArgb(myAlpha, myUserColor), myPenWidth);
                e.Graphics.DrawPath(p, mousePath);
            }
            catch
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            savedBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(savedBitmap, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            savedBitmap.Save($"{Application.StartupPath}\\Sign.png", ImageFormat.Png);


            //Bitmap bmp = savedBitmap;
            //BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            //int length = data.Stride * data.Height;
            //IntPtr prt = data.Scan0;
            //byte[] buff = new byte[length];
            //Marshal.Copy(prt, buff, 0, length);
            //for (int i = 3; i < length; i += 4)
            //{
            //    if (buff[i - 1] >= 230 && buff[i - 2] >= 230 && buff[i - 3] >= 230)
            //    {
            //        buff[i] = 0;
            //    }
            //}
            //Marshal.Copy(buff, 0, prt, length);
            //bmp.UnlockBits(data);
            //bmp.Save($"{Application.StartupPath}\\Sign.png", ImageFormat.Png);
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            pictureBox1.CreateGraphics().Clear(Color.White);
            mousePath.Reset();
        }
    }
}
