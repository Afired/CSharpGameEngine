 using System;
 using GameEngine.Core.Nodes;

// [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
// public class Has<T> : Attribute where T : Node {
//     public Has(string name) { }
// }
 
 [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
 public class Has<T> : Attribute where T : INode, new() {
     public Has(string name) { }
     public Has() { }
 }
 
 [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
 public class Has<TAbstract, TConcrete> : Attribute where TAbstract : INode where TConcrete : TAbstract, new() {
     public Has(string name) { }
     public Has() { }
 }
 
 [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
 public class Arr<T> : Attribute where T : Node {
     public Arr(string name) { }
     public Arr() { }
 }

// public class Component {
//     
// }
//
// [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
// public class ValidOn<T> : Attribute where T : Node {
//     public ValidOn(string name) { }
// }
 
 [Has<Trigger>]
 [Has<Trigger>("Trigger2")]
 [Arr<Transform>("SpawnLocations")]
 public partial class Spawner : Node {
     
 }

// [ValidOn<Spawner>("Spawner")]
// public partial class SpawnInput : Component {
     
// }
