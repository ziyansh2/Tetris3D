using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Components.NormalComps
{
    public class C_Model : Component
    {
        private Model model;
        private Matrix world;

        public C_Model(string name)
        {
            model = ResouceManager.GetModel(name);
        }

        public Model GetModel { get { return model; } }

        public Matrix GetWorld() {
            world = Matrix.CreateTranslation(entity.transform.Position);
            return world;
        }

        public Matrix GetWorld(Vector3 position) {
            world = Matrix.CreateTranslation(position);
            return world;
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
