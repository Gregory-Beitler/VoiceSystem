using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voice_System
{
    public partial class Form2 : Form
    {
        // Test with C:\Users\Greg\Desktop\Atom\atom.exe

     
        string cp;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            cp = textBox1.Text;

            try
            {
                System.Diagnostics.Process.Start(cp);
            }
            catch {
                return;
            }
            
        }





       





    }
}
