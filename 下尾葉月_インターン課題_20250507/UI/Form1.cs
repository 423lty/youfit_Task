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
            AttachPitch(DefaultPitch);

            //ダブルクリックの有効か
            SetStyle(ControlStyles.StandardDoubleClick, true);
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

            //クリックイベントの初期化
            LoadClickHandle();
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

        private void Button_DoubleClick(object sender, EventArgs e)
        {

        }

        private void Button_Click(object sender, EventArgs e)
        {

        }
    }
}
