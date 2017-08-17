using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCS.Library.Office.OpenXml.Excel;
using XingRanAccountTest;

namespace XingRanAccount
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private string _sourceFilePath = null;
        private string _scopeFilePath = null;


        private void btnSelectSource_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _sourceFilePath = openFileDialog.FileName;
                sourceFileLalel.Text = _sourceFilePath;
            }
        }

        private void btnSelectScope_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _scopeFilePath = openFileDialog.FileName;
                scopeFileLalel.Text = _scopeFilePath;
            }
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            WorkBook sourceBook = WorkBook.Load(_sourceFilePath);
            WorkBook scopeBook = WorkBook.Load(_scopeFilePath);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string msg;
            bool pass = ExcelDo.Do(sourceBook, scopeBook, out msg);
            if (pass)
            {
                scopeBook.Save(saveFileDialog.FileName + ".xlsx");
                MessageBox.Show(this, $"处理完毕:{saveFileDialog.FileName}");
            }
            else
            {
                MessageBox.Show(this, msg);
            }
        }
    }
}
