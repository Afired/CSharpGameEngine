using System.Runtime.InteropServices;

namespace GameEngine.Core.Rendering.Geometry;

[StructLayout(LayoutKind.Sequential)]
public record struct Normal(float X, float Y, float Z);
