using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MechanicsDetails
{
    public partial class Form3 : Form
    {
        bool flag = true;
        private String name;
        private int count;
        public Form3()
        {
            InitializeComponent();
        }

        public bool Flag { get => flag; set => flag = value; }
        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }

        private void Button1_Click(object sender, EventArgs e)
        {
         
            
            if ((textBox1.Text.Length == 0)||(textBox2.Text.Length ==0))
            {
                MessageBox.Show("Введите параметры компонента!");
            }
            else
            {
                name = textBox1.Text;
                count = Convert.ToInt32(textBox2.Text);
                this.Close();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Flag = false;
            this.Close();
        }
    }
}
