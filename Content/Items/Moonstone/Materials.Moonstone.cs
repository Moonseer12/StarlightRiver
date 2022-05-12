using StarlightRiver.Content.Tiles.Moonstone;
using StarlightRiver.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarlightRiver.Content.Items.Moonstone
{
	public class MoonstoneOreItem : QuickTileItem
    {
        public MoonstoneOreItem() : base("Moonstone", "", "MoonstoneOre", ItemRarityID.Blue, AssetDirectory.MoonstoneItem) { }

        public override void SafeSetDefaults() => Item.value = Item.sellPrice(0, 0, 1, 50);
    }

    public class MoonstoneBarItem : QuickTileItem
    {
        public MoonstoneBarItem() : base("Moonstone Bar", "'Shimmering with Beautiful Light'", "MoonstoneBar", ItemRarityID.White, AssetDirectory.MoonstoneItem) { }  //TODO: Fix place type

        public override void SafeSetDefaults() => Item.value = Item.sellPrice(0, 0, 13, 50);

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemType<MoonstoneOreItem>(), 3).AddTile(TileID.Furnaces).Register();

            //adds back neccisary vanilla recipies
            Mod.CreateRecipe(ItemID.DrillContainmentUnit).AddIngredient(ItemID.LunarBar, 40).AddIngredient(ItemID.ChlorophyteBar, 40).AddIngredient(ItemID.ShroomiteBar, 40).AddIngredient(ItemID.SpectreBar, 40).AddIngredient(ItemID.HellstoneBar, 40).AddIngredient(ItemType<MoonstoneBarItem>(), 40).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}