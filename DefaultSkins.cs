using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API;

namespace DefaultSkins;
public class DefaultSkins : BasePlugin
{
    public override string ModuleName => "CMod Default Skins";
    public override string ModuleAuthor => "Christopher Teljstedt @ Challengermode";
    public override string ModuleVersion => "0.0.1";
    public override void Load(bool hotReload)
    {
        base.RegisterListener<Listeners.OnMapStart>(map =>
        {
            Server.PrecacheModel(ModelPathCtmHeavy);
            Server.PrecacheModel(ModelPathCtmSas);
            Server.PrecacheModel(ModelPathTmHeavy);
            Server.PrecacheModel(ModelPathTmPhoenix);
        });
        this.RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawnEvent);

        RegisterListener<Listeners.OnMapEnd>(() => Unload(true));
        //gameeventmanager->AddListener(&g_PlayerSpawnEvent, "player_spawn", true);
        //gameeventmanager->AddListener(&g_RoundPreStartEvent, "round_prestart", true);
        Console.WriteLine($"{ModuleName} version {ModuleVersion} by {ModuleAuthor} is active.");
    }

    public static readonly string ModelPathCtmHeavy = "characters\\models\\ctm_heavy\\ctm_heavy.vmdl";
    public static readonly string ModelPathCtmSas = "characters\\models\\ctm_sas\\ctm_sas.vmdl";
    public static readonly string ModelPathTmHeavy = "characters\\models\\tm_heavy\\tm_phoenix_heavy.vmdl";
    public static readonly string ModelPathTmPhoenix = "characters\\models\\tm_phoenix\\tm_phoenix.vmdl";


    [GameEventHandler]
    public HookResult OnPlayerSpawnEvent(EventPlayerSpawn @event, GameEventInfo info)
    {
        CCSPlayerController player = @event.Userid;

        if (player == null
            || !player.IsValid
            || player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || player.PlayerPawn.Value.Handle == 0)
            return HookResult.Continue;

        if ((CsTeam)player.TeamNum == CsTeam.CounterTerrorist)
        {
            SetModel(player.PlayerPawn.Value.Handle, ModelPathCtmSas);
        }
        if ((CsTeam)player.TeamNum == CsTeam.Terrorist)
        {
            SetModel(player.PlayerPawn.Value.Handle, ModelPathTmPhoenix);
        }
        return HookResult.Continue;
    }

    // TODO: use player.PlayerPawn.Value.SetModel when signature is updated.
    public static Action<nint, string> SetModel = VirtualFunction.CreateVoid<nint, string>("\\x55\\x48\\x89\\xF2\\x48\\x89\\xE5\\x41\\x54\\x49\\x89\\xFC\\x48\\x8D\\x2A\\x2A\\x48\\x83\\x2A\\x2A\\x2A\\x8D\\x05\\x2A\\x2A\\x2A\\x00\\x48\\x8B\\x30\\x48\\x8B\\x06\\xFF\\x2A\\x2A\\x48\\x8B\\x45\\x2A\\x48\\x8D\\x2A\\x2A\\x4C\\x89\\x2A\\x48\\x89\\x45\\x2A\\x2A\\x68\\xFC");
    public void SetPlayerModel(CCSPlayerController player, string model)
    {
        Server.NextFrame((() =>
        {
            SetModel(player.PlayerPawn.Value.Handle, model);
            //player.PlayerPawn.Value.SetModel(model);
        }));
    }
}