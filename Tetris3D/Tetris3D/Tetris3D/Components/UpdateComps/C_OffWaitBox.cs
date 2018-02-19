//作成日：　2018.02.19
//作成者：　柏
//クラス内容：　Boxの削除待ち機能用
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris3D.Def;
using Tetris3D.Utility;

namespace Tetris3D.Components.UpdateComps
{
    class C_OffWaitBox : UpdateComponent
    {
        private Timer timer;
        private List<int[]> waitData;
        private GameDevice gameDevice;

        public C_OffWaitBox(List<int[]> waitData, GameDevice gameDevice) {
            this.waitData = waitData;
            this.gameDevice = gameDevice;
            timer = new Timer(1f);
            timer.Dt = new Timer.timerDelegate(DeActive);
        }


        public override void Update() {
            timer.Update();
        }


        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            Sound.PlaySE("Bom");
            waitData.Sort((x, y) => -x[2].CompareTo(y[2]));
            waitData.ForEach(d => {
                StageData.SetBlockOff(d[0], d[1], d[2]);
                for (int i = d[2] + 1; i < Parameter.StageMaxIndex; i++) {
                    StageData.SetBlockData(d[0], d[1], i - 1, StageData.GetBlockData(d[0], d[1], i));
                    //Vector2 position = new Vector2(d[0], d[1]) * 10.5f;
                    //CreateEffect(position);
                }
            });
            waitData.Clear();
        }

        private void CreateEffect(Vector2 postion) {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Ring",           //name
                20, 40,                               //count
                postion - new Vector2(130, 200),
                postion + new Vector2(270, 200),       //position
                1, 3,         //speed
                0.3f, 0.8f,         //size
                1, 1,               //alpha
                0, 360,           //angle
                2.0f, 2.0f,         //alive
                new MoveLine(),     //moveType
                new ChangeToLucency(new Timer(2))   //changeType
            );
        }

    }
}
