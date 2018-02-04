//�쐬���F�@2018.02.04
//�쐬�ҁF�@��
//�N���X���e�F�@���C���N���X
//�C�����e���X�g�F
//���O�F�@�@�@���t�F�@�@�@���e�F
//���O�F�@�@�@���t�F�@�@�@���e�F


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
            camera2D = new Camera2D(GraphicsDevice.Viewport, Parameter.StageSize);

            //�V�[���ݒ�
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
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            TaskManager.Draw();

            Renderer_2D.Begin();
            sceneManager.Draw();
            Renderer_2D.End();

            //�RD�����̐ݒ�ύX
            //SpriteBatch���ύX�����ݒ�����ɖ߂��B�i����̓J�����O�̐ݒ�݂̂�OK�j
            //GraphicsDevice.BlendState = BlendState.Opaque;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            gameDevice.GetParticleGroup.Draw();


        }
    }
}
