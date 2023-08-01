using Godot;

public partial class Player : Area2D
{
    [Export]
    public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

    public Vector2 ScreenSize; // Size of the game window.

    /**************************************************************************
    *** Functions
    **************************************************************************/

    // _Ready is called when a node enters the scene tree
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
    }

    // _Process
    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite2D.Play();

        if (Input.IsActionPressed("move_right")) velocity.X++;
        if (Input.IsActionPressed("move_left")) velocity.X--;
        if (Input.IsActionPressed("move_down")) velocity.Y++;
        if (Input.IsActionPressed("move_up")) velocity.Y--;

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            //animatedSprite2D.Play();
        }
        else
        {
            //animatedSprite2D.Stop();
        }

        // Clamp the position to the screen boundaries
        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );

        animatedSprite2D.Animation = updateAnimation(velocity);
    }

    private StringName updateAnimation(Vector2 velocity)
    {
        return velocity.X switch
        {
            > 0 => "right",
            < 0 => "left",
            _ => velocity.Y switch
            {
                > 0 => "down",
                < 0 => "up",
                _ => "idle"
            }

        };
    }
}
