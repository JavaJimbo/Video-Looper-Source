namespace VideoLooper
{
    partial class VideoLooperForm
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
            this.components = new System.ComponentModel.Container();
            this.captureImageBox = new Emgu.CV.UI.ImageBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // captureImageBox
            // 
            this.captureImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureImageBox.Location = new System.Drawing.Point(0, 0);
            this.captureImageBox.Name = "captureImageBox";
            this.captureImageBox.Size = new System.Drawing.Size(759, 582);
            this.captureImageBox.TabIndex = 2;
            this.captureImageBox.TabStop = false;
            this.captureImageBox.Click += new System.EventHandler(this.captureImageBox_Click);
            // 
            // VideoLooperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 582);
            this.Controls.Add(this.captureImageBox);
            this.Name = "VideoLooperForm";
            this.Text = "VideoLooper";
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox captureImageBox;
        private System.IO.Ports.SerialPort serialPort1;
    }
}