//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　caemraの演出管理
//修正内容リスト：
//名前：宮崎　　　日付：20180215　　　内容：カメラ処理をGamePlayから移行
//名前：　　　日付：　　　内容：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Entitys;
using MyLib.Utility;
using Tetris3D.Def;

namespace Tetris3D.Components.NormalComps
{
    class CameraManager
    {
        private InputState inputState;

        private float cameraAngle;
        private float angleMax;
        private float cameraHeight;

        public CameraManager(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState;

            cameraAngle = 0.79f;
            angleMax = 1.58f;
            cameraHeight = 0.5f;

            
        }

        public void Update(Entity focus)
        {
            Vector2 position = new Vector2(focus.transform.Position.X, focus.transform.Position.Y);
            Camera2D.Update(position);

            if (inputState.IsDown(Keys.Left))
            {
                cameraAngle += 0.05f;
            }
            if (inputState.IsDown(Keys.Right))
            {
                cameraAngle -= 0.05f;
            }
            if (inputState.IsDown(Keys.Down))
            {
                cameraHeight -= 0.05f;
                if (cameraHeight <= 0.5f) cameraHeight = 0.5f;
            }
            if (inputState.IsDown(Keys.Up))
            {
                cameraHeight += 0.05f;
                if (cameraHeight >= 1.2f) cameraHeight = 1.2f;
            }


            Vector3 cameraPosition = new Vector3((float)Math.Cos(cameraAngle), cameraHeight, (float)Math.Sin(cameraAngle)) * Parameter.DistanceFromStage;
            Camera3D.Update(cameraPosition);
            Console.WriteLine(cameraPosition);
        }
    }
}
