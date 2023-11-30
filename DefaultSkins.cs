using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;

namespace DefaultSkins;
public class DefaultSkins : BasePlugin
{
    public override string ModuleName => "CMod Default Skins";
    public override string ModuleAuthor => "Christopher Teljstedt @ Challengermode";
    public override string ModuleVersion => "0.0.2";


    public static readonly string ModelPathCtmHeavy = "characters\\models\\ctm_heavy\\ctm_heavy.vmdl";
    public static readonly string ModelPathCtmSas = "characters\\models\\ctm_sas\\ctm_sas.vmdl";
    public static readonly string ModelPathTmHeavy = "characters\\models\\tm_heavy\\tm_phoenix_heavy.vmdl";
    public static readonly string ModelPathTmPhoenix = "characters\\models\\tm_phoenix\\tm_phoenix.vmdl";


    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnMapStart>(map =>
        {
            Server.PrecacheModel(ModelPathCtmHeavy);
            Server.PrecacheModel(ModelPathCtmSas);
            Server.PrecacheModel(ModelPathTmHeavy);
            Server.PrecacheModel(ModelPathTmPhoenix);
        });
        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawnEvent);

        RegisterListener<Listeners.OnMapEnd>(() => Unload(true));
        Console.WriteLine($"{ModuleName} version {ModuleVersion} by {ModuleAuthor} is active.");
    }

    [GameEventHandler]
    public HookResult OnPlayerSpawnEvent(EventPlayerSpawn @event, GameEventInfo info)
    {
        if(@event == null)
        {
            return HookResult.Continue;
        }

        CCSPlayerController player = @event.Userid;

        if (player == null
            || !player.IsValid
            || player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || player.PlayerPawn.Value.Handle == 0)
        {
            return HookResult.Continue;
        }

        try
        {
            if ((CsTeam)player.TeamNum == CsTeam.CounterTerrorist)
            {
                SetModelNextServerFrame(player.PlayerPawn.Value, ModelPathCtmSas);
            }
            if ((CsTeam)player.TeamNum == CsTeam.Terrorist)
            {
                SetModelNextServerFrame(player.PlayerPawn.Value, ModelPathTmPhoenix);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not set player model: {0}", ex);
        }
        
        return HookResult.Continue;
    }

    public static void SetModelNextServerFrame(CCSPlayerPawn playerPawn, string model)
    {
        Server.NextFrame(() =>
        {
            playerPawn.SetModel(model);
        });
    }
}