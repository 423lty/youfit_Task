using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507
{
    class ClickHandler
    {
        /// <summary>
        /// 派生コンストラクタ
        /// </summary>
        /// <param name="DoubleClickThreshold">間隔</param>
        /// <param name="onSingleClick">シングルクリック処理</param>
        /// <param name="onDoubleClick">ダブルクリック処理</param>
        /// <param name="clickTimer">クリックに使用する変数</param>
        public ClickHandler(System.Windows.Forms.Button button,Action LeftClick,Action RightClick)
        {
            this.button = button;
            this.LeftClik = LeftClick;
            this.RightClik = RightClick;
        }

        /// <summary>
        /// ボタンのクリック
        /// </summary>
        public void OnButton_MouseClick(object sender, MouseEventArgs e)
        {
            int halfWidth = button.Width / 2;
            if (e.X < halfWidth)
                LeftClik.Invoke();
            else if(e.X >= halfWidth)
                RightClik.Invoke(); 
        }
        
        //ボタン
        readonly System.Windows.Forms.Button button;

        readonly Action LeftClik;

        readonly Action RightClik;

    }
}
