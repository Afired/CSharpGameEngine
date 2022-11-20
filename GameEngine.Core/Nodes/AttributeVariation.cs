// using System;
// using GameEngine.Core.Nodes;
//
// [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
// public class Has<T> : Attribute where T : Node {
//     public Has(string name) { }
// }
//
// [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
// public class Arr<T> : Attribute where T : Node {
//     public Arr(string name) { }
// }
//
// public class Component {
//     
// }
//
// [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
// public class ValidOn<T> : Attribute where T : Node {
//     public ValidOn(string name) { }
// }
//
//
//
// [Has<Trigger>]
// [Has<Trigger>("Trigger2")]
// [Arr<Transform3D>("SpawnLocations")]
// public partial class Spawner : Node {
//     
// }
//
// [ValidOn<Spawner>("Spawner")]
// public partial class SpawnInput : Component {
//     
// }
