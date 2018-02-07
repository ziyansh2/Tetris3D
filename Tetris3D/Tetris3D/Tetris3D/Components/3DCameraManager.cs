//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　GamePlayシーン
//修正内容リスト：
//名前：柏　　　日付：20171011　　　内容：カメラ対応
//名前：　　　日付：　　　内容：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Utility;
using Tetris3D.Def;

namespace Tetris3D.Components
{
    class _3DCameraManager
    {
        private InputState inputState;

        private float cameraAngle;
        private float cameraHeight;

        public _3DCameraManager(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState;

            cameraAngle = 0;
            cameraHeight = 0;
        }

        public void Update()
        {
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
                if (cameraHeight <= -1) cameraHeight = -1;
            }
            if (inputState.IsDown(Keys.Up))
            {
                cameraHeight += 0.05f;
                if (cameraHeight >= 1) cameraHeight = 1;
            }


            Vector3 cameraPosition = new Vector3((float)Math.Cos(cameraAngle), cameraHeight, (float)Math.Sin(cameraAngle)) * Parameter.DistanceFromStage;
            Camera3D.Update(cameraPosition);

            Console.WriteLine(cameraPosition);
        }
    }
}
