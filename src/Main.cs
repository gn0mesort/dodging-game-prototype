/**
 * Copyright (C) 2022 Alexander Rothman <gnomesort@megate.ch>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
using Godot;

/**
 * @brief Main game functionality.
 *
 * An instance of this node should be globally available.
 */
public class Main : Node {
  private RootScenes _previous = RootScenes.Exit;
  private RootScenes _current = RootScenes.Menu;

  /**
   * @brief The save data for the player.
   *
   * This is written to the disk when the game exits.
   */
  public PlayerData Player { get; private set; } = null;

  /**
   * @brief Exit the game gracefully.
   *
   * @param code The exit code to report. On some systems only the low 8-bits will be reported.
   */
  public void ExitGame(int code) {
    OS.ExitCode = code;
    GetTree().Notification(NotificationWmQuitRequest);
  }

  /**
   * @brief Transition between root game scenes.
   *
   * This will unpause the game if it was paused before the transition.
   *
   * @param to The scene to transition to as identified by a RootScenes value.
   */
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
    GetTree().ChangeScene($"res://scenes/root/{scenePaths[((int) to) - 1]}.tscn");
    Resume();
  }

  /**
   * @brief Check whether or not the game is paused.
   *
   * @return True if the game is paused. False in all other cases.
   */
  public bool IsPaused() {
    return GetTree().Paused;
  }

  /**
   * @brief Toggle the pause state of the game.
   */
  public void TogglePaused() {
    var tree = GetTree();
    tree.Paused = !tree.Paused;
  }

  /**
   * @brief Pause the game unconditionally.
   */
  public void Pause() {
    GetTree().Paused = true;
  }

  /**
   * @brief Resume the game unconditionally.
   */
  public void Resume() {
    GetTree().Paused = false;
  }

  /**
   * @brief Loads PlayerData out of the file "user://save.bin".
   *
   * If "user://save.bin" does not exist than the PlayerData is initialized.
   */
  public void LoadPlayerData() {
    var saveData = new File();
    if (saveData.FileExists("user://save.bin"))
    {
      saveData.Open("user://save.bin", File.ModeFlags.Read);
      // Should never be more than 32 bytes.
      var buffer = saveData.GetBuffer((int) saveData.GetLen());
      saveData.Close();
      Player = PlayerData.FromBytes(buffer);
      GD.Print($"Read \"{Player}\" from file.");
    }
    else
    {
      InitializePlayerData();
    }
  }

  /**
   * @brief Initializes the PlayerData.
   *
   * During normal operation this will initialize the PlayerData object held by the Main node but not clear the
   * PlayerData.Flags value. This ensures that flagged behavior (e.g., skipping the tutorial level) doesn't change
   * when the player chooses to be "reborn".
   *
   * @param retainFlags If true then the PlayerData.Flags value is saved before initialization. If false then the
   *                    PlayerData will be completely cleared. Defaults to true.
   */
  public void InitializePlayerData(bool retainFlags = true) {
    var flags = Player.Flags;
    Player = new PlayerData();
    if (retainFlags)
    {
      Player.Flags = flags;
    }
    GD.Print("Initialized player data.");
  }

  /**
   * @brief Write PlayerData to the file "user://save.bin".
   */
  public void StorePlayerData() {
    var saveData = new File();
    saveData.Open("user://save.bin", File.ModeFlags.Write);
    saveData.StoreBuffer(Player.GetBytes());
    saveData.Close();
    GD.Print($"Wrote \"{Player}\" to file.");
  }

  /**
   * @brief Get the previous root scene.
   *
   * @return The RootScene value of the previous root scene.
   */
  public RootScenes PreviousScene() {
    return _previous;
  }

  /**
   * @brief Get the current root scene.
   *
   * @return The RootScene value of the current root scene.
   */
  public RootScenes CurrentScene() {
    return _current;
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    LoadPlayerData();
  }

  /**
   * @brief Deinitialization method.
   *
   * Due to Main being an autoload Node, this should only be called when the game exits.
   */
  public override void _ExitTree() {
    StorePlayerData();
  }
}
