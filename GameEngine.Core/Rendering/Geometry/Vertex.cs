using System.Runtime.InteropServices;

namespace GameEngine.Core.Rendering.Geometry;


[StructLayout(LayoutKind.Sequential)]
public record struct Vertex(Position Position, UV UV, Normal Normal);


[StructLayout(LayoutKind.Sequential)]
public record struct Position(float X, float Y, float Z);


[StructLayout(LayoutKind.Sequential)]
public record struct UV(float U, float V);


[StructLayout(LayoutKind.Sequential)]
public record struct Normal(float X, float Y, float Z);
