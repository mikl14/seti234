using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Seti2
{
    public partial class Form1 : Form
    {
        string inputMessage = "";
        string outputMessage = "";
        string outputMessage2 = "";
        string outputMessage3 = "";
        string dopSymb = "";
        bool tactSignal = true;
        bool bufer = false;
        bool tumbler = false;
        public Form1()
        {
            InitializeComponent();
        }

        void bass()
        {
            inputMessage = textBox1.Text;
            outputMessage = "";
            outputMessage2 = "";
            tumbler = false;
            dopSymb = "";

            foreach (var symb in inputMessage)
            {
                if (symb!=' ')
                {
                    dopSymb = BitConverter.ToString(Encoding.Default.GetBytes(symb.ToString()));
                }
                else
                {
                    if (!tumbler)
                    {
                        dopSymb = "00";
                        tumbler = true;
                    }
                    else
                    {
                        dopSymb += "FF";
                    }
                }
                outputMessage += dopSymb;
                outputMessage2 += Convert.ToString(Convert.ToInt64(dopSymb, 16), 2).PadLeft(dopSymb.Length * 4, '0');
            }
            label1.Text = outputMessage2;
            label2.Text = outputMessage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            outputMessage3 = "";
            bass();
            chart1.Series[0].Points.Clear();
            foreach (var symb in outputMessage2)
            {
                if (symb == '0') bufer = false;
                else bufer = true;
                if (bufer)
                {
                    chart1.Series[0].Points.Add(1);
                    chart1.Series[0].Points.Add(0);
                    outputMessage3 +='1';
                }
                else
                {
                    chart1.Series[0].Points.Add(0);
                    chart1.Series[0].Points.Add(1);
                    outputMessage3 += '0';
                }
                tactSignal = !tactSignal;
            }
            richTextBox1.Text = outputMessage + '\n' + outputMessage2 + '\n' + outputMessage3;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            outputMessage3 = "";
            bool previousTrue = false;
            bass();
            chart1.Series[0].Points.Clear();
            foreach (var symb in outputMessage2)
            {
                if (symb == '1')
                {
                    if(previousTrue)
                    {
                        chart1.Series[0].Points.Add(-1);
                        outputMessage3+='1';
                    }
                    else
                    {
                        chart1.Series[0].Points.Add(1);
                        outputMessage3 += '1';
                    }
                    previousTrue = !previousTrue;
                }
                else
                {
                    chart1.Series[0].Points.Add(0);
                    outputMessage3 += '0';
                }
            }
            richTextBox1.Text = outputMessage + '\n' + outputMessage2 + '\n' + outputMessage3;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            outputMessage3 = "";
            bool previousTact = false;
            bass();
            chart1.Series[0].Points.Clear();
            foreach (var symb in outputMessage2)
            {
                if (symb == '1')
                {
                    previousTact = !previousTact;
                }
                if (previousTact)
                {
                    chart1.Series[0].Points.Add(1);
                    outputMessage3 += '1';
                }
                else
                {
                    chart1.Series[0].Points.Add(0);
                    outputMessage3 += '0';
                }
            }
            richTextBox1.Text = outputMessage + '\n' + outputMessage2 + '\n' + outputMessage3;
        }
    }
}
