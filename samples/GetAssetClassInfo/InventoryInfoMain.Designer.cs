namespace GetAssetClassInfo
{
    partial class InventoryInfoForm
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
            this.goButton = new System.Windows.Forms.Button();
            this.assetClassInfoTB = new System.Windows.Forms.TextBox();
            this.appIdNUD = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.itemLB = new System.Windows.Forms.ListBox();
            this.steamIdNUD = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.appIdNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.steamIdNUD)).BeginInit();
            this.SuspendLayout();
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(349, 9);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(82, 23);
            this.goButton.TabIndex = 1;
            this.goButton.Text = "Go!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // assetClassInfoTB
            // 
            this.assetClassInfoTB.Location = new System.Drawing.Point(277, 38);
            this.assetClassInfoTB.Multiline = true;
            this.assetClassInfoTB.Name = "assetClassInfoTB";
            this.assetClassInfoTB.ReadOnly = true;
            this.assetClassInfoTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.assetClassInfoTB.Size = new System.Drawing.Size(311, 212);
            this.assetClassInfoTB.TabIndex = 3;
            this.assetClassInfoTB.WordWrap = false;
            // 
            // appIdNUD
            // 
            this.appIdNUD.Location = new System.Drawing.Point(233, 12);
            this.appIdNUD.Maximum = new decimal(new int[] {
            -402653185,
            -1613725636,
            54210108,
            0});
            this.appIdNUD.Name = "appIdNUD";
            this.appIdNUD.Size = new System.Drawing.Size(110, 20);
            this.appIdNUD.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "AppId";
            // 
            // itemLB
            // 
            this.itemLB.FormattingEnabled = true;
            this.itemLB.Location = new System.Drawing.Point(12, 38);
            this.itemLB.Name = "itemLB";
            this.itemLB.Size = new System.Drawing.Size(259, 212);
            this.itemLB.TabIndex = 6;
            this.itemLB.SelectedIndexChanged += new System.EventHandler(this.itemLB_SelectedIndexChanged);
            // 
            // steamIdNUD
            // 
            this.steamIdNUD.Location = new System.Drawing.Point(76, 12);
            this.steamIdNUD.Maximum = new decimal(new int[] {
            268435455,
            1042612833,
            542101086,
            0});
            this.steamIdNUD.Name = "steamIdNUD";
            this.steamIdNUD.Size = new System.Drawing.Size(110, 20);
            this.steamIdNUD.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "SteamId64";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(437, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "AppId: 440 = TF2, 730 = CS:GO";
            // 
            // InventoryInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.steamIdNUD);
            this.Controls.Add(this.itemLB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.appIdNUD);
            this.Controls.Add(this.assetClassInfoTB);
            this.Controls.Add(this.goButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InventoryInfoForm";
            this.Text = "Get Inventory Info";
            ((System.ComponentModel.ISupportInitialize)(this.appIdNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.steamIdNUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.TextBox assetClassInfoTB;
        private System.Windows.Forms.NumericUpDown appIdNUD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox itemLB;
        private System.Windows.Forms.NumericUpDown steamIdNUD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}

