# C# Game Engine
![C# 10](https://img.shields.io/badge/-C%23_10-b5faaa)
![.NET 6.0](https://img.shields.io/badge/-.NET_6.0-acfcf0)
![GLFW](https://img.shields.io/badge/-GLFW-e1aafa)
![OpenGL 3.3](https://img.shields.io/badge/-OpenGL_3.3-faaaaa)
![Box2D](https://img.shields.io/badge/-Box2D-fad5aa)
![ImGui](https://img.shields.io/badge/-ImGui-f6faaa)

## Editor
Import Assets, configure Nodes, composite Scenes, preview in Play Mode, serialize Data, reload Assemblies...

## Nodes
The Node System is a type save and reliable way of structuring code and creating logic. Thanks to the use of source generators the correct way of implementing the subclass sandbox pattern is handled automatically. Nodes are capable of inheritance and Composition, making them both reusable and flexible at the same time.Even the Scene is a Node which can be inherited from, making it easy to create unique level logic/data or share similar ones.

## Physics
Nodes like Trigger, Collider or RigidBody come out of the box and provide 2D physics using Box 2D under the hood.

## Extensibility
One of the main goals of this project is to create an engine which can be easily extended. Providing an intuitive API for extending the Editor or making Tools is therefore also high priority.This is achieved by loading custom editor assemblies, in which you can define Editor Windows, Property Drawers, Node Drawers and more to come..
