using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface ICollidable
    {
        void Collide(CharacterType otherType);
    }

    public interface IRenderer
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
