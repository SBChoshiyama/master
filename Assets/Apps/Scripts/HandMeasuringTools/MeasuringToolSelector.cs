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
        /// 測定モード
        /// </summary>
        public int MeasurTool;

        /// <summary>
        /// 測定線中間化
        /// </summary>
        public bool MeasurMiddle;

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
            TwoHandsRulerThumbTip,

            /// <summary>
            /// 片手測定
            /// </summary>
            OneHandRulerMiddle,

            /// <summary>
            /// 両手人差し指測定
            /// </summary>
            TwoHandsRulerMiddle
        }

        [SerializeField]
        private List<GameObject> tools = new List<GameObject>();

        // Start is called before the first frame update
        private void Start()
        {
            Initialise();
        }

        /// <summary>
        /// 初回何も設定されていない場合の初期化
        /// </summary>
        private void Initialise()
        {
            // 片手モードで起動
            MeasurTool = (int)MeasuringTool.OneHandRuler;
            // 測定線の初期化
            MeasurMiddle = false;
            MeasurToolChange();
        }

        /// <summary>
        /// 片手測定モードのオブジェクト表示処理
        /// </summary>
        public void UseOneHandRuler()
        {
            // 片手測定
            MeasurTool = (int)MeasuringTool.OneHandRuler;
            MeasurToolChange();
        }

        /// <summary>
        /// 両手人差し指測定モードのオブジェクト表示処理
        /// </summary>
        public void UseTwoHandsRuler()
        {
            // 両手人差し指測定
            MeasurTool = (int)MeasuringTool.TwoHandsRuler;
            MeasurToolChange();
        }

        /// <summary>
        /// 両手親指測定モードのオブジェクト表示処理
        /// </summary>
        public void UseHandProtractor()
        {
            // 両手親指測定
            MeasurTool = (int)MeasuringTool.TwoHandsRulerThumbTip;
            MeasurToolChange();
        }
        /// <summary>
        /// 中間測定モード切替
        /// </summary>
        public void MeasurMiddleModeToggle()
        {
            // 中間測定モード トグル
            MeasurMiddle = !MeasurMiddle;
            MeasurToolChange();
        }

        /// <summary>
        /// 測定ツール切替
        /// </summary>
        public void MeasurToolChange()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            switch(MeasurTool)
            {
                /// 片手測定
                case (int)MeasuringTool.OneHandRuler:
                default:
                    if (MeasurMiddle)
                    {
                        tools[(int)MeasuringTool.OneHandRulerMiddle].SetActive(true);
                    }
                    else
                    {
                        tools[MeasurTool].SetActive(true);
                    }
                    break;
                /// 両手人差し指測定
                case (int)MeasuringTool.TwoHandsRuler:
                    if (MeasurMiddle)
                    {
                        tools[(int)MeasuringTool.TwoHandsRulerMiddle].SetActive(true);
                    }
                    else
                    {
                        tools[MeasurTool].SetActive(true);
                    }
                    break;
                /// 両手親指測定
                case (int)MeasuringTool.TwoHandsRulerThumbTip:
                    tools[MeasurTool].SetActive(true);
                    break;
            }
        }
    }
}