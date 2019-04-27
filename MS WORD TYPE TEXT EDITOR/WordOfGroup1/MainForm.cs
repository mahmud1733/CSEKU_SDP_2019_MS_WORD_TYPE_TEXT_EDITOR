using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Linq;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraBars;
using DevExpress.XtraRichEdit.API.Native;
using System.Text.RegularExpressions;
using DevExpress.XtraRichEdit.Forms;

namespace WordOfGroup1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        bool isTopPanelDragged = false;
        bool isLeftPanelDragged = false;
        bool isRightPanelDragged = false;
        bool isBottomPanelDragged = false;
        bool isTopBorderPanelDragged = false;

        bool isRightBottomPanelDragged = false;
        bool isLeftBottomPanelDragged = false;
        bool isRightTopPanelDragged = false;
        bool isLeftTopPanelDragged = false;

        bool isWindowMaximized = false;
        Point offset;
        Size _normalWindowSize;
        Point _normalWindowLocation = Point.Empty;

        public object DocumentModel { get; private set; }


        //********************************************************************
        // TopBorderPanel
        private void TopBorderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isTopBorderPanelDragged = true;
            }
            else
            {
                isTopBorderPanelDragged = false;
            }
        }

        private void TopBorderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Location.Y)
            {
                if (isTopBorderPanelDragged)
                {
                    if (this.Height < 50)
                    {
                        this.Height = 50;
                        isTopBorderPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                    }
                }
            }
        }

        private void TopBorderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopBorderPanelDragged = false;
        }


        //********************************************************************
        // TopPanel
        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isTopPanelDragged = true;
                Point pointStartPosition = this.PointToScreen(new Point(e.X, e.Y));
                offset = new Point();
                offset.X = this.Location.X - pointStartPosition.X;
                offset.Y = this.Location.Y - pointStartPosition.Y;
            }
            else
            {
                isTopPanelDragged = false;
            }
            if (e.Clicks == 2)
            {
                isTopPanelDragged = false;
                minMaxButton1_Click(sender, e);
            }
        }

        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTopPanelDragged)
            {
                Point newPoint = TopPanel.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(offset);
                this.Location = newPoint;

                if (this.Location.X > 2 || this.Location.Y > 2)
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        this.Location = _normalWindowLocation;
                        this.Size = _normalWindowSize;
                        toolTip1.SetToolTip(minMaxButton, "Maximize");
                        minMaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
                        isWindowMaximized = false;
                    }
                }
            }
        }

        private void TopPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopPanelDragged = false;
            if (this.Location.Y <= 5)
            {
                if (!isWindowMaximized)
                {
                    _normalWindowSize = this.Size;
                    _normalWindowLocation = this.Location;

                    Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                    this.Location = new Point(0, 0);
                    this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                    toolTip1.SetToolTip(minMaxButton, "Restore Down");
                    minMaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
                    isWindowMaximized = true;
                }
            }
        }


        //********************************************************************
        // LeftPanel
        private void LeftPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Location.X <= 0 || e.X < 0)
            {
                isLeftPanelDragged = false;
                this.Location = new Point(10, this.Location.Y);
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    isLeftPanelDragged = true;
                }
                else
                {
                    isLeftPanelDragged = false;
                }
            }
        }

        private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X)
            {
                if (isLeftPanelDragged)
                {
                    if (this.Width < 100)
                    {
                        this.Width = 100;
                        isLeftPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        /*FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);*/
                    }
                }
            }
        }

        private void LeftPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftPanelDragged = false;
        }


        //********************************************************************
        // RightPanel
        private void RightPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isRightPanelDragged = true;
            }
            else
            {
                isRightPanelDragged = false;
            }
        }

        private void RightPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightPanelDragged)
            {
                if (this.Width < 100)
                {
                    this.Width = 100;
                    isRightPanelDragged = false;
                }
                else
                {
                    this.Width = this.Width + e.X;
                    /*FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);*/
                }
            }
        }

        private void RightPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isRightPanelDragged = false;
        }


        //********************************************************************
        // BottomPanel
        private void BottomPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isBottomPanelDragged = true;
            }
            else
            {
                isBottomPanelDragged = false;
            }
        }

        private void BottomPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBottomPanelDragged)
            {
                if (this.Height < 50)
                {
                    this.Height = 50;
                    isBottomPanelDragged = false;
                }
                else
                {
                    this.Height = this.Height + e.Y;
                }
            }
        }

        private void BottomPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isBottomPanelDragged = false;
        }


        //********************************************************************
        // RightBottomPanel 1
        private void RightBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isRightBottomPanelDragged = true;
        }

        private void RightBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightBottomPanelDragged)
            {
                if (this.Width < 100 || this.Height < 50)
                {
                    this.Width = 100;
                    this.Height = 50;
                    isRightBottomPanelDragged = false;
                }
                else
                {
                    this.Width = this.Width + e.X;
                    this.Height = this.Height + e.Y;
                    /*FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);*/
                }
            }
        }

        private void RightBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isRightBottomPanelDragged = false;
        }

        //********************************************************************
        // RightBottomPanel 2
        private void RightBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseDown(sender, e);
        }

        private void RightBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseMove(sender, e);
        }

        private void RightBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseUp(sender, e);
        }


        //********************************************************************
        // LeftBottomPanel 1
        private void LeftBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftBottomPanelDragged = true;
        }

        private void LeftBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X)
            {
                if (isLeftBottomPanelDragged || this.Height < 50)
                {
                    if (this.Width < 100)
                    {
                        this.Width = 100;
                        this.Height = 50;
                        isLeftBottomPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        this.Height = this.Height + e.Y;
                        /*FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);*/
                    }
                }
            }
        }

        private void LeftBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftBottomPanelDragged = false;
        }


        //********************************************************************
        // LeftBottomPanel 2
        private void LeftBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseDown(sender, e);
        }

        private void LeftBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseMove(sender, e);
        }

        private void LeftBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseUp(sender, e);
        }



        //********************************************************************
        // RightTopPanel 1
        private void RightTopPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isRightTopPanelDragged = true;
        }

        private void RightTopPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Location.Y || e.X < this.Location.X)
            {
                if (isRightTopPanelDragged)
                {
                    if (this.Height < 50 || this.Width < 100)
                    {
                        this.Height = 50;
                        this.Width = 100;
                        isRightTopPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                        this.Width = this.Width + e.X;
                        /*FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);*/
                    }
                }
            }
        }

        private void RightTopPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isRightTopPanelDragged = false;
        }

        //********************************************************************
        // RightTopPanel 2
        private void RightTopPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseDown(sender, e);
        }

        private void RightTopPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseMove(sender, e);
        }

        private void RightTopPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseUp(sender, e);
        }


        //********************************************************************
        // LeftTopPanel 1
        private void LeftTopPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftTopPanelDragged = true;
        }

        private void LeftTopPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X || e.Y < this.Location.Y)
            {
                if (isLeftTopPanelDragged)
                {
                    if (this.Width < 100 || this.Height < 50)
                    {
                        this.Width = 100;
                        this.Height = 100;
                        isLeftTopPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                        /*FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);*/
                    }
                }
            }

        }

        private void LeftTopPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftTopPanelDragged = false;
        }


        //********************************************************************
        // LeftTopPanel 2
        private void LeftTopPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseDown(sender, e);
        }

        private void LeftTopPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseMove(sender, e);
        }

        private void LeftTopPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseUp(sender, e);
        }

        //FormText
        private void FormText_MouseDown(object sender, MouseEventArgs e)
        {
            TopPanel_MouseDown(sender, e);
        }

        private void FormText_MouseMove(object sender, MouseEventArgs e)
        {
            TopPanel_MouseMove(sender, e);
        }

        private void FormText_MouseUp(object sender, MouseEventArgs e)
        {
            TopPanel_MouseUp(sender, e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void TopPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void minMaxButton1_Click(object sender, EventArgs e)
        {

            if (isWindowMaximized)
            {
                this.Location = _normalWindowLocation;
                this.Size = _normalWindowSize;
                toolTip1.SetToolTip(minMaxButton, "Maximize");
                minMaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
                isWindowMaximized = false;

                /*FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);*/
            }
            else
            {
                _normalWindowSize = this.Size;
                _normalWindowLocation = this.Location;

                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Location = new Point(0, 0);
                this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                toolTip1.SetToolTip(minMaxButton, "Restore Down");
                minMaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
                isWindowMaximized = true;

                /*FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length,
                                            9);*/
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }



        private FontStyle ToggleFontStyle(FontStyle item, FontStyle toggle)
        {
            return item ^ toggle;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.MainRichTextBox.CanUndo)
                this.MainRichTextBox.Undo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.MainRichTextBox.CanRedo)
                this.MainRichTextBox.Redo();
        }

        private void button7_Click(object sender, EventArgs e)//increase botton font
        {
            try
            {
                this.MainRichTextBox.Font = new Font(this.MainRichTextBox.Font.FontFamily, this.MainRichTextBox.Font.Size + 1, this.MainRichTextBox.Font.Style);
            }
            catch { }
        }

        private void button8_Click(object sender, EventArgs e)//decrease font
        {
            try
            {
                this.MainRichTextBox.Font = new Font(this.MainRichTextBox.Font.FontFamily, this.MainRichTextBox.Font.Size - 1, this.MainRichTextBox.Font.Style);
            }
            catch { }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // this.MainRichTextBox.Font = System.Windows.Forms.HorizontalAlignment.Left;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //this.MainRichTextBox.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // this.MainRichTextBox.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Right;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //this.MainRichTextBox.SelectionAlignment = System.Windows.Forms.HorizontalAlignment
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //this.MainRichTextBox.SelectionIndent += 10;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //this.MainRichTextBox.SelectionIndent -= 10;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //this.MainRichTextBox.SelectionBullet = !this.MainRichTextBox.SelectionBullet;
        }


        //For the File button

        Timer timer = new Timer();
        FileOptionsControl fileOptionsControl;
        public static int width = 200;
        int count = 1;

        private void AddFileOptionsControl()
        {
            timer.Interval = 160;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            width += 400; //for slider timer fast slow
            count++;
            fileOptionsControl.Width = width;

            if (width >= MainPanel.Width - 100)
            {
                fileOptionsControl.Dock = DockStyle.Fill;
                timer.Stop();
            }
        }

        private void FileButton_Click(object sender, EventArgs e)
        {
            ButtonPanel.Visible = false;
            TextPanel.Visible = false;
            TopPanel.Separator = 125;
            fileOptionsControl = new FileOptionsControl(fileOptionsControl, this);
            fileOptionsControl.Location = new Point(0, 0);
            fileOptionsControl.Height = MainPanel.Height;
            MainPanel.Controls.Add(fileOptionsControl);
            fileOptionsControl.SetFileOptionsObject = fileOptionsControl;
            AddFileOptionsControl();
        }

        private void button1_Click_2(object sender, EventArgs e) //big paste button
        {
            MainRichTextBox.Paste();
        }

        private void button7_Click_1(object sender, EventArgs e) //click increase button
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontSize = crp.FontSize + 1;

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void CUT_Click_1(object sender, EventArgs e)//cut button
        {
            MainRichTextBox.Cut();
        }

        private void PASTE_Click_1(object sender, EventArgs e) //small paste button
        {
            MainRichTextBox.Paste();
        }

        private void Copy_Click_1(object sender, EventArgs e) //copy button
        {
            MainRichTextBox.Copy();
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)//panel buttons 
        {
            //ading all font family to conbobox fontname

            foreach (FontFamily font in FontFamily.Families)
            {
                FontNameBox.Items.Add(font.Name.ToString());
            }
        }

        private void FontName_SelectedIndexChanged_1(object sender, EventArgs e)//work of font name combobox
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontName = FontNameBox.Text; // crp.FontName = "Arial";

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void FontSize_SelectedIndexChanged(object sender, EventArgs e)//work of font size combobox
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontSize = float.Parse(FontSizeBox.SelectedItem.ToString()); //parse = string to float

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void DecreaseButton_Click(object sender, EventArgs e) //Click decrease button
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontSize = crp.FontSize - 1;

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void Bold_Click(object sender, EventArgs e) //bold button
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            if (crp.Bold == false)
            {
                crp.Bold = true;
            }
            else
            {
                crp.Bold = false;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void Italic_Click_1(object sender, EventArgs e)//Italic button
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);
            if (crp.Italic == false)
            {
                crp.Italic = true;
            }
            else
            {
                crp.Italic = false;
            }
            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void Underline_Click_1(object sender, EventArgs e)//Underline button
        {
            /*RichEditCommand underlineCommand = MainRichTextBox.CreateCommand(RichEditCommandId.ToggleFontUnderline);
            underlineCommand.ForceExecute(underlineCommand.CreateDefaultCommandUIState());*/

            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);


            if (crp.Underline == UnderlineType.None)
            {
                crp.Underline = UnderlineType.Single;
            }
            else
            {
                crp.Underline = UnderlineType.None;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);

        }

        private void Strikrout_Click(object sender, EventArgs e)//Strikeout button
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);


            if (crp.Strikeout == StrikeoutType.None)
            {
                crp.Strikeout = StrikeoutType.Single;
            }
            else
            {
                crp.Strikeout = StrikeoutType.None;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void FontColor_Click(object sender, EventArgs e) //Font Text color or fore color of text
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            ColorDialog col = new ColorDialog();
            col.ShowDialog();
            crp.ForeColor = col.Color;

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void BackColor_Click(object sender, EventArgs e) //Text Back Color, like highlighting
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            ColorDialog col = new ColorDialog();
            col.ShowDialog();
            crp.BackColor = col.Color;

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void SearchButton_Click(object sender, EventArgs e) //search a word
        {
            try
            {
                ISearchResult searchResult = MainRichTextBox.Document.StartSearch(SearchBox.Text);
                while (searchResult.FindNext())
                {
                    CharacterProperties cp = MainRichTextBox.Document.BeginUpdateCharacters(searchResult.CurrentResult);
                    //cp.Bold = true;
                    //cp.ForeColor = System.Drawing.Color.Blue;
                    cp.BackColor = Color.YellowGreen;
                    //cp.Underline = UnderlineType.ThickSingle;
                    MainRichTextBox.Document.EndUpdateCharacters(cp);
                }
            }
            catch { }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Document document = MainRichTextBox.Document;
            document.BeginUpdate();

            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(document.Range);
            crp.BackColor = Color.White;
            MainRichTextBox.Document.EndUpdateCharacters(crp);

            document.EndUpdate();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            //
        }

        //find panel slider

        private void FindPanel_Paint(object sender, PaintEventArgs e) //hiding find panel
        {
            //FindPanel.Visible = false;
        }

        private bool isCollapsed = true;
        //private int flag1;
        // private string i;

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                FindPanel.Height += 100;
                MainRichTextBox.Width -= 100;
                if (FindPanel.Size == FindPanel.MaximumSize)
                {
                    timer2.Stop();
                    isCollapsed = false;
                }

            }
            else
            {
                FindPanel.Height -= 100;
                MainRichTextBox.Width += 100;
                if (FindPanel.Size == FindPanel.MinimumSize)
                {
                    timer2.Stop();
                    isCollapsed = true;
                }
            }
        }

        private void FindButton_Click_1(object sender, EventArgs e)
        {
            timer2.Start();
            /*MainRichTextBox.SelectionStart = 0;
            MainRichTextBox.SelectAll();
            MainRichTextBox.SelectionBackColor = Color.White;*/
        }

        private void Bullet_Click(object sender, EventArgs e) // bulleting numbers
        {
            RichEditCommand bulletCommand = MainRichTextBox.CreateCommand(RichEditCommandId.ToggleBulletedListItem);
            bulletCommand.ForceExecute(bulletCommand.CreateDefaultCommandUIState());
        }

        private void Number_Click(object sender, EventArgs e)//number list
        {
            RichEditCommand numberlist = MainRichTextBox.CreateCommand(RichEditCommandId.ToggleNumberingListItem);
            numberlist.ForceExecute(numberlist.CreateDefaultCommandUIState());
        }

        private void button15_Click_1(object sender, EventArgs e)//right indendetation scaling
        {
            RichEditCommand increaseIndent = MainRichTextBox.CreateCommand(RichEditCommandId.IncreaseIndent);
            increaseIndent.ForceExecute(increaseIndent.CreateDefaultCommandUIState());
        }

        private void button16_Click_1(object sender, EventArgs e)//left indendetation scaling
        {
            RichEditCommand decreaseIndent = MainRichTextBox.CreateCommand(RichEditCommandId.DecreaseIndent);
            decreaseIndent.ForceExecute(decreaseIndent.CreateDefaultCommandUIState());
        }

        private void LeftAlign_Click(object sender, EventArgs e) //left allign
        {
            RichEditCommand paragraphAlignmentLeft = MainRichTextBox.CreateCommand(RichEditCommandId.ToggleParagraphAlignmentLeft);
            paragraphAlignmentLeft.ForceExecute(paragraphAlignmentLeft.CreateDefaultCommandUIState());
        }

        private void RightAlign_Click(object sender, EventArgs e) //right allign
        {
            RichEditCommand paragraphAlignmentRight = MainRichTextBox.CreateCommand(RichEditCommandId.ToggleParagraphAlignmentRight);
            paragraphAlignmentRight.ForceExecute(paragraphAlignmentRight.CreateDefaultCommandUIState());
        }

        private void MiddleAlign_Click(object sender, EventArgs e) //middle allign
        {
            RichEditCommand paragraphAlignmentCenter = MainRichTextBox.CreateCommand(RichEditCommandId.ToggleParagraphAlignmentCenter);
            paragraphAlignmentCenter.ForceExecute(paragraphAlignmentCenter.CreateDefaultCommandUIState());
        }

        private void Justify_Click(object sender, EventArgs e) //allign justify
        {
            RichEditCommand paragraphAlignmentJustify = MainRichTextBox.CreateCommand(RichEditCommandId.ToggleParagraphAlignmentJustify);
            paragraphAlignmentJustify.ForceExecute(paragraphAlignmentJustify.CreateDefaultCommandUIState());
        }

        private void Heading1Button_Click(object sender, EventArgs e) //heading 1
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontName = crp.FontName;
            crp.FontSize = 16;
            crp.ForeColor = Color.Blue;

            if (crp.Bold == true)
            {
                crp.Bold = false;
            }

            if (crp.Italic == true)
            {
                crp.Italic = false;
            }

            if (crp.Underline == UnderlineType.Single)
            {
                crp.Underline = UnderlineType.None;
            }

            if (crp.Strikeout == StrikeoutType.Single)
            {
                crp.Strikeout = StrikeoutType.None;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void Heading2Button_Click(object sender, EventArgs e) //heading 2
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontName = crp.FontName;
            crp.FontSize = 13;
            crp.ForeColor = Color.Blue;

            if (crp.Bold == true)
            {
                crp.Bold = false;
            }

            if (crp.Italic == true)
            {
                crp.Italic = false;
            }

            if (crp.Underline == UnderlineType.Single)
            {
                crp.Underline = UnderlineType.None;
            }

            if (crp.Strikeout == StrikeoutType.Single)
            {
                crp.Strikeout = StrikeoutType.None;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void NormalButton_Click(object sender, EventArgs e) //normal button
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontName = "Arial";
            crp.FontSize = 12;
            crp.ForeColor = Color.Black;
            crp.BackColor = Color.White;

            if (crp.Bold == true)
            {
                crp.Bold = false;
            }

            if (crp.Italic == true)
            {
                crp.Italic = false;
            }

            if (crp.Underline == UnderlineType.Single)
            {
                crp.Underline = UnderlineType.None;
            }

            if (crp.Strikeout == StrikeoutType.Single)
            {
                crp.Strikeout = StrikeoutType.None;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void TitleButton_Click(object sender, EventArgs e) //title button
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontName = crp.FontName;
            crp.FontSize = 28;
            crp.ForeColor = Color.Black;

            if (crp.Bold == true)
            {
                crp.Bold = false;
            }

            if (crp.Italic == true)
            {
                crp.Italic = false;
            }

            if (crp.Underline == UnderlineType.Single)
            {
                crp.Underline = UnderlineType.None;
            }

            if (crp.Strikeout == StrikeoutType.Single)
            {
                crp.Strikeout = StrikeoutType.None;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void Subtitle_Click(object sender, EventArgs e) //subtitle
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontName = crp.FontName;
            crp.FontSize = 11;
            crp.ForeColor = Color.Gray;

            if (crp.Bold == true)
            {
                crp.Bold = false;
            }

            if (crp.Italic == true)
            {
                crp.Italic = false;
            }

            if (crp.Underline == UnderlineType.Single)
            {
                crp.Underline = UnderlineType.None;
            }

            if (crp.Strikeout == StrikeoutType.Single)
            {
                crp.Strikeout = StrikeoutType.None;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void StrongButton_Click(object sender, EventArgs e) //strong
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontName = crp.FontName;
            crp.FontSize = 12;
            crp.ForeColor = Color.Black;

            if (crp.Bold == false)
            {
                crp.Bold = true;
            }

            if (crp.Italic == true)
            {
                crp.Italic = false;
            }

            if (crp.Underline == UnderlineType.Single)
            {
                crp.Underline = UnderlineType.None;
            }

            if (crp.Strikeout == StrikeoutType.Single)
            {
                crp.Strikeout = StrikeoutType.None;
            }

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void QuoteButton_Click(object sender, EventArgs e) //quoute
        {
            DocumentRange range = MainRichTextBox.Document.Selection;
            CharacterProperties crp = MainRichTextBox.Document.BeginUpdateCharacters(range);

            crp.FontName = crp.FontName;
            crp.FontSize = 12;
            crp.ForeColor = Color.Black;

            if (crp.Italic == false)
            {
                crp.Italic = true;
            }

            RichEditCommand paragraphAlignmentCenter = MainRichTextBox.CreateCommand(RichEditCommandId.ToggleParagraphAlignmentCenter);
            paragraphAlignmentCenter.ForceExecute(paragraphAlignmentCenter.CreateDefaultCommandUIState());

            MainRichTextBox.Document.EndUpdateCharacters(crp);
        }

        private void MainRichTextBox_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RichEditCommand bulletedlist = MainRichTextBox.CreateCommand(RichEditCommandId.None);
            bulletedlist.ForceExecute(bulletedlist.CreateDefaultCommandUIState());
        }

        private void button1_Click_1(object sender, EventArgs e) //search panel close
        {
            FindPanel.Height -= 200;
            MainRichTextBox.Width += 200;
            if (FindPanel.Size == FindPanel.MinimumSize)
            {
                timer2.Stop();
                isCollapsed = true;
            }
        }
        

        private void button2_Click(object sender, EventArgs e)// table
        {
            timer4.Start();
        }

        private void timer3_Tick(object sender, EventArgs e) // insert 
        {
            panel2.Visible = false;
            rowColumnPanel.Visible = false;
            InsertPanel.Visible = true;
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            timer3.Start();
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            if(InsertPanel.Visible == true)
            {
                InsertPanel.Visible = false;
            }
            if(rowColumnPanel.Visible == true)
            {
                rowColumnPanel.Visible = false;
            }
            timer3.Stop();
            timer4.Stop();
        }

        private void timer4_Tick(object sender, EventArgs e) // row column
        {
            timer3.Stop();
            panel2.Visible = false;
            InsertPanel.Visible = true;
            rowColumnPanel.Visible = true;
        }
        int start = 0;

        private void button6_Click(object sender, EventArgs e) //ok and table
        {
            start = MainRichTextBox.Text.Length;
            //int start = 0;
            Document doc = MainRichTextBox.Document;
            Table table1 = doc.InsertTable(doc.CreatePosition(start), int.Parse(rowbox.SelectedItem.ToString()), int.Parse(columnbox.SelectedItem.ToString())); // position, row, column
            table1.BeginUpdate();

            CharacterProperties cp = doc.BeginUpdateCharacters(table1.Rows[0].Range);
            cp.Bold = true;
            doc.EndUpdateCharacters(cp);

            table1.ForEachCell((cell, rowIndex, cellIndex) =>
            {
                doc.InsertText(cell.ContentRange.Start, "");
            });
            table1.EndUpdate();
        }

        private void button7_Click_2(object sender, EventArgs e) //image insert button
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPG|*.jpg|png|*.png|Bitmap|*.bmp", ValidateNames = true, Multiselect = false })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        MainRichTextBox.Document.Shapes.InsertPicture(MainRichTextBox.Document.Range.Start, Image.FromFile(ofd.FileName));
                        //MainRichTextBox.im = Image.FromFile(ofd.FileName);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
