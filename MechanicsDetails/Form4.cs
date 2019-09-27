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
    public partial class Form4 : Form
    {
        private String nameNode;
        private bool flag=true;
        public Form4()
        {
            InitializeComponent();
        }

        public string NameNode { get => nameNode; set => nameNode = value; }
        public bool Flag { get => flag; set => flag = value; }

        private void Button1_Click(object sender, EventArgs e)
        {
            String str = textBox1.Text;
            if (str.Length == 0)
            {
                MessageBox.Show("Введите наименование компонента!");
            }
            else
            {
                NameNode = str;
                this.Close();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            flag = false;
            this.Close();
        }
    }
}
