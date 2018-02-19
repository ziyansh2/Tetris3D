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
using Tetris3D.Def;

namespace Tetris3D.Utility
{
    enum eBoxState {
        Off,
        On,
        OffWait,
    }

    class StageData : Component
    {
        
        private static int maxIndex = Parameter.StageMaxIndex;
        private static int[,,] stageData = new int[maxIndex, maxIndex, maxIndex];
        private static int combo;

        private static List<Vector3> checkDirect = new List<Vector3>();

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
                stageData[z, y, x] = (int)eBoxState.Off;
            }, Vector3.One * maxIndex);
            combo = 0;
        }

        public static int GetBlockData(int x, int y, int z) { return stageData[z, y, x]; }
        public static void SetBlockData(int x, int y, int z, int data) { stageData[z, y, x] = data; }

        public static void SetBlockOn(int x, int y, int z) {
            if (z >= maxIndex) { return; }
            stageData[z, y, x] = (int)eBoxState.On;
        }

        public static void SetBlockOff(int x, int y, int z) {
            if (z >= maxIndex) { return; }
            stageData[z, y, x] = (int)eBoxState.Off;
        }

        public static void SetBlockOffWait(int x, int y, int z) {
            if (z >= maxIndex) { return; }
            stageData[z, y, x] = (int)eBoxState.OffWait;
        }

        public static bool IsBlock(int x, int y, int z) {
            if (x < 0 || x >= maxIndex) { return true; }
            if (y < 0 || y >= maxIndex) { return true; }
            if (z < 0) { return true; }
            if (z >= maxIndex) { return false; }

            return stageData[z, y, x] == (int)eBoxState.On;
        }

        public static bool IsBlock(float x, float y, float z) {
            if (x < 0 || x >= maxIndex) { return true; }
            if (y < 0 || y >= maxIndex) { return true; }
            if (z < 0) { return true; }
            if (z >= maxIndex) { return false; }

            return stageData[(int)z, (int)y, (int)x] == (int)eBoxState.On;
        }

        public static bool IsWaitOff(int x, int y, int z) {
            return stageData[z, y, x] == (int)eBoxState.OffWait;
        }

        public static List<int[]> GetRemoveData() {
            List<int[]> data = new List<int[]>();

            //data.AddRange(CheckRemoveData_X());
            //data.AddRange(CheckRemoveData_Y());
            //data.AddRange(CheckRemoveData_Z());

            //if (combo != 0) {
            //    GameConst.SetNowCombo(combo);
            //    combo = 0;
            //}

            //重複削除
            //for (int i = 0; i < data.Count; i++) {
            //    for (int j = i + 1; j < data.Count; j++) {
            //        if (data[i].Equals(data[j])) {
            //            data.RemoveAt(i);
            //            i--;
            //            j = data.Count;
            //            GameConst.AddScore(-10);
            //        }
            //    }
            //}

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


        #region OldCheckFun
        private static List<int[]> CheckRemoveData_X() {
            List<int[]> data = new List<int[]>();

            int count = 0;
            for (int z = 0; z < maxIndex; z++) {
                for (int y = 0; y < maxIndex; y++) {
                    count = 0;
                    for (int x = 0; x < maxIndex; x++) {
                        if (IsBlock(x,y,z)) {
                            count++;
                            if (x == maxIndex - 1) {
                                if (count >= 4) {
                                    for (int i = 0; i < count; i++) {
                                        data.Add(new int[3] { x - i, y, z });
                                    }
                                    GameConst.AddScore(count * 10);
                                    combo++;
                                }
                            }
                            continue;
                        }
                        else {
                            if (count >= 4) {
                                for (int i = 1; i <= count; i++) {
                                    data.Add(new int[3] { x - i, y, z });
                                }
                                GameConst.AddScore(count * 10);
                                combo++;
                                count = 0;
                            }
                            else {
                                count = 0;
                            }
                        }
                    }
                }
            }

            return data;
        }

        private static List<int[]> CheckRemoveData_Y() {
            List<int[]> data = new List<int[]>();

            int count = 0;
            for (int z = 0; z < maxIndex; z++) {
                for (int x = 0; x < maxIndex; x++) {
                    count = 0;
                    for (int y = 0; y < maxIndex; y++) {
                        if (IsBlock(x, y, z)) {
                            count++;

                            if (y == maxIndex - 1) {
                                if (count >= 4) {
                                    for (int i = 0; i < count; i++) {
                                        data.Add(new int[3] { x, y - i, z });
                                    }
                                    GameConst.AddScore(count * 10);
                                    combo++;
                                }
                            }
                            continue;
                        }
                        else {
                            if (count >= 4) {
                                for (int i = 1; i <= count; i++) {
                                    data.Add(new int[3] { x, y - i, z });
                                }
                                GameConst.AddScore(count * 10);
                                combo++;
                                count = 0;
                            }
                            else {
                                count = 0;
                            }
                        }
                    }
                }
            }


            return data;
        }

        private static List<int[]> CheckRemoveData_Z()
        {
            List<int[]> data = new List<int[]>();

            int count = 0;
            for (int x = 0; x < maxIndex; x++) {
                for (int y = 0; y < maxIndex; y++) {
                    count = 0;
                    for (int z = 0; z < maxIndex; z++) {
                        if (IsBlock(x, y, z)) {
                            count++;
                            if (z == maxIndex - 1) {
                                if (count >= 4) {
                                    for (int i = 0; i < count; i++) {
                                        data.Add(new int[3] { x, y, z - i });
                                    }
                                    GameConst.AddScore(count * 10);
                                    combo++;
                                }

                            }
                            continue;
                        }
                        else {
                            if (count >= 4) {
                                for (int i = 1; i <= count; i++) {
                                    data.Add(new int[3] { x, y, z - i });
                                }
                                GameConst.AddScore(count * 10);
                                combo++;
                                count = 0;
                            }
                            else {
                                count = 0;
                            }
                        }
                    }
                }
            }



            return data;
        }
        #endregion

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
