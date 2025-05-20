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
        private void pitchSlider_Scroll(object sender, EventArgs e)
        {
            if (pitchProvider != null)
            {
                // スライダーの値（5〜20）をピッチ係数（0.5〜2.0）に変換
                pitch = pitchSlider.Value / PitchCorrect;
                pitchProvider.PitchFactor = pitch;

                // ピッチのラベル表示更新（オプション）
                pitchLabel.Text = $"Pitch : {pitch:F1}";
            }
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            AttachVolume(volumeBar.Value);
        }

        //ピッチの補正量
        const float PitchCorrect = 10.0f;

    }
}
