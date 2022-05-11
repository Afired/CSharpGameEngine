using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows;

public class InspectorWindow : EditorWindow {
    
    private Node v_selected;
    public Node Selected {
        get => v_selected;
        private set {
            v_selected = value;
            // todo: needs a unique identifier, because if not provided it takes the name
            //Title = $"Inspector - {value.GetType().ToString()}";
        }
    }
    
    
    public InspectorWindow() {
        Title = "Inspector";
        HierarchyWindow.OnSelect += entity => Selected = entity;
    }
    
    protected override void Draw() {
        if(Selected == null) {
            ImGui.Text("select an entity");
            return;
        }
        if(Selected is Transform transform)
            DrawTransform(transform);
        if(Selected is Renderer renderer)
            DrawRenderer(renderer);
        if(Selected is Camera2D camera2D)
            DrawCamera2D(camera2D);
        if(Selected is Camera3D camera3D)
            DrawCamera3D(camera3D);
    }
    
    private void DrawTransform(Transform transform) {
        bool opened = ImGui.TreeNodeEx("Transform", ImGuiTreeNodeFlags.DefaultOpen);
        
        if(!opened)
            return;
        
        {
            System.Numerics.Vector3 transformLocalPosition = new System.Numerics.Vector3(transform.LocalPosition.X, transform.LocalPosition.Y, transform.LocalPosition.Z);
            ImGui.DragFloat3("Local Position", ref transformLocalPosition, 0.1f);
            transform.LocalPosition = new Vector3(transformLocalPosition.X, transformLocalPosition.Y, transformLocalPosition.Z);
            
            System.Numerics.Vector3 transformPosition = new System.Numerics.Vector3(transform.Position.X, transform.Position.Y, transform.Position.Z);
            ImGui.InputFloat3("World Position", ref transformPosition, "%g", ImGuiInputTextFlags.ReadOnly);
//            transform.Position = new Vector3(transformPosition.X, transformPosition.Y, transformPosition.Z);
            
            float transformRotation = transform.Rotation;
            ImGui.DragFloat("Rotation", ref transformRotation, 0.01f);
            transform.Rotation = transformRotation;
            
            System.Numerics.Vector3 transformScale = new System.Numerics.Vector3(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
            ImGui.DragFloat3("Scale", ref transformScale, 0.01f);
            transform.Scale = new Vector3(transformScale.X, transformScale.Y, transformScale.Z);
        }
        
        ImGui.TreePop();
        ImGui.Separator();
    }
    
    private void DrawRenderer(Renderer renderer) {
        bool opened = ImGui.TreeNodeEx("Renderer", ImGuiTreeNodeFlags.DefaultOpen);
        
        if(!opened)
            return;
        
        {
            string rendererShader = renderer.Shader;
            ImGui.InputText("Shader", ref rendererShader, 30);
            renderer.Shader = rendererShader;
            
            string rendererTexture = renderer.Texture;
            ImGui.InputText("Texture (dont edit)", ref rendererTexture, 30);
            renderer.Texture = rendererTexture;
        }
        
        ImGui.TreePop();
        ImGui.Separator();
    }
    
    private void DrawCamera2D(Camera2D camera2D) {
        bool opened = ImGui.TreeNodeEx("Camera 2D", ImGuiTreeNodeFlags.DefaultOpen);
        
        if(!opened)
            return;
        
        {
            float camera2DZoom = camera2D.Zoom;
            ImGui.DragFloat("Zoom", ref camera2DZoom);
            camera2D.Zoom = camera2DZoom;
            
            System.Numerics.Vector4 camera2DBackgroundColor = new System.Numerics.Vector4(camera2D.BackgroundColor.R, camera2D.BackgroundColor.G, camera2D.BackgroundColor.B, camera2D.BackgroundColor.A);
            ImGui.ColorEdit4("Background Color", ref camera2DBackgroundColor);
            camera2D.BackgroundColor = new Color(camera2DBackgroundColor.X, camera2DBackgroundColor.Y, camera2DBackgroundColor.Z, camera2DBackgroundColor.W);
            
            if(ImGui.Button("Set active")) {
                RenderingEngine.SetActiveCamera(camera2D);
            }
        }
        
        ImGui.TreePop();
        ImGui.Separator();
    }
    
    private void DrawCamera3D(Camera3D camera3D) {
        bool opened = ImGui.TreeNodeEx("Camera 3D", ImGuiTreeNodeFlags.DefaultOpen);
        
        if(!opened)
            return;
        
        {
            float camera3DFieldOfView = camera3D.FieldOfView;
            ImGui.DragFloat("Field of View", ref camera3DFieldOfView);
            camera3D.FieldOfView = camera3DFieldOfView;
            
            System.Numerics.Vector2 clippingDistance = new System.Numerics.Vector2(camera3D.NearPlaneDistance, camera3D.FarPlaneDistance);
            ImGui.InputFloat2("Clipping Distance", ref clippingDistance);
            camera3D.NearPlaneDistance = clippingDistance.X;
            camera3D.FarPlaneDistance = clippingDistance.Y;
            
            System.Numerics.Vector4 camera2DBackgroundColor = new System.Numerics.Vector4(camera3D.BackgroundColor.R, camera3D.BackgroundColor.G, camera3D.BackgroundColor.B, camera3D.BackgroundColor.A);
            ImGui.ColorEdit4("Background Color", ref camera2DBackgroundColor);
            camera3D.BackgroundColor = new Color(camera2DBackgroundColor.X, camera2DBackgroundColor.Y, camera2DBackgroundColor.Z, camera2DBackgroundColor.W);

            if(ImGui.Button("Set active")) {
                RenderingEngine.SetActiveCamera(camera3D);
            }
        }
        
        ImGui.TreePop();
        ImGui.Separator();
    }
    
}
