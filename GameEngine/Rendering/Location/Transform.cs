namespace GameEngine; 

public class Transform : IPosition, IScale, IRotation {
    
    public Position Position { get; set; }
    public Scale Scale { get; set; }
    public Rotation Rotation { get; set; }
    
    
    public Transform() {
        Position = new Position();
        Scale = new Scale();
        Rotation = new Rotation();
    }
    
}
