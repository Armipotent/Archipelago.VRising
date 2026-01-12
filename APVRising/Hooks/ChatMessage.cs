using APVRising.Archipelago;
using ProjectM.Network;
using ProjectM;
using Unity.Entities;
using Unity.Collections;
using HarmonyLib;

namespace APVRising.Hooks;

public static class ChatMessage
{
    // majority of this code adapted from VampireCommandFramework @ VCF.Core/Breadstone/ChatHook.cs
    [HarmonyPatch(typeof(ChatMessageSystem), nameof(ChatMessageSystem.OnUpdate))]
	public static void Prefix(ChatMessageSystem __instance)
	{
		if (__instance.__query_661171423_0 != null)
		{
			NativeArray<Entity> entities = __instance.__query_661171423_0.ToEntityArray(Allocator.Temp);
			foreach (var entity in entities)
			{
				// keeping this in case it's decided at some point that player names should be included in messages to AP
				// var fromData = __instance.EntityManager.GetComponentData<FromCharacter>(entity);
				// var userData = __instance.EntityManager.GetComponentData<User>(fromData.User);
				var chatEventData = __instance.EntityManager.GetComponentData<ChatMessageEvent>(entity);

				var messageText = chatEventData.MessageText.ToString();

				if (!(!messageText.StartsWith(".") || messageText.StartsWith(".."))) continue;

                if (ArchipelagoClient.Authenticated)
                    Plugin.ArchipelagoClient.SendMessage(messageText);
			}
		}
	}
}