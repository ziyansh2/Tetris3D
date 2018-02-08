//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　3Dモデル管理クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyLib.Device
{
    public class Modeler
    {
        /// <summary>
        /// Xna固有機能利用し、モデルを描画
        /// </summary>
        /// <param name="model">モデル</param>
        /// <param name="world">ワールド座標</param>
        public static void DrawModel(Model model, Matrix world) {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes) {
                foreach (BasicEffect be in mesh.Effects) {
                    be.EnableDefaultLighting(); 
                    be.Projection = Camera3D.GetProjection();
                    be.View = Camera3D.GetView();
                    be.World = world * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }
    }
}
