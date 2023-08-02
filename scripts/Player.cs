using Godot;

public partial class Player : Area2D
{
    [Export]
    public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

    [Signal]
    public delegate void HitEventHandler();

    public Vector2 ScreenSize; // Size of the game window.
    public CollisionShape2D CollisionShape;

    /**************************************************************************
    *** Functions
    **************************************************************************/

    // _Ready is called when a node enters the scene tree
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        Hide();
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
        }

        // Clamp the position to the screen boundaries
        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );

        animatedSprite2D.Animation = updateAnimation(velocity);
        animatedSprite2D.FlipH = velocity.X < 0;
    }

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        CollisionShape.Disabled = false;
    }

    private void OnBodyEntered(PhysicsBody2D body)
    {
        Hide(); // Player disappears after being hit.
        EmitSignal(SignalName.Hit);
        // Must be deferred as we can't change physics properties on a physics callback.
        CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    private StringName updateAnimation(Vector2 velocity)
    {
        return velocity.X switch
        {
            > 0 => "move",
            < 0 => "move",
            _ => velocity.Y switch
            {
                > 0 => "down",
                < 0 => "up",
                _ => "idle"
            }

        };
    }
}
