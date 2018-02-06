using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Components.DrawComps;
using MyLib.Components.NormalComps;
using MyLib.Entitys;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris3D.Def;

namespace Tetris3D.Utility
{
    class StageData : Component
    {
        
        private static int maxIndex = Parameter.StageMaxIndex;
        private static int[,,] stageData = new int[maxIndex, maxIndex, maxIndex];

        public StageData() {
            InitializeStage();
        }

        public static void InitializeStage() {
            Method.MyForeach((x, y, z) => {
                stageData[z, y, x] = 1;
            }, Vector3.One * maxIndex);

            stageData[0, 0, 0] = 0;
            stageData[1, 0, 0] = 0;
        }

        public static void SetBlockOn(int x, int y, int z) {
            stageData[z, y, x] = 1;
        }

        public static void SetBlockOff(int x, int y, int z) {
            stageData[z, y, x] = 0;
        }

        public static bool IsBlock(int x, int y, int z) {
            return stageData[z, y, x] == 1;
        }

        public static int[,,] GetStageData() {
            return stageData;
        }


        public override void Active() {
            base.Active();

            InitializeStage();
        }
        public override void DeActive() {
            base.DeActive();
            

        }

    }
}
