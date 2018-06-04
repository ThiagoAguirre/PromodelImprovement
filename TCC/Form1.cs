using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interactivity;

namespace TCC
{
    public partial class Form1 : Form
    {       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ProService.Manager.AlgoritimoGenetico(textBox1.Text);

            //manager.EndSimulation(1800);
            //manager.RedrawTable(ProService.Modules.Entities, 1, 3, "2");
            //manager.SaveAs(textBox2.Text);          
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
