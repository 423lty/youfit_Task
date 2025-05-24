using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507.UI
{
    public class DoubleClickableButton : System.Windows.Forms.Button
    {
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
    }
}
