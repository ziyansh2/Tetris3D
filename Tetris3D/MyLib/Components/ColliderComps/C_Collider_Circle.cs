﻿//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　当たり判定Component-Circle
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：


using Microsoft.Xna.Framework;
using MyLib.Components.DrawComps;
using MyLib.Entitys;

namespace MyLib.Components.ColliderComps
{
    public class C_Collider_Circle : ColliderComponent
    {
        private C_DrawSpriteAutoSize drawCircle;
        private Entity drawEntity;

        public C_Collider_Circle(
            string colliderName,
            Vector2 offsetPosition,
            int radius,
            eCollitionType collisionType = eCollitionType.Through,
            bool isLocal = true
            ) : base (colliderName, offsetPosition, radius, collisionType, eCollitionForm.Circle, isLocal)
        {
            drawEntity = Entity.CreateEntity("Empty", "Empty", new Transform2D());
            InitializeCollision();
        }

        public override void Update()
        {
            base.Update();
                       
            //if (isLocal) { return; }
            drawEntity.transform.Position = centerPosition;
            drawCircle.SetSize(Vector2.One * radius);
        }

        //public override void Collition(ColliderComponent other) { base.Collition(other); }

        protected override void DoJostleCollision(ColliderComponent otherComp) {
            if (otherComp.collisionForm == eCollitionForm.Circle) {
                Jostle_Circle_Circle(otherComp);
            }
            else if (otherComp.collisionForm == eCollitionForm.Line) {
                Jostle_Circle_Line(otherComp);
            }
        }

        protected override void DoThroughCollision(ColliderComponent otherComp) {
            if (otherComp.collisionForm == eCollitionForm.Circle)
            {
                Through_Circle_Circle(otherComp);
            }
            else if (otherComp.collisionForm == eCollitionForm.Line)
            {
                Through_Circle_Line(otherComp);
            }
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            if (colliderName == "OverseeCircle") {
                drawCircle = new C_DrawSpriteAutoSize("E_CheckArea", offsetPosition, Vector2.One * radius, 15, 0.5f);
                drawEntity.RegisterComponent(drawCircle);
            }
            else {
                drawCircle = new C_DrawSpriteAutoSize("CollisionArea", offsetPosition, Vector2.One * radius, 100);
                drawEntity.RegisterComponent(drawCircle);
            }
            centerPosition = entity.transform.Position + offsetPosition;
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
            drawEntity.DeActive();
            InitializeCollision();
        }
    }
}
