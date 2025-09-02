using Microsoft.AspNetCore.SignalR;

public class BattleHub : Hub
{
    private readonly BattleService _battleService;
    private readonly IHubContext<BattleHub> _hubContext;
    private readonly ILogger<BattleHub> _logger;

    public BattleHub(BattleService battleService, IHubContext<BattleHub> hubContext, ILogger<BattleHub> logger)
    {
        _battleService = battleService;
        _hubContext = hubContext;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        if (!string.IsNullOrEmpty(userId))
        {
            Context.Items["UserId"] = userId;

            _battleService.AddUserConnection(userId, Context.ConnectionId);
            await Clients.Others.SendAsync("UserConnected", userId);
            await SendActiveUsersToCaller();
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.Items["UserId"] as string;
        if (!string.IsNullOrEmpty(userId))
        {
            _battleService.RemoveUserConnection(userId);
            await Clients.Others.SendAsync("UserDisconnected", userId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendInvitationToBattle(string targetUser, string inviterUser, Guid testId)
    {
        await _battleService.SendInvitationToBattle(targetUser, inviterUser, testId, _hubContext);
    }

    public async Task AcceptBattleRequest(string currentUser, string inviterUser)
    {
        await _battleService.HandleInvitationResponse(currentUser, inviterUser, accepted: true, _hubContext);
    }

    public async Task DeclineBattleRequest(string currentUser, string inviterUser)
    {
        await _battleService.HandleInvitationResponse(currentUser, inviterUser, accepted: false, _hubContext);
    }

    public async Task ReportTestCompleted(string battleId, double completionTime)
    {
        var userId = Context.Items["UserId"] as string;
        if (!string.IsNullOrEmpty(userId))
        {
            await _battleService.ReportTestCompleted(userId, battleId, completionTime, _hubContext);
        }
    }
    
    public async Task<IEnumerable<object>> GetActiveUsers()
    {
        return await _battleService.GetActiveUsersAsync();
    }

    private async Task SendActiveUsersToCaller()
    {
        var activeUsers = await _battleService.GetActiveUsersAsync();
        await Clients.All.SendAsync("ActiveUsersUpdated", activeUsers);
    }

    public async Task RequestLeaderboard()
    {
        var leaderboardData = await _battleService.GetLeaderboardAsync(20);
        await Clients.All.SendAsync("ReceiveLeaderboard", leaderboardData);
    }
}
