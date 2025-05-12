using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507
{
    /// <summary>
    /// 音楽の情報管理
    /// </summary>
    public partial class Form1 : Form
    {

        /// <summary>
        /// 音楽ファイルを追加する
        /// </summary>
        /// <param name="file"></param>
        private void MusicFileAddItems(string file)
        {
            //音楽の情報を区分
            FileInfo fi = new FileInfo(file);

            //それぞれの要素を取得
            string fileName = fi.Name;
            string addDate = DateTime.Now.ToString("yyyy/MM/dd");
            string fileType = fi.Extension.TrimStart('.');
            string fileSize = (fi.Length / (B * B)).ToString("0.0") + "MB";

            //パスを追加
            fullPathDirectory[fileName] = file;

            //すべての要素をlistに格納
            List<string> filesInfo = new List<string>() { fileName, addDate, fileType, fileSize };

            //ListViewItemをnewする
            ListViewItem listViewItem = GetListViewItem(filesInfo);

            //音楽ファイルを追加
            this.musicList.Items.Add(listViewItem);
        }

        /// <summary>
        /// 音楽の形式チェック
        /// </summary>
        /// <param name="file">選択したファイル</param>
        private void FilterMusicType(string file)
        {
            //.mp3または.wavのみ
            if (file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                //音楽ファイルを追加
                MusicFileAddItems(file);
            else
                MessageBox.Show("選択されたファイルは対応しておりません");
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

            return listViewItem;
        }

        /// <summary>
        /// 指定した音楽を削除する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteMusic(object sender, EventArgs e)
        {
            //音楽ファイルを選択していない場合処理をしない
            if (musicList.SelectedItems.Count == FileNameIndex)
            {
                MessageBox.Show("削除する音楽ファイルを選択してください");
                return;
            }

            DialogResult result = MessageBox.Show(
                "本当に削除しますか？",
                "リスト削除",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation
                );

            if (result == DialogResult.Yes)
            {
                //指定した音楽ファイルを取得
                ListViewItem listViewItem = musicList.SelectedItems[FileNameIndex];

                //指定した音楽のインデックス番号を取得
                int selectedIndex = musicList.Items.IndexOf(listViewItem);

                //指定した要素を削除する
                musicList.Items.RemoveAt(selectedIndex);
            }
        }

        //1MB
        const int B = 1024;

    }
}
