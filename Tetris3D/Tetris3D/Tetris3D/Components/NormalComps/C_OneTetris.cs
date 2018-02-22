using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Components;
using MyLib.Device;
using MyLib.Entitys;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris3D.Components.UpdateComps;
using Tetris3D.Def;
using Tetris3D.Utility;

namespace Tetris3D.Components.NormalComps
{
    class C_OneTetris : UpdateComponent
    {
        private List<C_BoxStatus> tetris;

        private Timer timer;
        private InputState inputState;
        private GameDevice gameDevice;
        private bool isBomb;


        public C_OneTetris(GameDevice gameDevice, List<C_BoxStatus> tetris) {
            this.tetris = tetris;
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;

            timer = new Timer(0.5f);
            isBomb = false;

            tetris.Sort((x, y) => -x.GetEntity().transform.Position.Z.CompareTo(y.GetEntity().transform.Position.Z));
        }

        public List<C_BoxStatus> GetTetris() {
            return tetris; 
        }


        public override void Update()
        {
            BoxControll();
            timer.Update();
            if (timer.IsTime) {
                BoxMoveDown();
                timer.Initialize();
            }

            List<int[]> removeData = StageData.GetRemoveData();
            SetOffWait(removeData);

        }


        private void SetOffWait(List<int[]> removeData)
        {
            if (removeData.Count == 0) { return; }

            C_OffWaitBox waitBox = new C_OffWaitBox(removeData, gameDevice);
            waitBox.Active();
            TaskManager.AddTask(waitBox);
        }

        private void BoxControll()
        {
            int moveX = 0;
            int moveY = 0;
            bool isMove = false;

            if (inputState.WasDown(Keys.A)) {
                moveY = -1;
                isMove = true;
            }
            else if (inputState.WasDown(Keys.D)) {
                moveY = 1;
                isMove = true;
            }
            if (inputState.WasDown(Keys.W)) {
                moveX = -1;
                isMove = true;
            }
            else if (inputState.WasDown(Keys.S)) {
                moveX = 1;
                isMove = true;
            }
            if (inputState.WasDown(Keys.Enter)) {
                isBomb = true;
            }

            tetris.ForEach(t => {
                Vector3 point = t.GetEntity().transform.Position / Parameter.BoxSize;
                int x = (int)point.X;
                int y = (int)point.Y;
                int z = (int)point.Z;

                if (StageData.IsBlock(x + moveX, y + moveY, z)) {
                    isMove = false;
                    return;
                }
            });

            if (!isMove) { return; }
            Sound.PlaySE("Shoot");
            tetris.ForEach(t => {
                t.GetEntity().transform.SetPositionX += moveX * Parameter.BoxSize;
                t.GetEntity().transform.SetPositionY += moveY * Parameter.BoxSize;
            });
        }

        private void BoxMoveDown()
        {
            do
            {
                bool isLand = false;

                //着地チェック
                tetris.ForEach(t => {
                    Entity entity = t.GetEntity();
                    Vector3 point = entity.transform.Position / Parameter.BoxSize;
                    int x = (int)point.X;
                    int y = (int)point.Y;
                    int z = (int)point.Z;

                    if (StageData.IsBlock(x, y, z - 1) || StageData.IsBlockWaitOff(x, y, z - 1)) {
                        isLand = true;
                    }
                });

                //着地できない場合、下に移動
                if (!isLand) {
                    tetris.ForEach(t => {
                        Entity entity = t.GetEntity();
                        entity.transform.SetPositionZ -= Parameter.BoxSize;
                    });
                }

                //着地できる場合、着地して消せるかどうかをチェック
                else {
                    Sound.PlaySE("Laser");
                    GameConst.CanCreateBox = true;

                    //着地処理
                    tetris.ForEach(t => {
                        Entity entity = t.GetEntity();
                        Vector3 point = entity.transform.Position / Parameter.BoxSize;
                        int x = (int)point.X;
                        int y = (int)point.Y;
                        int z = (int)point.Z;

                        if (z >= Parameter.StageMaxIndex) { GameConst.IsEnding = true; }
                        StageData.SetBlockType(x, y, z, StageData.TypeChange[entity.GetName()]);
                        StageData.SetBlockOn(x, y, z);
                    });

                    if (GameConst.IsEnding) { return; }

                    //消すBoxをチェックして設定
                    List<List<int[]>> datas = new List<List<int[]>>();
                    List<int[]> data = new List<int[]>();
                    if (isBomb) {
                        tetris.ForEach(t => {
                            Entity entity = t.GetEntity();
                            Vector3 point = entity.transform.Position / Parameter.BoxSize;
                            int x = (int)point.X;
                            int y = (int)point.Y;
                            int z = (int)point.Z;
                            datas.Add(StageData.CheckRemoveData(new int[] { x, y, z }));
                        });
                        isBomb = false;
                    }

                    datas.RemoveAll(d => d.Count == 0);
                    if (datas.Count == 0) {
                        DeActive();
                        continue;
                    }
                    Method.MyForeach((x, y) => {
                        data.Add(datas[y][x]);
                    }, new Vector2(datas[0].Count, datas.Count));
                    
                    for (int i = 0; i < data.Count; i++) {
                        for (int j = i + 1; j < data.Count; j++) {
                            if( data[i][0] == data[j][0] &&
                                data[i][1] == data[j][1] &&
                                data[i][2] == data[j][2] )
                            {
                                data.RemoveAt(j);
                                j--;
                            }
                        }
                    }

                    SetOffWait(data);
                    DeActive();
                }
            } while (isBomb);
        }


        public override void Active()
        {
            base.Active();
        }

        public override void DeActive()
        {
            base.DeActive();

            for (int i = 0; i < tetris.Count; i++) {
                tetris[i].GetEntity().DeActive();
            }

            tetris.Clear();
        }

    }
}
