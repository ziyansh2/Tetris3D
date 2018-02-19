//作成日：　2018.02.19
//作成者：　柏
//クラス内容：　UI描画用クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Components;
using MyLib.Device;
using Tetris3D.Def;

namespace Tetris3D.Components.DrawComps
{
    class C_DrawGameUI : DrawComponent
    {
        public C_DrawGameUI(float depth = 100, float alpha = 1)
        {
            this.alpha = alpha;
            this.depth = depth;

        }
        public override void Draw()
        {
            Renderer_2D.Begin();

            
            Renderer_2D.DrawTexture("UI_BackGround", Vector2.Zero);
            Renderer_2D.DrawString("Score : " + GameConst.Score, new Vector2(1050, 80), Color.White, 1);
            Renderer_2D.DrawString("Combo : " + GameConst.Combo, new Vector2(1050, 130), Color.White, 1);
            Renderer_2D.DrawString("Level : " + GameConst.Level, new Vector2(1050, 180), Color.White, 1);

            Renderer_2D.End();
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
