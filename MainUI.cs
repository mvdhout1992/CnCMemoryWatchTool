using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace CnCMemoryWatchTool
{
    public partial class MainUI : Form
    {
        MainManager mainManager;
        int CurrentPage = -1;

        public MainUI()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                            ControlStyles.UserPaint |
                            ControlStyles.DoubleBuffer, true);

            mainManager = new MainManager();

            SetMemoryBytesTextBoxOutput();

            var OutputTextBoxRefreshTimer = new System.Windows.Forms.Timer();
            OutputTextBoxRefreshTimer.Tick += new EventHandler(RefreshOutput);
            OutputTextBoxRefreshTimer.Enabled = true;
            OutputTextBoxRefreshTimer.Interval = 40;

            SetCurrentPage(0);
        }

        // Fix stupid flickering
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_COMPOSITED = 0x02000000;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_COMPOSITED;
                return cp;
            }
        }

        private void RefreshOutput(object sender, EventArgs e)
        {
            SetMemoryBytesTextBoxOutput();
        }

        private void SetCurrentPage(int page)
        {
            CurrentPage = page;
            labelCurrentPage.Text = "" + CurrentPage;
        }

        private void SetMemoryBytesTextBoxOutput()
        {
            mainManager.SetAdJustedOffset(CurrentPage * 0xFA0);
            mainManager.UpdateMemoryBytes();
            byte[] MemBytes = mainManager.GetMemoryBytes();

            byte[] FilteredBytes = new byte[0x2000];

            /* for (int i = 0, j = 0, count = 0; i < 0x2000; i += 2, j++, count++)
             {
                 if (count == 80)
                 {
                     FilteredBytes[j] = (byte)'\r';
                     j++;
                     FilteredBytes[j] = (byte)'\n';
                     j++;
                     count = 0;
                 }

                 if (MemBytes[i] == 0xB3 || MemBytes[i] == 0xDA || MemBytes[i] == 0xBF || MemBytes[i] == 0xB4)
                 {
                     FilteredBytes[j] = (byte)'|';
                 }
                 else if (MemBytes[i] == 0xC2 || MemBytes[i] == 0xC4)
                 {
                     FilteredBytes[j] = (byte)'-';
                 }
                 else {
                     FilteredBytes[j] = MemBytes[i];
                 }
             } */

            for (int i = 0, j = 0, count = 0; i < 0x2000; i += 2, j++, count++)
            {
                if (count == 80)
                {
                    FilteredBytes[j] = (byte)'\r';
                    j++;
                    FilteredBytes[j] = (byte)'\n';
                    j++;
                    count = 0;
                }

                if (MemBytes[i] == 0x00)
                {
                    FilteredBytes[j] = (byte)' ';
                }
                else
                {
                    FilteredBytes[j] = MemBytes[i];
                }
    
            }
            

          //String s = System.Text.Encoding.UTF8.GetString(FilteredBytes);
          String s = System.Text.Encoding.GetEncoding(437).GetString(FilteredBytes);

            //Suspend(textBoxOutput);
            textBoxOutput.Text = s;
            //Resume(textBoxOutput);
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        public static void Suspend(Control control)
        {
            LockWindowUpdate(control.Handle);
        }

        public static void Resume(Control control)
        {
            LockWindowUpdate(IntPtr.Zero);
        }

        private void MainUI_Load(object sender, EventArgs e)
        {

        }

        private void textBoxPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int page = int.Parse(textBoxPage.Text);
                SetCurrentPage(page);            
            }
        }
    }
}
