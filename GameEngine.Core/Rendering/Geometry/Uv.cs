using System.Runtime.InteropServices;

namespace GameEngine.Core.Rendering.Geometry;

[StructLayout(LayoutKind.Sequential)]
public record struct Uv(float U, float V);
