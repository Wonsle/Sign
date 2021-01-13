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
    /// <summary>
    /// 參考:CodingPioneer C#Winform实现手写录入签名与保存为透明png图片
    /// 連結:https://blog.csdn.net/zlbdmm/article/details/111977043
    /// </summary>
    public partial class formSing : Form
    {
        // 滑鼠軌跡
        private GraphicsPath mousePath = new GraphicsPath();
        // 透明度
        private int myAlpha = 255;
        // 畫筆顏色
        private Color paintColor = new Color();
        //畫筆寬度
        private int myPenWidth = 3;
        // 儲存BMP檔案
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
        /// 左鍵點下時，開始簽名
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
        /// 繪製元件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                paintColor = Color.Black;                
                Pen p = new Pen(Color.FromArgb(myAlpha, paintColor), myPenWidth);
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
            //存檔至執行檔目錄底下
            savedBitmap.Save($"{Application.StartupPath}\\Sign.png", ImageFormat.Png);

            // 去被
            Bitmap bmp = savedBitmap;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            int length = data.Stride * data.Height;
            IntPtr prt = data.Scan0;
            byte[] buff = new byte[length];
            Marshal.Copy(prt, buff, 0, length);
            for (int i = 3; i < length; i += 4)
            {
                if (buff[i - 1] >= 230 && buff[i - 2] >= 230 && buff[i - 3] >= 230)
                {
                    buff[i] = 0;
                }
            }
            Marshal.Copy(buff, 0, prt, length);
            bmp.UnlockBits(data);
            bmp.Save($"{Application.StartupPath}\\去被Sign.png", ImageFormat.Png);
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            pictureBox1.CreateGraphics().Clear(Color.White);
            mousePath.Reset();
        }
    }
}
