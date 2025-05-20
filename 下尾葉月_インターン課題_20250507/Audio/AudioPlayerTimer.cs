using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507
{
    /// <summary>
    /// オーディオの再生時間監視
    /// </summary>
    public partial class Form1
    {
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return TimeSpan.Zero;
        }

        /// <summary>
        /// 再生時間の取得
        /// </summary>
        /// <param name="time">取得する総再生時間</param>
        /// <returns>時間、分、秒の変換</returns>
        string ObtainPlaybackTime(TimeSpan time)
        {
            //再生時間が1時間以上
            if (time.TotalHours >= TimeTotalTimeHours)
                return time.ToString(MoreHours);
            //再生時間が1時間未満
            return time.ToString(LessHours);
        }

        /// <summary>
        /// ProgressBarの更新処理
        /// </summary>
        void UpdateProgressBar()
        {
            //再生されている場合
            if (allMusicPlaybackTime.TotalMilliseconds > 0)
            {
                //totalsecondの誤差対策
                AudioProgressBar.Value = Math.Min((int)currentMusicPlaybackTime.TotalSeconds, AudioProgressBar.Maximum);

                //ラベルの更新
                PlaybackTimeLabel.Text = $"{ObtainPlaybackTime(currentMusicPlaybackTime)}/{ObtainPlaybackTime(allMusicPlaybackTime)}";
            }
        }

        //設定する時間の変換型(1時間以上)
        readonly string MoreHours = @"hh\:mm\:ss";

        //設定する時間の変換型(1時間未満)
        readonly string LessHours = @"mm\:ss";

        //プログレスバーの値の初期値
        readonly int InitProgressBarValue = 0;

        readonly string InitAudioLabelText = "--:--";

        //総再生時間が一時間
        readonly int TimeTotalTimeHours = 1;

        //再生する音楽の総再生時間を管理する変数
        TimeSpan allMusicPlaybackTime;

        //現在の再生時間
        TimeSpan currentMusicPlaybackTime;
    }
}
