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
    public partial class Form1
    {
        /// <summary>
        /// 音量の初期化
        /// </summary>
        void InitVolume()
        {
            //音量
            volumeBar.Value = DefaultVolume / VolumeTrance;
            volume = volumeBar.Value;
            wavePlayer.Volume = volume;
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
                    //選択した音楽ファイルのインデックスを取得
                    int index = musicList.SelectedIndices[FileNameIndex];

                    //音楽の再生を開始
                    PlayNextMusic(index);
                }
            }
            else
                MessageBox.Show("音楽ファイルを追加してください");
        }

        /// <summary>
        /// 音楽の再生開始処理
        /// </summary>
        /// <param name="index">再生する音楽のインデックス</param>
        void PlayNextMusic(int index)
        {
            try
            {
                //再生する音楽が同じかつ途中から再生
                if (wavePlayer.PlaybackState == PlaybackState.Paused && index == currentMusicIndex)
                {
                    //音楽の再生開始
                    wavePlayer.Play();

                    //progressbarの再開
                    musicTimer.Start();
                }
                //新規再生
                else
                {
                    //新しく確保
                    wavePlayer = new WaveOutEvent();

                    //ボリュームの設定
                    wavePlayer.Volume = volumeBar.Value / VolumeTrance;

                    //選択したインデックスを現在再生のインデックスに変更
                    currentMusicIndex = index;

                    //選択しているファイルを取得
                    string musicFile = musicList.Items[currentMusicIndex].Text;

                    //ふるパスの取得
                    string fullPath = fullPathDirectory[musicFile];

                    //ファイルのパスを取得し新しいaudioに変更
                    audio = new AudioFileReader(fullPath);

                    //音楽ファイルを初期化で設定
                    wavePlayer.Init(audio);

                    //再生
                    wavePlayer.Play();

                    //音楽の名前を取得
                    string musicName = musicList.Items[currentMusicIndex].SubItems[FileNameIndex].Text;

                    //音楽のリストから選択した名前を適応
                    playerAudioName.Text = musicName;

                    //総再生時間を取得
                    allMusicPlaybackTime = GetMusicAllTime(fullPath);

                    //音楽の再生時間の監視を開始
                    musicTimer.Start();

                    //progressbarの設定
                    AudioProgressBar.Maximum = (int)allMusicPlaybackTime.TotalSeconds;
                    AudioProgressBar.Minimum = InitProgressBarValue;
                    AudioProgressBar.Value = InitProgressBarValue;

                }
                //再生ボタンの変更
                UpdatePlayButtonIcon(false);
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
            //再生ボタンの変更
            UpdatePlayButtonIcon();

            //音楽の再生を停止する
            wavePlayer.Pause();
        }

        /// <summary>
        /// 音楽のスキップ機能
        /// </summary>
        /// <param name="index">次のインデックス</param>
        /// <param name="isPlay">再生するか デフォルトでtrue</param>
        void MusicSkip(int index, bool isPlay = true)
        {
            try
            {
                //現在の音楽の停止
                StopMusic();

                //破棄
                wavePlayer.Dispose();

                //processbarのリセット
                PlaybackTimeLabel.Text = InitAudioLabelText;
                AudioProgressBar.Value = InitProgressBarValue;

                //現在の音楽の指定をすべて削除
                foreach (ListViewItem item in musicList.Items)
                    item.Selected = false;

                //もし範囲を超えた場合スキップする
                //if (index < StartMusicIndex || index >= musicList.Items.Count)
                //  return;

                //インデックス
                currentMusicIndex = index;

                //音楽が選択されている状態にする
                musicList.Items[currentMusicIndex].Selected = true;

                //次の音楽の再生
                if (isPlay)
                    PlayNextMusic(currentMusicIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
            //再生
            else if (wavePlayer != null && wavePlayer.PlaybackState != PlaybackState.Playing)
                PlayMusic();
        }

        //再生player
        IWavePlayer wavePlayer = new WaveOutEvent();

        //再生する音楽ファイルのパス
        AudioFileReader audio;

        //音楽のリピート機能
        bool isRepeatMusic = false;

        //現在の音楽のインデックス
        int currentMusicIndex = 0;

        //最初の音楽インデックス
        const int StartMusicIndex = 0;

        //デフォルトのボリューム
        const int DefaultVolume = 40;

        //0.0～1.0に変換
        const int VolumeTrance = 100;

        //音量
        float volume;
    }
}
