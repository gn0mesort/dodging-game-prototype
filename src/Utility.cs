using Godot;

public class Utility : Node {
  public void ExitGame(int code) {
    OS.ExitCode = code;
    GetTree().Notification(NotificationWmQuitRequest);
  }

  public void TransitionRoot(RootScenes to) {
    if (to == RootScenes.Exit)
    {
      ExitGame(0);
      return;
    }
  }

  public void Pause() {
    GetTree().Paused = true;
  }

  public void Resume() {
    GetTree().Paused = false;
  }
}
