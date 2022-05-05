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
            
            foreach((Node node, int cost) neighbor in current.Neighbors) {
                if(!neighbor.node.IsValid)
                    continue;
                if(closed.Contains(neighbor.node))
                    continue;

                if(!open.Contains(neighbor.node)) { // new node
                    // set FCost of neighborNode
                    neighbor.node.GCost = current.GCost + neighbor.cost;
                    neighbor.node.UpdateHCost(endNode);
                    neighbor.node.Parent = current;
                    
                    open.Add(neighbor.node);
                }
                
                else { // previous calculated
                    int newGCost = current.GCost + neighbor.cost;
                    if(neighbor.node.GCost > newGCost) { // path to neighbor is shorter than previous calculated
                        // update node
                        neighbor.node.GCost = newGCost;
                        neighbor.node.Parent = current;
                    }
                }
                
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
