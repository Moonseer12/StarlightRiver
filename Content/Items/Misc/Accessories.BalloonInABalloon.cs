﻿using StarlightRiver.Content.Items.BaseTypes;
using StarlightRiver.Content.WorldGeneration;
using StarlightRiver.Core;
using StarlightRiver.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Items.Misc
{
	public class BalloonInABalloon : SmartAccessory
    {
        public override string Texture => AssetDirectory.MiscItem + Name;

        public BalloonInABalloon() : base("Balloon In A Balloon", "Increases jump height \nIncreases mid air maneuverability\nHold up to fall slower") { }

        public override void SafeSetDefaults()
        {
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void SafeUpdateEquip(Player player)
        {
            player.jumpBoost = true;
            if (player.velocity.Y != 0 && player.wings <= 0 && !player.mount.Active)
            {
                player.runAcceleration *= 2f;
                player.maxRunSpeed *= 1.5f;
            }

            if (player.controlUp && player.velocity.Y > 0)
                player.AddBuff(BuffID.Featherfall, 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.ShinyRedBalloon, 1).AddIngredient(ItemID.Silk, 15).AddTile(TileID.TinkerersWorkbench).Register();

            CreateRecipe().AddIngredient(ItemID.ShinyRedBalloon, 1).AddIngredient(ItemID.ShinyRedBalloon, 1).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}