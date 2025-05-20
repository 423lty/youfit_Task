using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// ピッチの調節
        /// </summary>
        void AttachPitch(float pitch)
        {
            // スライダーの値（5〜20）をピッチ係数（0.5〜2.0）に変換
            //this.pitch = Math.Max(MinPitch, Math.Min(MaxPitch, pitch));
            this.pitch = pitch;
            pitchSlider.Value = (int)this.pitch;

            //pitchProviderがnull以外
            if (pitchProvider != null)
                pitchProvider.PitchFactor = this.pitch / PitchCorrect;

            // ピッチのラベル表示更新（オプション）
            pitchLabel.Text = $"Pitch : {this.pitch / PitchCorrect:F1}";
        }

        /// <summary>
        /// ボリュームのアタッチ
        /// </summary>
        /// <param name="volume"></param>
        void AttachVolume(float volume)
        {
            //音量の制限
            this.volume = Math.Max(MinVolume, Math.Min(MaxVolume, volume));
            volumeBar.Value = (int)this.volume;

            //wavePlayerがnull以外
            if (wavePlayer != null)
                wavePlayer.Volume = this.volume / VolumeTrance;

            //音量のラベル
            audioSizeLabel.Text = $"音量:{this.volume}";

            //音量画像の切り替え
            UpdateMusicSizeButtonIcon(this.volume);
        }
        //ピッチの補正量
        const float PitchCorrect = 10.0f;

        //最大の音量
        const float MaxPitch = 20.0f;

        //最低の音量
        const float MinPitch = 5f;

        //最大の音量
        const float MaxVolume = 100.0f;

        //最低の音量
        const float MinVolume = 0.0f;

        //デフォルトのボリューム
        const int DefaultVolume = 40;

        //0.0～1.0に変換
        const float VolumeTrance = 100f;

        //デフォルトのピッチ
        const float DefaultPitch = 10f;
    }
}
