﻿//作成日：　2018.02.07
//作成者：　柏
//クラス内容：　生成されたBoxを移動させる
//修正内容リスト：
//名前：柏　　　日付：2018.02.19　　　内容：Boxコントロール可能に
//名前：柏　　　日付：2018.02.19　　　内容：Boxの削除待ち追加
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Components;
using MyLib.Device;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris3D.Components.NormalComps;
using Tetris3D.Def;
using Tetris3D.Utility;

namespace Tetris3D.Components.UpdateComps
{
    class C_BoxMoveUpdate : UpdateComponent
    {
        private Timer timer;
        private InputState inputState;
        private GameDevice gameDevice;
        private bool isBomb;

        public C_BoxMoveUpdate(GameDevice gameDevice) {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;

            timer = new Timer(0.5f);
            timer.Dt = new Timer.timerDelegate(BoxMoveDown);
            isBomb = false;
        }


        public override void Update() {
            BoxControll();
            timer.Update();

            List<int[]> removeData = StageData.GetRemoveData();
            SetOffWait(removeData);
        }

        private void SetOffWait(List<int[]> removeData) {
            if (removeData.Count == 0) { return; }

            C_OffWaitBox waitBox = new C_OffWaitBox(removeData, gameDevice);
            waitBox.Active();
            TaskManager.AddTask(waitBox);
        }

        private void BoxControll() {
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

            Vector3 point = entity.transform.Position / Parameter.BoxSize;
            int x = (int)point.X;
            int y = (int)point.Y;
            int z = (int)point.Z;

            if (StageData.IsBlock(x + moveX, y + moveY, z)) { return; }
            if (isMove) { Sound.PlaySE("Shoot"); }
            entity.transform.SetPositionX += moveX * Parameter.BoxSize;
            entity.transform.SetPositionY += moveY * Parameter.BoxSize;
        }

        private void BoxMoveDown() {
            do {
                entity.transform.SetPositionZ -= Parameter.BoxSize;
                Vector3 point = entity.transform.Position / Parameter.BoxSize;
                int x = (int)point.X;
                int y = (int)point.Y;
                int z = (int)point.Z;
                if (StageData.IsBlock(x, y, z - 1) || StageData.IsBlockWaitOff(x, y, z - 1)) {
                    StageData.SetBlockType(x, y, z, StageData.TypeChange[entity.GetName()]);
                    StageData.SetBlockOn(x, y, z);
                    entity.DeActive();
                    Sound.PlaySE("Laser");
                    GameConst.CanCreateBox = true;

                    if (isBomb) {
                        SetOffWait(StageData.CheckRemoveData(new int[]{ x, y, z }));
                    }
                    isBomb = false;
                }
            } while (isBomb);
        }


        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

        }
    }
}
