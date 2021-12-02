using UnityEngine;

public class StemModeSelector : MonoBehaviour
{
    /// <summary>
    /// Œs‘ª’èƒ‚[ƒh
    /// </summary>
    public enum StemMode
    {
        /// <summary>
        /// Œs’·
        /// </summary>
        Length,

        /// <summary>
        /// ŒsŒa
        /// </summary>
        Diameter,

        /// <summary>
        /// 1•Ó‚Å‚ÌŒsŒa
        /// </summary>
        SingleDiameter
    }

    /// <summary>
    /// Œs‘ª’èƒ‚[ƒh
    /// </summary>
    public StemMode InnerStemMode { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        // ‰Šú‰»
        InnerStemMode = StemMode.Length;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void UseStemLength()
    {
        // Œs’·ƒ‚[ƒh‚É•ÏX
        InnerStemMode = StemMode.Length;

        Debug.Log($"InnerStemMode = {InnerStemMode}");
    }

    public void UseStemdiameter()
    {
        // ŒsŒaƒ‚[ƒh‚É•ÏX
        InnerStemMode = StemMode.Diameter;

        Debug.Log($"InnerStemMode = {InnerStemMode}");
    }

    public void UseStemSimpleDiameter()
    {
        // 1•Ó‚Å‚ÌŒsŒaƒ‚[ƒh‚É•ÏX
        InnerStemMode = StemMode.SingleDiameter;

        Debug.Log($"InnerStemMode = {InnerStemMode}");
    }
}