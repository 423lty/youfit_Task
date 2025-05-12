namespace 下尾葉月_インターン課題_20250507
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.reference = new System.Windows.Forms.Button();
            this.playBack = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.dragDropBox = new System.Windows.Forms.TextBox();
            this.musicList = new System.Windows.Forms.ListView();
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.repeat = new System.Windows.Forms.Button();
            this.File = new System.Windows.Forms.GroupBox();
            this.musicTimer = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.playbackTime = new System.Windows.Forms.GroupBox();
            this.File.SuspendLayout();
            this.playbackTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // reference
            // 
            this.reference.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.reference.Font = new System.Drawing.Font("ＭＳ 明朝", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.reference.Location = new System.Drawing.Point(565, 94);
            this.reference.Name = "reference";
            this.reference.Size = new System.Drawing.Size(158, 53);
            this.reference.TabIndex = 0;
            this.reference.Text = "参照";
            this.reference.UseVisualStyleBackColor = false;
            this.reference.Click += new System.EventHandler(this.BrowseFilesEvent);
            // 
            // playBack
            // 
            this.playBack.Font = new System.Drawing.Font("ＭＳ 明朝", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playBack.Location = new System.Drawing.Point(594, 42);
            this.playBack.Name = "playBack";
            this.playBack.Size = new System.Drawing.Size(64, 64);
            this.playBack.TabIndex = 1;
            this.playBack.Text = "再生";
            this.playBack.UseVisualStyleBackColor = true;
            this.playBack.Click += new System.EventHandler(this.MusicPlayer);
            // 
            // stop
            // 
            this.stop.Font = new System.Drawing.Font("ＭＳ 明朝", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stop.Location = new System.Drawing.Point(664, 42);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(64, 64);
            this.stop.TabIndex = 2;
            this.stop.Text = "削除";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.DeleteMusic);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("ＭＳ 明朝", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(519, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "コンピュータから参照する場合はこちらから";
            // 
            // dragDropBox
            // 
            this.dragDropBox.AllowDrop = true;
            this.dragDropBox.Font = new System.Drawing.Font("ＭＳ 明朝", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dragDropBox.Location = new System.Drawing.Point(17, 9);
            this.dragDropBox.Multiline = true;
            this.dragDropBox.Name = "dragDropBox";
            this.dragDropBox.Size = new System.Drawing.Size(463, 188);
            this.dragDropBox.TabIndex = 5;
            this.dragDropBox.Text = "ここにwavファイルまたはmp3ファイルをドラッグ&ドロップしてください";
            this.dragDropBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.dragDropBox_DragDrop);
            this.dragDropBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragDropBox_DragEnter);
            // 
            // musicList
            // 
            this.musicList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileName,
            this.Date,
            this.Type,
            this.Size});
            this.musicList.HideSelection = false;
            this.musicList.Location = new System.Drawing.Point(11, 29);
            this.musicList.Name = "musicList";
            this.musicList.Size = new System.Drawing.Size(567, 158);
            this.musicList.TabIndex = 6;
            this.musicList.UseCompatibleStateImageBehavior = false;
            this.musicList.View = System.Windows.Forms.View.Details;
            // 
            // FileName
            // 
            this.FileName.Text = "ファイル名";
            this.FileName.Width = 200;
            // 
            // Date
            // 
            this.Date.Text = "日付";
            this.Date.Width = 150;
            // 
            // Type
            // 
            this.Type.Text = "種類";
            this.Type.Width = 100;
            // 
            // Size
            // 
            this.Size.Text = "サイズ";
            this.Size.Width = 100;
            // 
            // repeat
            // 
            this.repeat.Font = new System.Drawing.Font("ＭＳ 明朝", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repeat.Image = ((System.Drawing.Image)(resources.GetObject("repeat.Image")));
            this.repeat.Location = new System.Drawing.Point(594, 112);
            this.repeat.Name = "repeat";
            this.repeat.Size = new System.Drawing.Size(64, 64);
            this.repeat.TabIndex = 7;
            this.repeat.UseVisualStyleBackColor = true;
            this.repeat.Click += new System.EventHandler(this.repeat_Click);
            // 
            // File
            // 
            this.File.Controls.Add(this.musicList);
            this.File.Controls.Add(this.repeat);
            this.File.Controls.Add(this.playBack);
            this.File.Controls.Add(this.stop);
            this.File.Font = new System.Drawing.Font("ＭＳ 明朝", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.File.Location = new System.Drawing.Point(17, 209);
            this.File.Margin = new System.Windows.Forms.Padding(2);
            this.File.Name = "File";
            this.File.Padding = new System.Windows.Forms.Padding(2);
            this.File.Size = new System.Drawing.Size(740, 213);
            this.File.TabIndex = 8;
            this.File.TabStop = false;
            this.File.Text = "選択ファイル";
            // 
            // musicTimer
            // 
            this.musicTimer.Interval = 1000;
            this.musicTimer.Tick += new System.EventHandler(this.musicTimer_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 31);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(717, 10);
            this.progressBar1.TabIndex = 9;
            // 
            // playbackTime
            // 
            this.playbackTime.Controls.Add(this.progressBar1);
            this.playbackTime.Font = new System.Drawing.Font("ＭＳ 明朝", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.playbackTime.Location = new System.Drawing.Point(12, 437);
            this.playbackTime.Name = "playbackTime";
            this.playbackTime.Size = new System.Drawing.Size(745, 47);
            this.playbackTime.TabIndex = 9;
            this.playbackTime.TabStop = false;
            this.playbackTime.Text = "再生時間";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 496);
            this.Controls.Add(this.playbackTime);
            this.Controls.Add(this.File);
            this.Controls.Add(this.dragDropBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reference);
            this.Name = "Form1";
            this.Text = "VoiShredder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.File.ResumeLayout(false);
            this.playbackTime.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button reference;
        private System.Windows.Forms.Button playBack;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox dragDropBox;
        private System.Windows.Forms.ListView musicList;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader Size;
        private System.Windows.Forms.Button repeat;
        private System.Windows.Forms.GroupBox File;
        private System.Windows.Forms.Timer musicTimer;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox playbackTime;
    }
}

