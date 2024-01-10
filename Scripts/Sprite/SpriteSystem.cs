using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmGalaxy.ECS;
using Raylib_cs;
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
            Raylib.DrawTexture(spriteComponent.texture2D, (int)transformComponent.xPosition, (int)transformComponent.yPosition, Color.WHITE);
            // update this line of code ^^
            base.Update1ComponentSet(componentSet);
        }
    }
}
