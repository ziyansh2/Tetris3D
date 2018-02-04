using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris3D.Def
{
    static class Parameter
    {
        public static Vector2 ScreenSize = new Vector2(1920, 1080);
        public static Vector2 StageSize = new Vector2(100000, 2048);
        public static readonly int CollitionRadius_O = 25;
        public static readonly int CollideAbleDistance = 500;
        public static readonly int CollideAbleDistanceSquare = CollideAbleDistance * CollideAbleDistance;
        public static readonly int BackGroundSize = 2000;

        public static readonly float PlayerLimitSpeed = 10;

        public static Vector2 FrameLT = Vector2.Zero;
        public static Vector2 FrameRB = ScreenSize;
    }
}
