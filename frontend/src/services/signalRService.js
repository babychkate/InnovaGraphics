import { baseUrl } from "@/config/constants";
import * as signalR from "@microsoft/signalr";

let connection = null;

export const connectToBattleHub = async (
    userId,
    onReceiveInvitation,
    onUserConnected,
    onUserDisconnected,
    onInvitationTimedOut,
    onActiveUsersUpdated,
    onInvitationAccepted,
    onBattleStart,
    onPauseTest,
    onShowResult,
) => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl(`${baseUrl}/battleHub?userId=${userId}`)
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveInvitation", (inviterUser, testId) => {
        console.log(`[SignalR] Запрошення: ${JSON.stringify(inviterUser)}`);
        onReceiveInvitation?.(inviterUser, testId);
    });

    connection.on("UserConnected", (userId) => {
        console.log("[SignalR] UserConnected:", userId);
        onUserConnected?.(userId);
    });

    connection.on("UserDisconnected", (userId) => {
        console.log("[SignalR] UserDisconnected:", userId);
        onUserDisconnected?.(userId);
    });

    connection.on("InvitationTimedOut", (targetUserId) => {
        console.log(`[SignalR] Час запрошення вичерпано. ${targetUserId}`);
        onInvitationTimedOut?.(targetUserId);
    });

    connection.on("InvitationAccepted", (accepterUser) => {
        console.log("[SignalR] Invitation accepted:", accepterUser);
        onInvitationAccepted?.(accepterUser);
    });

    connection.on("InvitationDeclined", (declinerUser, inviterUser) => {
        console.log(`[SignalR] Користувач ${declinerUser.fullName || declinerUser.id} відхилив запрошення від ${inviterUser.fullName || inviterUser.id}`);
    });

    connection.on("ActiveUsersUpdated", (activeUsers) => {
        console.log(`[SignalR] Отримано активних користувачів:`, activeUsers);
        onActiveUsersUpdated?.(activeUsers);
    });

    connection.on("StartBattle", (battleId) => {
        console.log("[SignalR] StartBattle отримано", battleId);
        onBattleStart?.(battleId);
    });

    connection.on("PauseTest", () => {
        console.log("[SignalR] Отримано PauseTest — тест призупинено");
        onPauseTest?.();
    });

    connection.on("showResult", (battleState) => {
        console.log("[SignalR] Отримано showResult:", battleState);
        onShowResult?.(battleState);
    });

    try {
        await connection.start();
        console.log("SignalR Connected");
    } catch (err) {
        console.error("SignalR Connection Error: ", err);
    }
};

export const sendInvitation = async (inviterId, targetId, testId) => {
    if (!connection) {
        console.error("SignalR connection is not established.");
        return;
    }
    try {
        await connection.invoke("SendInvitationToBattle", targetId, inviterId, testId);
        console.log(`[SignalR] Відправлено запрошення від ${inviterId} до ${targetId} з тестом ${testId}`);
    } catch (error) {
        console.error("Error sending invitation:", error);
    }
};

export const respondToInvitation = async (currentUserId, inviterUserId, accepted) => {
    if (!connection) {
        console.error("SignalR connection is not established.");
        return;
    }

    console.log(`[SignalR] Відповідь на запрошення від ${inviterUserId} користувачем ${currentUserId}: ${accepted ? "Прийнято" : "Відхилено"}`);
    try {
        if (accepted) {
            await connection.invoke("AcceptBattleRequest", currentUserId, inviterUserId);
            console.log(`Запрошення від ${inviterUserId} прийняте`);
        } else {
            await connection.invoke("DeclineBattleRequest", currentUserId, inviterUserId);
            console.log(`Запрошення від ${inviterUserId} відхилене`);
        }
    } catch (error) {
        console.error("Помилка відповіді на запрошення:", error);
    }
};

export const reportTestCompleted = async (battleId, competitionTime) => {
    if (!connection) {
        console.error("SignalR connection is not established.");
        return;
    }
    try {
        await connection.invoke("ReportTestCompleted", battleId, competitionTime);
        console.log(`[SignalR] Тест завершено для battleId: ${battleId}`);
    } catch (error) {
        console.error("Error reporting test completion:", error);
    }
}