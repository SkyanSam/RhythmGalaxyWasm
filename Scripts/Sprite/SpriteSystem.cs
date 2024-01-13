using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmGalaxy.ECS;
using Raylib_cs;
using System.Numerics;
namespace RhythmGalaxy
{
    public class SpriteSystem : BaseSystem
    {
        public override void Initialize()
        {
            typeset = new Type[2] { typeof(SpriteComponent), typeof(TransformComponent) };
            updateFormat = UpdateFormat.Update1Request1ComponentSet;
            base.Initialize();
        }
        public override void Update1ComponentSet(Dictionary<Type, int> componentSet)
        {
            // note to self.. make a short hand for this next line.. a macro or function...
            var spriteComponent = (SpriteComponent)Database.components[typeof(SpriteComponent)][componentSet[typeof(SpriteComponent)]];
            var transformComponent = (TransformComponent)Database.components[typeof(TransformComponent)][componentSet[typeof(TransformComponent)]];
            

            Raylib.DrawTexturePro(
                spriteComponent.texture2D,
                new Rectangle(0, 0, spriteComponent.texture2D.width, spriteComponent.texture2D.height),
                new Rectangle((int)transformComponent.xPosition, (int)transformComponent.yPosition, spriteComponent.texture2D.width * transformComponent.xScale * spriteComponent.scaleX, spriteComponent.texture2D.height * transformComponent.yScale * spriteComponent.scaleY),
                Vector2.Zero, 0, Color.WHITE // UPDATE THIS LINE LATER
                );
            base.Update1ComponentSet(componentSet);
        }
    }
}
