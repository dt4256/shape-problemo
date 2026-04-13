using System;
using System.Windows.Forms;

namespace Project_v1
{

    public partial class Rad : Form
    {
        public event RadDelegate Rad_Changed;
        private readonly Form1 _ownerForm;
        RadiusEvent _rad;
        int r;
        public Rad()
        {
            InitializeComponent();
        }

        public Rad(Form1 owner, int r)
        {
            InitializeComponent();
            hScrollBar1.Minimum = 30;
            hScrollBar1.Maximum = 400;
            _ownerForm = owner;
            hScrollBar1.Value = r;
        }
        private void Rad_Load(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Rad_Changed(this, new RadiusEvent(e.NewValue));
        }
        private void bench_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            _ownerForm.Show();
        }
    }
}
