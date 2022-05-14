namespace GameEngine.Editor.PropertyDrawers;

public abstract class PropertyDrawer<T> {
    
    internal static Type Type => typeof(T); 
    public abstract void Draw(T property);
    
}

public class PropertyDrawerInt : PropertyDrawer<int> {
    
    public override void Draw(int property) {
        
    }
    
}
