﻿using Microsoft.Xna.Framework;
using StarlightRiver.Content.Buffs;
using StarlightRiver.Content.Items.Vitric;
using StarlightRiver.Content.Tiles.Forest;
using StarlightRiver.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Items.Potions
{
	public abstract class BarrierPotion : ModItem
	{
		int amount;
		int duration;
		readonly string prefix;

		public override string Texture => AssetDirectory.PotionsItem + Name;

		public BarrierPotion(int amount, int duration, string prefix)
		{
			this.amount = amount;
			this.duration = duration;
			this.prefix = prefix;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(prefix + " Barrier Potion");
			Tooltip.SetDefault($"Grants {amount} barrier\nGreatly reduces overcharge barrier loss for {duration / 60} seconds");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.consumable = true;
			Item.maxStack = 30;
			Item.UseSound = SoundID.Item3;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = 15;
			Item.useAnimation = 15;
		}

		public override bool CanUseItem(Player Player) => !Player.HasBuff(ModContent.BuffType<NoShieldPot>()) && !Player.HasBuff(BuffID.PotionSickness);

		public override bool? UseItem(Player player)
		{
			player.GetModPlayer<BarrierPlayer>().Barrier += amount;
			player.AddBuff(ModContent.BuffType<ShieldDegenReduction>(), duration);
			player.AddBuff(ModContent.BuffType<NoShieldPot>(), 3600);
			player.AddBuff(BuffID.PotionSickness, 1200);

			CombatText.NewText(player.Hitbox, new Color(150, 255, 255), amount);

			return true;
		}
	}

	public class LesserBarrierPotion  : BarrierPotion
	{
		public LesserBarrierPotion() : base(40, 180, "Lesser") { }

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ModContent.ItemType<Slimeberry>(), 2).AddIngredient(ItemID.Glass, 5).AddIngredient(ItemID.BottledWater).AddTile(TileID.Bottles).Register();
		}
	}

	public class RegularBarrierPotion : BarrierPotion
	{
		public RegularBarrierPotion() : base(80, 240, "") { }

		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<LesserBarrierPotion>(), 5).AddIngredient(ModContent.ItemType<VitricOre>(), 2).AddIngredient(ItemID.GlowingMushroom, 2).AddTile(TileID.Bottles).Register();
		}
	}

	public class GreaterBarrierPotion : BarrierPotion
	{
		public GreaterBarrierPotion() : base(120, 300, "Greater") { }

		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ModContent.ItemType<RegularBarrierPotion>(), 5).AddIngredient(ItemID.SoulofLight).AddIngredient(ItemID.SoulofNight).AddTile(TileID.Bottles).Register();

			CreateRecipe(5).AddIngredient(ItemID.BottledWater, 5).AddIngredient(ModContent.ItemType<Slimeberry>(), 10).AddIngredient(ItemID.SoulofLight).AddIngredient(ItemID.SoulofNight).AddTile(TileID.Bottles).Register();
		}
	}

	public class NoShieldPot : SmartBuff
	{
		public NoShieldPot() : base("Barrier Sickness", "Cannot consume more barrier potions", true ) { }

		public override string Texture => AssetDirectory.PotionsItem + Name;
	}

	public class ShieldDegenReduction : SmartBuff
	{
		public ShieldDegenReduction() : base("Barrier Affinity", "Barrier sticks to you better", false) { }

		public override string Texture => AssetDirectory.PotionsItem + Name;

		public override void Update(Player Player, ref int buffIndex)
		{
			Player.GetModPlayer<BarrierPlayer>().OverchargeDrainRate -= 50;
		}
	}
}
