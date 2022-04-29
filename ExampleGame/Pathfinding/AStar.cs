using System.Collections.Generic;
using System.Linq;
using ExampleGame.Entities;

namespace ExampleGame.Pathfinding;

public class AStar {
    
    
//    public static List<Node> Search(Grid grid, Node startNode, Node endNode) {
//        
//        // nodes to be evaluated
//        List<Node> open = new();
//        // nodes already evaluated
//        List<Node> closed = new();
//        
//        open.Add(startNode);
//
//        while(open.Count > 0) {
//            // node in open with lowest FCost
//            Node current = open.OrderBy(n => n.FCost).First();
//            open.Remove(current);
//            closed.Add(current);
//            
//            if(current == endNode)
//                break;
//            
//            foreach(Node neighborNode in current.Neighbors) {
//                if(!neighborNode.IsValid)
//                    continue;
//                if(closed.Contains(neighborNode))
//                    continue;
//
//                if(!open.Contains(neighborNode)) { // new node
//                    // set FCost of neighborNode
//                    neighborNode.FCost = ;
//                    neighborNode.Parent = current;
//                    
//                    open.Add(neighborNode);
//                } else if() { // path to neighbor is shorter than previous calculated
//                    // update node
//                    neighborNode.FCost = ;
//                    neighborNode.Parent = current;
//                }
//            }
//        }
//        
//    }
    
}
