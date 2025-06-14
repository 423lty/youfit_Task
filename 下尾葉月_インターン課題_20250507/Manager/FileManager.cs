﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace 下尾葉月_インターン課題_20250507
{
    /// <summary>
    /// ファイルの保存や管理のクラス
    /// </summary>
    public partial class Form1
    {
        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        public void LoadFile(object sender, EventArgs e)
        {
            try
            {
                //データファイルを取得
                string dataFilePath = Path.Combine(saveDataDir, DataName);

                //データファイルを取得
                string volumePath = Path.Combine(saveSoundDir, VolumePitchTextName);

                //データファイルが存在する場合のみ実行
                if (System.IO.File.Exists(dataFilePath))
                {
                    //データを取得
                    string[] saveAllTextData = System.IO.File.ReadAllLines(dataFilePath);

                    //保存されているデータの取り出し
                    foreach (var saveTextData in saveAllTextData)
                    {
                        //カンマ区切りで取得
                        string[] datas = saveTextData.Split(Convert.ToChar(StorageDesignator));

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

                //ボリュームファイルが存在する場合
                if (System.IO.File.Exists(volumePath))
                {
                    //ボリュームとピッチを取得
                    string[] volumePitchAllTextData = System.IO.File.ReadAllLines(volumePath, Encoding.UTF8);

                    //配列ないかつボリュームのテキストデータが存在している場合|float に変換した場合Defaultの値を代入
                    if (volumePitchAllTextData.Length > VolumeTextIndex)
                        if (!float.TryParse(volumePitchAllTextData[VolumeTextIndex], NumberStyles.Float, CultureInfo.InvariantCulture, out volume))
                            volume = DefaultVolume;
                    //配列ないかつピッチのテキストデータが存在している場合|float に変換した場合Defaultの値を代入
                    if (volumePitchAllTextData.Length > PitchTextIndex)
                        if (!float.TryParse(volumePitchAllTextData[PitchTextIndex], NumberStyles.Float, CultureInfo.InvariantCulture, out pitch))
                            pitch = DefaultVolume;
                }

                //それぞれの音量設定
                AttachVolume(volume);
                AttachPitch(pitch);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// ファイルを保存
        /// </summary>
        public void SaveFile(object sender, EventArgs e)
        {
            //trycatchで失敗した場合に防止
            try
            {
                //再生されていたりする場合の対処
                EndPlayBack();

                //musicList内にオブジェクトが存在している場合
                if (musicList.Items.Count > 0)
                {
                    //ファイルが存在しない場合は作成
                    Directory.CreateDirectory(saveDataDir);

                    //データを保存するファイルのパスを取得
                    string fileDataPath = Path.Combine(saveDataDir, DataName);

                    //関連するデータを連結
                    List<string> content = new List<string>();
                    foreach (ListViewItem item in musicList.Items)
                    {
                        //itemを格納する配列
                        List<string> items = new List<string>();

                        //item配列の要素を取り出して格納
                        foreach (ListViewItem.ListViewSubItem i in item.SubItems)
                            items.Add(i.Text);

                        //ファイルにカンマ区切りで格納
                        content.Add(string.Join(StorageDesignator, items));
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
                //else
                //{
                //    if (Directory.Exists(saveDataDir) && Directory.Exists(saveSoundDir))
                //    {
                //        //保存データファイルを検索
                //        string fileDataPath = Path.Combine(saveDataDir, DataName);

                //        //
                //        string leftTextFile = Path.Combine(saveDataDir, VolumePitchTextName);

                //        //音楽ファイルのそれぞれのデータを削除する
                //        if (Directory.Exists(saveSoundDir))
                //        {
                //            //ディレクトリないに存在する音声ファイルを一つずつ取り出して削除
                //            foreach (string file in Directory.GetFiles(saveSoundDir))
                //                System.IO.File.Delete(file);

                //            //ディレクトリないにファイルが存在しない場合ファイルを削除
                //            //DeleteDirectory(saveSoundDir);
                //        }

                //        if (System.IO.File.Exists(fileDataPath) && !fileDataPath.Equals(leftTextFile, StringComparison.OrdinalIgnoreCase))
                //            System.IO.File.Delete(fileDataPath);

                //        //保存するテキストファイルが存在しない場合削除する
                //        if (Directory.Exists(saveDataDir))
                //        {
                //            string[] remainingFiles = Directory.GetFiles(saveDataDir);
                //            if (!remainingFiles.Any(f => !f.Equals(leftTextFile, StringComparison.OrdinalIgnoreCase)))
                //                Directory.Delete(saveDataDir);
                //        }
                //    }
                //}

                //テキストファイルに保存するデータを格納するList
                List<string> volumePitchContent = new List<string>() {
                    volume.ToString(CultureInfo.InvariantCulture),
                    pitch.ToString(CultureInfo.InvariantCulture),
                };

                //音量の保存位置の取得
                string volumePitchDataPath = Path.Combine(saveSoundDir, VolumePitchTextName);

                //文字列型にして保存
                System.IO.File.WriteAllLines(volumePitchDataPath, volumePitchContent, Encoding.UTF8);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// ファイルを参照する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BrowseFilesEvent(object sender, EventArgs e)
        {
            //ファイルを開く
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = OpenTitleText;
            ofd.Filter = FilterText;

            //選択したファイルが大丈夫な場合
            if (ofd.ShowDialog() == DialogResult.OK)
                FilterMusicType(ofd.FileName);
        }

        /// <summary>
        /// ドラッグ＆ドロップの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dragDropBox_DragDrop(object sender, DragEventArgs e)
        {
            //try catch
            try
            {
                //すべてのデータを取得
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                //ファイルが以下に該当するか
                foreach (string file in files)
                    FilterMusicType(file);
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
        public void dragDropBox_DragEnter(object sender, DragEventArgs e)
        {
            //ファイルをコピーする
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// 指定したディレクトリが存在する場合ディレクトリを削除する
        /// </summary>
        /// <param name="dirPath">削除するディレクトリ</param>
        public void DeleteDirectory(string dirPath)
        {
            //ディレクトリないにファイルが存在しない場合ファイルを削除
            if (Directory.GetFiles(dirPath).Length == NoFileDirectory)
                Directory.Delete(dirPath);
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

            //格納したListViewItemの返却
            return listViewItem;
        }

        //ローカルディスクへのパス
        const Environment.SpecialFolder localFilePath = Environment.SpecialFolder.MyDocuments;

        //作成または参照するファイル名
        const string SaveFileName = "VoiShredder";

        //作成して保存するファイル名
        const string DataName = "Data.txt";

        //読み込むデータファイル名
        const string LoadFileName = "LoadFile";

        //読み込むサウンドファイル名
        const string SoundFileName = "soundFile";

        //ボリュームのテキストファイル名
        const string VolumePitchTextName = "volumePitchFile.txt";

        //保存するデータファイルパス
        readonly string saveDataDir = Path.Combine(Environment.GetFolderPath(localFilePath), SaveFileName, LoadFileName);

        //保存する音楽ファイルパス
        readonly string saveSoundDir = Path.Combine(Environment.GetFolderPath(localFilePath), SaveFileName, SoundFileName);

        //保存するときの格納指定子
        const string StorageDesignator = ",";

        //ボリュームのテキストファイルのインデックス番号
        const int VolumeTextIndex = 0;

        //ピッチのテキストファイルのインデックス番号
        const int PitchTextIndex = 1;

        //ディレクトリに存在しないファイルの内部個数
        const int NoFileDirectory = 0;

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

        //ファイルのパスを記憶する変数
        Dictionary<string, string> fullPathDirectory = new Dictionary<string, string>();


        //Filterで使用するテキストデータ
        const string FilterText = "*.mp3ファイル(*.mp3) *.wavファイル(*.wav)|*.mp3;*wav";

        //表示するタイトルのテキスト
        const string OpenTitleText = "ファイルを開く";
    }
}
