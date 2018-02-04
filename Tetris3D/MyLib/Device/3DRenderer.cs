//作成日：　2017.02.07
//クラス内容：　3Dモデル管理クラス
//修正内容リスト：
//2017.03.07: リファクタリング(リソースいじる部分を抽出)

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Shooting3D.Device
{
    class Modeler
    {
        public Modeler() { }

        /// <summary>
        /// Xna固有機能利用し、モデルを描画
        /// </summary>
        /// <param name="camera">カメラ</param>
        /// <param name="model">モデル</param>
        /// <param name="world">ワールド座標</param>
        public void DrawModel(Camera camera, Model model, Matrix world) {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes) {
                foreach (BasicEffect be in mesh.Effects) {
                    be.EnableDefaultLighting();
                    be.Projection = camera.Projection;
                    be.View = camera.View;
                    be.World = world * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }
    }
}
