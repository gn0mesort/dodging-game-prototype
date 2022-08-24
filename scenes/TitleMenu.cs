using Godot;
using System;

public class TitleMenu : Control, IDependsOnMain {
  private Main _main = null;
  private Button _playButton = null;
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

  private void _PlayPressed() {
    var config = new Play.Configuration("res://levels/tunnel.json");
    _main.Scenes.LoadScene("res://scenes/Play.tscn", config.Apply);
  }

  public override void _Ready() {
    _playButton = GetNode<Button>("Options/Play");
    _playButton.Connect("pressed", this, "_PlayPressed");
    _quitButton = GetNode<Button>("Options/Quit");
    _quitButton.Connect("pressed", this, "_QuitPressed");
  }
}
