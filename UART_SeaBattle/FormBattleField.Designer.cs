namespace UART_SeaBattle
{
    partial class FormBattleField
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBattleField));
            tableBattlefield = new TableLayoutPanel();
            spltInfoBattle = new SplitContainer();
            panelInfo = new Panel();
            spltBattlefieldGun = new SplitContainer();
            panelGun = new Panel();
            pctrbxGun = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)spltInfoBattle).BeginInit();
            spltInfoBattle.Panel1.SuspendLayout();
            spltInfoBattle.Panel2.SuspendLayout();
            spltInfoBattle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spltBattlefieldGun).BeginInit();
            spltBattlefieldGun.Panel1.SuspendLayout();
            spltBattlefieldGun.Panel2.SuspendLayout();
            spltBattlefieldGun.SuspendLayout();
            panelGun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pctrbxGun).BeginInit();
            SuspendLayout();
            // 
            // tableBattlefield
            // 
            tableBattlefield.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableBattlefield.ColumnCount = 8;
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableBattlefield.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableBattlefield.Dock = DockStyle.Fill;
            tableBattlefield.Location = new Point(0, 0);
            tableBattlefield.Name = "tableBattlefield";
            tableBattlefield.RowCount = 8;
            tableBattlefield.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableBattlefield.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableBattlefield.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableBattlefield.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableBattlefield.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableBattlefield.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableBattlefield.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableBattlefield.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableBattlefield.Size = new Size(552, 500);
            tableBattlefield.TabIndex = 0;
            // 
            // spltInfoBattle
            // 
            spltInfoBattle.Dock = DockStyle.Fill;
            spltInfoBattle.IsSplitterFixed = true;
            spltInfoBattle.Location = new Point(0, 0);
            spltInfoBattle.Name = "spltInfoBattle";
            spltInfoBattle.Orientation = Orientation.Horizontal;
            // 
            // spltInfoBattle.Panel1
            // 
            spltInfoBattle.Panel1.Controls.Add(panelInfo);
            // 
            // spltInfoBattle.Panel2
            // 
            spltInfoBattle.Panel2.Controls.Add(spltBattlefieldGun);
            spltInfoBattle.Size = new Size(552, 815);
            spltInfoBattle.SplitterDistance = 100;
            spltInfoBattle.TabIndex = 2;
            // 
            // panelInfo
            // 
            panelInfo.Dock = DockStyle.Fill;
            panelInfo.Location = new Point(0, 0);
            panelInfo.Name = "panelInfo";
            panelInfo.Size = new Size(552, 100);
            panelInfo.TabIndex = 0;
            // 
            // spltBattlefieldGun
            // 
            spltBattlefieldGun.Dock = DockStyle.Fill;
            spltBattlefieldGun.IsSplitterFixed = true;
            spltBattlefieldGun.Location = new Point(0, 0);
            spltBattlefieldGun.Name = "spltBattlefieldGun";
            spltBattlefieldGun.Orientation = Orientation.Horizontal;
            // 
            // spltBattlefieldGun.Panel1
            // 
            spltBattlefieldGun.Panel1.Controls.Add(tableBattlefield);
            // 
            // spltBattlefieldGun.Panel2
            // 
            spltBattlefieldGun.Panel2.Controls.Add(panelGun);
            spltBattlefieldGun.Size = new Size(552, 711);
            spltBattlefieldGun.SplitterDistance = 500;
            spltBattlefieldGun.TabIndex = 0;
            // 
            // panelGun
            // 
            panelGun.Controls.Add(pctrbxGun);
            panelGun.Dock = DockStyle.Fill;
            panelGun.Location = new Point(0, 0);
            panelGun.Name = "panelGun";
            panelGun.Size = new Size(552, 207);
            panelGun.TabIndex = 0;
            // 
            // pctrbxGun
            // 
            pctrbxGun.Image = (Image)resources.GetObject("pctrbxGun.Image");
            pctrbxGun.Location = new Point(245, 66);
            pctrbxGun.Name = "pctrbxGun";
            pctrbxGun.Size = new Size(63, 60);
            pctrbxGun.SizeMode = PictureBoxSizeMode.Zoom;
            pctrbxGun.TabIndex = 0;
            pctrbxGun.TabStop = false;
            // 
            // FormBattleField
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(552, 815);
            Controls.Add(spltInfoBattle);
            KeyPreview = true;
            Name = "FormBattleField";
            Text = "Sea battle";
            KeyDown += FormBattleField_KeyDown;
            spltInfoBattle.Panel1.ResumeLayout(false);
            spltInfoBattle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spltInfoBattle).EndInit();
            spltInfoBattle.ResumeLayout(false);
            spltBattlefieldGun.Panel1.ResumeLayout(false);
            spltBattlefieldGun.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spltBattlefieldGun).EndInit();
            spltBattlefieldGun.ResumeLayout(false);
            panelGun.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pctrbxGun).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableBattlefield;
        private SplitContainer spltInfoBattle;
        private Panel panelInfo;
        private SplitContainer spltBattlefieldGun;
        private Panel panelGun;
        private PictureBox pctrbxGun;
    }
}
