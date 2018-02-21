//作成日：　2018.02.07
//作成者：　柏
//クラス内容：　StageDataにより描画
//修正内容リスト：
//名前：柏　　　日付：2018.02.19　　　内容：Boxの削除待ち追加
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Components.DrawComps;
using MyLib.Components.NormalComps;
using MyLib.Device;
using MyLib.Entitys;
using MyLib.Utility;
using Tetris3D.Components.NormalComps;
using Tetris3D.Def;
using Tetris3D.Utility;

namespace Tetris3D.Components.DrawComps
{
    class C_DrawStage : DrawComponent
    {
        private static int maxIndex = Parameter.StageMaxIndex;
        private C_Model[] protoModel;
        private C_Model[,,] models = new C_Model[maxIndex, maxIndex, maxIndex];
        private Entity pedestal;
        private int timer;

        public C_DrawStage(float depth = -1, float alpha = 1)
        {
            this.alpha = alpha;
            this.depth = depth;

            protoModel = new C_Model[] {
                new C_Model("Yellow"),
                new C_Model("Green"),
                new C_Model("Red"),
            };
        }
        public override void Draw()
        {
            timer++;
            Method.Warp(0, 600, ref timer);
            Method.MyForeach((x, y, z) => {
                if (StageData.IsBlock(x, y, z)) {
                    DrawOneModel(x, y, z, StageData.GetBlockType(x, y, z));
                }
                else if (StageData.IsWaitOff(x, y, z)) {
                    if ((timer / 5) % 2 == 0) {
                        DrawOneModel(x, y, z, StageData.GetBlockType(x, y, z));
                    }
                }
            }, Vector3.One * maxIndex);
            
        }

        private void DrawOneModel(int x, int y, int z, eBoxType type) {
            Vector3 position = new Vector3(x, y, z) * Parameter.BoxSize;
            switch (type) {
                case eBoxType.Yellow:
                    Modeler.DrawModel(protoModel[(int)eBoxType.Yellow].GetModel, protoModel[(int)eBoxType.Yellow].GetWorld(position));
                    break;
                case eBoxType.Green:
                    Modeler.DrawModel(protoModel[(int)eBoxType.Green].GetModel, protoModel[(int)eBoxType.Green].GetWorld(position));
                    break;
                case eBoxType.Red:
                    Modeler.DrawModel(protoModel[(int)eBoxType.Red].GetModel, protoModel[(int)eBoxType.Red].GetWorld(position));
                    break;
            }
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
