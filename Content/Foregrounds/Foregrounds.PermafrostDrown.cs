using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarlightRiver.Core;
using Terraria;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Foregrounds
{
    class PermafrostDrown : ParticleForeground
    {
        public override bool Visible => Main.LocalPlayer.GetModPlayer<BiomeHandler>().ZonePermafrost;

        public override void Draw(SpriteBatch spriteBatch, float opacity)
        {
            var effect = Terraria.Graphics.Effects.Filters.Scene["AuroraDrown"].GetShader().Shader;
            effect.Parameters["sampleTexture0"].SetValue(Main.screenTarget);
            effect.Parameters["sampleTexture1"].SetValue(ModContent.GetTexture("StarlightRiver/Assets/Noise/VoroniSolid"));
            effect.Parameters["sampleTexture2"].SetValue(ModContent.GetTexture("StarlightRiver/Assets/Noise/PerlinDense"));
            effect.Parameters["sampleTexture3"].SetValue(ModContent.GetTexture("StarlightRiver/Assets/Noise/CircularGradient"));
            effect.Parameters["time"].SetValue(Main.GameUpdateCount / 100f);
            effect.Parameters["air"].SetValue(0.65f - (Main.LocalPlayer.breath / (float)Main.LocalPlayer.breathMax) * 0.65f);

            spriteBatch.End();
            spriteBatch.Begin(default, default, default, default, default, effect, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);

            spriteBatch.End();
            spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
