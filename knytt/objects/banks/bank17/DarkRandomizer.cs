using Godot;
using System;
using YUtil.Random;

public class DarkRandomizer : Muff
{
    private bool shoot = false;

    public override void _Ready()
    {
        GDArea.Selector.Register(this);
        GDArea.Bullets.RegisterEmitter(this, "DropStuff", 20,
            (p, i) => 
            {
                p.Translate(new Vector2(12, 14));
                p.VelocityMMF2 = 20 + random.Next(40);
                p.DirectionMMF2 = 8 + random.Next(1, 5) * (random.NextBoolean() ? 1 : -1);
                p.GravityMMF2 = 12;
            });
        base._Ready();
    }
    
    protected override void init()
    {
        _on_ShotTimer_timeout();
    }

    private async void _on_ShotTimer_timeout()
    {
        shoot = !shoot;
        if (shoot)
        {
            speed = 0;
            sprite.Play("ready");
            await ToSignal(sprite, "animation_finished");
            if (GDArea.Selector.IsObjectSelected(this))
            {
                GetNode<AudioStreamPlayer2D>("ShotPlayer").Play();
                for (int i = 0; i < 10; i++)
                {
                    GDArea.Bullets.Emit(this, i);
                }
            }
            sprite.Play("aftershot");
        }
        else
        {
            changeSpeed(8 + random.Next(5));
            _on_DirectionTimer_timeout();
        }
    }
}
