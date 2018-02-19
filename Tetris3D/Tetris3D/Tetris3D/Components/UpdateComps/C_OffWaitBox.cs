//作成日：　2018.02.19
//作成者：　柏
//クラス内容：　Boxの削除待ち機能用
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

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
    class C_OffWaitBox : UpdateComponent
    {
        private Timer timer;
        private List<int[]> waitData;

        public C_OffWaitBox(List<int[]> waitData) {
            this.waitData = waitData;
            timer = new Timer(2f);
            timer.Dt = new Timer.timerDelegate(DeActive);
        }


        public override void Update()
        {
            timer.Update();
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

            Sound.PlaySE("Bom");
            waitData.ForEach(d => {
                StageData.SetBlockOff(d[0], d[1], d[2]);
                for (int i = d[2] + 1; i < Parameter.StageMaxIndex; i++) {
                    StageData.SetBlockData(d[0], d[1], i - 1, StageData.GetBlockData(d[0], d[1], i));
                }
            });
            waitData.Clear();
        }
    }
}
