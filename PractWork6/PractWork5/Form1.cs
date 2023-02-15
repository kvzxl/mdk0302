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

namespace PractWork6
{
    public partial class Form1 : Form
    {
        private int IsSimp(int v)
        {
            if (v >= 2)
            {
                for (int i = 2; i < v; i++)
                    if ((v % i) == 0)
                        throw new Exception();
            }
            else
                throw new Exception();
            return v;
        }

        private int VzaimoSimp(int eilFunc)
        {
            int e = 0;
            for (int i = 2; i < eilFunc; i++)
            {
                if ((eilFunc % i) != 0)
                {
                    e = i;
                    break;
                }
            }
            if (e == 0)
                throw new Exception();
            return e;
        }

        private byte[] ReadBytes()
        {
            return File.ReadAllBytes(pathTextBox.Text);
        }

        private void WriteFile(string bytCryptoPipto)
        {
            var sf = new SaveFileDialog();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sf.FileName))
                {
                    File.Delete(sf.FileName);
                }
                File.WriteAllText(sf.FileName, bytCryptoPipto);
            }
        }


        private void WriteBytes(byte[] asciiBytes)
        {
            var sf = new SaveFileDialog();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sf.FileName))
                {
                    File.Delete(sf.FileName);
                }
                File.WriteAllBytes(sf.FileName, asciiBytes);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                int q = IsSimp(Convert.ToInt32(qTextBox.Text)),
                    p = IsSimp(Convert.ToInt32(pTextBox.Text)),
                    n = q * p,
                    eilFunc = (p - 1) * (q - 1),
                    openExp = VzaimoSimp(eilFunc);
                if (n < 127)
                {
                    MessageBox.Show("Неккоректные значения, p*q должно быть больше 127", "Ошибка");
                    return;
                }
                if (n > 1681)
                {
                    MessageBox.Show("Значение p*q слишком большое", "Ошибка");
                    return;
                }
                double d = 0;
                while (true)
                {
                    if ((d * openExp) % eilFunc == 1)
                    {
                        break;
                    }
                    d++;
                }

                int b;
                int c = 1;
                byte[] asciiMessage = ReadBytes();
                string res = "";
                for (int i = 0; i < asciiMessage.Length; i++)
                {
                    b = (int)asciiMessage[i];
                    for (int j = 0; j < openExp; j++)
                    {
                        c = b * c % n;
                    }
                    res += c + " ";
                    c = 1;
                }
                res = res.TrimEnd(new char[1] { ' ' });
                WriteFile(res);

            }
            catch (Exception)
            {
                MessageBox.Show("Некорректный ввод");
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                int q = IsSimp(Convert.ToInt32(qTextBox.Text)),
                    p = IsSimp(Convert.ToInt32(pTextBox.Text)),
                    n = q * p,
                    eilFunc = (p - 1) * (q - 1),
                    openExp = VzaimoSimp(eilFunc);
                string[] messArray = File.ReadAllText(pathTextBox.Text).Split(' ');
                double d = 0;
                int[] intArr = new int[messArray.Length];
                for (int i = 0; i < messArray.Length; i++)
                {
                    intArr[i] = Convert.ToInt32(messArray[i]);
                }
                while (true)
                {
                    if ((d * openExp) % eilFunc == 1)
                    {
                        break;
                    }
                    d++;
                }
                int[] decryptArr = new int[messArray.Length];

                for (int i = 0; i < intArr.Length; i++)
                {
                    int b = intArr[i],
                       c = 1;
                    for (int j = 0; j < d; j++)
                    {
                        c = (b * c) % n;
                    }
                    decryptArr[i] = c;
                }

                var asciiBytes = new byte[decryptArr.Length];
                for (int i = 0; i < decryptArr.Length; i++)
                {
                    asciiBytes[i] = Convert.ToByte(decryptArr[i]);
                }
                WriteBytes(asciiBytes);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректный ввод");
            }
        }


        private void selectFileButton_Click(object sender, EventArgs e)
        {
            var of = new OpenFileDialog();
            if (of.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = of.FileName;
            }
        }
    }
}
