using VampireCommandFramework;

namespace APVRising.Commands;

[CommandGroup("archipelago", "ap")]
public class ArchipelagoCommands
{
    [Command("connect", shortHand: "c", description: "Connect to Archipelago", adminOnly: false)]
    public static void APConnect(ICommandContext ctx, string slotName = "Player1", string uri = "archipelago.gg:38281", string password = "")
    {
        Archipelago.ArchipelagoClient.ServerData.Uri = uri;
        Archipelago.ArchipelagoClient.ServerData.Password = password;
        Archipelago.ArchipelagoClient.ServerData.SlotName = slotName;
        Archipelago.ArchipelagoClient.Instance.Connect();
    }

    [Command("deathlink", shortHand: "dl", description: "Toggle Death Link", adminOnly: false)]
    public static void APDeathLinkToggle(ICommandContext ctx, bool? value = null)
    {
        if (value != null)
        {
            Archipelago.DeathLinkHandler.deathLinkEnabled = value.Value;
        }
        ctx.Reply($"[Archipelago] Death link is {(Archipelago.DeathLinkHandler.deathLinkEnabled ? "on" : "off")}");
    }
}