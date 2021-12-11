using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using HarmonyLib;

[assembly: ModInfo( "Workable Temperature",
	Description = "Show workable temperature for all workable items",
	Website     = "https://github.com/ThePuzzlemaker/WorkableTemp",
	Authors     = new []{ "ThePuzzlemaker" } )]

namespace WorkableTemp
{
    // TODO: maybe forge?
	public class WorkableTempMod : ModSystem
	{
		public override bool AllowRuntimeReload {
			get { return true; }
		}

		public override void StartClientSide(ICoreClientAPI api) {
			var harmony = new Harmony("workabletemp");
			var original = typeof(CollectibleObject).GetMethod("GetHeldItemInfo");
			var patches = Harmony.GetPatchInfo(original);
			if (patches != null && patches.Owners.Contains("workabletemp")) {
				return;
			}
			harmony.PatchAll();
		}

		public override void Dispose() {
			var harmony = new Harmony("workabletemp");
			harmony.UnpatchAll("workabletemp");
		}

		public override bool ShouldLoad(EnumAppSide side) {
			return side == EnumAppSide.Client;
		}
	}
}
