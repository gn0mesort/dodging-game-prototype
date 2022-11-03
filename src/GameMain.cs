using Godot;

public class GameMain : Node {
  private RootScenes _previous = RootScenes.Exit;
  private RootScenes _current = RootScenes.Menu;
  private PlayerData _playerData = null;

  public void ExitGame(int code) {
    OS.ExitCode = code;
    GetTree().Notification(NotificationWmQuitRequest);
  }

  public void TransitionRoot(RootScenes to) {
    GD.Print(to);
    if (to == RootScenes.Exit)
    {
      ExitGame(0);
      return;
    }
    _previous = _current;
    _current = to;
    var scenePaths = new string[]{ "MenuRoot", "PlayRoot", "GameCompleteRoot", "GameOverRoot" };
    GetTree().ChangeScene($"res://scenes/roots/{scenePaths[((int) to) - 1]}.tscn");
  }

  public void Pause() {
    GetTree().Paused = true;
  }

  public void Resume() {
    GetTree().Paused = false;
  }

  public void LoadPlayerData() {
    var saveData = new File();
    if (saveData.FileExists("user://save.bin"))
    {
      saveData.Open("user://save.bin", File.ModeFlags.Read);
      // Should never be more than 32 bytes.
      var buffer = saveData.GetBuffer((int) saveData.GetLen());
      saveData.Close();
      _playerData = PlayerData.FromBytes(buffer);
      GD.Print($"Read \"{_playerData}\" from file.");
    }
    else
    {
      _playerData = new PlayerData();
      GD.Print("Initialized player data.");
    }
  }

  public void StorePlayerData() {
    var saveData = new File();
    saveData.Open("user://save.bin", File.ModeFlags.Write);
    saveData.StoreBuffer(_playerData.GetBytes());
    GD.Print($"Wrote \"{_playerData}\" to file.");
  }

  public RootScenes PreviousScene() {
    return _previous;
  }

  public RootScenes CurrentScene() {
    return _current;
  }

  public override void _Ready() {
    LoadPlayerData();
  }

  public override void _ExitTree() {
    StorePlayerData();
  }
}
