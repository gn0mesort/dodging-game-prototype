using Godot;

public class GameMain : Node {
  private PlayerData _playerData = null;

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

  public void LoadPlayerData() {
    var saveData = new File();
    if (saveData.FileExists("user://save.bin"))
    {
      saveData.Open("user://save.bin", File.ModeFlags.Read);
      // Should never be more than 32 bytes.
      var buffer = saveData.GetBuffer((int) saveData.GetLen());
      saveData.Close();
      _playerData = PlayerData.FromBytes(buffer);
      GD.Print($"Read player data file: {_playerData}");
    }
    else
    {
      _playerData = new PlayerData();
      GD.Print("Initialized player data file.");
    }
  }

  public void StorePlayerData() {
    var saveData = new File();
    saveData.Open("user://save.bin", File.ModeFlags.Write);
    saveData.StoreBuffer(_playerData.GetBytes());
    GD.Print("Wrote player data file.");
  }

  public override void _Ready() {
    LoadPlayerData();
  }

  public override void _ExitTree() {
    StorePlayerData();
  }
}
