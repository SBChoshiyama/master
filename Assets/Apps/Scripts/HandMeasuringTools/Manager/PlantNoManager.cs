using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantNoManager : MonoBehaviour
{
    /// <summary>
    /// �v���R���g���[���pGameObject
    /// </summary>
    private GameObject MeasureControllerObj;

    /// <summary>
    /// �v���R���g���[���pObject
    /// </summary>
    private MeasureController MeasureController;

    /// <summary>
    /// �c�ԍ����͉��GameObject
    /// </summary>
    private GameObject PlantNoPlateObj;

    /// <summary>
    /// �c�ԍ�A�p�[�g(�A���t�@�x�b�g��)�e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh PlantNoAText = default;

    /// <summary>
    /// �c�ԍ�B�o�[�g(�i���o�[��)�e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh PlantNoBText = default;

    /// <summary>
    /// �c�ԍ��o�[�g�e�L�X�g
    /// </summary>
    private string[] PlantNoASel = new string[] { "A", "B", "C","D","E","F","G","H","I","J","K","L","M","N","O","P"};
    private string[] PlantNoBSel = new string[] { "1", "2" };

    /// <summary>
    /// �c�ԍ��I��ԍ�
    /// </summary>
    private int ASel;
    private int BSel;

    /// <summary>
    /// �c�ԍ��f�[�^
    /// </summary>
    private string PlantNoData;

    // Start is called before the first frame update
    void Start()
    {
        // �ݒ胁�j���[�I�u�W�F�N�g
        MeasureControllerObj = GameObject.Find("MeasureController");
        MeasureController = MeasureControllerObj.GetComponent<MeasureController>();

        // �c�ԍ����͉��GameObject
        PlantNoPlateObj = GameObject.Find("PlantNoInputPlate");

        // ������
        PlantNoInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ������
    /// </summary>
    private void PlantNoInit()
    {
        // �ݒ菉����
        ASel = 0;
        BSel = 0;

        PlantNoAText.text = PlantNoASel[ASel];
        PlantNoBText.text = PlantNoBSel[BSel];
        PlantNoDataMake();
    }

    /// <summary>
    /// �c�ԍ�A�p�[�gUP�L�[
    /// </summary>
    public void PlantNoAUp()
    {
        ASel++;
        if (ASel >= PlantNoASel.Length)
            ASel = 0;
        PlantNoAText.text = PlantNoASel[ASel];
    }

    /// <summary>
    /// �c�ԍ�A�p�[�gDOWN�L�[
    /// </summary>
    public void PlantNoADown()
    {
        ASel--;
        if (ASel < 0)
            ASel = PlantNoASel.Length - 1;
        PlantNoAText.text = PlantNoASel[ASel];
    }

    /// <summary>
    /// �c�ԍ�B�p�[�gUP�L�[
    /// </summary>
    public void PlantNoBUp()
    {
        BSel++;
        if (BSel >= PlantNoBSel.Length)
            BSel = 0;
        PlantNoBText.text = PlantNoBSel[BSel];
    }

    /// <summary>
    /// �c�ԍ�B�p�[�gDOWN�L�[
    /// </summary>
    public void PlantNoBDown()
    {
        BSel--;
        if (BSel < 0)
            BSel = PlantNoBSel.Length - 1;
        PlantNoBText.text = PlantNoBSel[BSel];
    }

    /// <summary>
    /// �c�ԍ��擾
    /// </summary>
    public string GetPlantNo()
    {
        return PlantNoData;
    }

    /// <summary>
    /// Next�L�[
    /// </summary>
    public void NextKey()
    {
        // �c�ԍ�����
        PlantNoDataMake();
        // �c�ԍ���ʏ���
        PlantNoPlateObj.SetActive(false);
        // �ݒ�J�n�{�^���L����
        MeasureController.SettingMenuEnable();
    }

    /// <summary>
    /// �c�ԍ��f�[�^����
    /// </summary>
    private void PlantNoDataMake()
    {
        PlantNoData = PlantNoASel[ASel] + "-" + PlantNoBSel[BSel];
    }

}
