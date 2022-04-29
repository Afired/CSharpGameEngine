using System.Collections.Generic;
using System.Linq;

namespace ExampleGame.Pathfinding;

public class AStar {
    
    
    public static List<Node> Search(Node startNode, Node endNode) {
        
        // nodes to be evaluated
        List<Node> open = new();
        // nodes already evaluated
        List<Node> closed = new();
        
        open.Add(startNode);
        
        while(true) {
            //return null if there is no possible path
            if(open.Count == 0) return null;
            
            // node in open with lowest FCost
            Node current = open.OrderBy(n => n.FCost).First();
            open.Remove(current);
            closed.Add(current);
            
            if(current == endNode)
                break;
            
            foreach(Node neighborNode in current.Neighbors) {
                if(!neighborNode.IsValid)
                    continue;
                if(closed.Contains(neighborNode))
                    continue;

                if(!open.Contains(neighborNode)) { // new node
                    // set FCost of neighborNode
                    neighborNode.GCost = current.GCost + 10;
                    neighborNode.UpdateHCost(endNode);
                    neighborNode.Parent = current;
                    
                    open.Add(neighborNode);
                }/* else if() { // path to neighbor is shorter than previous calculated
                    // update node
                    neighborNode.GCost = current.GCost + 10;
                    neighborNode.Parent = current;
                }*/
            }
        }
        
        
        List<Node> path = new();
        Node currentNode = endNode;
        while(currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Add(startNode);
        return path;
    }
    
}
