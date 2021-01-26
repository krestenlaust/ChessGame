
namespace ChessForms
{
    partial class MatchMaker
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
            this.radioButtonWhiteLocal = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxWhiteName = new System.Windows.Forms.TextBox();
            this.textBoxWhiteHost = new System.Windows.Forms.TextBox();
            this.radioButtonWhiteNetworked = new System.Windows.Forms.RadioButton();
            this.radioButtonWhiteBot = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButtonBlackLichessPlayer = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBlackName = new System.Windows.Forms.TextBox();
            this.textBoxBlackServerIP = new System.Windows.Forms.TextBox();
            this.radioButtonBlackNetworked = new System.Windows.Forms.RadioButton();
            this.radioButtonBlackBot = new System.Windows.Forms.RadioButton();
            this.radioButtonBlackLocal = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonStartMatch = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxBlackLichessMatchID = new System.Windows.Forms.TextBox();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonWhiteLocal
            // 
            this.radioButtonWhiteLocal.AutoSize = true;
            this.radioButtonWhiteLocal.Location = new System.Drawing.Point(21, 32);
            this.radioButtonWhiteLocal.Name = "radioButtonWhiteLocal";
            this.radioButtonWhiteLocal.Size = new System.Drawing.Size(63, 21);
            this.radioButtonWhiteLocal.TabIndex = 0;
            this.radioButtonWhiteLocal.TabStop = true;
            this.radioButtonWhiteLocal.Text = "Local";
            this.radioButtonWhiteLocal.UseVisualStyleBackColor = true;
            this.radioButtonWhiteLocal.CheckedChanged += new System.EventHandler(this.radioButtonWhiteLocal_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.radioButtonWhiteNetworked);
            this.groupBox3.Controls.Add(this.radioButtonWhiteBot);
            this.groupBox3.Controls.Add(this.radioButtonWhiteLocal);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(500, 200);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "White player type";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxWhiteName);
            this.groupBox1.Controls.Add(this.textBoxWhiteHost);
            this.groupBox1.Location = new System.Drawing.Point(253, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 173);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Souce IP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Name";
            // 
            // textBoxWhiteName
            // 
            this.textBoxWhiteName.Location = new System.Drawing.Point(9, 37);
            this.textBoxWhiteName.Name = "textBoxWhiteName";
            this.textBoxWhiteName.Size = new System.Drawing.Size(199, 22);
            this.textBoxWhiteName.TabIndex = 1;
            this.textBoxWhiteName.Text = "White";
            // 
            // textBoxWhiteHost
            // 
            this.textBoxWhiteHost.Enabled = false;
            this.textBoxWhiteHost.Location = new System.Drawing.Point(9, 89);
            this.textBoxWhiteHost.Name = "textBoxWhiteHost";
            this.textBoxWhiteHost.Size = new System.Drawing.Size(199, 22);
            this.textBoxWhiteHost.TabIndex = 0;
            // 
            // radioButtonWhiteNetworked
            // 
            this.radioButtonWhiteNetworked.AutoSize = true;
            this.radioButtonWhiteNetworked.Location = new System.Drawing.Point(21, 86);
            this.radioButtonWhiteNetworked.Name = "radioButtonWhiteNetworked";
            this.radioButtonWhiteNetworked.Size = new System.Drawing.Size(191, 21);
            this.radioButtonWhiteNetworked.TabIndex = 2;
            this.radioButtonWhiteNetworked.TabStop = true;
            this.radioButtonWhiteNetworked.Text = "Networked (You are host)";
            this.radioButtonWhiteNetworked.UseVisualStyleBackColor = true;
            this.radioButtonWhiteNetworked.CheckedChanged += new System.EventHandler(this.radioButtonWhiteNetworked_CheckedChanged);
            // 
            // radioButtonWhiteBot
            // 
            this.radioButtonWhiteBot.AutoSize = true;
            this.radioButtonWhiteBot.Location = new System.Drawing.Point(21, 59);
            this.radioButtonWhiteBot.Name = "radioButtonWhiteBot";
            this.radioButtonWhiteBot.Size = new System.Drawing.Size(50, 21);
            this.radioButtonWhiteBot.TabIndex = 1;
            this.radioButtonWhiteBot.TabStop = true;
            this.radioButtonWhiteBot.Text = "Bot";
            this.radioButtonWhiteBot.UseVisualStyleBackColor = true;
            this.radioButtonWhiteBot.CheckedChanged += new System.EventHandler(this.radioButtonWhiteBot_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.radioButtonBlackLichessPlayer);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.radioButtonBlackNetworked);
            this.groupBox4.Controls.Add(this.radioButtonBlackBot);
            this.groupBox4.Controls.Add(this.radioButtonBlackLocal);
            this.groupBox4.Location = new System.Drawing.Point(3, 209);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(500, 200);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Black player type";
            // 
            // radioButtonBlackLichessPlayer
            // 
            this.radioButtonBlackLichessPlayer.AutoSize = true;
            this.radioButtonBlackLichessPlayer.Location = new System.Drawing.Point(21, 113);
            this.radioButtonBlackLichessPlayer.Name = "radioButtonBlackLichessPlayer";
            this.radioButtonBlackLichessPlayer.Size = new System.Drawing.Size(121, 21);
            this.radioButtonBlackLichessPlayer.TabIndex = 5;
            this.radioButtonBlackLichessPlayer.TabStop = true;
            this.radioButtonBlackLichessPlayer.Text = "Lichess Player";
            this.radioButtonBlackLichessPlayer.UseVisualStyleBackColor = true;
            this.radioButtonBlackLichessPlayer.CheckedChanged += new System.EventHandler(this.radioButtonBlackLichessPlayer_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.textBoxBlackLichessMatchID);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.textBoxBlackName);
            this.groupBox6.Controls.Add(this.textBoxBlackServerIP);
            this.groupBox6.Location = new System.Drawing.Point(253, 21);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(241, 173);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Host IP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // textBoxBlackName
            // 
            this.textBoxBlackName.Location = new System.Drawing.Point(9, 37);
            this.textBoxBlackName.Name = "textBoxBlackName";
            this.textBoxBlackName.Size = new System.Drawing.Size(199, 22);
            this.textBoxBlackName.TabIndex = 1;
            this.textBoxBlackName.Text = "Black";
            // 
            // textBoxBlackServerIP
            // 
            this.textBoxBlackServerIP.Enabled = false;
            this.textBoxBlackServerIP.Location = new System.Drawing.Point(9, 87);
            this.textBoxBlackServerIP.Name = "textBoxBlackServerIP";
            this.textBoxBlackServerIP.Size = new System.Drawing.Size(199, 22);
            this.textBoxBlackServerIP.TabIndex = 0;
            // 
            // radioButtonBlackNetworked
            // 
            this.radioButtonBlackNetworked.AutoSize = true;
            this.radioButtonBlackNetworked.Location = new System.Drawing.Point(21, 86);
            this.radioButtonBlackNetworked.Name = "radioButtonBlackNetworked";
            this.radioButtonBlackNetworked.Size = new System.Drawing.Size(197, 21);
            this.radioButtonBlackNetworked.TabIndex = 2;
            this.radioButtonBlackNetworked.TabStop = true;
            this.radioButtonBlackNetworked.Text = "Networked (You are client)";
            this.radioButtonBlackNetworked.UseVisualStyleBackColor = true;
            this.radioButtonBlackNetworked.CheckedChanged += new System.EventHandler(this.radioButtonBlackNetworked_CheckedChanged);
            // 
            // radioButtonBlackBot
            // 
            this.radioButtonBlackBot.AutoSize = true;
            this.radioButtonBlackBot.Location = new System.Drawing.Point(21, 59);
            this.radioButtonBlackBot.Name = "radioButtonBlackBot";
            this.radioButtonBlackBot.Size = new System.Drawing.Size(50, 21);
            this.radioButtonBlackBot.TabIndex = 1;
            this.radioButtonBlackBot.TabStop = true;
            this.radioButtonBlackBot.Text = "Bot";
            this.radioButtonBlackBot.UseVisualStyleBackColor = true;
            this.radioButtonBlackBot.CheckedChanged += new System.EventHandler(this.radioButtonBlackBot_CheckedChanged);
            // 
            // radioButtonBlackLocal
            // 
            this.radioButtonBlackLocal.AutoSize = true;
            this.radioButtonBlackLocal.Location = new System.Drawing.Point(21, 32);
            this.radioButtonBlackLocal.Name = "radioButtonBlackLocal";
            this.radioButtonBlackLocal.Size = new System.Drawing.Size(63, 21);
            this.radioButtonBlackLocal.TabIndex = 0;
            this.radioButtonBlackLocal.TabStop = true;
            this.radioButtonBlackLocal.Text = "Local";
            this.radioButtonBlackLocal.UseVisualStyleBackColor = true;
            this.radioButtonBlackLocal.CheckedChanged += new System.EventHandler(this.radioButtonBlackLocal_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.groupBox3);
            this.flowLayoutPanel1.Controls.Add(this.groupBox4);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(516, 427);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // buttonStartMatch
            // 
            this.buttonStartMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStartMatch.Location = new System.Drawing.Point(12, 445);
            this.buttonStartMatch.Name = "buttonStartMatch";
            this.buttonStartMatch.Size = new System.Drawing.Size(182, 30);
            this.buttonStartMatch.TabIndex = 4;
            this.buttonStartMatch.Text = "Start match";
            this.buttonStartMatch.UseVisualStyleBackColor = true;
            this.buttonStartMatch.Click += new System.EventHandler(this.buttonStartMatch_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Lichess Match ID";
            // 
            // textBoxBlackLichessMatchID
            // 
            this.textBoxBlackLichessMatchID.Enabled = false;
            this.textBoxBlackLichessMatchID.Location = new System.Drawing.Point(9, 132);
            this.textBoxBlackLichessMatchID.Name = "textBoxBlackLichessMatchID";
            this.textBoxBlackLichessMatchID.Size = new System.Drawing.Size(199, 22);
            this.textBoxBlackLichessMatchID.TabIndex = 4;
            // 
            // MatchMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 481);
            this.Controls.Add(this.buttonStartMatch);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "MatchMaker";
            this.Text = "MatchMaker";
            this.Load += new System.EventHandler(this.MatchMaker_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButtonWhiteLocal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonWhiteNetworked;
        private System.Windows.Forms.RadioButton radioButtonWhiteBot;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButtonBlackNetworked;
        private System.Windows.Forms.RadioButton radioButtonBlackBot;
        private System.Windows.Forms.RadioButton radioButtonBlackLocal;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox textBoxBlackName;
        private System.Windows.Forms.TextBox textBoxBlackServerIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonStartMatch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxWhiteName;
        private System.Windows.Forms.TextBox textBoxWhiteHost;
        private System.Windows.Forms.RadioButton radioButtonBlackLichessPlayer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxBlackLichessMatchID;
    }
}