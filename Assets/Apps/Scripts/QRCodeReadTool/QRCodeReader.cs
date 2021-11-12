using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.QR;


public class QRCodeReader : MonoBehaviour
{
    public Text QRTextFrame;

    // QRCodeWatcher
    private QRCodeWatcher qRCodeWatcher;

    // 最新の検出文字列
    public string CurrentQRText;

    // Start is called before the first frame update
    async void Start()
    {
        // QRCodeWatcherの許可リクエスト
        await QRCodeWatcher.RequestAccessAsync();

        // インスタンスの生成とイベント登録
        qRCodeWatcher = new QRCodeWatcher();
        // 新規QRコード追加時イベント
        qRCodeWatcher.Added += QRCodeWatcherAdded;
        // QRコード更新時イベント
        qRCodeWatcher.Updated += QRCodeWatcherUpdated;
        // QRコード削除時イベント
        qRCodeWatcher.Removed += QRCodeWatcherRemoved;

        // テキスト情報を削除する
        CurrentQRText = "";

        // QRコード検出の開始
        qRCodeWatcher.Start();

    }

    // Update is called once per frame
    void Update()
    {
        QRTextFrame.text = CurrentQRText;
    }
    /// <summary>
    /// 新規QRコード追加時イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QRCodeWatcherAdded(object sender, QRCodeAddedEventArgs e)
    {
        // 検出したQRコード情報を取得する
        QRCode codeInfo = e.Code;
        // テキスト情報を保存する
        CurrentQRText = codeInfo.Data;
    }

    /// <summary>
    /// QRコード更新時イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QRCodeWatcherUpdated(object sender, QRCodeUpdatedEventArgs e)
    {
        // 検出したQRコード情報を取得する
        QRCode codeInfo = e.Code;
        // テキスト情報を保存する
        CurrentQRText = codeInfo.Data;
    }

    /// <summary>
    /// QRコード削除時イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QRCodeWatcherRemoved(object sender, QRCodeRemovedEventArgs e)
    {
        // 検出したQRコード情報を取得する
        QRCode codeInfo = e.Code;
        // テキスト情報を削除する
        CurrentQRText = "";
    }

    // QR Code Text Erase
    public void EraseQRText()
    {
        Debug.Log("Push");
        CurrentQRText = "";
        QRTextFrame.text = CurrentQRText;

    }
}
