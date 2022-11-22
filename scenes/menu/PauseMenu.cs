using Godot;

public class PauseMenu : Control {
  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  [Signal]
  public delegate void RequestResume();

  [Signal]
  public delegate void RequestRestartLevel();

  [Signal]
  public delegate void RequestSettings();

  private GameMain _main = null;

  public override void _EnterTree() {
    _main = GetNode<GameMain>("/root/Main");
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
