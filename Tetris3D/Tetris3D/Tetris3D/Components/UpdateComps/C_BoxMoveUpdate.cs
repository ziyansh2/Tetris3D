//作成日：　2018.02.07
//作成者：　柏
//クラス内容：　生成されたBoxを移動させる
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Device;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris3D.Def;
using Tetris3D.Utility;

namespace Tetris3D.Components.UpdateComps
{
    class C_BoxMoveUpdate : UpdateComponent
    {
        private Timer timer;

        public C_BoxMoveUpdate() {
            timer = new Timer(0.5f);
            timer.Dt = new Timer.timerDelegate(BoxMove);
        }


        public override void Update()
        {
            timer.Update();

            List<int[]> removeData = StageData.GetRemoveData();

            removeData.ForEach(d => {
                StageData.SetBlockOff(d[0], d[1], d[2]);
                for (int i = d[2] + 1; i < Parameter.StageMaxIndex; i++) {
                    StageData.SetBlockData(d[0], d[1], i - 1, StageData.GetBlockData(d[0], d[1], i));
                }
            });
        }


        private void BoxMove() {
            entity.transform.SetPositionZ -= Parameter.BoxSize;
            Vector3 point = entity.transform.Position / Parameter.BoxSize;
            int x = (int)point.X;
            int y = (int)point.Y;
            int z = (int)point.Z;
            if (StageData.IsBlock(x, y, z - 1)) {
                StageData.SetBlockOn(x, y, z);
                entity.DeActive();
                Sound.PlaySE("Bom");
            }
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
