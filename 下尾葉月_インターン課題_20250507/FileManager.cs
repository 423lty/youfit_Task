using NAudio.Wave;
using System;
using System.Collections.Generic;
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
    /// ファイルの保存や管理のクラス
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// フォームを開いたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1_Load(object sender, EventArgs e)
        {
            //読み込み
            LoadFile(sender, e);

            //リピート画像のリサイズ
            Bitmap resizedIcon = new Bitmap(Properties.Resources._default, repeat.Width, repeat.Height);

            //画像の添付
            repeat.Image = resizedIcon;
            
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
        /// ファイルを読み込む
        /// </summary>
        void LoadFile(object sender, EventArgs e)
        {
            try
            {
                //データファイルを取得
                string dataFilePath = Path.Combine(saveDataDir, dataName);

                //ファイルが存在する場合のみ実行
                if (System.IO.File.Exists(dataFilePath))
                {
                    //データを取得
                    string[] saveAllTextData = System.IO.File.ReadAllLines(dataFilePath);

                    //保存されているデータの取り出し
                    foreach (var saveTextData in saveAllTextData)
                    {
                        //カンマ区切りで取得
                        string[] datas = saveTextData.Split(',');

                        if (datas.Length == SaveFileDataMaxIndex)
                        {

                            //各々の要素を取得
                            string fileName = datas[FileNameIndex];
                            string dateName = datas[DateNameIndex];
                            string typeName = datas[TypeNameIndex];
                            string sizeName = datas[SizeNameIndex];

                            List<string> fileInfo = new List<string>() {
                                fileName , dateName , typeName, sizeName
                            };

                            //パスを取得
                            string soundDataPath = Path.Combine(saveSoundDir, fileName);

                            //音楽データが存在するか確認
                            if (System.IO.File.Exists(soundDataPath))
                            {
                                //Listに格納してmusicListに格納
                                ListViewItem data = GetListViewItem(fileInfo);
                                musicList.Items.Add(data);

                                //データをfullPathDirectoryに保存
                                fullPathDirectory[fileName] = soundDataPath;

                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// ファイルを保存
        /// </summary>
        void SaveFile(object sender, EventArgs e)
        {
            //trycatchで失敗した場合に防止
            try
            {
                //再生されていたりする場合の対処
                if (wavePlayer.PlaybackState == PlaybackState.Playing || wavePlayer.PlaybackState == PlaybackState.Stopped)
                {
                    wavePlayer.Stop();
                    wavePlayer.Dispose();
                    wavePlayer = null;
                }
                if (audio != null)
                {
                    audio.Dispose();
                    audio = null;
                }

                //musicList内にオブジェクトが存在している場合
                if (musicList.Items.Count > 0)
                {
                    //ファイルが存在しない場合は作成
                    Directory.CreateDirectory(saveDataDir);

                    //データを保存するファイルのパスを取得
                    string fileDataPath = Path.Combine(saveDataDir, dataName);

                    //関連するデータを連結
                    List<string> content = new List<string>();
                    foreach (ListViewItem item in musicList.Items)
                    {
                        //itemを格納する配列
                        List<string> items = new List<string>();

                        //item配列の要素を取り出して格納
                        foreach (ListViewItem.ListViewSubItem i in item.SubItems)
                        {
                            items.Add(i.Text);
                        }

                        //ファイルにカンマ区切りで格納
                        content.Add(string.Join(",", items));

                    }

                    //ファイルのデータを文字化け防止して保存
                    System.IO.File.WriteAllLines(fileDataPath, content, Encoding.UTF8);

                    //音楽ファイルが存在しない場合は作成
                    Directory.CreateDirectory(saveSoundDir);

                    //fullpathの音楽データの保存
                    foreach (var v in fullPathDirectory)
                    {
                        //ファイルのキーとパスを取得
                        string fileName = v.Key;
                        string filePath = v.Value;
                        string dst = Path.Combine(saveSoundDir, fileName);

                        //すでにファイルが存在していた場合保存をしない
                        if (System.IO.File.Exists(dst))
                            continue;

                        //上書きで保存
                        System.IO.File.Copy(filePath, dst, true);
                    }

                    //保存音楽データのディレクトリ
                    string fileSoundDir = saveSoundDir;

                    //データを取得
                    string[] saveAllTextData = System.IO.File.ReadAllLines(fileDataPath);

                    //ファイルのパスを格納するリスト
                    List<string> fileDataList = new List<string>();

                    //カンマ区切りでデータを取得してファイルパスをListに格納
                    foreach (string saveTextData in saveAllTextData)
                        fileDataList = saveAllTextData.Select(line => line.Split(',')[FileNameIndex]).ToList();

                    //ファイルの内部に一致するデータがない場合削除
                    foreach (string soundData in Directory.GetFiles(fileSoundDir).Where(path => !fileDataList.Contains(Path.GetFileName(path))))
                    {
                        try
                        {
                            System.IO.File.Delete(soundData);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }


                }
                //一つも保存するデータが存在しない場合
                else
                {
                    if (Directory.Exists(saveDataDir) && Directory.Exists(saveSoundDir))
                    {
                        //保存データファイルを検索
                        string fileDataPath = Path.Combine(saveDataDir, dataName);

                        //保存データのディレクトリ
                        string fielDataDir = saveDataDir;

                        //保存音楽データのディレクトリ
                        string fileSoundDir = saveSoundDir;


                        //音楽ファイルのそれぞれのデータを削除する
                        if (Directory.Exists(fileSoundDir))
                        {
                            //ディレクトリないに存在する音声ファイルを一つずつ取り出して削除
                            foreach (string file in Directory.GetFiles(fileSoundDir))
                                System.IO.File.Delete(file);

                            //ディレクトリないにファイルが存在しない場合ファイルを削除
                            DeleteDirectory(fileSoundDir);

                        }

                        if (System.IO.File.Exists(fileDataPath))
                            System.IO.File.Delete(fileDataPath);

                        //保存するテキストファイルが存在しない場合削除する
                        if (Directory.Exists(fielDataDir))
                            Directory.Delete(fielDataDir);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 音楽ファイルの要素を取得して詳しい情報を返却
        /// </summary>
        /// <returns></returns>
        List<string> GetListViewItem()
        {
            List<string> content = new List<string>();

            foreach (ListViewItem item in musicList.Items)
            {
                //それぞれの要素を格納
                string fileName = item.SubItems[FileNameIndex].Text;
                string date = item.SubItems[DateNameIndex].Text;
                string size = item.SubItems[TypeNameIndex].Text;
                string type = item.SubItems[SizeNameIndex].Text;

                //要素を追加
                content.Add($"{fileName},{date},{size},{type}");
            }

            return content;

        }

        /// <summary>
        /// 指定したディレクトリが存在する場合ディレクトリを削除する
        /// </summary>
        /// <param name="dirPath">削除するディレクトリ</param>
        void DeleteDirectory(string dirPath)
        {
            //ディレクトリないにファイルが存在しない場合ファイルを削除
            if (Directory.GetFiles(dirPath).Length == 0)
                Directory.Delete(dirPath);
        }

        /// <summary>
        /// ファイルを参照する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BrowseFilesEvent(object sender, EventArgs e)
        {
            //ファイルを開く
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "ファイルを開く";
            ofd.Filter = "*.mp3ファイル(*.mp3) *.wavファイル(*.wav)|*.mp3;*wav";

            //選択したファイルが大丈夫な場合
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;
                FilterMusicType(filePath);
            }
        }

        /// <summary>
        /// ドラッグ＆ドロップの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dragDropBox_DragDrop(object sender, DragEventArgs e)
        {
            //try catch
            try
            {
                //すべてのデータを取得
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                //ファイルが以下に該当するか
                foreach (string file in files)
                {
                    FilterMusicType(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// ファイルを置いたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dragDropBox_DragEnter(object sender, DragEventArgs e)
        {
            //ファイルをコピーする
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        //ローカルディスクへのパス
        const Environment.SpecialFolder localFilePath = Environment.SpecialFolder.MyDocuments;

        //作成または参照するファイル名
        const string saveFileName = "VoiShredder";

        //作成して保存するファイル名
        const string dataName = "Data.txt";

        //読み込むデータファイル名
        const string LoadFileName = "LoadFile";

        //読み込むサウンドファイル名
        const string soundFileName = "soundFile";

        //保存するデータファイルパス
        readonly string saveDataDir = Path.Combine(Environment.GetFolderPath(localFilePath), saveFileName, LoadFileName);

        //保存する音楽ファイルパス
        readonly string saveSoundDir = Path.Combine(Environment.GetFolderPath(localFilePath), saveFileName, soundFileName);

        //ファイルのインデックス
        const int FileNameIndex = 0;

        //日付のインデックス
        const int DateNameIndex = 1;

        //種類のインデックス
        const int TypeNameIndex = 2;

        //サイズのインデックス
        const int SizeNameIndex = 3;

        //一行に存在する最大の要素数
        const int SaveFileDataMaxIndex = 4;

    }
}
