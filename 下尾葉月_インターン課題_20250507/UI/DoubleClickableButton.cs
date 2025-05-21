using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;

namespace 下尾葉月_インターン課題_20250507.UI
{
    public class DoubleClickableButton : System.Windows.Forms.Button
    {
        public DoubleClickableButton(string leftText,string rightText)
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.BackColor = Color.Transparent; 
            this.FlatAppearance.BorderSize = 0;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.leftText = leftText;
            this.rightText = rightText;
        }

        protected override CreateParams CreateParams {  
            get {
                //ダブルクリックの受付
                const int CS_DBLCLKS = 0x8;

                //親クラスの設定を取得
                CreateParams cp = base.CreateParams;
                
                //クラススタイルにダブルクリック対応を付与
                cp.ClassStyle |= CS_DBLCLKS;

                //カスタムのパラメータの返却
                return cp;
            } 
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int halfWidth = this.Width / 2;

            // マウス位置に応じてフラグを更新
            bool newHoverLeft = e.X < halfWidth;
            bool newHoverRight = e.X >= halfWidth;

            if (newHoverLeft != isHoverLeft || newHoverRight != isHoverRight)
            {
                isHoverLeft = newHoverLeft;
                isHoverRight = newHoverRight;
                Invalidate(); // 再描画
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            isHoverLeft = false;
            isHoverRight = false;
            Invalidate(); // マウスが離れたら再描画
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs prev)
        {
            //規定クラスを呼び出す
            base.OnPaint(prev);

            Graphics g = prev.Graphics;
            Rectangle leftRect = new Rectangle(0, 0, this.Width / Half, this.Height);
            Rectangle rightRect = new Rectangle(this.Width / Half, 0, this.Width / Half, this.Height);
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            if (isHoverLeft)
                using (Brush hover = new SolidBrush(Color.LightBlue))
                    g.FillRectangle(hover, leftRect);

            if (isHoverRight)
                using (Brush hover = new SolidBrush(Color.LightGreen))
                    g.FillRectangle(hover, rightRect);


            //中央に線を引く状態にする
            if (DrawCenterLine)
            {
                int centerX = this.Width / Half;

                if (HighlightLeft)
                    g.FillRectangle(System.Drawing.Brushes.Blue, 0, 0, centerX, this.Height);

                if (HighlightRight)
                    g.FillRectangle(System.Drawing.Brushes.Green, centerX, 0, this.Width - centerX, this.Height);

                //線の太さを決めて描画
                using (System.Drawing.Pen pen = new System.Drawing.Pen(Color.Black))
                    prev.Graphics.DrawLine(pen, centerX, 0, centerX, this.Height);

                //ブラシを生成して描画
                using (Brush brush = new SolidBrush(this.ForeColor))
                {
                    Font leftFont = new Font(this.Font.FontFamily, FontSize, System.Drawing.FontStyle.Regular);
                    Font rightFont = new Font(this.Font.FontFamily, FontSize, System.Drawing.FontStyle.Bold);

                    using (StringFormat format = new StringFormat())
                    {
                        format.LineAlignment = StringAlignment.Center;
                        format.Alignment = StringAlignment.Center;
                        prev.Graphics.DrawString(leftText, leftFont, brush, leftRect, format);
                    }

                    using (StringFormat format = new StringFormat())
                    {
                        format.LineAlignment = StringAlignment.Center;
                        format.Alignment = StringAlignment.Center;
                        prev.Graphics.DrawString(rightText, rightFont, brush, rightRect, format);
                    }
                }
            }

        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            int center=this.Width/Half;

            HighlightLeft = mevent.X < center;
            HighlightRight = mevent.X >= center;

            this.Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            HighlightLeft= false;
            HighlightRight= false;
            this.Invalidate();
        }

        /// <summary>
        /// 中央に線を引くか
        /// </summary>
        public bool DrawCenterLine { get; set; }

        /// <summary>
        /// 左側に描画するテキスト
        /// </summary>
        public string leftText { get;private set; }

        /// <summary>
        /// 右側に描画するテキスト
        /// </summary>
        public string rightText { get;private set; }

        //半分にする
        const int Half = 2;

        //フォントの大きさ
        const int FontSize = 30;

        //線の太さ
        const int LineWeight = 1;

        //
        public bool HighlightLeft { get; set; }

        //
        public bool HighlightRight { get; set; }

        bool isHoverLeft = false;
        bool isHoverRight = false;

        const int borderRadius = 10;

    }
}
