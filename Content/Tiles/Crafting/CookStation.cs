﻿using Microsoft.Xna.Framework;
using StarlightRiver.Content.GUI;
using StarlightRiver.Core;
using StarlightRiver.Core.Loaders;
using StarlightRiver.Items.Herbology.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarlightRiver.Content.Tiles.Crafting
{
    internal class CookStation : ModTile
    {
        public override string Texture => AssetDirectory.CraftingTile + Name;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 1;

        public override void SetStaticDefaults() => 
            this.QuickSetFurniture(6, 4, DustID.t_LivingWood, SoundID.Dig, true, new Color(151, 107, 75), false, false, "Cooking Station");

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => 
            Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, ItemType<CookStationItem>());

        public override bool RightClick(int i, int j)
        {
            var state = UILoader.GetUIState<CookingUI>();
            if (!state.Visible) { state.Visible = true; Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuOpen); }
            else { state.Visible = false; Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuClose); }
            return true;
        }
    }

    public class CookStationItem : QuickTileItem
    {
        public CookStationItem() : base("Prep Station", "Right click to prepare meals", "CookStation", 0, AssetDirectory.CraftingTile) { }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Wood, 20).AddIngredient(RecipeGroupID.IronBar, 5).AddTile(TileID.WorkBenches).Register();
        }
    }
}