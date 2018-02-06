//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　GamePlayシーン
//修正内容リスト：
//名前：柏　　　日付：20171011　　　内容：カメラ対応
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
using System;
using MyLib.Utility;
using Tetris3D.Utility;
using Tetris3D.Components.DrawComps;

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
        private StageData stage;

        private float cameraAngleXZ;
        private float cameraAngleXY;


        public GamePlay(GameDevice gameDevice) {
            this.gameDevice = gameDevice;

            inputState = gameDevice.GetInputState;

            isPause = false;
            stageNo = 1;

            stage = new StageData();
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

            cameraAngleXZ = 0;
            cameraAngleXY = 0;

            StageData.InitializeStage();

            //Debug用
            DebugInitialize();
        }

        private void DebugInitialize() {
            //ShaderTest用
            CreatShaderTest();

            //CreateBox
            //Entity box = Entity.CreateEntity("Box", "Box", new Transform());
            //box.RegisterComponent(new C_Model("Box"));
            //box.RegisterComponent(new C_DrawModel());

            //CreateStage
            C_DrawStage draw = new C_DrawStage();
            draw.Active();
            TaskManager.AddTask(draw);
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
            if (inputState.IsDown(Keys.Left)) { cameraAngleXZ += 0.05f; }
            if (inputState.IsDown(Keys.Right)) { cameraAngleXZ -= 0.05f; }
            if (inputState.IsDown(Keys.Up)) { cameraAngleXY += 0.05f; }
            if (inputState.IsDown(Keys.Down)) { cameraAngleXY -= 0.05f; }

            //cameraAngle = Method.AngleClamp(cameraAngle);

            Vector3 cameraPosition = new Vector3(
                (float)Math.Cos(cameraAngleXZ), 
                (float)Math.Sin(cameraAngleXY),
                (float)Math.Sin(cameraAngleXZ)) * Parameter.DistanceFromStage;

            Camera3D.Update(cameraPosition);

            Console.WriteLine(cameraPosition);


            Sound.PlayBGM("GamePlay");
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            //Renderer_2D.DrawString("ObjectsCount:" + EntityManager.GetEntityCount(), new Vector2(10, 520), Color.Red, 0.5f);
            //Renderer_2D.DrawString("ParticlesCount:" + gameDevice.GetParticlesCount(), new Vector2(10, 550), Color.Red, 0.5f);
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
