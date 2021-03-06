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
        /// 線描画制御用GameObject
        /// </summary>
        private GameObject LineManagerObj;

        /// <summary>
        /// 線描画制御用スクリプトObject
        /// </summary>
        private RulerLineManager LineManager;

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
            TwoHandsRulerMiddle,
 
                /// <summary>
            /// 手測定OFF
            /// </summary>
            HandRulerNone
        }

        [SerializeField]
        private List<GameObject> tools = new List<GameObject>();

        // Start is called before the first frame update
        private void Start()
        {
            LineManagerObj = GameObject.Find("RulerLineManager");
            LineManager = LineManagerObj.GetComponent<RulerLineManager>();

            //Initialise();
        }

        /// <summary>
        /// 初回何も設定されていない場合の初期化
        /// </summary>
        private void Initialise()
        {
            // 測定線の初期化
            MeasurMiddle = false;
            // 手測定OFFモードで起動
            UseHandRulerOFF();
        }

        /// <summary>
        /// 片手測定モードのオブジェクト表示処理
        /// </summary>
        public void UseOneHandRuler()
        {
            // 片手測定
            MeasurTool = (int)MeasuringTool.OneHandRuler;
            MeasurToolSet();
        }

        /// <summary>
        /// 両手人差し指測定モードのオブジェクト表示処理
        /// </summary>
        public void UseTwoHandsRuler()
        {
            // 両手人差し指測定
            MeasurTool = (int)MeasuringTool.TwoHandsRuler;
            MeasurToolSet();
        }

        /// <summary>
        /// 手測定OFFモードのオブジェクト表示処理
        /// </summary>
        public void UseHandRulerOFF()
        {
            // 手測定OFF
            MeasurTool = (int)MeasuringTool.HandRulerNone;
            MeasurToolSet();
        }

        /// <summary>
        /// 両手親指測定モードのオブジェクト表示処理
        /// </summary>
        public void UseHandProtractor()
        {
            // 両手親指測定
            MeasurTool = (int)MeasuringTool.TwoHandsRulerThumbTip;
            MeasurToolSet();
        }
        /// <summary>
        /// 中間測定モード切替
        /// </summary>
        public void MeasurMiddleModeToggle()
        {
            // 中間測定モード トグル
            MeasurMiddle = !MeasurMiddle;
            MeasurToolSet();
        }

        /// <summary>
        /// 中間測定モード有効
        /// </summary>
        public void MeasurMiddleModeOn()
        {
            // 中間測定モード トグル
            MeasurMiddle = true;
            MeasurToolSet();
        }

        /// <summary>
        /// 測定ツール停止
        /// </summary>
        public void MeasurToolOff()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
        }

        /// <summary>
        /// 測定ツール切替
        /// </summary>
         public void MeasurToolSet()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            // 線描画初期化
            LineManager.RulerLineInit();
            switch (MeasurTool)
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
                ///  手測定OFF
                case (int)MeasuringTool.HandRulerNone:
                    break;
            }
        }

        /// <summary>
        /// 片手モードか確認
        /// </summary>
        public bool isUseOneHands()
        {
            if( MeasurTool == (int)MeasuringTool.OneHandRuler)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 手測定OFFモードか確認
        /// </summary>
        public bool isUseHandRulerOFF()
        {
            if (MeasurTool == (int)MeasuringTool.HandRulerNone)
            {
                return true;
            }
            return false;
        }
    }
}