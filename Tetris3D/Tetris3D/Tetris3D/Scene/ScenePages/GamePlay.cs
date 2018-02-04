﻿//作成日：　2017.10.04
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

            focus = Entity.CreateEntity("Focus", "Forcus", new Transform2D());
            focus.Active();

            //Debug用
            DebugInitialize();
        }

        private void DebugInitialize() {
            //ShaderTest用
            //CreatShaderTest();

            //StageCheck
            if (inputState.IsDown(Keys.W, Buttons.LeftShoulder)) {
                Camera2D.ZoomIn();
            }
            if (inputState.IsDown(Keys.S, Buttons.RightShoulder)) {
                Camera2D.ZoomOut();
            }
        }

        private void CreatShaderTest() {
            Entity test = Entity.CreateEntity("Test","Test", new Transform2D());
            test.transform.Position = new Vector2(1000, 1200);

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
            if (EntityManager.GetEntityCount() > 0) {
                Camera2D.Update(focus.transform.Position);
            }
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