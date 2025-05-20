using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public ClickHandler(int DoubleClickThreshold, Action onSingleClick, Action onDoubleClick, System.Windows.Forms.Timer clickTimer)
        {
            this.DoubleClickThreshold = DoubleClickThreshold;
            this.onSingleClick = onSingleClick;
            this.onDoubleClick = onDoubleClick;

            this.clickTimer = clickTimer;
            this.clickTimer.Interval = this.DoubleClickThreshold;
            this.clickTimer.Tick += this.clickTimer_Tick;
        }

        /// <summary>
        /// ボタンのクリック
        /// </summary>
        public void OnButton_Click(object sender, EventArgs e)
        {
            //現在の時間を取得
            DateTime nowTime = DateTime.Now;

            if (waitingSecondClick)
            {
                //間隔を取得
                double elapsed = (nowTime - lastClickTime).TotalMilliseconds;

                //一定値いかの場合
                if (elapsed <= DoubleClickThreshold)
                {
                    //時間の停止
                    clickTimer.Stop();

                    //二回目を待つ状態の解除
                    waitingSecondClick = false;

                    onDoubleClick.Invoke();

                    return;
                }

            }

            //最初のクリック
            waitingSecondClick = true;
            lastClickTime = nowTime;
            clickTimer.Stop();
            clickTimer.Start();

        }

        private void clickTimer_Tick(object sender, EventArgs e)
        {
            //タイマーを止める
            clickTimer.Stop();

            //一回かそれ以上クリックされなかった
            if (waitingSecondClick)
            {
                //状態の解除
                waitingSecondClick = false;
                onSingleClick.Invoke();
            }
        }

        //間隔
        readonly int DoubleClickThreshold;

        //クリック
        System.Windows.Forms.Timer clickTimer;

        //クリックを待っている状態
        bool waitingSecondClick = false;

        //最後にクリックした時間
        DateTime lastClickTime = DateTime.MinValue;

        //シングル処理
        readonly Action onSingleClick;

        //ダブルクリック
        readonly Action onDoubleClick;

    }
}
