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
                    //選択したインデックスを現在再生のインデックスに変更
                    currentMusicIndex = index;

                    //選択しているファイルを取得
                    string musicFile = musicList.Items[currentMusicIndex].Text;

                    //ふるパスの取得
                    string fullPath = fullPathDirectory[musicFile];

                    //ファイルのパスを取得し新しいaudioに変更
                    PitchShifter(fullPath);

                    //音量の調節
                    AttachVolume(volume);

                    //再生
                    wavePlayer.Play();

                    //音楽の名前を取得
                    string musicName = musicList.Items[currentMusicIndex].SubItems[FileNameIndex].Text;

                    //音楽のリストから選択した名前を適応
                    playerAudioName.Text = musicName;

                    UpdateNowPlayingMarker();

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
        /// 再生の終了
        /// </summary>
        void EndPlayBack()
        {
            //nullじゃないかつ再生している場合
            if (wavePlayer != null && wavePlayer.PlaybackState == PlaybackState.Playing)
            {
                wavePlayer.Stop();
                wavePlayer.Dispose();
                wavePlayer = null;
            }
            //audioがnullじゃない場合
            if (audio != null)
            {
                audio.Dispose();
                audio = null;
            }
            //再生ボタンの更新
            UpdatePlayButtonIcon();
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
                    MusicSelected(item, false);

                //インデックス
                currentMusicIndex = index;

                //音楽が選択されている状態にする
                MusicSelected(musicList.Items[currentMusicIndex]);

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
            //音楽の停止
            if (wavePlayer != null && wavePlayer.PlaybackState == PlaybackState.Playing)
                StopMusic();
            //再生
            else if (wavePlayer != null && wavePlayer.PlaybackState != PlaybackState.Playing)
                PlayMusic();
        }

        /// <summary>
        /// ボリュームのアタッチ
        /// </summary>
        /// <param name="volume"></param>
        void AttachVolume(float volume)
        {
            //音量の制限
            this.volume = Math.Max(MinVolume, Math.Min(MaxVolume * VolumeTrance, volume));
            volumeBar.Value = (int)this.volume;
            if (wavePlayer != null)
                wavePlayer.Volume = this.volume / VolumeTrance;
        }

        /// <summary>
        /// 音楽の再生時の強調や選択
        /// </summary>
        /// <param name="item">選択したListViewItem</param>
        /// <param name="flg">状態のフラグ デフォルトでtrue</param>
        void MusicSelected(ListViewItem item, bool flg = true)
        {
            item.Selected = flg;
            item.Focused = flg;
        }

        /// <summary>
        /// 現在描画されている音楽にマークをつける
        /// </summary>
        void UpdateNowPlayingMarker()
        {
            //音楽ファイルが存在している場合
            if (currentMusicIndex >= 0 && currentMusicIndex < musicList.Items.Count)
            {
                //範囲と要素等の取得
                Rectangle itemRect = musicList.GetItemRect(currentMusicIndex);
                Point listViewLocationPos = musicList.Location;
                Point markerPos = new Point(listViewLocationPos.X + itemRect.X - PointCorrectX, listViewLocationPos.Y + itemRect.Y);

                //位置の指定と表示設定
                pictureBox.Visible = true;
                pictureBox.Location = markerPos;
            }
            else
            {
                pictureBox.Visible = false;
            }
        }

        /// <summary>
        /// ダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewDoubleClick(object sender, MouseEventArgs e)
        {
            //クリック時のアイテムを取得
            ListViewHitTestInfo hit = musicList.HitTest(e.Location);
            ListViewItem clickedItem = hit.Item;

            //クリックされたものがnullじゃなかったら
            if (clickedItem != null)
            {
                //クリックされたのが含まれている場合
                if (musicList.Items.Contains(clickedItem))
                {
                    //クリックした要素のインデックスを取得
                    int index = clickedItem.Index;

                    //クリックした要素を再生
                    MusicSkip(index);

                }
                //含まれていない場合
                else
                {
                    MessageBox.Show("クリックされたアイテムはリストに存在しません。");
                }
            }
            else
            {
                // 空白部分がダブルクリックされた場合の処理
                MessageBox.Show("アイテムが選択されていません。");
            }
        }

        /// <summary>
        /// ピッチの調節や初期化
        /// </summary>
        /// <param name="fileName">ファイルのパス</param>
        void PitchShifter(string fileName)
        {
            //音楽ファイルを取得
            audio = new AudioFileReader(fileName);

            //プロバイダーの生成
            var sampleProvider = audio.ToSampleProvider();

            //newして生成
            pitchProvider = new SMBPitchShiftingSampleProvider(sampleProvider) { PitchFactor = pitch };

            //音量の初期化
            audio.Volume = volume / VolumeTrance;
            
            //代入
            var waveProvider = pitchProvider.ToWaveProvider();

            //新しく確保
            wavePlayer = new WaveOutEvent();

            //音楽ファイルを初期化で設定
            wavePlayer.Init(waveProvider);
        }

        //再生player
        IWavePlayer wavePlayer = new WaveOutEvent();

        //再生する音楽ファイルのパス
        AudioFileReader audio;

        //ピッチのプロバイダー
        SMBPitchShiftingSampleProvider pitchProvider;

        //音楽のリピート機能
        bool isRepeatMusic = false;

        //現在の音楽のインデックス
        int currentMusicIndex = 0;

        //最初の音楽インデックス
        const int StartMusicIndex = 0;

        //デフォルトのボリューム
        const int DefaultVolume = 40;

        //0.0～1.0に変換
        const float VolumeTrance = 100f;

        //音量
        float volume;

        //最大の音量
        const float MaxVolume = 1.0f;

        //最低の音量
        const float MinVolume = 0.0f;

        //再生中の音楽ファイル目印
        readonly Bitmap playingItemImage = Properties.Resources._this;

        //point座標をずらす量
        const int PointCorrectX = 30;

        //デフォルトのピッチ
        const float DefaultPitch = 1f;

        //今のピッチ
        float pitch;
    }
}
