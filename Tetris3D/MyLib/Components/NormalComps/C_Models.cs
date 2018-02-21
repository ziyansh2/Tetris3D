using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;

namespace MyLib.Components.NormalComps
{
    //ModelのList用クラス
    public class C_Models : Component
    {
        private int num;
        private int boxsizeX, boxsizeY;
        private List<Vector3> rist = new List<Vector3>();

        public C_Models()
        {

        }

        public List<Vector3> GetList(int num)
        {
            this.num = num;
            if (num == 1)
            {
                rist = new List<Vector3>
                {
                    new Vector3(0,0,0),
                    new Vector3(1,0,0)
                };

                boxsizeX = 2;

            }
            else if (num == 2)
            {
                rist = new List<Vector3>
                {
                    new Vector3(0,0,1),
                    new Vector3(0,0,2),
                };
            }
            else if (num == 3)
            {
                rist = new List<Vector3>
                {
                    new Vector3(0,0,0),
                    new Vector3(0,1,0),
                };
                boxsizeY = 2;
            }
            else if (num == 4)
            {
                rist = new List<Vector3>
                {
                    new Vector3(3,0,0),
                    new Vector3(2,0,0),
                    new Vector3(1,0,0),
                    new Vector3(2,1,0)
                };
                boxsizeX = 3;
                boxsizeY = 1;
            }
            return rist;
        }

        public int GetListX()
        {
            return boxsizeX;
        }
        public int GetListY()
        {
            return boxsizeY;
        }
    }
}
