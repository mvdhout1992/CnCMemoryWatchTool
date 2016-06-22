using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReadWriteMemory;

namespace CnCMemoryWatchTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainUI());
        }
    }
}

/*             if (!Mem.CheckProcess())
                MessageBox.Show("Make sure your application is running or try running this as Admin");
            else
                Mem.OpenProcess();


            // 0x6C00062 
           int offset = test + Mem.ImageAddress();

            byte[] MemBytes = Mem.ReadMem(test, 0x8000 80*25*2*, false);

            String s = String.Format("{0:X02} {1:X02} {2:X02} {3:X02} {4:X02} {5:X02}", MemBytes[0],
                MemBytes[1], MemBytes[2], MemBytes[3], MemBytes[4], MemBytes[5]);

string result = System.Text.Encoding.UTF8.GetString(MemBytes);
result.Replace((char)0xC2, '┬');

            //  String hex = String.Format("{0:X}", offset);
            MessageBox.Show(result); */