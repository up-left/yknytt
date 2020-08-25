using Godot;

public class GameButton : Button
{
    public GDKnyttWorldImpl World { get; private set; }

    [Signal]
    public delegate void GamePressed();

    public void initialize(Texture icon, GDKnyttWorldImpl world)
    {
        this.World = world;
        GetNode<TextureRect>("MainContainer/IconTexture").Texture = icon;
        GetNode<Label>("MainContainer/TextContainer/NameLabel").Text = string.Format("{0} {1}", world.Info.Name, world.Info.Author);
        GetNode<Label>("MainContainer/TextContainer/DescriptionLabel").Text = world.Info.Description;
    }

    public void _on_GameButton_pressed()
    {
        EmitSignal(nameof(GamePressed), this);
    }
}
