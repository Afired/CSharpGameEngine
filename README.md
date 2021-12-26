# Simple C# Game Engine
![C# 10](https://img.shields.io/badge/-C%23_10-b5faaa)
![.NET 6.0](https://img.shields.io/badge/-.NET_6.0-acfcf0)
![GLFW](https://img.shields.io/badge/-GLFW-e1aafa)
![OpenGL 3.3](https://img.shields.io/badge/-OpenGL_3.3-faaaaa)
![Box2D](https://img.shields.io/badge/-Box2D-fad5aa)
![ImGui](https://img.shields.io/badge/-ImGui-f6faaa)

## About
Goal of this project is to create a game engine with an entity component system,
**capable of inheritance** and **composition** patterns while at the same time
enforcing implementation to provide **type-safety** and avoiding anonymous game
objects. Although this is also preventing run-time changes like adding new components,
it brings the advantage of being able to cast game objects into it's components
`(GameObject as ITransform)` instead of having to retrieve it through some generic
getter method `(GetComponent<T>)`. Since casts are not creating much overhead, there
is **no need to cache references** to components like it is common in other game engines.
This also prevents null references, should a component be switched out with another instance.

## Branches
- master
    - uml-planning
    - development
        - feature-branches

## Setup
_Currently there is no special setup needed._
