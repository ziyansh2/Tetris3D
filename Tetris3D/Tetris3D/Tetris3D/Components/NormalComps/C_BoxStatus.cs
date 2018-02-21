using Microsoft.Xna.Framework;
using MyLib.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris3D.Utility;

namespace Tetris3D.Components.NormalComps
{
    enum eBoxState {
        Off,
        On,
        OffWait,
    }

    enum eBoxType {
        Yellow,
        Green,
        Red,
        None,
    }

    class C_BoxStatus : Component
    {
        public eBoxState State { get; set; }
        public eBoxType Type { get; set; }

        private Vector3 point;

        public C_BoxStatus(eBoxState state, eBoxType type, Vector3 point) {
            State = state;
            Type = type;
            this.point = point;
        }

        public int PointX {
            get { return (int)point.X; }
            set { point.X = value; }
        }

        public int PointY {
            get { return (int)point.Y; }
            set { point.Y = value; }
        }

        public int PointZ {
            get { return (int)point.Z; }
            set { point.Z = value; }
        }

    }
}
