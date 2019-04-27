using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraRichEdit.Commands;

namespace WordOfGroup1
{
    public partial class FileOptionsControl : UserControl
    {
        public FileOptionsControl()
        {
            InitializeComponent();
        }

        FileOptionsControl fileOptionsControl;
        MainForm mainForm;
        public FileOptionsControl(FileOptionsControl foptctrl, MainForm mainFrm)
        {
            InitializeComponent();
            fileOptionsControl = foptctrl;
            mainForm = mainFrm;
        }

        public FileOptionsControl SetFileOptionsObject
        {
            get { return fileOptionsControl; }
            set { fileOptionsControl = value; }
        }

        //for file button
        Timer timer = new Timer();

        private void RemoveFileOptionsControl()
        {
            timer.Interval = 60;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            MainForm.width -= 400; //width changing, first slow load
            fileOptionsControl.Width = MainForm.width;

            if (MainForm.width <= 200)
            {
                mainForm.TopPanel.Separator = 0;
                mainForm.ButtonPanel.Visible = true;
                mainForm.TextPanel.Visible = true;
                mainForm.MainPanel.Controls.Remove(fileOptionsControl);
                timer.Stop();
            }
        }

        //file button end

        private void backButton1_Click(object sender, EventArgs e) //back button
        {
            fileOptionsControl.Dock = DockStyle.None;
            RemoveFileOptionsControl();
        }

        private void button7_Click(object sender, EventArgs e) //info button 
        {
            //
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FileOptionsControl_Load_1(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)//Save button
        {
            mainForm.MainRichTextBox.SaveDocument();
        }

        private void button4_Click(object sender, EventArgs e)//Save as button
        {
            mainForm.MainRichTextBox.SaveDocumentAs();
        }

        private void button1_Click(object sender, EventArgs e) //New Button
        {
            // create new file means clear all text

            mainForm.MainRichTextBox.Text = "";

            //back to editor section again

            fileOptionsControl.Dock = DockStyle.None; 
            RemoveFileOptionsControl();
        }

        private void button8_Click(object sender, EventArgs e)//close button
        {
            mainForm.Close();
        }

        private void button2_Click(object sender, EventArgs e) //open button
        {
            mainForm.MainRichTextBox.LoadDocument();
            fileOptionsControl.Dock = DockStyle.None;
            RemoveFileOptionsControl();
        }

        private void button5_Click(object sender, EventArgs e) //print
        {
            PrintDialog print = new PrintDialog();
            print.ShowDialog();
        }
    }
}
