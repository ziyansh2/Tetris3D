//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　メインクラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyLib.Components;
using MyLib.Device;
using MyLib.Entitys;
using Tetris3D.Def;
using Tetris3D.Scene;
using Tetris3D.Scene.ScenePages;

namespace Tetris3D
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private GameDevice gameDevice;
        private SceneManager sceneManager;
        private Camera3D camera3D;
        private Camera2D camera2D;

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphicsDeviceManager.PreferredBackBufferHeight = (int)Parameter.ScreenSize.Y;
            graphicsDeviceManager.PreferredBackBufferWidth = (int)Parameter.ScreenSize.X;


            graphicsDeviceManager.IsFullScreen = false;
        }


        protected override void Initialize()
        {
            gameDevice = new GameDevice(Content, graphicsDeviceManager.GraphicsDevice);
            camera3D = new Camera3D(GraphicsDevice.Viewport, Parameter.StageSize);
            camera2D = new Camera2D(GraphicsDevice.Viewport, Parameter.StageSize);

            //シーン設定
            sceneManager = new SceneManager();
            sceneManager.Add(E_Scene.LOADING, new Loading(gameDevice));
            sceneManager.Add(E_Scene.TITLE, new Title(gameDevice));
            sceneManager.Add(E_Scene.GAMEPLAY, new GamePlay(gameDevice));
            sceneManager.Add(E_Scene.OPERATE, new Operate(gameDevice));
            sceneManager.Add(E_Scene.STAFFROLL, new StaffRoll(gameDevice));
            sceneManager.Add(E_Scene.ENDING, new Ending(gameDevice));
            sceneManager.Add(E_Scene.CLEAR, new Clear(gameDevice));
            sceneManager.Change(E_Scene.LOADING);

            Window.Title = "Tetris3D";
            base.Initialize();
        }


        protected override void LoadContent() { }


        protected override void UnloadContent()
        {
            gameDevice.UnLoad();
            TaskManager.CloseAllTask();
            TaskManager.Update();
            EntityManager.Clear();
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)
                )
            {
                UnloadContent();
                Exit();
            }

            gameDevice.Update();
            sceneManager.Update(gameTime);
            TaskManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CadetBlue);

            base.Draw(gameTime);

            TaskManager.Draw();

            Renderer_2D.Begin();
            sceneManager.Draw();
            Renderer_2D.End();

            //３D向けの設定変更
            //SpriteBatchが変更した設定を元に戻す。
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            gameDevice.GetParticleGroup.Draw();
        }
    }
}
