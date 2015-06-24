using System;
using System.Windows.Forms;

namespace EATeX.UI
{
    public partial class SettingsWindow : Form
    {
        private readonly EATeXConfig configuration;

        public SettingsWindow(EATeXConfig configuration)
        {
            InitializeComponent();

            this.configuration = configuration;

            txtTexLocation.Text = configuration.Read("texPath");
            txtTemplateLocation.Text = configuration.Read("templatePath");
        }

        private void btnBrowseMikTexLocation_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtTexLocation.Text = openFileDialog.FileName;
                configuration.Write("texPath", openFileDialog.FileName);
            }
        }

        private void btnBrowseTemplateLocation_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtTemplateLocation.Text = openFileDialog.FileName;
                configuration.Write("templatePath", openFileDialog.FileName);
            }
        }
    }
}