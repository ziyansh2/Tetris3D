//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　GamePlayシーン
//修正内容リスト：
//名前：柏　　　日付：20171011　　　内容：カメラ対応
//名前：宮崎　　日付：20180205　　　内容：カメラのをCameraManagerに移した
//名前：　　　日付：　　　内容：

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Entitys;
using MyLib.Components;
using Tetris3D.Def;
using MyLib.Components.DrawComps;
using MyLib.Components.NormalComps;
using Tetris3D.Components;
using System;
using MyLib.Utility;

namespace Tetris3D.Scene.ScenePages
{
    class GamePlay : IScene
    {
        private GameDevice gameDevice;
        private InputState inputState;
        private bool isEnd;

        private E_Scene next;

        private bool isPause;
        private int stageNo;

        private Entity focus;

        private _3DCameraManager cameraManager; 

        //test用
        private Vector3 boxPosi;


        public GamePlay(GameDevice gameDevice) {
            this.gameDevice = gameDevice;

            inputState = gameDevice.GetInputState;

            isPause = false;
            stageNo = 1;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            isEnd = false;
            isPause = false;
            GameConst.Initialize();

            focus = Entity.CreateEntity("Focus", "Forcus", new Transform());
            focus.transform.Position = new Vector3(0, 0, 0);
            focus.Active();

            //Debug用
            DebugInitialize();

            cameraManager = new _3DCameraManager(gameDevice);
        }

        private void DebugInitialize() {
            //ShaderTest用
            CreatShaderTest();

            //CreateBox
            Entity box = Entity.CreateEntity("Box", "Box", new Transform());
            box.RegisterComponent(new C_Model("Box"));
            box.RegisterComponent(new C_DrawModel());

            boxPosi = box.transform.Position;

            //CreateStage
            //Entity pedestal = Entity.CreateEntity("Pedestal", "Pedestal", new Transform());
            //pedestal.RegisterComponent(new C_Model("Pedestal"));
            //pedestal.RegisterComponent(new C_DrawModel());

        }

        private void CreatShaderTest() {
            Entity test = Entity.CreateEntity("Test","Test", new Transform());
            test.transform.Position = new Vector3(0, 0,0);

            test.RegisterComponent(new C_DrawWithShader("TestImg", "UIMask", Vector2.Zero, 100));   //TestMask
        }
        
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime) {
            if (isEnd) { return; }
            if (inputState.WasDown(Keys.P, Buttons.X)) {
                TaskManager.ChangePause();
                isPause = !isPause;
            }
            if (isPause) { return; }

            if (GameConst.IsEnding) {
                next = E_Scene.ENDING;
                isEnd = true;
                return;
            }

            if (GameConst.IsClear) {
                stageNo++;
                Shutdown();
                Initialize();
                return;
            }
            Vector2 position = new Vector2(focus.transform.Position.X, focus.transform.Position.Y);
            Camera2D.Update(position);



            //StageCheck
            if (inputState.IsDown(Keys.W, Buttons.LeftShoulder)) {
                //Camera2D.ZoomIn();
            }
            if (inputState.IsDown(Keys.S, Buttons.RightShoulder)) {
                //Camera2D.ZoomOut();
            }


            //CameraMoveCheck
            cameraManager.Update();


            Sound.PlayBGM("GamePlay");
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            //Renderer_2D.DrawString("ObjectsCount:" + EntityManager.GetEntityCount(), new Vector2(10, 520), Color.Red, 0.5f);
            //Renderer_2D.DrawString("ParticlesCount:" + gameDevice.GetParticlesCount(), new Vector2(10, 550), Color.Red, 0.5f);
            //Renderer_2D.DrawString("cameraPosition:" + new Vector3(Camera3D.GetView().Translation.X, Camera3D.GetView().Translation.Y, Camera3D.GetView().Translation.Y), new Vector2(700, 100), Color.White);
            //Renderer_2D.DrawString("boxPosition:" + boxPosi, new Vector2(700, 150), Color.White);
            //Renderer_2D.DrawString("angle:" + cameraAngle, new Vector2(700, 200), Color.White);
        }


        /// <summary>
        /// シーンを閉める
        /// </summary>
        public void Shutdown() {
            gameDevice.GetParticleGroup.Clear();
            TaskManager.CloseAllTask();
            EntityManager.Clear();
            Camera2D.Initialize();
        }

        /// <summary>
        /// 終わりフラッグを取得
        /// </summary>
        /// <returns>エンドフラッグ</returns>
        public bool IsEnd() { return isEnd; }

        /// <summary>
        /// 次のシーン
        /// </summary>
        /// <returns>シーンのEnum</returns>
        public E_Scene Next() { return next; }
    }
}
