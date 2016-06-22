using ReadWriteMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CnCMemoryWatchTool
{
    class MainManager
    {
        ProcessMemory ProcMem;
        byte[] MemBytes;
        int MemoryOffset = -1;

        public MainManager()
        {
            LoadMemory();
        }

        private void LoadMemory()
        {
            LoadProcessMemory();
            LoadMemoryOffset();
            LoadMemoryBytes();
        }

        public void UpdateMemoryBytes()
        {
            LoadMemoryBytes();
        }

        public byte[] GetMemoryBytes()
        {
            return MemBytes;
        }

        private void LoadMemoryBytes()
        {
            this.MemBytes = this.ProcMem.ReadMem(this.MemoryOffset, 0x8000, false);
        }

        private void LoadMemoryOffset()
        {
            //byte[] pattern = new byte[] { 0x54, 0x02, 0x61, 0x02, 0x72, 0x02 };
            // 30 02 30 02 30 02 30 02 29 02
            byte[] pattern = new byte[] { 0x54, 0x02, 0x61, 0x02, 0x72, 0x02 };

            for (int searchOffset = 0x01000062; searchOffset < 0x081E0062; searchOffset += 0x100)
            {
                byte[] MemB = this.ProcMem.ReadMem(searchOffset, 0x100, false);

                var foundList = SearchBytePattern(pattern, MemB);

                if (foundList.Count > 0)
                {
                    this.MemoryOffset = searchOffset + foundList[0];
                    this.MemoryOffset -= 66; // Fix up offset
                    return;
                }
            }
            this.MemoryOffset = -1;

            MessageBox.Show(String.Format("Offset for mono page found at: {0:X}", MemoryOffset));
        }

        private void LoadProcessMemory()
        {
            this.ProcMem = new ProcessMemory("dosbox");
            this.ProcMem.OpenProcess();
            // TODO: add error handling
        }


        static private List<int> SearchBytePattern(byte[] pattern, byte[] bytes)
        {
            List<int> positions = new List<int>();
            int patternLength = pattern.Length;
            int totalLength = bytes.Length;
            byte firstMatchByte = pattern[0];
            for (int i = 0; i < totalLength; i++)
            {
                if (firstMatchByte == bytes[i] && totalLength - i >= patternLength)
                {
                    byte[] match = new byte[patternLength];
                    Array.Copy(bytes, i, match, 0, patternLength);
                    if (match.SequenceEqual<byte>(pattern))
                    {
                        positions.Add(i);
                        i += patternLength - 1;
                    }
                }
            }
            return positions;
        }
    }
}
