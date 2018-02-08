//作成日：　2018.02.04
//作成者：　柏
//クラス内容：　3Dカメラクラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Device
{
    public class Camera3D
    {
        private static Vector3 cameraPosition;       //targetは左上の（0,0）から描画したい座標
        private static Vector3 targetForcus;
        private static Vector3 nowForcus;
        private static Vector3 offsetPosition;
        private static Vector3 priviousOffset;
        private static Rectangle cameraRect;

        private static float zoomSize;
        private static Matrix transform = Matrix.CreateTranslation(Vector3.Zero);

        //初期化してから変わらない
        private static Matrix projection;
        private static Matrix view;
        private static Vector2 stageSize;
        private static Vector2 screenSize;
        private static Vector3 offsetDirect;


        public Camera3D(Viewport viewport, Vector2 stageS) {
            stageSize = stageS;
            screenSize = new Vector2(viewport.Width, viewport.Height);
            offsetDirect = new Vector3(0, 0, 1);
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, //標準視覚
                screenSize.X / screenSize.Y, //幅と高さの比
                1,       //near
                5000     //far
            );
            view = Matrix.Identity;

            offsetPosition = Vector3.Zero;

            //targetは画面の中心からちょっと下の位置に描画するよう
            cameraPosition = offsetDirect;
            targetForcus = cameraPosition + offsetDirect;
            nowForcus = cameraPosition + offsetDirect;

            cameraRect = new Rectangle();

            zoomSize = 1;
            transform = Matrix.CreateTranslation(Vector3.Zero);
        }

        public static float GetZoom() { return zoomSize; }
        public static Matrix GetTransform() { return transform; }
        public static void ZoomIn() { zoomSize += 0.01f; }
        public static void ZoomOut() { zoomSize -= 0.01f; }
        public static void ZoomInitialize() { zoomSize = 1; }
        public static void TurnRight() { targetForcus.X = (cameraPosition + offsetDirect).X; }
        public static void TurnLeft() { targetForcus.X = (cameraPosition - offsetDirect).X; }


        public static void Initialize() {
            cameraRect = new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y);
            Vector3 targetPosition = Vector3.Zero;
            //InScreenClip(ref targetPosition);

            ////2Dカメラ処理(ZoomInの状況に合わせて移動)
            //transform =
            //    Matrix.CreateTranslation(new Vector3(-targetPosition, 0)) *     //targetのところに移動
            //    Matrix.CreateScale(zoomSize, zoomSize, 1) *                     //Zoom処理
            //    Matrix.CreateRotationZ(0) *                                     //回転処理
            //    Matrix.CreateTranslation(new Vector3(startPosition, 0));        //targetを画面に描画したい所に映りための移動

            //Shader描画できるEntityはEffect使って描画するため、Entityの映すPositionを算出
            priviousOffset = cameraPosition - targetPosition;
            offsetPosition = priviousOffset;
        }

        public static void Update(Vector3 targetPosition) {
            //view = Matrix.CreateLookAt(cameraPosition, targetPosition, Vector3.Up);
            view = Matrix.CreateLookAt(targetPosition, Vector3.Zero, Vector3.Up);


            //cameraRect = new Rectangle(
            //    (int)(targetPosition - nowForcus).X,
            //    (int)(targetPosition - nowForcus).Y,
            //    (int)screenSize.X,
            //    (int)screenSize.Y
            //);
            //InScreenClip(ref targetPosition);
            //LocusSlowMove(ref targetPosition);
            //LocusTurnMove();

            ////2Dカメラ処理(ZoomInの状況に合わせて移動)
            //transform =
            //    Matrix.CreateTranslation(new Vector3(-targetPosition, 0)) *     //targetのところに移動
            //    Matrix.CreateScale(zoomSize, zoomSize, 1) *                     //Zoom処理
            //    Matrix.CreateRotationZ(0) *                                     //回転処理
            //    Matrix.CreateTranslation(new Vector3(nowForcus, 0));        //targetを画面に描画したい所に映りための移動

            ////Shader描画できるEntityはEffect使って描画するため、Entityの映すPositionを算出
            //priviousOffset = offsetPosition;
            //offsetPosition = nowForcus - targetPosition;
        }

        private static void LocusTurnMove() {
            nowForcus += (targetForcus - nowForcus) * 0.02f;
        }

        private static void LocusSlowMove(ref Vector3 targetPosition) {
            Vector3 privioustargetPosition = nowForcus - priviousOffset;
            targetPosition = privioustargetPosition + (targetPosition - privioustargetPosition) * 0.2f;
        }

        public static Vector3 GetCameraMove() { return offsetPosition - priviousOffset; }
        public static Vector3 GetOffsetPosition() { return offsetPosition; }
        public static Matrix GetProjection() { return projection; }
        public static Matrix GetView() { return view; }

    }
}
