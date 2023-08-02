using Godot;
using System;

public partial class HUD : CanvasLayer
{
    public Label Message, Score;
    public Timer MessageTimer;
    public Button StartButton;

    [Signal]
    public delegate void StartGameEventHandler();

    public override void _Ready()
    {
        base._Ready();

        Message = GetNode<Label>("Message");
        Score = GetNode<Label>("ScoreLabel");
        MessageTimer = GetNode<Timer>("MessageTimer");
        StartButton = GetNode<Button>("StartButton");
    }

    public void ShowMessage(string text)
    {
        Message.Text = text;
        Message.Show();

        MessageTimer.Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over");

        await ToSignal(MessageTimer, Timer.SignalName.Timeout);

        Message.Text = "Dodgeball";
        Message.Show();

        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        StartButton.Show();
    }

    public void UpdateScore(int s)
    {
        Score.Text = s.ToString();
    }

    private void OnStartButtonPressed()
    {
        StartButton.Hide();
        EmitSignal(SignalName.StartGame);
    }

    private void OnMessageTimerTimeout()
    {
        Message.Hide();
    }
}
