using System.Numerics;
using GameEngine.Debugging;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class ConsoleWindow : EditorWindow {
    
    private static List<LogMessage> _logMessages;
    private int _maxLogs = 50;
    
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
        for(int i = 0; i < _logMessages.Count; i++) {
            _logMessages[i].Draw();
        }
    }
    
    private class LogMessage {
        
        private string _message;
        private LogSeverity _logSeverity;
        
        public LogMessage(string message, LogSeverity logSeverity) {
            _message = message;
            _logSeverity = logSeverity;
        }

        public void Draw() {
            ImGui.TextColored(ToColor(_logSeverity), _message);
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
