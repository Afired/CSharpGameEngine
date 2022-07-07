using GameEngine.Core.Numerics;
using GameEngine.Editor.PropertyDrawers;
using Console = GameEngine.Core.Debugging.Console;

namespace ExampleGame.Editor.PropertyDrawers;

public class Vector2Drawer : PropertyDrawer<Vector2> {
    
    protected override void DrawProperty(ref Vector2 value, Property property) {
        Console.LogSuccess("Drawing Vec2");
    }
    
}
