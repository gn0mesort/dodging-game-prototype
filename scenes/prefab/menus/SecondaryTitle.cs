using Godot;
using System;

public class SecondaryTitle : VBoxContainer {
  [Signal]
  public delegate void Transition(GameState to);


  private void _OnExitPressed() {
    EmitSignal("Transition", GameState.Exit);
  }

  public override void _Ready() {
    GetNode("Exit").Connect("pressed", this, "_OnExitPressed");
  }

}
