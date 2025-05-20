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
        /// ピッチバーの調節
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pitchSlider_Scroll(object sender, EventArgs e)
        {
            AttachPitch(pitchSlider.Value);
        }

        /// <summary>
        /// ボリュームバーの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            AttachVolume(volumeBar.Value);
        }
    }
}
