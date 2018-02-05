using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Components.NormalComps;
using MyLib.Device;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris3D.Def;
using Tetris3D.Utility;

namespace Tetris3D.Components.DrawComps
{
    class C_DrawStage : DrawComponent
    {
        private static int maxIndex = Parameter.StageMaxIndex;
        private C_Model[,,] models = new C_Model[maxIndex, maxIndex, maxIndex];

        public C_DrawStage(float depth = -1, float alpha = 1)
        {
            this.alpha = alpha;
            this.depth = depth;


        }
        public override void Draw()
        {
            if (models[0, 0, 0] == null) { return; }
            int boxSize = Parameter.BoxSize;
            Method.MyForeach((x, y, z) => {
                if (StageData.IsBlock(x,y,z)) {
                    Vector3 position = new Vector3(boxSize * x, boxSize * z, boxSize * y);
                    Modeler.DrawModel(models[z, y, x].GetModel, models[z, y, x].GetWorld(position));
                }
            }, Vector3.One * maxIndex);
            
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            Method.MyForeach((x, y, z) => {
                models[z, y, x] = new C_Model("Box");
                models[z, y, x].Active();
                TaskManager.AddTask(models[z, y, x]);
            }, Vector3.One * maxIndex);

        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            Method.MyForeach((x, y, z) => {
                models[z, y, x].DeActive();
            }, Vector3.One * maxIndex);
        }
    }
}
