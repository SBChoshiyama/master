using UnityEngine;

public class StemModeSelector : MonoBehaviour
{
    /// <summary>
    /// �s���胂�[�h
    /// </summary>
    public enum StemMode
    {
        /// <summary>
        /// �s��
        /// </summary>
        Length,

        /// <summary>
        /// �s�a
        /// </summary>
        Diameter,

        /// <summary>
        /// 1�ӂł̌s�a
        /// </summary>
        SingleDiameter
    }

    /// <summary>
    /// �s���胂�[�h
    /// </summary>
    public StemMode InnerStemMode { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        // ������
        InnerStemMode = StemMode.Length;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void UseStemLength()
    {
        // �s�����[�h�ɕύX
        InnerStemMode = StemMode.Length;

        Debug.Log($"InnerStemMode = {InnerStemMode}");
    }

    public void UseStemdiameter()
    {
        // �s�a���[�h�ɕύX
        InnerStemMode = StemMode.Diameter;

        Debug.Log($"InnerStemMode = {InnerStemMode}");
    }

    public void UseStemSimpleDiameter()
    {
        // 1�ӂł̌s�a���[�h�ɕύX
        InnerStemMode = StemMode.SingleDiameter;

        Debug.Log($"InnerStemMode = {InnerStemMode}");
    }
}