//作成日：　2018.02.07
//作成者：　柏
//クラス内容：　Modelを描画する
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using MyLib.Components.NormalComps;
using MyLib.Device;

namespace MyLib.Components.DrawComps
{
    public class C_DrawModel : DrawComponent
    {
        private C_Model model;

        public C_DrawModel(float alpha = 1, float depth = -1)
        {
            this.alpha = alpha;
            this.depth = depth;
        }

        public override void Draw() {
            if (model == null) { return; }
            Modeler.DrawModel(model.GetModel, model.GetWorld());
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            model = (C_Model)entity.GetNormalComponent("C_Model");
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }


    }
}