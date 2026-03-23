namespace Project_v1
{
    partial class bench
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.backtomainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startBenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startonlyjarvisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backtomainToolStripMenuItem,
            this.startBenchToolStripMenuItem,
            this.startonlyjarvisToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // backtomainToolStripMenuItem
            // 
            this.backtomainToolStripMenuItem.Name = "backtomainToolStripMenuItem";
            this.backtomainToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.backtomainToolStripMenuItem.Text = "Back_to_main";
            this.backtomainToolStripMenuItem.Click += new System.EventHandler(this.backtomainToolStripMenuItem_Click);
            // 
            // startBenchToolStripMenuItem
            // 
            this.startBenchToolStripMenuItem.Name = "startBenchToolStripMenuItem";
            this.startBenchToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.startBenchToolStripMenuItem.Text = "Start bench";
            this.startBenchToolStripMenuItem.Click += new System.EventHandler(this.startBenchToolStripMenuItem_Click);
            this.startBenchToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.startBenchToolStripMenuItem_Paint);
            // 
            // startonlyjarvisToolStripMenuItem
            // 
            this.startonlyjarvisToolStripMenuItem.Name = "startonlyjarvisToolStripMenuItem";
            this.startonlyjarvisToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.startonlyjarvisToolStripMenuItem.Text = "start_only_jarvis";
            this.startonlyjarvisToolStripMenuItem.Click += new System.EventHandler(this.startonlyjarvisToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // bench
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "bench";
            this.Text = "bench";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.bench_FormClosing);
            this.Load += new System.EventHandler(this.bench_Load);
            this.SizeChanged += new System.EventHandler(this.bench_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.bench_Paint);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem backtomainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startBenchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startonlyjarvisToolStripMenuItem;
        private System.Windows.Forms.Label label1;
    }
}