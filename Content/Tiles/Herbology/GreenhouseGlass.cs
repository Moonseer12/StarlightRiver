using Microsoft.Xna.Framework;
using StarlightRiver.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Tiles.Herbology
{
	public class GreenhouseGlass : ModTile
    {
        public override string Texture => AssetDirectory.HerbologyTile + Name;

        public override void SetStaticDefaults()
        {
            this.QuickSet(0, 13, SoundID.Shatter, new Color(156, 172, 177), ModContent.ItemType<GreenhouseGlassItem>(), false, false, "Greenhouse Glass");
            Main.tileBlockLight[Type] = false;
            //Main.tileLighted[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
        }

        public override void RandomUpdate(int i, int j)
        {

        }
    }

    public class GreenhouseGlassItem : QuickTileItem
    {
        public GreenhouseGlassItem() : base("Greenhouse Glass", "Speeds up the growth of any plant below it\nNeeds a clear area above it", "GreenhouseGlass", 1, AssetDirectory.HerbologyTile) { }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Glass, 10).AddIngredient(ModContent.ItemType<Items.Moonstone.MoonstoneOreItem>()).AddTile(TileID.WorkBenches).Register();

            CreateRecipe().AddIngredient(ModContent.ItemType<GreenhouseWallItem>(), 4).AddTile(TileID.WorkBenches).Register();
        }
    }

    public class GreenhouseWall : ModWall
    {
        public override string Texture => AssetDirectory.HerbologyTile + Name;

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            ItemDrop = ModContent.ItemType<GreenhouseWallItem>();
        }
    }

    public class GreenhouseWallItem : QuickWallItem
    {
        public GreenhouseWallItem() : base("Greenhouse Glass Wall", "Fancy!", ModContent.WallType<GreenhouseWall>(), 0, AssetDirectory.HerbologyTile) { }

        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient(ModContent.ItemType<GreenhouseGlassItem>(), 1).AddTile(TileID.WorkBenches).Register();
        }
    }
}