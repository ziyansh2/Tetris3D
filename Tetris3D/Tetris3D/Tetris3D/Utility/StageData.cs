//作成日：　2018.02.07
//作成者：　柏
//クラス内容：　StageData保存用
//修正内容リスト：
//名前：柏　　　日付：2018.02.19　　　内容：Boxコントロール可能に合わせて調整
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Utility;
using System.Collections.Generic;
using Tetris3D.Components.NormalComps;
using Tetris3D.Def;

namespace Tetris3D.Utility
{
    class StageData : Component
    {
        
        private static int maxIndex = Parameter.StageMaxIndex;
        private static C_BoxStatus[,,] stageData = new C_BoxStatus[maxIndex, maxIndex, maxIndex];
        private static int combo;

        private static List<Vector3> checkDirect = new List<Vector3>();

        public static Dictionary<string, eBoxType> TypeChange = new Dictionary<string, eBoxType>() {
            { "Yellow", eBoxType.Yellow },
            { "Green", eBoxType.Green },
            { "Red", eBoxType.Red },
        };

        public StageData() {
            InitializeStage();

            checkDirect.Add(new Vector3(-1,  0,  0));
            checkDirect.Add(new Vector3( 1,  0,  0));
            checkDirect.Add(new Vector3( 0, -1,  0));
            checkDirect.Add(new Vector3( 0,  1,  0));
            checkDirect.Add(new Vector3( 0,  0, -1));
            checkDirect.Add(new Vector3( 0,  0,  1));
        }

        public static void InitializeStage() {
            Method.MyForeach((x, y, z) => {
                stageData[z, y, x] = new C_BoxStatus(eBoxState.Off, eBoxType.None, new Vector3(x, y, z));
            }, Vector3.One * maxIndex);
            combo = 0;
        }

        public static C_BoxStatus GetBlockData(int x, int y, int z) {
            if (z >= maxIndex) { return new C_BoxStatus(eBoxState.Off, eBoxType.None, Vector3.Zero); }
            return stageData[z, y, x];
        }
        public static eBoxType GetBlockType(int x, int y, int z) {
            if (z >= maxIndex) { return eBoxType.None; }
            return stageData[z, y, x].Type;
        }

        #region Set
        public static void SetBlockData(int x, int y, int z, C_BoxStatus data) {
            if (z >= maxIndex) { return; }
            if (x >= maxIndex) { return; }
            if (y >= maxIndex) { return; }
            stageData[z, y, x].State = data.State;
            stageData[z, y, x].Type = data.Type;
        }

        public static void SetBlockType(int x, int y, int z, eBoxType type) {
            if (z >= maxIndex) { return; }
            if (x >= maxIndex) { return; }
            if (y >= maxIndex) { return; }
            stageData[z, y, x].Type = type;
        }

        public static void SetBlockOn(int x, int y, int z) {
            if (z >= maxIndex) { return; }
            if (x >= maxIndex) { return; }
            if (y >= maxIndex) { return; }
            stageData[z, y, x].State = eBoxState.On;
        }

        public static void SetBlockOff(int x, int y, int z) {
            if (z >= maxIndex) { return; }
            if (x >= maxIndex) { return; }
            if (y >= maxIndex) { return; }
            stageData[z, y, x].State = eBoxState.Off;
            stageData[z, y, x].Type = eBoxType.None;
        }

        public static void SetBlockOffWait(int x, int y, int z) {
            if (z >= maxIndex) { return; }
            if (x >= maxIndex) { return; }
            if (y >= maxIndex) { return; }
            stageData[z, y, x].State = eBoxState.OffWait;
        }
        #endregion

        public static bool IsBlock(int x, int y, int z) {
            if (x < 0 || x >= maxIndex) { return true; }
            if (y < 0 || y >= maxIndex) { return true; }
            if (z < 0) { return true; }
            if (z >= maxIndex) { return false; }

            return stageData[z, y, x].State == eBoxState.On;
        }

        public static bool IsBlockWaitOff(int x, int y, int z) {
            if (x < 0 || x >= maxIndex) { return true; }
            if (y < 0 || y >= maxIndex) { return true; }
            if (z < 0) { return true; }
            if (z >= maxIndex) { return false; }

            return stageData[z, y, x].State == eBoxState.OffWait;
        }


        public static bool IsBlock(float x, float y, float z) {
            return IsBlock((int)x, (int)y, (int)z);
        }

        public static bool IsWaitOff(int x, int y, int z) {
            return stageData[z, y, x].State == eBoxState.OffWait;
        }



        public static List<int[]> GetRemoveData() {
            List<int[]> data = new List<int[]>();

            List<int> floorFulls = CheckFloorFull();
            for (int i = 0; i < floorFulls.Count; i++) {
                Method.MyForeach((x, y) =>
                {
                    data.Add(new int[] { x, y, floorFulls[i] });
                }, Vector2.One * Parameter.StageMaxIndex);
            }

            return data;
        }


        #region CheckFun

        //満タンになったフロアをチェックして返す
        public static List<int> CheckFloorFull() {
            List<int> fullFloors = new List<int>();

            for (int z = 0; z < maxIndex; z++) {
                for (int y = 0; y < maxIndex; y++) {
                    for (int x = 0; x < maxIndex; x++) {
                        if (!IsBlock(x, y, z)) {
                            y = maxIndex;
                            x = maxIndex;
                        }
                        if (y == maxIndex - 1 && x == maxIndex - 1) {
                            fullFloors.Add(z);
                        }
                    }
                }
            }

            return fullFloors;
        }

        public static List<int[]> CheckRemoveData(int[] startPoint) {
            List<int[]> data = new List<int[]>();
            data.Add(startPoint);

            for (int i = 0; i < data.Count; i++) {
                checkDirect.ForEach(c => {
                    int x = (int)Method.Clamp(0, Parameter.StageMaxIndex - 1, data[i][0] + (int)c.X);
                    int y = (int)Method.Clamp(0, Parameter.StageMaxIndex - 1, data[i][1] + (int)c.Y);
                    int z = (int)Method.Clamp(0, Parameter.StageMaxIndex - 1, data[i][2] + (int)c.Z);

                    int[] point = new int[]{ x, y, z };
                    if (IsBlock(point[0], point[1], point[2])) {
                        bool putInAble = true;
                        data.ForEach(d => {
                            bool typeCheck = GetBlockType(point[0], point[1], point[2]) == GetBlockType(d[0], d[1], d[2]);
                            if (!typeCheck) {
                                putInAble = false;
                                return;
                            }
                            if (point[0] == d[0] &&
                                point[1] == d[1] &&
                                point[2] == d[2])
                            {
                                putInAble = false;
                                return;
                            }
                        });
                        if (putInAble) { data.Add(point); }
                    }
                });
            }

            if (data.Count < 4) { data.Clear(); }
            GameConst.AddScore(data.Count * 10);
            return data;
        }

        #endregion


        public override void Active() {
            base.Active();

            InitializeStage();
        }
        public override void DeActive() {
            base.DeActive();
            

        }

    }
}
