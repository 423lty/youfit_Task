using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            InitializeComponent();
        }

        //ファイルのパスを記憶する変数
        Dictionary<string, string> fullPathDirectory = new Dictionary<string, string>();

        private void musicTimer_Tick(object sender, EventArgs e)
        {
            //audioに音楽ファイルが付与されている状態
            if (audio != null)
            {
                //現在の音楽再生時間を取得
                currentMusicPlaybackTime = GetMusicCurrentTime();

                //現在の再生時間が総再生時間に達した場合
                if (currentMusicPlaybackTime >= allMusicPlaybackTime)
                {

                    //TimerツールのEnableを切る
                    musicTimer.Stop();

                    //リピートしていない場合
                    if (!isRepeatMusic)
                    {
                        //次のインデックスを取得
                        int nextIndex = currentMusicIndex + 1;

                        //もし次のインデックスが存在している場合
                        if (nextIndex < musicList.Items.Count)
                        {
                            //選択された状態を付与
                            musicList.Items[nextIndex].Selected = true;
                            musicList.Items[nextIndex].Focused = true;
                            musicList.EnsureVisible(nextIndex);

                            //現在の再生インデックスの更新
                            currentMusicIndex = nextIndex;

                            //次の音楽を再生
                            PlayNextMusic(currentMusicIndex);
                        }
                        //存在していない場合
                        else
                            StopMusic();

                    }
                    //リピート再生
                    else
                        PlayNextMusic(currentMusicIndex);
                }
            }
                
        }
    }
}
