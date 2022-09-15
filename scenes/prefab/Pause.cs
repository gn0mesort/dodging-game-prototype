using Godot;
using System;

public class Pause : ColorRect, IDependsOnMain {
  private Main _main = null;
  private Button _resume = null;
  private Button _settings = null;
  private Button _quitToMenu = null;
  private Button _quitToDesktop = null;

  public void SetMainNode(Main main) {
    _main = main;
  }

  public Main GetMainNode() {
    return _main;
  }

  private void _OnResumePressed() {
    Visible = false;
    _main.Paused = false;
    QueueFree();
  }

  private void _OnQuitToMenuPressed() {
    _main.Paused = false;
    _main.Scenes.LoadScene("res://scenes/TitleMenu.tscn");
  }

  private void _OnQuitToDesktopPressed() {
    _main.ExitGame(0);
  }

  public override void _Ready() {
    _resume = GetNode<Button>("Menu/Resume");
    _resume.Connect("pressed", this, "_OnResumePressed");
    _settings = GetNode<Button>("Menu/Settings");
    _quitToMenu = GetNode<Button>("Menu/QuitToMenu");
    _quitToMenu.Connect("pressed", this, "_OnQuitToMenuPressed");
    _quitToDesktop = GetNode<Button>("Menu/QuitToDesktop");
    _quitToDesktop.Connect("pressed", this, "_OnQuitToDesktopPressed");
  }

}
