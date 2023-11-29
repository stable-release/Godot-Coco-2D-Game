using System;
using Godot;

public partial class main_character : CharacterBody2D
{
    public const float Speed = 400.0f;
    public const float JumpVelocity = -900.0f;
    public AnimatedSprite2D sprite_2d;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
        sprite_2d = base.GetNode<AnimatedSprite2D>("Sprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

		// Animations
		if (Math.Abs(velocity.X) > 1)
		{
			sprite_2d.Animation = "running";
		}
		else
		{
			sprite_2d.Animation = "default";
		}

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
            sprite_2d.Animation = "jumping";
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("left", "right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, 15);
        }

        Velocity = velocity;
        MoveAndSlide();

        bool isLeft = velocity.X < 0;
        sprite_2d.FlipH = isLeft;
    }
}
