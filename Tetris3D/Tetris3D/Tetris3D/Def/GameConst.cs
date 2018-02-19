//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　ゲームルール管理
//修正内容リスト：
//名前：柏　　　日付：2018.02.19　　　内容：Boxの生成を一個ずつに
//名前：柏　　　日付：2018.02.19　　　内容：Score,Combo,Level機能追加
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris3D.Def
{
    static class GameConst
    {
        public static int Score = 0;
        public static int Combo = 0;
        public static int Level = 1;
        public static bool IsClear = false;
        public static bool IsEnding = false;
        public static bool CanCreateBox { get; set; }

        public static void SetNowCombo(int combo) {
            Combo = combo;
        }

        public static void AddScore(int point)
        {
            Score += point;
            if (Score >= 100) { Level = 2; }
            if (Score >= 200) { Level = 3; }
        }

        public static void SetClear() {
            IsClear = true;
        }

        public static void SetEnding() {
            IsEnding = true;
        }

        public static void Initialize() {
            Score = 0;
            Level = 1;
            IsClear = false;
            IsEnding = false;
            CanCreateBox = true;
        }
    }
}
