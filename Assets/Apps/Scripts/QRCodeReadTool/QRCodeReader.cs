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

    // �ŐV�̌��o������
    public string CurrentQRText;

    // Start is called before the first frame update
    async void Start()
    {
        // QRCodeWatcher�̋����N�G�X�g
        await QRCodeWatcher.RequestAccessAsync();

        // �C���X�^���X�̐����ƃC�x���g�o�^
        qRCodeWatcher = new QRCodeWatcher();
        // �V�KQR�R�[�h�ǉ����C�x���g
        qRCodeWatcher.Added += QRCodeWatcherAdded;
        // QR�R�[�h�X�V���C�x���g
        qRCodeWatcher.Updated += QRCodeWatcherUpdated;
        // QR�R�[�h�폜���C�x���g
        qRCodeWatcher.Removed += QRCodeWatcherRemoved;

        // �e�L�X�g�����폜����
        CurrentQRText = "";

        // QR�R�[�h���o�̊J�n
        qRCodeWatcher.Start();

    }

    // Update is called once per frame
    void Update()
    {
        QRTextFrame.text = CurrentQRText;
    }
    /// <summary>
    /// �V�KQR�R�[�h�ǉ����C�x���g
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QRCodeWatcherAdded(object sender, QRCodeAddedEventArgs e)
    {
        // ���o����QR�R�[�h�����擾����
        QRCode codeInfo = e.Code;
        // �e�L�X�g����ۑ�����
        CurrentQRText = codeInfo.Data;
    }

    /// <summary>
    /// QR�R�[�h�X�V���C�x���g
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QRCodeWatcherUpdated(object sender, QRCodeUpdatedEventArgs e)
    {
        // ���o����QR�R�[�h�����擾����
        QRCode codeInfo = e.Code;
        // �e�L�X�g����ۑ�����
        CurrentQRText = codeInfo.Data;
    }

    /// <summary>
    /// QR�R�[�h�폜���C�x���g
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QRCodeWatcherRemoved(object sender, QRCodeRemovedEventArgs e)
    {
        // ���o����QR�R�[�h�����擾����
        QRCode codeInfo = e.Code;
        // �e�L�X�g�����폜����
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
