﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarlightRiver.Core;
using StarlightRiver.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Items.Dungeon
{
    class Cloudstrike : ModItem
    {
        int charge;
        public override string Texture => AssetDirectory.DungeonItem + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloudstrike");

            Tooltip.SetDefault("Update this later");
        }

        public override void SetDefaults()
        {
            item.damage = 32;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.magic = true;
            item.mana = 60;
            item.shoot = ModContent.ProjectileType<CloudstrikeShot>();
            item.shootSpeed = 10;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.autoReuse = true;
            item.channel = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 dir = Vector2.Normalize(new Vector2(speedX, speedY));
            Projectile.NewProjectile(position + (dir * 60), new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-0.2f,0.2f)), type, damage, knockBack, player.whoAmI, charge);
            charge = 1;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void HoldItem(Player player)
        {
            if (charge < 100 && !player.channel)
                charge++;

            item.damage = (int)MathHelper.Lerp(20, 70, charge / 100f);
            base.HoldItem(player);
        }

    }

    public class CloudstrikeShot : ModProjectile, IDrawAdditive, IDrawPrimitive
    {
        public override string Texture => AssetDirectory.DungeonItem + "Cloudstrike";

        private float charge => projectile.ai[0];

        private float chargeSqrt => (float)Math.Sqrt(charge);

        private int reach => ((int)charge * 3) + 100;

        private bool initialized = false;

        private bool reachedMouse = false;

        private int power => (int)(chargeSqrt * 3) + 10;

        private Player player => Main.player[projectile.owner];

        private List<Vector2> cache;
        private List<Vector2> cache2;
        private Trail trail;

        private List<NPC> hitTargets = new List<NPC>();
        private NPC target = default;

        private Vector2 startPoint = Vector2.Zero;


        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.extraUpdates = 14;
            projectile.penetrate = -1;
            projectile.hide = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Electro Shock");
        }

        public override void AI()
        {
            if (!initialized)
            {
                startPoint = projectile.Center;
                ManageCaches();
                initialized = true;
                projectile.timeLeft = (int)(Math.Sqrt(chargeSqrt) * 30) + 45;
            }

            if (Main.netMode != NetmodeID.Server && (projectile.timeLeft % 4 == 0 || projectile.timeLeft <= 25))
            {
                if (projectile.timeLeft % 2 == 0)
                    ManageCaches();
                ManageTrails();
            }
            if (projectile.timeLeft > 36)
                player.itemTime = player.itemAnimation = (int)(chargeSqrt + 1) * 3;

            if (projectile.timeLeft <= 25)
            {
                projectile.velocity = Vector2.Zero;
                projectile.extraUpdates = 0;
                return;
            }

            var temptarget = Main.npc.Where(x => x.active && !x.townNPC /*&& !x.immortal && !x.dontTakeDamage /&& !x.friendly*/ && !hitTargets.Contains(x) && x.Distance(projectile.Center) < reach).OrderBy(x => x.Distance(projectile.Center)).FirstOrDefault();
            if (Main.rand.NextBool(30))
            {
                target = temptarget;
            }
            if (hitTargets.Contains(target))
                target = default;
            if (target != default)
            {
                projectile.velocity = Vector2.Normalize(target.Center - projectile.Center) * 10;
            }
            else
            {
                Vector2 dir = Vector2.Zero;
                if (!reachedMouse)
                {
                    dir = Main.MouseWorld - projectile.Center;
                    if (dir.Length() < 30)
                        reachedMouse = true;
                }
                else
                {
                    dir = Main.MouseWorld - player.Center;
                }
                projectile.velocity = Vector2.Normalize(dir) * 10;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            hitTargets.Add(target);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        private void ManageCaches()
        {
            if (cache == null)
            {
                cache = new List<Vector2>();

                for (int i = 0; i < 150; i++)
                {
                    cache.Add(projectile.Center);
                }
            }

            cache.Add(projectile.Center);

            while (cache.Count > 150)
            {
                cache.RemoveAt(0);
            }

            cache2 = new List<Vector2>();
            for (int i = 0; i < 150; i++)
            {
                Vector2 point = cache[i];
                Vector2 nextPoint = i == 149 ? projectile.Center + projectile.velocity : cache[i + 1];
                Vector2 dir = Vector2.Normalize(nextPoint - point).RotatedBy(Main.rand.NextBool() ? -1.57f : 1.57f);
                cache2.Add(point + (dir * Main.rand.NextFloat(5) * (float)Math.Sqrt(chargeSqrt)));
            }
        }

        private void ManageTrails()
        {
            trail = trail ?? new Trail(Main.instance.GraphicsDevice, 150, new TriangularTip(40 * 4), factor => 4 * (float)Math.Pow(chargeSqrt, 0.7f), factor =>
            {
                if (factor.X > 0.99f)
                    return Color.Transparent;

                return new Color(160, 220, 255) * (projectile.extraUpdates == 0 ? EaseFunction.EaseCubicOut.Ease(projectile.timeLeft / 25f) : 1) * 0.05f;
            });

            trail.Positions = cache.ToArray();
            trail.NextPosition = projectile.Center + projectile.velocity;
        }
        public void DrawPrimitives()
        {
            Effect effect = Filters.Scene["LightningTrail"].GetShader().Shader;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.05f);
            effect.Parameters["repeats"].SetValue(1f);
            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.GetTexture("StarlightRiver/Assets/GlowTrail"));

            trail?.Render(effect);
        }

        public void DrawAdditive(SpriteBatch sb)
        {
            var point1 = startPoint;
            var point2 = startPoint + new Vector2(1, 1);

            if (point1 == Vector2.Zero || point2 == Vector2.Zero)
                return;

            var tex = ModContent.GetTexture("StarlightRiver/Assets/GlowTrail");

            for (int k = 1; k < cache2.Count; k++)
            {
                Vector2 prevPos = k == 1 ? point1 : cache2[k - 1];

                var target = new Rectangle((int)(prevPos.X - Main.screenPosition.X), (int)(prevPos.Y - Main.screenPosition.Y), (int)Vector2.Distance(cache2[k], prevPos) + 1, power);
                var origin = new Vector2(0, tex.Height / 2);
                var rot = (cache2[k] - prevPos).ToRotation();
                var color = new Color(200, 230, 255) * (projectile.extraUpdates == 0 ? EaseFunction.EaseCubicOut.Ease(projectile.timeLeft / 25f) : 1);

                sb.Draw(tex, target, null, color, rot, origin, 0, 0);

                if (Main.rand.Next(20) == 0)
                    Dust.NewDustPerfect(prevPos + new Vector2(0, 30), ModContent.DustType<Dusts.GlowLine>(), Vector2.Normalize(cache2[k] - prevPos) * Main.rand.NextFloat(-3, -2), 0, new Color(100, 150, 200) * (power / 30f), 0.5f);
            }
        }
    }
}