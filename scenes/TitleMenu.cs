using Godot;
using System;

public class TitleMenu : Control, IDependsOnMain {
  private Main _main = null;

  private Button _quitButton = null;

  public void SetMainNode(Main main) {
    _main = main;
  }

  public Main GetMainNode() {
    return _main;
  }

  private void _QuitPressed() {
    _main.ExitGame(0);
  }

  public override void _Ready() {
    _quitButton = GetNode<Button>("Options/Quit");
    _quitButton.Connect("pressed", this, "_QuitPressed");
  }
}
