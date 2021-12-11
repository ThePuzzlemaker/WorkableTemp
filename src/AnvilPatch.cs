using System.Text;
using System;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.API.Config;

namespace WorkableTemp {
    [HarmonyPatch(typeof(BlockEntityAnvil), "GetBlockInfo")]
    class AnvilPatch {
        static void Postfix(BlockEntityAnvil __instance, IPlayer forPlayer, StringBuilder dsc) {
            if (__instance.WorkItemStack == null || __instance.SelectedRecipe == null) {
                return;
            }

            ItemStack stack = __instance.WorkItemStack;

            float meltingpoint = stack.Collectible.GetMeltingPoint(__instance.Api.World, null, new DummySlot(stack));
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
        }
    }
}