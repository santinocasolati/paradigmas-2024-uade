using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public struct Vector2
    {
        private float x;
        private float y;

        public Vector2(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public void ScaleVector(float scale)
        {
            X *= scale;
            Y *= scale;
        }

        public void Add(Vector2 v)
        {
            X += v.X;
            Y += v.Y;
        }

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public string VecToString()
        {
            return "X: " + X + ", " + Y + ")";
        }
    }
}
