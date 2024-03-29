/*using System.Collections.Generic;
using System.Linq;

namespace ExampleGame.Pathfinding;

public class AStar {
    
    
    public static List<PNode> Search(PNode startNode, PNode endNode) {
        
        // nodes to be evaluated
        List<PNode> open = new();
        // nodes already evaluated
        List<PNode> closed = new();
        
        open.Add(startNode);
        
        while(true) {
            //return null if there is no possible path
            if(open.Count == 0) return null;
            
            // node in open with lowest FCost
            PNode current = open.OrderBy(n => n.FCost).First();
            open.Remove(current);
            closed.Add(current);
            
            if(current == endNode)
                break;
            
            foreach(Edge edge in current.Edges) {
                if(!edge.ConnectedNode.IsValid)
                    continue;
                if(closed.Contains(edge.ConnectedNode))
                    continue;

                if(!open.Contains(edge.ConnectedNode)) { // new node
                    // set FCost of neighborNode
                    edge.ConnectedNode.GCost = current.GCost + edge.Cost;
                    edge.ConnectedNode.UpdateHCost(endNode);
                    edge.ConnectedNode.Parent = current;
                    
                    open.Add(edge.ConnectedNode);
                }
                
                else { // previous calculated
                    int newGCost = current.GCost + edge.Cost;
                    if(edge.ConnectedNode.GCost > newGCost) { // path to neighbor is shorter than previous calculated
                        // update node
                        edge.ConnectedNode.GCost = newGCost;
                        edge.ConnectedNode.Parent = current;
                    }
                }
                
            }
        }
        
        
        List<PNode> path = new();
        PNode currentNode = endNode;
        while(currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Add(startNode);
        return path;
    }
    
}
*/