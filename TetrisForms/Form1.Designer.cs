namespace TetrisForms
{
    partial class Forms
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            File = new ToolStripMenuItem();
            NewGame = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            Save = new ToolStripMenuItem();
            Load = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            Exit = new ToolStripMenuItem();
            Size = new ToolStripMenuItem();
            SmallSize = new ToolStripMenuItem();
            MediumSize = new ToolStripMenuItem();
            LargeSize = new ToolStripMenuItem();
            Pause = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            Strip = new ToolStripStatusLabel();
            TimeStrip = new ToolStripStatusLabel();
            panel = new PictureBox();
            saveFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panel).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { File, Size, Pause });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1089, 40);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // File
            // 
            File.DropDownItems.AddRange(new ToolStripItem[] { NewGame, toolStripSeparator1, Save, Load, toolStripSeparator2, Exit });
            File.Name = "File";
            File.Size = new Size(71, 36);
            File.Text = "File";
            // 
            // NewGame
            // 
            NewGame.Name = "NewGame";
            NewGame.Size = new Size(264, 44);
            NewGame.Text = "New Game";
            NewGame.Click += NewGame_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(261, 6);
            // 
            // Save
            // 
            Save.Name = "Save";
            Save.Size = new Size(264, 44);
            Save.Text = "Save";
            Save.Click += Save_Click;
            // 
            // Load
            // 
            Load.Name = "Load";
            Load.Size = new Size(264, 44);
            Load.Text = "Load";
            Load.Click += Load_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(261, 6);
            // 
            // Exit
            // 
            Exit.Name = "Exit";
            Exit.Size = new Size(264, 44);
            Exit.Text = "Exit";
            Exit.Click += Exit_Click;
            // 
            // Size
            // 
            Size.DropDownItems.AddRange(new ToolStripItem[] { SmallSize, MediumSize, LargeSize });
            Size.Name = "Size";
            Size.Size = new Size(143, 36);
            Size.Text = "Game size";
            // 
            // SmallSize
            // 
            SmallSize.Name = "SmallSize";
            SmallSize.Size = new Size(237, 44);
            SmallSize.Text = "Small";
            SmallSize.Click += SmallSize_Click;
            // 
            // MediumSize
            // 
            MediumSize.Name = "MediumSize";
            MediumSize.Size = new Size(237, 44);
            MediumSize.Text = "Medium";
            MediumSize.Click += MediumSize_Click;
            // 
            // LargeSize
            // 
            LargeSize.Name = "LargeSize";
            LargeSize.Size = new Size(237, 44);
            LargeSize.Text = "Large";
            LargeSize.Click += LargeSize_Click;
            // 
            // Pause
            // 
            Pause.Name = "Pause";
            Pause.Size = new Size(95, 36);
            Pause.Text = "Pause";
            Pause.Click += Pause_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(32, 32);
            statusStrip1.Items.AddRange(new ToolStripItem[] { Strip, TimeStrip });
            statusStrip1.Location = new Point(0, 1138);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1089, 42);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // Strip
            // 
            Strip.Name = "Strip";
            Strip.Size = new Size(72, 32);
            Strip.Text = "Time:";
            // 
            // TimeStrip
            // 
            TimeStrip.Name = "TimeStrip";
            TimeStrip.Size = new Size(89, 32);
            TimeStrip.Text = "0:00:00";
            // 
            // panel
            // 
            panel.Location = new Point(198, 117);
            panel.Name = "panel";
            panel.Size = new Size(407, 601);
            panel.TabIndex = 3;
            panel.TabStop = false;
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "Tetris (*.stl)|*.stl";
            saveFileDialog.Title = "Tetris mentése";
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "Tetris (*.stl)|*.stl";
            openFileDialog.Title = "Tetris játék betöltése";
            // 
            // Forms
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(1089, 1180);
            Controls.Add(panel);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            ForeColor = SystemColors.ActiveCaptionText;
            MainMenuStrip = menuStrip1;
            Name = "Forms";
            Text = "Form1";
            KeyDown += Forms_KeyDown;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)panel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem File;
        private ToolStripMenuItem NewGame;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem Save;
        private ToolStripMenuItem Load;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem Exit;
        private ToolStripMenuItem Size;
        private ToolStripMenuItem SmallSize;
        private ToolStripMenuItem MediumSize;
        private ToolStripMenuItem LargeSize;
        private ToolStripMenuItem Pause;
        private ToolStripStatusLabel Strip;
        private ToolStripStatusLabel TimeStrip;
        private PictureBox panel;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
    }
}