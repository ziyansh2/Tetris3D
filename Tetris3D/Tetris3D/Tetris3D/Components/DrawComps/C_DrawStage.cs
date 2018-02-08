//作成日：　2018.02.07
//作成者：　柏
//クラス内容：　StageDataにより描画
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Components.DrawComps;
using MyLib.Components.NormalComps;
using MyLib.Device;
using MyLib.Entitys;
using MyLib.Utility;
using Tetris3D.Def;
using Tetris3D.Utility;

namespace Tetris3D.Components.DrawComps
{
    class C_DrawStage : DrawComponent
    {
        private static int maxIndex = Parameter.StageMaxIndex;
        private C_Model[,,] models = new C_Model[maxIndex, maxIndex, maxIndex];
        private Entity pedestal;

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
                if (StageData.IsBlock(x, y, z)) {
                    Vector3 position = new Vector3(x, y, z) * boxSize;
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

            pedestal = Entity.CreateEntity("Pedestal", "Pedestal", new Transform());
            pedestal.transform.Position = new Vector3(2.5f, -3.5f, 0 ) * Parameter.BoxSize;
            pedestal.RegisterComponent(new C_Model("Pedestal"));
            pedestal.RegisterComponent(new C_DrawModel());
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            Method.MyForeach((x, y, z) => {
                models[z, y, x].DeActive();
            }, Vector3.One * maxIndex);

            pedestal.DeActive();
        }
    }
}
