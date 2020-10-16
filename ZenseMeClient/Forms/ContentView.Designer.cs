namespace ZenseMe.Client.Forms
{
	partial class ContentView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._cTrackContentView = new System.Windows.Forms.ListView();
            this.content_Track = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.content_Artist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.content_Album = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.content_Genre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.content_Duration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.content_PlayCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.content_Device = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // _cTrackContentView
            // 
            this._cTrackContentView.AllowColumnReorder = true;
            this._cTrackContentView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._cTrackContentView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.content_Track,
            this.content_Artist,
            this.content_Album,
            this.content_Genre,
            this.content_Duration,
            this.content_PlayCount,
            this.content_Device});
            this._cTrackContentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cTrackContentView.FullRowSelect = true;
            this._cTrackContentView.GridLines = true;
            this._cTrackContentView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._cTrackContentView.HideSelection = false;
            this._cTrackContentView.Location = new System.Drawing.Point(0, 0);
            this._cTrackContentView.Name = "_cTrackContentView";
            this._cTrackContentView.Size = new System.Drawing.Size(804, 402);
            this._cTrackContentView.TabIndex = 0;
            this._cTrackContentView.UseCompatibleStateImageBehavior = false;
            this._cTrackContentView.View = System.Windows.Forms.View.Details;
            // 
            // content_Track
            // 
            this.content_Track.Text = "Track";
            this.content_Track.Width = 200;
            // 
            // content_Artist
            // 
            this.content_Artist.Text = "Artist";
            this.content_Artist.Width = 154;
            // 
            // content_Album
            // 
            this.content_Album.Text = "Album";
            this.content_Album.Width = 175;
            // 
            // content_Genre
            // 
            this.content_Genre.Text = "Genre";
            this.content_Genre.Width = 95;
            // 
            // content_Duration
            // 
            this.content_Duration.Text = "Duration";
            this.content_Duration.Width = 75;
            // 
            // content_PlayCount
            // 
            this.content_PlayCount.Text = "Play/Scrob";
            this.content_PlayCount.Width = 79;
            // 
            // content_Device
            // 
            this.content_Device.Text = "Device";
            this.content_Device.Width = 129;
            // 
            // ContentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._cTrackContentView);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "ContentView";
            this.Size = new System.Drawing.Size(804, 402);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView _cTrackContentView;
		private System.Windows.Forms.ColumnHeader content_Track;
		private System.Windows.Forms.ColumnHeader content_Artist;
		private System.Windows.Forms.ColumnHeader content_Album;
		private System.Windows.Forms.ColumnHeader content_Duration;
		private System.Windows.Forms.ColumnHeader content_PlayCount;
        private System.Windows.Forms.ColumnHeader content_Device;
        private System.Windows.Forms.ColumnHeader content_Genre;
    }
}
