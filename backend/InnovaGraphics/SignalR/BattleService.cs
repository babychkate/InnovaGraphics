using InnovaGraphics.Dtos;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class BattleService
{
    private ConcurrentDictionary<string, string> _userConnections = new();
    private ConcurrentDictionary<string, DateTime> _activeInvitations = new();
    private ConcurrentDictionary<string, BattleState> _activeBattles = new();
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _invitationLocks = new();
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _battleLocks = new();
    private readonly ConcurrentDictionary<string, UserGetDto> _userStore;
    private readonly ILogger<BattleService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public BattleService(IServiceScopeFactory scopeFactory, ILogger<BattleService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    private async Task ExecuteWithLockAsync(string key, ConcurrentDictionary<string, SemaphoreSlim> lockDictionary, Func<Task> action)
    {
        var semaphore = lockDictionary.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();
        try
        {
            await action();
        }
        finally
        {
            semaphore.Release();
        }
    }

    public async Task SendInvitationToBattle(string targetUser, string inviterUser, Guid testId, IHubContext<BattleHub> hubContext)
    {
        if (targetUser.Equals(inviterUser, StringComparison.OrdinalIgnoreCase))
        {
            if (_userConnections.TryGetValue(inviterUser, out string inviterConnectionId))
            {
                try
                {
                    await hubContext.Clients.Client(inviterConnectionId).SendAsync("SelfInvitationError", targetUser);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Помилка при надсиланні SelfInvitationError користувачу {InviterUser}", inviterUser);
                }
            }
            return;
        }

        if (_userConnections.TryGetValue(targetUser, out string targetConnectionId))
        {
            string invitationKey = GetInvitationKey(inviterUser, targetUser);
            _activeInvitations[invitationKey] = DateTime.UtcNow;

            try
            {
                await hubContext.Clients.Client(targetConnectionId).SendAsync("ReceiveInvitation", inviterUser, testId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не вдалося надіслати ReceiveInvitation від {InviterUser} до {TargetUser}", inviterUser, targetUser);
            }

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(1));

                await ExecuteWithLockAsync(invitationKey, _invitationLocks, async () =>
                {
                    if (_activeInvitations.TryGetValue(invitationKey, out var inviteTime))
                    {
                        if (DateTime.UtcNow - inviteTime >= TimeSpan.FromMinutes(1))
                        {
                            _activeInvitations.TryRemove(invitationKey, out _);

                            if (_userConnections.TryGetValue(inviterUser, out string inviterConnectionId))
                            {
                                try
                                {
                                    await hubContext.Clients.Client(inviterConnectionId)
                                        .SendAsync("InvitationTimedOut", targetUser);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Помилка при надсиланні InvitationTimedOut користувачу {InviterUser}", inviterUser);
                                }
                            }
                        }
                    }
                });
            });
        }
        else
        {
            if (_userConnections.TryGetValue(inviterUser, out string inviterConnectionId))
            {
                try
                {
                    await hubContext.Clients.Client(inviterConnectionId).SendAsync("UserNotFound", targetUser);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не вдалося надіслати UserNotFound користувачу {InviterUser}", inviterUser);
                }
            }
        }
    }

    public void AddUserConnection(string userId, string connectionId)
    {
        _userConnections.AddOrUpdate(userId, connectionId, (key, oldValue) => connectionId);
    }

    public void RemoveUserConnection(string userId)
    {
        _userConnections.TryRemove(userId, out _);
    }

    public async Task HandleInvitationResponse(string currentUser, string inviterUser, bool accepted, IHubContext<BattleHub> hubContext)
    {
        var invitationKey = GetInvitationKey(inviterUser, currentUser);

        await ExecuteWithLockAsync(invitationKey, _invitationLocks, async () =>
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                if (_activeInvitations.TryRemove(invitationKey, out _))
                {
                    if (accepted)
                    {
                        if (_userConnections.TryGetValue(inviterUser, out var inviterConnectionId) &&
                            _userConnections.TryGetValue(currentUser, out var currentUserConnectionId))
                        {
                            await hubContext.Clients.Client(inviterConnectionId)
                                .SendAsync("InvitationAccepted", currentUser);

                            string battleId = Guid.NewGuid().ToString();
                            _activeBattles.TryAdd(battleId, new BattleState
                            {
                                User1 = inviterUser,
                                User2 = currentUser,
                                Status = BattleStatus.InProgress
                            });

                            await hubContext.Clients.Client(inviterConnectionId).SendAsync("StartBattle", battleId);
                            await hubContext.Clients.Client(currentUserConnectionId).SendAsync("StartBattle", battleId);
                        }
                        else
                        {
                            if (_userConnections.TryGetValue(inviterUser, out var existingInviterConnectionId))
                            {
                                await hubContext.Clients.Client(existingInviterConnectionId)
                                    .SendAsync("UserNotFound", currentUser);
                            }
                        }
                    }
                    else
                    {
                        if (_userConnections.TryGetValue(inviterUser, out var inviterConnectionId))
                        {
                            await hubContext.Clients.Client(inviterConnectionId)
                                .SendAsync("InvitationDeclined", currentUser);
                        }
                    }
                }
            }
        });
    }

    private string GetInvitationKey(string user1, string user2)
    {
        return string.Compare(user1, user2, StringComparison.OrdinalIgnoreCase) < 0
            ? $"{user1}:{user2}"
            : $"{user2}:{user1}";
    }

    public string GetConnectionId(string userId)
    {
        _userConnections.TryGetValue(userId, out var connectionId);
        return connectionId;
    }

    public string GetUserId(string connectionId)
    {
        return _userConnections.FirstOrDefault(kvp => kvp.Value == connectionId).Key;
    }

    public async Task<IEnumerable<UserGetDto>> GetActiveUsersAsync()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var activeUserIds = _userConnections.Keys.ToList();

            if (!activeUserIds.Any())
                return new List<UserGetDto>();

            var users = await userRepository.GetUsersByIdsAsync(activeUserIds);

            var result = users.Select(user =>
            {
                ProfileDto profileDto = null;
                if (user.Profile != null)
                {
                    profileDto = new ProfileDto
                    {
                        Id = user.Profile.Id,
                        AvatarId = user.Profile.AvatarId,
                    };
                }
                return new UserGetDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Profile = profileDto
                };
            }).ToList();

            _logger.LogInformation("Result users: {Users}", string.Join(", ", result.Select(u => $"{u.Id} - {u.UserName} - {u.Email} - ProfileId: {u.Profile?.Id} - AvatarId: {u.Profile?.AvatarId}")));

            return result;
        }
    }
    
    public Task ReportTestCompleted(string userId, string battleId, double completionTime, IHubContext<BattleHub> hubContext)
    {
        return ExecuteWithLockAsync(battleId, _battleLocks, async () =>
        {
            try
            {
                if (_activeBattles.TryGetValue(battleId, out var battleState))
                {
                    if (battleState.User1 == userId)
                    {
                        battleState.User1CompletionTime = completionTime;
                        battleState.User1Completed = true;
                    }
                    else if (battleState.User2 == userId)
                    {
                        battleState.User2CompletionTime = completionTime;
                        battleState.User2Completed = true;
                    }

                    string user1ConnectionId = _userConnections.GetValueOrDefault(battleState.User1);
                    string user2ConnectionId = _userConnections.GetValueOrDefault(battleState.User2);

                    if (!string.IsNullOrEmpty(user1ConnectionId) && !string.IsNullOrEmpty(user2ConnectionId))
                    {
                        await hubContext.Clients.Clients(user1ConnectionId, user2ConnectionId)
                            .SendAsync("showResult", battleState);

                        if (battleState.User1Completed && !battleState.User2Completed)
                        {
                            await hubContext.Clients.Client(user2ConnectionId).SendAsync("PauseTest");
                        }
                        else if (!battleState.User1Completed && battleState.User2Completed)
                        {
                            await hubContext.Clients.Client(user1ConnectionId).SendAsync("PauseTest");
                        }
                        else if (battleState.User1Completed && battleState.User2Completed)
                        {
                            _activeBattles.TryRemove(battleId, out _);
                            _invitationLocks.TryRemove(battleId, out _);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReportTestCompleted for battle {BattleId}", battleId);
            }
        });
    }
    
    public async Task<IEnumerable<object>> GetLeaderboardAsync(int topCount)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var topUsers = await userRepository.GetTopUsersByMarkCountAsync(topCount);
            return topUsers.Select(user => new
            {
                Nickname = user.UserName,
                Score = user.MarkCount,
                Level = GetUserLevel(user.MarkCount)
            });
        }
    }

    private string GetUserLevel(int markCount)
    {
        if (markCount >= 140) return "Високий";
        if (markCount >= 80) return "Середній";
        return "Низький";
    }
}
