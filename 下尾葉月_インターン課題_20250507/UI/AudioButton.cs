using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507
{

    /// <summary>
    /// ボタンの初期化
    /// </summary>
    public partial class Form1
    {
        /// <summary>
        /// iconの初期化
        /// </summary>
        public void InitButtonIcon()
        {
            //リピート
            repeat.Image = SettingButtonIcon(repeat, defaultButtonIconImage);

            //次
            nextMusicButton.Image = SettingButtonIcon(nextMusicButton, nextMusicButtonIconImage);

            //前
            previousMusicButton.Image = SettingButtonIcon(previousMusicButton, previousMusicButtonIconImage);

            deleteButton.Image=SettingButtonIcon(deleteButton,trashBoxButtonIconImage);
            //初期のアイコンの設定
            UpdatePlayButtonIcon();

            //ボタンを丸くする
            ButtonRound(repeat);
            ButtonRound(nextMusicButton);
            ButtonRound(previousMusicButton);
            ButtonRound(playBack);

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

            //リピートする場合
            if (repeat.Image != null && isRepeatMusic)
                repeat.Image = SettingButtonIcon(repeat, repeatButtonIconImage);

            //リピートしない場合
            if (repeat.Image != null && !isRepeatMusic)
                repeat.Image = new Bitmap(Properties.Resources._default, repeat.Width, repeat.Height);
        }

        /// <summary>
        /// 音楽の次再生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextMusicButton_Click(object sender, EventArgs e)
        {
            // 現在選択されているインデックス選択していなければ-1を返却
            int currentIndex = musicList.SelectedIndices.Count > 0 ? musicList.SelectedIndices[0] : -1;

            //音楽ファイルを選択している場合
            if (currentIndex != -1)
            {
                // 次の要素が存在するか格納
                bool hasNext = currentIndex < musicList.Items.Count - 1;

                //存在している場合次の音楽を指定して再生/存在しない場合最初の音楽に戻して停止する
                if (hasNext)
                    MusicSkip(currentIndex + 1);
                else
                    MusicSkip(StartMusicIndex);
            }
            else
                MessageBox.Show("音楽ファイルを選択してください");
        }

        /// <summary>
        /// 前回の音楽再生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previousMusicButton_Click(object sender, EventArgs e)
        {
            // 現在選択されているインデックス選択していなければ-1を返却
            int currentIndex = musicList.SelectedIndices.Count > 0 ? musicList.SelectedIndices[0] : -1;

            //音楽ファイルを選択している場合
            if (currentIndex != -1)
            {
                //音楽が0以上
                if (currentIndex > 0)
                    MusicSkip(currentIndex - 1);
                //最初の曲の場合は停止
                else
                    MusicSkip(StartMusicIndex, false);
            }
            else
                MessageBox.Show("音楽ファイルを選択してください");
        }

        /// <summary>
        /// iconの設定
        /// </summary>
        /// <param name="button">設定するiconボタン</param>
        /// <param name="icon">設定するicon画像</param>
        /// <returns>buttonにフィットしたサイズの画像のへ客</returns>
        public Bitmap SettingButtonIcon(Button button, Bitmap icon)
        {
            //buttonの要素がnullまたは空ではない場合空にする
            if (!String.IsNullOrEmpty(button.Text))
                button.Text = String.Empty;

            //リピート画像のリサイズ
            return new Bitmap(icon, button.Width - ButtonImageCorrect, button.Height - ButtonImageCorrect);
        }

        /// <summary>
        /// 再生ボタン等の切り替え
        /// </summary>
        /// <param name="flg">Playbutton:true/Stopbotton:false デフォルトでtrue</param>
        public void UpdatePlayButtonIcon(bool flg = true)
        {
            //再生ボタンに変更
            if (flg)
                playBack.Image = SettingButtonIcon(playBack, playMusicButtonIconImage);
            else
                playBack.Image = SettingButtonIcon(playBack, stopMusicButtonIconImage);

        }

        /// <summary>
        /// ボタンを丸くする
        /// </summary>
        /// <param name="button"></param>
        public void ButtonRound(Button button)
        {
            //適応するボタンの余計な余白を取り除く
            button.Padding = new Padding(0);
            button.Margin = new Padding(0);

            //位置を中央に再設定
            button.ImageAlign = ContentAlignment.MiddleCenter;

            //新しい図形を定義
            GraphicsPath gp = new GraphicsPath();

            //図形に円を追加
            gp.AddEllipse(BtnCenter, BtnCenter, button.Width, button.Height);

            //ボタンの描画の指定
            button.Region = new Region(gp);

            //ボタンの背景の調整
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

        }


        //リピートしない場合のアイコン
        readonly Bitmap defaultButtonIconImage = Properties.Resources._default;

        //リピートする場合のアイコン
        readonly Bitmap repeatButtonIconImage = Properties.Resources.repeat;

        //次の再生のアイコン
        readonly Bitmap nextMusicButtonIconImage = Properties.Resources.next;

        //前の再生のアイコン
        readonly Bitmap previousMusicButtonIconImage = Properties.Resources.previous;

        //再生のアイコン
        readonly Bitmap playMusicButtonIconImage = Properties.Resources.play;

        //停止のアイコン
        readonly Bitmap stopMusicButtonIconImage = Properties.Resources.stop;

        //ゴミ箱のアイコン
        readonly Bitmap trashBoxButtonIconImage = Properties.Resources.TrashBox;

        //ボタンの中心
        const int BtnCenter = 0;

        //ボタンのサイズの調節
        const int ButtonImageCorrect = 5;
    }
}
