using Godot;

/**
 * @brief Behavior script for the pause menu.
 */
public class PauseMenu : Control {
  /**
   * @brief A Signal emitted to request a transition of the root scene.
   *
   * @param to The root scene to transition to.
   */
  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  /**
   * @brief A Signal emitted to request the game resume.
   */
  [Signal]
  public delegate void RequestResume();

  /**
   * @brief A Signal emitted to request the level restart.
   */
  [Signal]
  public delegate void RequestRestartLevel();

  /**
   * @brief A Signal emitted to request the settings menu.
   */
  [Signal]
  public delegate void RequestSettings();

  private Main _main = null;

  /**
   * @brief Initialization method.
   */
  public override void _EnterTree() {
    _main = GetNode<Main>("/root/Main");
    Connect("TransitionRoot", _main, "TransitionRoot");
  }

  private void _OnResumePressed() {
    EmitSignal("RequestResume");
  }

  private void _OnRestartLevelPressed() {
    EmitSignal("RequestRestartLevel");
  }

  private void _OnReturnToTitlePressed() {
    EmitSignal("TransitionRoot", RootScenes.Menu);
  }

  private void _OnExitGamePressed() {
    EmitSignal("TransitionRoot", RootScenes.Exit);
  }

  private void _OnSettingsPressed() {
    EmitSignal("RequestSettings");
  }

  /**
   * @brief Post-_EnterTree initialization/
   */
  public override void _Ready() {
    var resume = GetNode<Button>("Resume");
    resume.Connect("pressed", this, "_OnResumePressed");
    GetNode<Button>("RestartLevel").Connect("pressed", this, "_OnRestartLevelPressed");
    GetNode<Button>("Settings").Connect("pressed", this, "_OnSettingsPressed");
    GetNode<Button>("ReturnToTitle").Connect("pressed", this, "_OnReturnToTitlePressed");
    GetNode<Button>("ExitGame").Connect("pressed", this, "_OnExitGamePressed");
    resume.GrabFocus();
  }
}
