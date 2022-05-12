using StarlightRiver.Content.Items.BaseTypes;
using StarlightRiver.Content.WorldGeneration;
using StarlightRiver.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Items.Misc
{
	public class Cheapskates : SmartAccessory, IChestItem
    {
        public override string Texture => AssetDirectory.MiscItem + Name;

        public int Stack => 1;

        public ChestRegionFlags Regions => ChestRegionFlags.Ice;

        public Cheapskates() : base("Cheapskates", "Maximum movement speed is doubled\nYou take 30% more damage and acceleration is reduced") { }

        public override void Load()
        {
            StarlightPlayer.PreHurtEvent += PreHurtAccessory;
        }

		public override void Unload()
		{
            StarlightPlayer.PreHurtEvent -= PreHurtAccessory;
        }

		public override void SafeUpdateEquip(Player Player)
        {
            Player.runAcceleration *= 2;
            Player.maxRunSpeed *= 2;
        }

        private bool PreHurtAccessory(Player Player, bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Equipped(Player))
            {
                damage = (int)(damage * 1.3f);
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Wood, 50).AddIngredient(ItemID.Chain, 10).AddIngredient(ItemID.DemoniteBar, 20).AddTile(TileID.IceMachine).Register();

            CreateRecipe().AddIngredient(ItemID.Wood, 50).AddIngredient(ItemID.Chain, 10).AddIngredient(ItemID.CrimtaneBar, 20).AddTile(TileID.IceMachine).Register();
        }
    }
}