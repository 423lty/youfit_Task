using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.MediaFoundation;
using NAudio.Wave;


namespace 下尾葉月_インターン課題_20250507
{

    /// <summary>
    /// 音楽再生ファイルの分割
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 音楽の処理一覧
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MusicPlayer(object sender, EventArgs e)
        {
            if (wavePlayer != null && isRepeatMusic)
            {
                //wavePlayer.
            }
            //音楽の停止
            else if (wavePlayer != null && wavePlayer.PlaybackState == PlaybackState.Playing)
                StopMusic();
            else if (wavePlayer != null && wavePlayer.PlaybackState == PlaybackState.Stopped)
                PlayMusic();
        }

        /// <summary>
        /// 音楽を再生処理
        /// </summary>
        public void PlayMusic()
        {

            //音楽の要素数
            if (musicList.Items.Count > 0)
            {
                //音楽ファイルを選択していない場合処理をしない
                if (musicList.SelectedItems.Count == FileNameIndex)
                {
                    MessageBox.Show("再生する音楽ファイルを選択してください");
                    return;
                }

                if (wavePlayer.PlaybackState != PlaybackState.Playing)
                {
                    //音楽の再生を一時停止する
                    //wavePlayer.Stop();

                    ////オーディオを解放
                    //wavePlayer?.Dispose();
                    //audio?.Dispose();

                    //選択した音楽ファイルのインデックスを取得
                    currentMusicIndex = musicList.SelectedIndices[FileNameIndex];

                    //音楽の再生を開始
                    PlayNextMusic(currentMusicIndex);
                }
            }
            else
            {
                MessageBox.Show("音楽ファイルを追加してください");
            }
        }

        /// <summary>
        /// 音楽の再生開始処理
        /// </summary>
        /// <param name="index">再生する音楽のインデックス</param>
        void PlayNextMusic(int index)
        {
            try
            {
                //新しく確保
                wavePlayer = new WaveOutEvent();

                //選択しているファイルを取得
                string musicFile = musicList.Items[index].Text;
                //MessageBox.Show(musicFile, fullPathDirectory[musicFile]);

                string fullPath = fullPathDirectory[musicFile];

                //ファイルのパスを取得し新しいaudioに変更
                audio = new AudioFileReader(fullPath);

                //音楽ファイルを初期化で設定
                wavePlayer.Init(audio);

                //再生
                wavePlayer.Play();

                //音楽の再生時間の監視を開始
                musicTimer.Start();

                //総再生時間を取得
                allMusicPlaybackTime = GetMusicAllTime(fullPath);

                //テキストの変更
                playBack.Text = "停止";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 音楽の停止処理
        /// </summary>
        private void StopMusic()
        {
            //テキストの変更
            playBack.Text = "再生";

            //音楽の再生を停止する
            wavePlayer.Stop();

            //オーディオを解放
            audio?.Dispose();
            audio = null;
        }

        /// <summary>
        /// リピート機能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repeat_Click(object sender, EventArgs e)
        {
            //フラグの切り替え
            isRepeatMusic = !isRepeatMusic;

            //画像を格納する変数の定義
            Bitmap changeIcon = null;

            //リピートする場合
            if (repeat.Image != null && isRepeatMusic)
                changeIcon = new Bitmap(Properties.Resources.repeat, repeat.Width, repeat.Height);

            //リピートしない場合
            if (repeat.Image != null && !isRepeatMusic)
                changeIcon = new Bitmap(Properties.Resources._default, repeat.Width, repeat.Height);

            //画像の張替
            repeat.Image = changeIcon;
        }

        /// <summary>
        /// 総再生時間を取得
        /// </summary>
        /// <param name="filePath">取得するパス</param>
        /// <returns>総再生時間を取得</returns>
        TimeSpan GetMusicAllTime(string filePath)
        {
            try
            {
                //指定したパスの総再生時間を取得
                using (var reader = new AudioFileReader(filePath))
                    return reader.TotalTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return TimeSpan.Zero;
        }

        /// <summary>
        /// 現在の再生時間を取得
        /// </summary>
        /// <returns>現在の再生時間を取得</returns>
        TimeSpan GetMusicCurrentTime()
        {
            try
            {
                if (audio != null)
                    return audio.CurrentTime;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return TimeSpan.Zero;
        }

        //音楽のリピート機能
        bool isRepeatMusic = false;

        //再生player
        IWavePlayer wavePlayer = new WaveOutEvent();

        //再生する音楽ファイルのパス
        AudioFileReader audio;

        //現在の音楽のインデックス
        int currentMusicIndex = 0;

        //再生する音楽の総再生時間を管理する変数
        TimeSpan allMusicPlaybackTime;

        //現在の再生時間
        TimeSpan currentMusicPlaybackTime;
    }
}
