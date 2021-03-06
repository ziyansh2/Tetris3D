﻿//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　アニメ登録用クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Entitys
{

    public class AnimData
    {
        public AnimData(int keyCount, int rowCount, float keySecond, string animName, bool isLoop = true) {
            KeyCount = keyCount;
            RowCount = rowCount;
            KeySecond = keySecond;
            AnimName = animName;
            IsLoop = isLoop;
        }

        public int RowCount { get; set; }
        public int KeyCount { get; set; }
        public float KeySecond { get; set; }
        public string AnimName { get; set; }
        public bool IsLoop { get; set; }

    }
}
