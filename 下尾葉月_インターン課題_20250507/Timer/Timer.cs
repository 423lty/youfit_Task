using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507
{
    public partial class Form1
    {
        /// <summary>
        /// 音楽再生管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void musicTimer_Tick(object sender, EventArgs e)
        {
            //audioに音楽ファイルが付与されている状態
            if (audio != null)
            {
                //現在の音楽再生時間を取得
                currentMusicPlaybackTime = GetMusicCurrentTime();

                //再生時間からバーの更新をする
                UpdateProgressBar();

                //現在の再生時間が総再生時間に達した場合
                if (wavePlayer.PlaybackState == PlaybackState.Stopped && wavePlayer != null)
                {
                    //TimerツールのEnableを切る
                    musicTimer.Stop();

                    //音楽の再生を止める
                    EndPlayBack();

                    //同じ曲のリピートしていない場合
                    if (!isRepeatMusic)
                    {
                        //次のインデックスを取得
                        int nextIndex = currentMusicIndex + 1;

                        //すべての選択状態の解除
                        foreach (ListViewItem item in musicList.Items)
                            MusicSelected(item, false);

                        //もし次のインデックスが存在している場合
                        if (nextIndex < musicList.Items.Count)
                            currentMusicIndex = nextIndex;
                        //最後の曲だった場合最初の曲に戻す
                        else
                            currentMusicIndex = StartMusicIndex;

                        //選択された状態を付与
                        MusicSelected(musicList.Items[currentMusicIndex]);
                    }
                    //次の音楽を再生かリピート祭祀
                    PlayNextMusic(currentMusicIndex);
                }
            }
        }

        //ダブルクリックするのに必要な時間
        readonly int DoubleClickThreshold = SystemInformation.DoubleClickTime;


    }
}
