//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　当たり判定Component-Line
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Components.DrawComps;
using MyLib.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Components.ColliderComps
{
    public class C_Collider_Line : ColliderComponent
    {
        public Vector2 Position1;
        public Vector2 Position2;

        private C_DrawLine drawLine;
        private Entity drawEntity;

        public C_Collider_Line(
            string colliderName,
            Vector2 Position1,
            Vector2 Position2,
            eCollitionType collisionType = eCollitionType.Through,
            bool isLocal = false
            ) : base (colliderName, Vector2.Zero, 0, collisionType, eCollitionForm.Line, isLocal)
        {
            this.Position1 = Position1;
            this.Position2 = Position2;
            centerPosition = (Position1 + Position2) / 2;

            drawEntity = Entity.CreateEntity("Empty", "Empty", new Transform());
            InitializeCollision();
        }

        //public override void Update() { base.Update(); }
        //public override void Collition(ColliderComponent other) { base.Collition(other); }

        protected override void DoJostleCollision(ColliderComponent otherComp) {
            if (otherComp.collisionForm == eCollitionForm.Line) { }
            else {
                Jostle_Circle_Line(otherComp);
            }
        }

        protected override void DoThroughCollision(ColliderComponent otherComp) {
            if (otherComp.collisionForm == eCollitionForm.Line) { }
            else {
                Through_Circle_Line(otherComp);
            }
        }


        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            List<Vector2> linePoint = new List<Vector2>() { Position1, Position2 };
            drawLine = new C_DrawLine(linePoint);
            //drawEntity.RegisterComponent(drawLine);
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            drawEntity.DeActive();
            InitializeCollision();
        }
    }
}
