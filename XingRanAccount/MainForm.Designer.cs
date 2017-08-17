namespace XingRanAccount
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelectSource = new System.Windows.Forms.Button();
            this.btnDo = new System.Windows.Forms.Button();
            this.sourceFileLalel = new System.Windows.Forms.Label();
            this.btnSelectScope = new System.Windows.Forms.Button();
            this.scopeFileLalel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSelectSource
            // 
            this.btnSelectSource.Location = new System.Drawing.Point(49, 27);
            this.btnSelectSource.Name = "btnSelectSource";
            this.btnSelectSource.Size = new System.Drawing.Size(165, 23);
            this.btnSelectSource.TabIndex = 0;
            this.btnSelectSource.Text = "select source file";
            this.btnSelectSource.UseVisualStyleBackColor = true;
            this.btnSelectSource.Click += new System.EventHandler(this.btnSelectSource_Click);
            // 
            // btnDo
            // 
            this.btnDo.Location = new System.Drawing.Point(49, 149);
            this.btnDo.Name = "btnDo";
            this.btnDo.Size = new System.Drawing.Size(75, 23);
            this.btnDo.TabIndex = 2;
            this.btnDo.Text = "do";
            this.btnDo.UseVisualStyleBackColor = true;
            this.btnDo.Click += new System.EventHandler(this.btnDo_Click);
            // 
            // sourceFileLalel
            // 
            this.sourceFileLalel.AutoSize = true;
            this.sourceFileLalel.Location = new System.Drawing.Point(235, 32);
            this.sourceFileLalel.Name = "sourceFileLalel";
            this.sourceFileLalel.Size = new System.Drawing.Size(0, 13);
            this.sourceFileLalel.TabIndex = 3;
            // 
            // btnSelectScope
            // 
            this.btnSelectScope.Location = new System.Drawing.Point(49, 76);
            this.btnSelectScope.Name = "btnSelectScope";
            this.btnSelectScope.Size = new System.Drawing.Size(165, 23);
            this.btnSelectScope.TabIndex = 0;
            this.btnSelectScope.Text = "select scope file";
            this.btnSelectScope.UseVisualStyleBackColor = true;
            this.btnSelectScope.Click += new System.EventHandler(this.btnSelectScope_Click);
            // 
            // sourceScopeFileLalel
            // 
            this.scopeFileLalel.AutoSize = true;
            this.scopeFileLalel.Location = new System.Drawing.Point(235, 85);
            this.scopeFileLalel.Name = "scopeFileLalel";
            this.scopeFileLalel.Size = new System.Drawing.Size(0, 13);
            this.scopeFileLalel.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 359);
            this.Controls.Add(this.scopeFileLalel);
            this.Controls.Add(this.sourceFileLalel);
            this.Controls.Add(this.btnDo);
            this.Controls.Add(this.btnSelectScope);
            this.Controls.Add(this.btnSelectSource);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSelectSource;
        private System.Windows.Forms.Button btnDo;
        private System.Windows.Forms.Label sourceFileLalel;
        private System.Windows.Forms.Button btnSelectScope;
        private System.Windows.Forms.Label scopeFileLalel;
    }
}

