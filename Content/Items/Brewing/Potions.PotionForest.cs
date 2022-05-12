using StarlightRiver.Content.Tiles.Crafting;
using StarlightRiver.Content.Tiles.Forest;
using StarlightRiver.Items.Herbology.Materials;
using StarlightRiver.Items.Herbology.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarlightRiver.Content.Items.Brewing
{
	internal class PotionForest : QuickPotion
    {
        public override string Texture => "StarlightRiver/Assets/Items/Brewing/PotionForest";

        public PotionForest() : base("Forest Tonic", "Provides regenration and immunity to poision", 1800, BuffType<Buffs.ForestTonic>(), 2)
        {
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.BottledWater, 1).AddIngredient(ItemType<ForestBerries>(), 5).AddIngredient(ItemType<Ivy>(), 20).Register();
        }
    }
}