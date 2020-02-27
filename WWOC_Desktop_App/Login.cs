using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WWOC_Desktop_App
{
    //to use panels or tab control for multiple form selection.
    public partial class Login : Form
    {
        private int originalWidth;
        private int originalHeight;
        public Login()
        {
            InitializeComponent();
            originalWidth = this.Width;
            originalHeight = this.Height;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Width = 1000;
            this.Height = 1000;
        }
    }
}
