﻿using StarlightRiver.Content.Biomes;
using Terraria.ID;

namespace StarlightRiver.Content.CustomHooks
{
	class PassiveLight : ModSystem
	{
		private static float mult = 0;
		private static bool vitricLava = false;

		private static Rectangle squidDomeRect;

		public override void Load()
		{
			Terraria.Graphics.Light.On_TileLightScanner.GetTileLight += VitricLightingNew;
		}

		public override void ClearWorld()
		{
			squidDomeRect = default;
		}

		private void VitricLightingNew(Terraria.Graphics.Light.On_TileLightScanner.orig_GetTileLight orig, Terraria.Graphics.Light.TileLightScanner self, int x, int y, out Vector3 outputColor)
		{
			orig(self, x, y, out outputColor);

			if (Main.LocalPlayer.InModBiome(ModContent.GetInstance<Biomes.PermafrostTempleBiome>()))
			{
				if (squidDomeRect.Contains(x, y))
					outputColor += new Vector3(0.15f, 0.175f, 0.25f) * 1.25f;
			}

			// If the tile is in the vitric biome and doesn't block light, emit light.
			if (VitricDesertBiome.onScreen && StarlightWorld.vitricBiome.Contains(x, y))
			{
				Tile tile = Main.tile[x, y];
				bool tileBlock = tile.HasTile && Main.tileBlockLight[tile.TileType] && !(tile.Slope != SlopeType.Solid || tile.IsHalfBlock);
				bool wallBlock = Main.wallLight[tile.WallType];
				bool lava = tile.LiquidType == LiquidID.Lava;
				bool lit = Main.tileLighted[tile.TileType];

				if (vitricLava && lava)
					outputColor = new Vector3(1, 0, 0);

				if (!tileBlock && wallBlock && !lava && !lit)
				{
					int yOff = y - StarlightWorld.vitricBiome.Y;

					if (mult > 1)
						mult = 1;

					float progress = 0.5f + yOff / (float)StarlightWorld.vitricBiome.Height * 0.7f;
					progress = MathHelper.Max(0.5f, progress);

					outputColor.X = (0.3f + (yOff > 70 ? ((yOff - 70) * 0.006f) : 0)) * progress * mult;
					outputColor.Y = (0.48f + (yOff > 70 ? ((yOff - 70) * 0.0005f) : 0)) * progress * mult;
					outputColor.Z = (0.65f - (yOff > 70 ? ((yOff - 70) * 0.005f) : 0)) * progress * mult;

					if (yOff > 90 && mult < 1)
					{
						outputColor.X += (1 - mult * mult) * progress * 0.6f;
						outputColor.Y += (1 - mult * mult) * progress * 0.2f;
					}
				}
			}
		}

		public override void PostUpdateEverything()
		{
			if (!Main.gameMenu)
			{
				mult = 0.8f + (Main.dayTime ? (float)System.Math.Sin(Main.time / Main.dayLength * 3.14f) * 0.35f : -(float)System.Math.Sin(Main.time / Main.nightLength * 3.14f) * 0.35f);
				vitricLava = Main.LocalPlayer.InModBiome(ModContent.GetInstance<VitricDesertBiome>());
			}

			if (squidDomeRect == default)
			{
				squidDomeRect = StarlightWorld.squidBossArena;
				squidDomeRect.X += 26;
				squidDomeRect.Width -= 52;
				squidDomeRect.Y += 35;
				squidDomeRect.Height = 76;
			}
		}
	}
}