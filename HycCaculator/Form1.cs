using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HycCaculator
{
    public partial class MainWin : Form
    {
        private combine<double> funcBuffer;
        private double numBuffer = 0;
        bool hasDot = false;
        bool inputComplete = false;

        // 表示二元谓词
        private delegate double combine<T>(T a, T b);

        //将操作符的名称映射到相应的函数
        private static Dictionary<char, combine<double>> operatorDict;

        static MainWin()
        {
            operatorDict = new Dictionary<char, combine<double>>();
            operatorDict['+'] = (a, b) => a + b;
            operatorDict['-'] = (a, b) => a - b;
            operatorDict['×'] = (a, b) => a * b;
            operatorDict['÷'] = (a, b) => a / b;
        }

        public MainWin()
        {
            InitializeComponent();
        }
        
        private void onBtnNumClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string text = b.Text;
            string num = textBox.Text;
            inputNum(text);
        }

        private void inputNum(string num)
        {
            if (inputComplete || textBox.Text.Equals("0"))
            {
                textBox.Text = num;
                inputComplete = false;
            }
            else
            {
                textBox.Text += num;
            }
        }

        private void btnNeg_Click(object sender, EventArgs e)
        {
            string text = textBox.Text;
            double num = double.Parse(text);
            if (!inputComplete && num != 0)
            {
                num = -num;
                textBox.Text = num.ToString();
            }
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            inputDot();
        }

        private void inputDot()
        {
            if (inputComplete)
            {
                inputNum("0");
                inputDot();
                return;
            }
            if (!hasDot)
            {
                textBox.Text += '.';
                hasDot = true;
            }
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            string text = textBox.Text;
            if (text.Length > 1)
            {
                int index = text.Length - 1;
                char delChar = text[index];
                if (delChar == '.') { hasDot = false; }
                textBox.Text = text.Remove(index, 1);
            }
            else
            {
                textBox.Text = "0";
            }
        }

        private void onBtnSignClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            char sign = btn.Text[0];
            if (funcBuffer != null)
            {// 如果存在尚未结算的操作，则先结算该操作
                cacu();
            }
            funcBuffer = operatorDict[sign];// 根据运算符号查找相应的函数，然后等待用户输入右操作数
            setInputComplete();
        }
        private void setInputComplete()
        {
            if (!inputComplete)
            {
                inputComplete = true;
                hasDot = false;
                numBuffer = Double.Parse(textBox.Text);
            }
        }
        private void cacu()
        {
            if (funcBuffer != null)
            {
                var numRight = Double.Parse(textBox.Text);
                var result = funcBuffer(numBuffer, numRight);
                textBox.Text = result.ToString();
                funcBuffer = null;
            }
        }
        private void onBtnCacuClick(object sender, EventArgs e)
        {
            cacu();
            setInputComplete();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            textBox.Text = "0";
            numBuffer = 0;
            funcBuffer = null;
            inputComplete = false;
            hasDot = false;
        }

        private void btnClearCur_Click(object sender, EventArgs e)
        {
            textBox.Text = "0";
        }

    }
}
