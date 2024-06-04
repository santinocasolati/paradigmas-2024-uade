using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface ICharacter
    {
        int PosX { get; }
        int PosY { get; }
        Vector2 Position { get; }
        Vector2 Size { get; }
        Vector2 RealSize { get; }
        CharacterType Type { get; }
    }

    public interface ICollidable
    {
        void Collide(CharacterType otherType);
    }

    public interface IDrawable
    {
        void Draw();
        void Update(float deltaTime);
    }

    public interface IRecievesInput
    {
        void Input();
    }

    public interface IHasAnimations
    {
        void AddAnimations();
    }

    public interface IPickable
    {
        void Pick();
    }
}
