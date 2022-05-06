using System.Numerics;
using GameEngine.Core.Debugging;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class ConsoleWindow : EditorWindow {
    
    private List<LogMessage> _logMessages;
    private int _maxLogs = 50;
    private bool _showNormalLogMessages = true;
    private bool _showSuccessLogMessages = true;
    private bool _showWarningLogMessages = true;
    private bool _showErrorLogMessages = true;
    
    
    public ConsoleWindow() {
        Title = "Console";
        _logMessages = new List<LogMessage>();
        Console.OnLog += OnLog;
    }
    
    //! CURRENTLY NOT THREAD SAFE
    private void OnLog(string message, LogSeverity logSeverity) {
        _logMessages.Add(new LogMessage(message, logSeverity));
        if(_logMessages.Count > _maxLogs)
            _logMessages.RemoveAt(0);
    }
    
    protected override void Draw() {
        DrawToolBar();
        DrawLogs();
    }
    
    private void DrawLogs() {
        for(int i = 0; i < _logMessages.Count; i++) {
//            if(_logMessages[i].LogSeverity == LogSeverity.Error && !_showErrorLogMessages)
//                continue;
            _logMessages[i].Draw();
        }
    }
    
    private void DrawToolBar() {
        if(ImGui.Button("Clear"))
            _logMessages.Clear();
//        ImGui.Checkbox("Errors", ref _showErrorLogMessages);
    }
    
    private record LogMessage(string Message, LogSeverity LogSeverity) {
        
        public readonly string Message = Message;
        public readonly LogSeverity LogSeverity = LogSeverity;
        
        public void Draw() {
            ImGui.TextColored(ToColor(LogSeverity), Message);
        }
        
        private static Vector4 ToColor(LogSeverity logSeverity) => logSeverity switch {
            LogSeverity.Normal => new Vector4(1, 1, 1, 1),
            LogSeverity.Success => new Vector4(0, 1, 0, 1),
            LogSeverity.Warning => new Vector4(1, 1, 0, 1),
            LogSeverity.Error => new Vector4(1, 0, 0, 1),
            _ => throw new NotImplementedException("log level not implemented")
        };
        
    }
    
}
