using Godot;
using System;
using YUtil.Random;

public class HomingCannon : Cannon
{
    protected override void reinitializeBullet(BaseBullet p, int i)
    {
        p.Translate(new Vector2(8f, 8f));
        // TODO: not the right formula, maybe DS remake approximates it better
        p.Direction = Mathf.Pi / 4;
        p.VelocityMMF2 = -((Juni.ApparentPosition.x - GlobalPosition.x) / 8f - 13f - GDKnyttDataStore.random.NextFloat(6));
        p.GravityMMF2 = 10 + GDKnyttDataStore.random.NextFloat(5);
    }

    protected override void playSound()
    {
        player.Play(1f);
    }
}