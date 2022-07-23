using Godot;
using System;

public class PlayGUI : Control {
  [Export]
  public NodePath Target { get; set; } = "";
  private Player _target = null;

  private Label _score = null;
  private Label _health = null;

  private void _OnTargetStatusChanged() {
    _UpdateLabels();
  }

  private void _UpdateLabels() {
    _score.Text = $"Score: {_target.Score,9:D09}";
    _health.Text = $"Health: {_target.Health,3:D03}";
   }

  public override void _Ready() {
    if (Target == "")
    {
      return;
    }
    _score = GetNode<Label>("Score");
    _health = GetNode<Label>("Health");
    _target = GetNode<Player>(Target);
    _target.Connect("StatusChanged", this, "_OnTargetStatusChanged");
    _UpdateLabels();
  }
}
