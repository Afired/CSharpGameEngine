using System.Runtime.InteropServices;

namespace GameEngine.Core.Rendering.Geometry;

[StructLayout(LayoutKind.Sequential)]
public record struct Vertex(Position Position, Uv UV, Normal Normal);
