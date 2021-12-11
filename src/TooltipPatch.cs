using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.API.Config;

namespace WorkableTemp {
    [HarmonyPatch(typeof(CollectibleObject), "GetHeldItemInfo")]
    class TooltipPatch {
        static void Postfix(ItemSlot inSlot, [HarmonyArgument(1, "dsc")] StringBuilder dsc, IWorldAccessor world, bool withDebugInfo) {
            ItemStack stack = inSlot.Itemstack;
            if (typeof(IAnvilWorkable).IsAssignableFrom(stack.Collectible.GetType())) {
                float temperature = stack.Collectible.GetTemperature(world, stack);
                float meltingpoint = stack.Collectible.GetMeltingPoint(world, null, inSlot);
                float workableTemp;
                if (stack.Collectible.Attributes?["workableTemperature"].Exists == true) {
                    workableTemp = stack.Collectible.Attributes["workableTemperature"].AsFloat(meltingpoint / 2);
                } else {
                    workableTemp = meltingpoint / 2;
                }
                
                if ((int)workableTemp == 0) {
                    dsc.AppendLine(Lang.Get("workabletemp:always-workable"));
                } else {
                    dsc.AppendLine(Lang.Get("workabletemp:workable-temp", (int)workableTemp));
                }

                if (temperature < workableTemp) {
                    dsc.AppendLine(Lang.Get("Too cold to work"));
                }
            }
        }    
    }
}