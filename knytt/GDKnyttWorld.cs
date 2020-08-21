using Godot;
using YKnyttLib;

public class GDKnyttWorld : Node2D
{
    PackedScene area_scene;

    public string MapPath { get; internal set; }
    public string WorldConfigPath { get; internal set; }
    public KnyttRectPaging<GDKnyttArea> Areas { get; }
    public GDKnyttAssetManager AssetManager { get; }

    public KnyttWorld<string> World { get; }

    public GDKnyttWorld()
    {
        this.AssetManager = new GDKnyttAssetManager(this, 16, 16, 4, 8);
        this.Areas = new KnyttRectPaging<GDKnyttArea>(new KnyttPoint(1, 1));
        this.Areas.OnPageIn = (KnyttPoint loc) => instantiateArea(loc);
        this.Areas.OnPageOut = (KnyttPoint loc, GDKnyttArea area) => destroyArea(loc, area);

        this.World = new KnyttWorld<string>();
        this.area_scene = ResourceLoader.Load("res://knytt/GDKnyttArea.tscn") as PackedScene;
    }

    public GDKnyttArea getArea(KnyttPoint area)
    {
        return this.Areas.Areas[area];
    }

    public GDKnyttArea instantiateArea(KnyttPoint point)
    {
        //if (this.Areas.ContainsKey(point)) { return this.Areas[point]; }

        var area = this.World.getArea(point);
        if (area == null) { return null; }

        var area_node = this.area_scene.Instance() as GDKnyttArea;
        area_node.loadArea(this, area);
        this.GetNode("Areas").AddChild(area_node);
        //this.Areas.Add(area.Position, area_node);

        return area_node;
    }

    public void destroyArea(KnyttPoint point, GDKnyttArea area)
    {
        if (area == null) { return; }
        area.destroyArea();
        area.QueueFree();
    }

    public void loadWorld(Directory world_dir)
    {
        this.AssetManager.discoverWorldStructure(world_dir);

        var map_file = new File();
        map_file.Open(this.MapPath, File.ModeFlags.Read);
        var data = map_file.GetBuffer((int)map_file.GetLen());
        map_file.Close();

        System.IO.MemoryStream map_stream = new System.IO.MemoryStream(data);
        
        this.World.parseWorldFiles(map_stream, null);
        GD.Print(this.World.getArea(new KnyttPoint(1000, 1000)));
    }

    
}
