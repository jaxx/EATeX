using System;
using System.Windows.Forms;

namespace EATeX.UI
{
    public partial class SettingsWindow : Form
    {
        private readonly EATeXConfig configuration = new EATeXConfig();

        public SettingsWindow()
        {
            InitializeComponent();

            txtTexLocation.Text = configuration.Read("texPath");
            txtTemplateLocation.Text = configuration.Read("templatePath");
        }

        private void btnBrowseMikTexLocation_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtTexLocation.Text = openFileDialog.FileName;
            }
        }

        private void btnBrowseTemplateLocation_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtTemplateLocation.Text = openFileDialog.FileName;
            }
        }
    }
}