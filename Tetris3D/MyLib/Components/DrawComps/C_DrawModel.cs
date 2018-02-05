using Microsoft.Xna.Framework;
using MyLib.Components.NormalComps;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Components.DrawComps
{
    public class C_DrawModel : DrawComponent
    {
        private Modeler modeler;
        private C_Model model;

        public C_DrawModel(float alpha = 1, float depth = 100)
        {
            this.alpha = alpha;
            this.depth = depth;

            modeler = new Modeler();
        }

        public override void Draw() {
            if (model == null) { return; }
            modeler.DrawModel(model.GetModel, model.GetWorld());
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