//作成日：　2018.02.07
//作成者：　柏
//クラス内容：　StageData保存用
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：


using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Device;
using MyLib.Utility;
using System.Collections.Generic;
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
                stageData[z, y, x] = 0;
            }, Vector3.One * maxIndex);
        }

        public static int GetBlockData(int x, int y, int z) { return stageData[z, y, x]; }
        public static void SetBlockData(int x, int y, int z, int data) { stageData[z, y, x] = data; }

        public static void SetBlockOn(int x, int y, int z) {
            if (z >= maxIndex) { return; }
            stageData[z, y, x] = 1;
        }

        public static void SetBlockOff(int x, int y, int z) {
            if (z >= maxIndex) { return; }
            stageData[z, y, x] = 0;
        }

        public static bool IsBlock(int x, int y, int z) {
            if (z < 0) { return true; }
            if (z >= maxIndex) { return false; }
            return stageData[z, y, x] == 1;
        }

        public static List<int[]> GetRemoveData() {
            List<int[]> data = new List<int[]>();

            data.AddRange(CheckRemoveData_X());
            data.AddRange(CheckRemoveData_Y());
            data.AddRange(CheckRemoveData_Z());

            //重複削除
            for (int i = 0; i < data.Count; i++) {
                for (int j = i + 1; j < data.Count; j++) {
                    if (data[i].Equals(data[j])) {
                        data.RemoveAt(i);
                        i--;
                        j = data.Count;
                    }
                }
            }


            if (data.Count > 0) { Sound.PlaySE("Laser"); }

            return data;
        }



        private static List<int[]> CheckRemoveData_X() {
            List<int[]> data = new List<int[]>();

            int count = 0;
            for (int z = 0; z < maxIndex; z++) {
                for (int y = 0; y < maxIndex; y++) {
                    count = 0;
                    for (int x = 0; x < maxIndex; x++) {
                        if (IsBlock(x,y,z)) {
                            count++;

                            if (x == maxIndex - 1)
                            {
                                if (count >= 4)
                                {
                                    for (int i = 0; i < count; i++)
                                    {
                                        data.Add(new int[3] { x - i, y, z });
                                    }
                                }

                            }
                            continue;
                        }



                        else {
                            if (count >= 4)
                            {
                                for (int i = 1; i <= count; i++)
                                {
                                    data.Add(new int[3] { x - i, y, z });
                                }
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

        private static List<int[]> CheckRemoveData_Y()
        {
            List<int[]> data = new List<int[]>();

            int count = 0;
            for (int z = 0; z < maxIndex; z++)
            {
                for (int x = 0; x < maxIndex; x++)
                {
                    count = 0;
                    for (int y = 0; y < maxIndex; y++)
                    {
                        if (IsBlock(x, y, z))
                        {
                            count++;

                            if (y == maxIndex - 1)
                            {
                                if (count >= 4)
                                {
                                    for (int i = 0; i < count; i++)
                                    {
                                        data.Add(new int[3] { x, y - i, z });
                                    }
                                }

                            }
                            continue;
                        }



                        else {
                            if (count >= 4)
                            {
                                for (int i = 1; i <= count; i++)
                                {
                                    data.Add(new int[3] { x, y - i, z });
                                }
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
            for (int x = 0; x < maxIndex; x++)
            {
                for (int y = 0; y < maxIndex; y++)
                {
                    count = 0;
                    for (int z = 0; z < maxIndex; z++)
                    {
                        if (IsBlock(x, y, z))
                        {
                            count++;

                            if (z == maxIndex - 1)
                            {
                                if (count >= 4)
                                {
                                    for (int i = 0; i < count; i++)
                                    {
                                        data.Add(new int[3] { x, y, z - i });
                                    }
                                }

                            }
                            continue;
                        }



                        else {
                            if (count >= 4)
                            {
                                for (int i = 1; i <= count; i++)
                                {
                                    data.Add(new int[3] { x, y, z - i });
                                }
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
