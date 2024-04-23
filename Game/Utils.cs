using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public struct Vector2
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void ScaleVector(float scale)
        {
            x *= scale;
            y *= scale;
        }
    }
    public class Utils
    {

      
    }
}
