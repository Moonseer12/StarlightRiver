using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarlightRiver
{
    public class StarlightRiver : Mod
    {
    }

    public class SLRMenu : ModMenu
    {
        public override Asset<Texture2D> Logo => ModContent.GetTexture("StarlightRiver/amogus");

        public override int Music => MusicID.WindyDay;
    }
}
