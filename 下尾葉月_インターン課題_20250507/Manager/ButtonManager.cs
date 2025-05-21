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
        /// クリックハンドルの呼び出し
        /// </summary>
        void LoadClickHandle()
        {
            //それぞれの初期化
            musicSizeClickHandler = new ClickHandler(ClickableMusicSizeButton, MusicSizeButton_LeftClick, MusicSizeButton_RightClick);
            pitchClickHandler = new ClickHandler(ClickablePitchButton, PitchButton_LeftClick, PitchButton_RightClick);

            //それぞれのクリックイベントの割り当て
            ClickableMusicSizeButton.MouseClick += musicSizeClickHandler.OnButton_MouseClick;
            ClickablePitchButton.MouseClick += pitchClickHandler.OnButton_MouseClick;
        }

        /// <summary>
        /// ないナス
        /// </summary>
        void MusicSizeButton_LeftClick()
        {
            //音量の調節
            AttachVolume(volume - MusicSizefluctuation);
        }

        /// <summary>
        /// ダブルクリックの確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MusicSizeButton_RightClick()
        {
            //音量の調節
            AttachVolume(volume + MusicSizefluctuation);
        }
        /// <summary>
        /// シングルクリック処理
        /// </summary>
        void PitchButton_LeftClick()
        {
            //音量の調節
            AttachPitch(pitch - Pitchfluctuation);
        }

        /// <summary>
        /// ダブルクリックの確認
        /// </summary>
        void PitchButton_RightClick()
        {
            //音量の調節
            AttachPitch(pitch + Pitchfluctuation);
        }

        //音量
        ClickHandler musicSizeClickHandler = null;

        //ピッチ
        ClickHandler pitchClickHandler = null;

        //音量の増減量
        const float MusicSizefluctuation = 1f;

        //音域の増減量
        const float Pitchfluctuation = 1f;
    }
}
