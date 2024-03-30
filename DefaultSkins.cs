using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Commands;

namespace DefaultSkins;
public class DefaultSkins : BasePlugin
{
    public override string ModuleName => "CMod Default Skins";
    public override string ModuleAuthor => "Christopher Teljstedt @ Challengermode";
    public override string ModuleVersion => "0.0.3";


    public static readonly string ModelPathCtmHeavy = "characters\\models\\ctm_heavy\\ctm_heavy.vmdl";
    public static readonly string ModelPathCtmSas = "characters\\models\\ctm_sas\\ctm_sas.vmdl";
    public static readonly string ModelPathTmHeavy = "characters\\models\\tm_phoenix_heavy\\tm_phoenix_heavy.vmdl";
    public static readonly string ModelPathTmPhoenix = "characters\\models\\tm_phoenix\\tm_phoenix.vmdl";

    public bool EnableDefaultSkins { get; set; } = false;
    public bool HeavySkins { get; set; } = false;
    
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
        Console.WriteLine($"{ModuleName} version {ModuleVersion} by {ModuleAuthor} is loaded.");
    }
    
    [ConsoleCommand("cm_default_skins", "Enable / disable default skins mod")]
    public void OnEnableDefaultSkins(CCSPlayerController? player, CommandInfo command)
    {
        try
        {
            if (command.ArgCount < 2)
            {
                Console.WriteLine("Missing argument for cm_default_skins");
                return;
            }
            if (!int.TryParse(command.GetArg(1), out int enable))
            {
                Console.WriteLine("Invalid argument for cm_default_skins: {0}", command.GetArg(1));
                return;
            }
            EnableDefaultSkins = Convert.ToBoolean(enable);
            Console.WriteLine("version: {0}, enabled: {1}", ModuleVersion, EnableDefaultSkins);           
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error processing rcon command", ex.Message);
        }
    }
    
    [ConsoleCommand("cm_heavy_models", "Enable / disable heavy model skins")]
    public void OnHeavyModels(CCSPlayerController? player, CommandInfo command)
    {
        try
        {
            if (command.ArgCount < 2)
            {
                Console.WriteLine("Missing argument for cm_heavy_models");
                return;
            }
            if (!int.TryParse(command.GetArg(1), out int enable))
            {
                Console.WriteLine("Invalid argument for cm_heavy_models: {0}", command.GetArg(1));
                return;
            }
            HeavySkins = Convert.ToBoolean(enable);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error processing rcon command", ex.Message);
        }
    }
    
    [GameEventHandler]
    public HookResult OnPlayerSpawnEvent(EventPlayerSpawn @event, GameEventInfo info)
    {
        if(@event == null || !EnableDefaultSkins)
        {
            return HookResult.Continue;
        }

        CCSPlayerController player = @event.Userid;

        if (player == null
            || !player.IsValid
            || player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || !player.PlayerPawn.Value.IsValid)
        {
            return HookResult.Continue;
        }

        try
        {
            // TODO: Server crash if player connects, mp_swapteams and reconnect       
            CsTeam team = player.PendingTeamNum != player.TeamNum ? (CsTeam)player.PendingTeamNum : (CsTeam)player.TeamNum;

            if ((CsTeam)player.TeamNum == CsTeam.CounterTerrorist)
            {
                SetModelNextServerFrame(player.PlayerPawn.Value, HeavySkins ? ModelPathCtmHeavy : ModelPathCtmSas);
            }
            if ((CsTeam)player.TeamNum == CsTeam.Terrorist)
            {
                SetModelNextServerFrame(player.PlayerPawn.Value, HeavySkins ? ModelPathTmHeavy : ModelPathCtmSas);
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
