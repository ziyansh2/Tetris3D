//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　エネルギー用クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using MyLib.Components.DrawComps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Components.NormalComps
{
    class C_Energy : Component
    {
        private float limitEnergy;
        private float nowEnergy;
        private string imgName;
        private float offsetY;

        public C_Energy(string imgName, float limitEnergy, float offsetY)
        {
            this.limitEnergy = limitEnergy;
            this.imgName = imgName;
            this.offsetY = offsetY;
            nowEnergy = limitEnergy;
        }

        public string GetImgName() { return imgName; }

        public float GetEnergy() { return nowEnergy; }
        public float GetLimitEnery() { return limitEnergy; }

        public float GetRate() { return nowEnergy / limitEnergy; }

        public bool IsDead() { return nowEnergy == 0; }

        public void Damage(float damage) {
            nowEnergy -= damage;
            nowEnergy = Math.Max(nowEnergy, 0);
        }

        public void Heal(float healPower) {
            nowEnergy += healPower;
            nowEnergy = Math.Min(nowEnergy, limitEnergy);
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる
            if (imgName != "") { entity.RegisterComponent(new C_DrawEnergyBar(this, offsetY)); }
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }


    }
}
