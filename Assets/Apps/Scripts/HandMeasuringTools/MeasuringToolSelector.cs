using System.Collections.Generic;
using UnityEngine;

namespace HKT
{
    /// <summary>
    /// ハンドメジャーツールを切り替えるためのクラス
    /// 参考動画：https://twitter.com/hi_rom_/status/1267544392962699264
    /// </summary>
    public class MeasuringToolSelector : MonoBehaviour
    {
        /// <summary>
        /// 測定結果
        /// </summary>
        public float LineDistance;

        /// <summary>
        /// MeasuringToolモード
        /// </summary>
        public enum MeasuringTool
        {
            /// <summary>
            /// 片手測定
            /// </summary>
            OneHandRuler = 0,

            /// <summary>
            /// 両手人差し指測定
            /// </summary>
            TwoHandsRuler,

            /// <summary>
            /// 両手親指測定
            /// </summary>
            TwoHandsRulerThumbTip
        }

        [SerializeField]
        private List<GameObject> tools = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            Initialise();
        }

        /// <summary>
        /// 初回何も設定されていない場合の初期化
        /// </summary>
        private void Initialise()
        {
            // 片手モードで起動
            UseOneHandRuler();
        }

        /// <summary>
        /// 片手測定モードのオブジェクト表示処理
        /// </summary>
        public void UseOneHandRuler()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[(int) MeasuringTool.OneHandRuler].SetActive(true);
        }

        /// <summary>
        /// 両手人差し指測定モードのオブジェクト表示処理
        /// </summary>
        public void UseTwoHandsRuler()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[(int) MeasuringTool.TwoHandsRuler].SetActive(true);
        }

        /// <summary>
        /// 両手親指測定モードのオブジェクト表示処理
        /// </summary>
        public void UseHandProtractor()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[(int) MeasuringTool.TwoHandsRulerThumbTip].SetActive(true);
        }
    }
}
