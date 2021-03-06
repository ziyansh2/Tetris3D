﻿//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　GamePlayシーン
//修正内容リスト：
//名前：柏　　　日付：2018.02.19　　　内容：Boxの生成を一個ずつに
//名前：柏　　　日付：2018.02.19　　　内容：UI描画追加
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Entitys;
using MyLib.Components;
using Tetris3D.Def;
using MyLib.Components.DrawComps;
using MyLib.Components.NormalComps;
using System;
using Tetris3D.Utility;
using Tetris3D.Components.DrawComps;
using Tetris3D.Components.UpdateComps;

using System.Collections.Generic;
using Tetris3D.Components.NormalComps;


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

        private static Random rand = new Random();
        private int num;
        private C_Models models;

        public GamePlay(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;

            inputState = gameDevice.GetInputState;

            isPause = false;
            stageNo = 1;

            stage = new StageData();

        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEnd = false;
            isPause = false;
            GameConst.Initialize();

            focus = Entity.CreateEntity("Focus", "Forcus", new Transform());
            focus.transform.Position = new Vector3(0, 0, 0);
            focus.Active();

            C_DrawGameUI uiDraw = new C_DrawGameUI();
            uiDraw.Active();
            TaskManager.AddTask(uiDraw);

            cameraAngleXZ = 0;
            cameraAngleXY = 0;

            StageData.InitializeStage();

            models = new C_Models();

            //Debug用
            DebugInitialize();
        }

        private void DebugInitialize()
        {
            //ShaderTest用
            CreatShaderTest();

            //CreateStage
            C_DrawStage draw = new C_DrawStage();
            draw.Active();
            TaskManager.AddTask(draw);
        }


        private void CreatShaderTest()
        {
            Entity test = Entity.CreateEntity("Test", "Test", new Transform());

            test.transform.Position = new Vector3(0, 0, 0);

            test.RegisterComponent(new C_DrawWithShader("TestImg", "UIMask", Vector2.Zero, 100));   //TestMask
        }

        public void BoxUpdate() {
            //focus.ClearVoidComponent();
            if (!GameConst.CanCreateBox) { return; }
            num = rand.Next(1, 5);

            List<Vector3> data = models.GetList(num);
            Vector3 creatsPosition = new Vector3(
                rand.Next(Parameter.StageMaxIndex - models.GetListX()), 
                rand.Next(Parameter.StageMaxIndex - models.GetListY()), 
                10) * Parameter.BoxSize;

            List<C_BoxStatus> statuses = new List<C_BoxStatus>();

            data.ForEach(d =>{
                statuses.Add(CreatBox(creatsPosition + d * Parameter.BoxSize));
            });

            C_OneTetris tetris = new C_OneTetris(gameDevice, statuses);
            tetris.Active();
            TaskManager.AddTask(tetris);

            GameConst.CanCreateBox = false;
        }

        private C_BoxStatus CreatBox(Vector3 position)
        {
            Transform trans = new Transform();
            trans.Position = position;

            int type = rand.Next(3);
            string name = ((eBoxType)type).ToString();

            Entity box = Entity.CreateEntity(name, name, trans);
            C_BoxStatus status = new C_BoxStatus(eBoxState.On, StageData.TypeChange[name], position / Parameter.BoxSize);
            box.RegisterComponent(status);
            box.RegisterComponent(new C_Model(name));
            box.RegisterComponent(new C_DrawModel());
            //box.RegisterComponent(new C_BoxMoveUpdate(gameDevice));

            return status;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime)
        {
            if (isEnd) { return; }
            if (inputState.WasDown(Keys.P, Buttons.X))
            {
                TaskManager.ChangePause();
                isPause = !isPause;
            }
            if (isPause) { return; }

            if (GameConst.IsEnding)
            {
                next = E_Scene.ENDING;
                isEnd = true;
                return;
            }

            if (GameConst.IsClear)
            {
                stageNo++;
                Shutdown();
                Initialize();
                return;
            }
            Vector2 position = new Vector2(focus.transform.Position.X, focus.transform.Position.Y);
            Camera2D.Update(position);

            //CreateBox
            BoxUpdate();


            //StageCheck
            if (inputState.IsDown(Keys.W, Buttons.LeftShoulder))
            {
                //Camera2D.ZoomIn();
            }
            if (inputState.IsDown(Keys.S, Buttons.RightShoulder))
            {
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

            Console.WriteLine("XZ:" + cameraAngleXZ + ", XY:" + cameraAngleXY);


            Sound.PlayBGM("GamePlay");
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw()
        {
            //Renderer_2D.DrawString("ObjectsCount:" + EntityManager.GetEntityCount(), new Vector2(10, 520), Color.Red, 0.5f);
            //Renderer_2D.DrawString("ParticlesCount:" + gameDevice.GetParticlesCount(), new Vector2(10, 550), Color.Red, 0.5f);
        }


        /// <summary>
        /// シーンを閉める
        /// </summary>
        public void Shutdown()
        {
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
