using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class GDKnyttDataStore : Node
{
    public static Random random = new Random();
    public static GDKnyttWorldImpl KWorld { get; set; }

    private static SceneTree Tree { get; set; }

    public static string CutsceneName { get; private set; }
    public static string CutsceneAfter { get; private set; }
    public static Node CutsceneReturn { get; private set; }

    public override void _Ready()
    {
        Tree = GetTree();
    }

    public static void startGame(bool new_game)
    {
        if (new_game) { startCutscene("Intro", "res://knytt/GDKnyttGame.tscn"); }
        else { Tree.ChangeScene("res://knytt/GDKnyttGame.tscn"); }
    }

    public static void winGame(string ending="Ending")
    {
        startCutscene(ending, "res://knytt/ui/MainMenu.tscn");
    }

    public static void playCutscene(string cutscene)
    {
        CutsceneName = cutscene;
        CutsceneAfter = null;
        CutsceneReturn = Tree.CurrentScene;
        if (!KWorld.worldFileExists(Cutscene.makeScenePath(1))) { return; }
        Tree.Paused = true;
        Tree.Root.RemoveChild(Tree.CurrentScene);
        Tree.ChangeScene("res://knytt/ui/Cutscene.tscn");
    }

    private static void startCutscene(string cutscene, string after)
    {
        CutsceneName = cutscene;
        CutsceneAfter = after;
        CutsceneReturn = null;
        if (!KWorld.worldFileExists(Cutscene.makeScenePath(1))) { Tree.ChangeScene(after); return; }
        Tree.ChangeScene("res://knytt/ui/Cutscene.tscn");
    }
}

public static class RandomExtension
{
    public static T NextElement<T>(this Random random, ICollection<T> e)
    {
        return e.ElementAt(GDKnyttDataStore.random.Next(e.Count()));
    }
}
