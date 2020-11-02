using Godot;
using YUtil.Random;

public class SceneCPUParticles : Node2D
{
    [Export]public int Particles = 1;
    [Export]public float Lifetime = 1f;
    [Export]PackedScene ParticleScene;

    [Export]public float Direction = 0f;
    [Export]public float DirectionVariation = 0f;

    [Export]public float Velocity = 0f;
    [Export]public float VelocityVariation = 0f;

    [Export]public float Mass = 1f;
    [Export]public float MassVariation = 0f;

    [Export]public float GravityDirection = 1.571f;
    [Export]public float GravityDirectionVariation = 0f;

    [Export]public float Gravity = 1f;
    [Export]public float GravityVariation = 0f;

    [Export]public string ParticleParams = "";
    
    PackedScene pinstance_scene;

    public override void _Ready()
    {
        pinstance_scene = ResourceLoader.Load("res://knytt/objects/banks/common/SceneCPUParticleInstance.tscn") as PackedScene;
    }

    public void spawnParticles()
    {
        for (int i = 0; i < Particles; i++)
        {
            spawnParticle();
        }
    }

    private void spawnParticle()
    {
        var p = pinstance_scene.Instance() as SceneCPUParticleInstance;
        p.Velocity = MagnitudeVector(Direction, DirectionVariation, Velocity, VelocityVariation);
        p.Gravity = MagnitudeVector(GravityDirection, GravityDirectionVariation, Gravity, GravityVariation);
        p.Mass = CalcVariation(Mass, MassVariation);
        p.Params = this.ParticleParams;
        
        var ps = ParticleScene.Instance() as Node2D;
        p.AddChild(ps);
        ps.Position = Vector2.Zero;

        AddChild(p);
        p.Position = Vector2.Zero;
    }

    private Vector2 MagnitudeVector(float angle, float angle_var, float mag, float mag_var)
    {
        return InputToVector(angle, angle_var) * CalcVariation(mag, mag_var);
    }

    private Vector2 InputToVector(float angle, float variation)
    {
        float ua = CalcVariation(angle, variation);
        return new Vector2(Mathf.Cos(ua), Mathf.Sin(ua));
    }

    private float CalcVariation(float value, float variation)
    {
        return value + (GDKnyttDataStore.random.NextFloat(2*variation) - variation);
    }
}