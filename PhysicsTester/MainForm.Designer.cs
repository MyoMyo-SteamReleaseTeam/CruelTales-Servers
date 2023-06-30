namespace PhysicsTester
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
			components = new System.ComponentModel.Container();
			MainCanvas = new PictureBox();
			Timer_Tick = new System.Windows.Forms.Timer(components);
			((System.ComponentModel.ISupportInitialize)MainCanvas).BeginInit();
			SuspendLayout();
			// 
			// MainCanvas
			// 
			MainCanvas.Dock = DockStyle.Fill;
			MainCanvas.Location = new Point(0, 0);
			MainCanvas.Name = "MainCanvas";
			MainCanvas.Size = new Size(1184, 711);
			MainCanvas.TabIndex = 0;
			MainCanvas.TabStop = false;
			MainCanvas.Paint += MainCanvas_Paint;
			MainCanvas.MouseDown += Main_MouseDown;
			MainCanvas.MouseMove += Main_MouseMove;
			MainCanvas.MouseUp += Main_MouseUp;
			// 
			// Timer_Tick
			// 
			Timer_Tick.Enabled = true;
			Timer_Tick.Interval = 1;
			Timer_Tick.Tick += Timer_Tick_Tick;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1184, 711);
			Controls.Add(MainCanvas);
			Name = "MainForm";
			Text = "MainForm";
			KeyDown += Main_KeyDown;
			KeyUp += Main_KeyUp;
			((System.ComponentModel.ISupportInitialize)MainCanvas).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private PictureBox MainCanvas;
		private System.Windows.Forms.Timer Timer_Tick;
	}
}