﻿using DiscOut.Avalonia;
using DiscOut.GameObjects.Dynamic;
using DiscOut.GameObjects.Static;
using DiscOut.Renderer;
using DiscOut.Util;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace DiscOut.GameObjects.World;
internal class Level
{
    protected List<Brick> bricks = new(114);
    protected Ball ball;
    protected class BrickData
    {
        public string Colour { get; set; } = "";
        public BrickType Type { get; set; } = BrickType.NORMAL;
        [JsonPropertyName("Repeat")]
        public int Loop { get; set; } = 0;
    }

    protected struct LevelData
    {
        public byte BaseBallSpeed { get; set; }
        public BrickData[] Bricks { get; set; }
    };

    protected enum BrickDataColour
    {
        DiscPink,
        DiscBlue,
        DiscOrange,
        DiscGreen,
    };

    private Brick MakeBrick(BrickType type, int x, int y, string str)
    {
        byte colour = (BrickDataColour)Enum.Parse(typeof(BrickDataColour), str) switch
        {
            BrickDataColour.DiscPink => 3,
            BrickDataColour.DiscBlue => 4,
            BrickDataColour.DiscOrange => 5,
            BrickDataColour.DiscGreen => 6,
            _ => throw new ArgumentException("a colour provided in the level is not one implemented in the game"),
        };
        return type switch
        {
            BrickType.NORMAL => new NormalBrick(x, y, colour, this),
            BrickType.LIFE => new LifeBrick(x, y, this),
            _ => throw new ArgumentException("a type provided in the level is not one implemented in the game"),
        };
    }

    private void AddBrick(BrickData brick, ref int x, ref int y)
    {
        if (bricks.Count >= 114) throw new ArgumentException("trying to add to many brings to the live");
        Brick created = MakeBrick(brick.Type, x, y, brick.Colour);
        bricks.Add(created);
        x += 6;
        if (x > 40)
        {
            x = -39;
            y -= 6;
        }
    }

    public Level(string json_level)
    {
        LevelData data = JsonSerializer.Deserialize<LevelData>(AssetUtil.ReadEmbeddedFile(json_level));
        ball = new(data.BaseBallSpeed);
        int x = -39;
        int y = 100;
        foreach (BrickData brick in data.Bricks)
        {
            if (brick.Loop == 0)
                AddBrick(brick, ref x, ref y);
            else for (byte i = 0; i < brick.Loop; i++)
                    AddBrick(brick, ref x, ref y);
        }
    }

    public Ball GetBall() => ball;
    public List<Brick> GetBricks() => bricks;

    public sbyte bricks_to_gc = 5;
    public void OnBrickDeath(object sender, EventArgs e)
    {
        bricks.Remove((Brick)sender);
        bricks_to_gc--;
        if (bricks_to_gc < 0)
        {
            bricks_to_gc = 5;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);
            GC.WaitForPendingFinalizers();
        }
    }

    public void OnRendering(QuadBatchRenderer renderer)
    {
        foreach (Brick brick in bricks)
            brick.OnRendering(renderer);
        renderer.Flush();
        ball.OnRendering(renderer);
        renderer.FlushAntiGhost();
    }
    public void OnUpdate()
    {
        if (bricks.Count <= 0)
            DiscWindow.Instance.LevelWon();
        else
            ball.OnUpdate(this);
    }
}