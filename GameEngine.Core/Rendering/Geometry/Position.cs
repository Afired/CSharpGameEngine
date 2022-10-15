using System.Runtime.InteropServices;

namespace GameEngine.Core.Rendering.Geometry;

[StructLayout(LayoutKind.Sequential)]
public record struct Position(float X, float Y, float Z);
