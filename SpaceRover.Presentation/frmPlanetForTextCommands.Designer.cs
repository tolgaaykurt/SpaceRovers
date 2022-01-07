namespace SpaceRovers
{
    partial class frmPlanetForTextCommands
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
            this.pnlCommandPanel = new System.Windows.Forms.Panel();
            this.btnBorderViolation = new System.Windows.Forms.Button();
            this.brnCrashTest = new System.Windows.Forms.Button();
            this.btnWaitingForAnotherRoverTest = new System.Windows.Forms.Button();
            this.pnlPlateau = new System.Windows.Forms.Panel();
            this.btnMistakeCommand = new System.Windows.Forms.Button();
            this.pnlCommandPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCommandPanel
            // 
            this.pnlCommandPanel.Controls.Add(this.btnMistakeCommand);
            this.pnlCommandPanel.Controls.Add(this.btnBorderViolation);
            this.pnlCommandPanel.Controls.Add(this.brnCrashTest);
            this.pnlCommandPanel.Controls.Add(this.btnWaitingForAnotherRoverTest);
            this.pnlCommandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCommandPanel.Location = new System.Drawing.Point(0, 670);
            this.pnlCommandPanel.Name = "pnlCommandPanel";
            this.pnlCommandPanel.Size = new System.Drawing.Size(1397, 42);
            this.pnlCommandPanel.TabIndex = 1;
            // 
            // btnBorderViolation
            // 
            this.btnBorderViolation.Location = new System.Drawing.Point(331, 7);
            this.btnBorderViolation.Name = "btnBorderViolation";
            this.btnBorderViolation.Size = new System.Drawing.Size(95, 23);
            this.btnBorderViolation.TabIndex = 4;
            this.btnBorderViolation.Text = "Sınır İhlali Testi";
            this.btnBorderViolation.UseVisualStyleBackColor = true;
            this.btnBorderViolation.Click += new System.EventHandler(this.btnBorderViolation_Click);
            // 
            // brnCrashTest
            // 
            this.brnCrashTest.Location = new System.Drawing.Point(221, 7);
            this.brnCrashTest.Name = "brnCrashTest";
            this.brnCrashTest.Size = new System.Drawing.Size(104, 23);
            this.brnCrashTest.TabIndex = 3;
            this.brnCrashTest.Text = "Çarpışma Testi";
            this.brnCrashTest.UseVisualStyleBackColor = true;
            this.brnCrashTest.Click += new System.EventHandler(this.brnCrashTest_Click);
            // 
            // btnWaitingForAnotherRoverTest
            // 
            this.btnWaitingForAnotherRoverTest.Location = new System.Drawing.Point(12, 7);
            this.btnWaitingForAnotherRoverTest.Name = "btnWaitingForAnotherRoverTest";
            this.btnWaitingForAnotherRoverTest.Size = new System.Drawing.Size(203, 23);
            this.btnWaitingForAnotherRoverTest.TabIndex = 2;
            this.btnWaitingForAnotherRoverTest.Text = "Birbirlerinin Hareketlerini Bekleme Testi";
            this.btnWaitingForAnotherRoverTest.UseVisualStyleBackColor = true;
            this.btnWaitingForAnotherRoverTest.Click += new System.EventHandler(this.btnWaitingForAnotherRoverTest_Click);
            // 
            // pnlPlateau
            // 
            this.pnlPlateau.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlateau.Location = new System.Drawing.Point(0, 0);
            this.pnlPlateau.Name = "pnlPlateau";
            this.pnlPlateau.Size = new System.Drawing.Size(1397, 670);
            this.pnlPlateau.TabIndex = 2;
            // 
            // btnMistakeCommand
            // 
            this.btnMistakeCommand.Location = new System.Drawing.Point(433, 7);
            this.btnMistakeCommand.Name = "btnMistakeCommand";
            this.btnMistakeCommand.Size = new System.Drawing.Size(100, 23);
            this.btnMistakeCommand.TabIndex = 5;
            this.btnMistakeCommand.Text = "Hatalı Komut Testi";
            this.btnMistakeCommand.UseVisualStyleBackColor = true;
            this.btnMistakeCommand.Click += new System.EventHandler(this.btnMistakeCommand_Click);
            // 
            // frmPlanetForTextCommands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1397, 712);
            this.Controls.Add(this.pnlPlateau);
            this.Controls.Add(this.pnlCommandPanel);
            this.Name = "frmPlanetForTextCommands";
            this.Text = "Space Rovers";
            this.pnlCommandPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlCommandPanel;
        private System.Windows.Forms.Button btnWaitingForAnotherRoverTest;
        private System.Windows.Forms.Panel pnlPlateau;
        private System.Windows.Forms.Button btnBorderViolation;
        private System.Windows.Forms.Button brnCrashTest;
        private System.Windows.Forms.Button btnMistakeCommand;
    }
}