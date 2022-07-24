using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class TerminalWindow : EditorWindow {
    
    private string _input = string.Empty;

    public TerminalWindow() {
        Title = "Terminal";
    }
    
    protected override void Draw() {
        if(ImGui.InputText("##TerminalInput", ref _input, 100, ImGuiInputTextFlags.EnterReturnsTrue)) {
            ProcessCmd(_input);
            _input = string.Empty;
        }
    }
    
    private void ProcessCmd(string command) {
        Console.LogWarning("Command execution is not Implemented yet!");
    }
    
}
