using System.Runtime.InteropServices;

namespace GameEngine.Core.Rendering.Geometry;


[StructLayout(LayoutKind.Sequential)]
public record struct _Vertex(_Position Position, _UV UV, _Normal Normal);


[StructLayout(LayoutKind.Sequential)]
public record struct _Position(float X, float Y, float Z);


[StructLayout(LayoutKind.Sequential)]
public record struct _UV(float U, float V);


[StructLayout(LayoutKind.Sequential)]
public record struct _Normal(float X, float Y, float Z);
