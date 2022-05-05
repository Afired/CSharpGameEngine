namespace ExampleGame.Pathfinding; 

public record Edge {
    public readonly Node ConnectedNode;
    public readonly int Cost;
    
    public Edge(Node connectedNode, int cost) =>
        (ConnectedNode, Cost) = (connectedNode, cost);
}
