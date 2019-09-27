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
    public partial class Form5 : Form
    {
        private bool flag = true;
        public Form5()
        {
            InitializeComponent();
        }

        public bool Flag { get => flag; set => flag = value; }

        private void Button1_Click(object sender, EventArgs e)
        {
            flag = true;
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            flag = false;
            this.Close();
        }
    }
}
