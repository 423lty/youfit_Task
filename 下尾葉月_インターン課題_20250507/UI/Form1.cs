using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507
{
    /// <summary>
    /// デフォルト
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            //コンポーネントの初期化
            InitializeComponent();

            //アイコンの初期化
            InitButtonIcon();

            //ピッチの初期化
            InitPitch();
        }

        /// <summary>
        /// フォームを開いたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1_Load(object sender, EventArgs e)
        {
            //前回の履歴の読み込み
            LoadFile(sender, e);
        }

        /// <summary>
        /// 終了をしたときの保存処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ファイルの保存
            SaveFile(sender, e);
        }

        /// <summary>
        /// List<string>をListViewItemに変換して変換
        /// </summary>
        /// <param name="items">ListViewItemに格納するList<string>配列</param>
        /// <returns></returns>
        ListViewItem GetListViewItem(List<string> items)
        {
            //ListViewItemをnewする
            ListViewItem listViewItem = new ListViewItem(items[FileNameIndex]);

            //残りの要素を追加
            for (int i = DateNameIndex; i < items.Count; i++)
                listViewItem.SubItems.Add(items[i]);

            //格納したListViewItemの返却
            return listViewItem;
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

        /// <summary>
        /// ピッチの調節
        /// </summary>
        void InitPitch()
        {
            //ピッチの調節
            pitch = DefaultPitch;

            //スライダーの値を初期化
            pitchSlider.Value = (int)(pitch * PitchCorrect);
        }


    }
}
