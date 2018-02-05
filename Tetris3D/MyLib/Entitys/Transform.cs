//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　変換の基本情報クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Entitys
{
    public class Transform
    {
        public Vector3 Position { get; set; }
        public float Angle { get; set; }
        
        public float Scale { get; set; }

        public float SetPositionX {
            get{ return Position.X; }
            set { Position = new Vector3(value, Position.Y, Position.Z); }
        }
        public float SetPositionY {
            get { return Position.Y; }
            set { Position = new Vector3(Position.X, value, Position.Z); }
        }

        public float SetPositionZ {
            get { return Position.Z; }
            set { Position = new Vector3(Position.X, Position.Y, value); }
        }

    }
}
