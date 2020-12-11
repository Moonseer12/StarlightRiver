﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using StarlightRiver.Core;
using StarlightRiver.Content.Items;

namespace StarlightRiver.Tiles.Interactive
{
    internal class VoidDoorOn : ModTile
    {
        public override void SetDefaults()
        {
            QuickBlock.QuickSet(this, int.MaxValue, DustType<Content.Dusts.Void>(), SoundID.Drown, Color.Black, ItemType<VoidDoorItem>());
            Main.tileMerge[Type][TileType<VoidDoorOff>()] = true;
            animationFrameHeight = 88;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 5)
            {
                frameCounter = 0;
                if (++frame >= 3) frame = 0;
            }
        }
    }

    internal class VoidDoorOff : ModTile
    {
        public override void SetDefaults()
        {
            drop = ItemType<VoidDoorItem>();
            dustType = DustType<Content.Dusts.Void>();
            Main.tileMerge[Type][TileType<VoidDoorOn>()] = true;
        }
    }

    public class VoidDoorItem : QuickTileItem { public VoidDoorItem() : base("Void Barrier", "Dissappears when Purified", TileType<VoidDoorOn>(), 8) { } }
}