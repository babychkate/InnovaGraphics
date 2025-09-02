import React from 'react';
import { useSelector } from 'react-redux';

const defaultUsers = [
    { id: 1, avatar: "/user-avatar/user-avatar1.png", nickname: "Liam Carter", level: "Level 1", status: true, points: 120 },
    { id: 2, avatar: "/user-avatar/user-avatar2.png", nickname: "Emma Brooks", level: "Level 2", status: true, points: 230 },
    { id: 3, avatar: "/user-avatar/user-avatar3.png", nickname: "Noah Turner", level: "Level 3", status: false, points: 310 },
    { id: 4, avatar: "/user-avatar/user-avatar4.png", nickname: "Ava Morgan", level: "Level 2", status: false, points: 185 },
    { id: 5, avatar: "/user-avatar/user-avatar5.png", nickname: "James Bennett", level: "Level 4", status: true, points: 450 },
    { id: 6, avatar: "/user-avatar/user-avatar1.png", nickname: "Mia Rogers", level: "Level 1", status: true, points: 95 },
    { id: 7, avatar: "/user-avatar/user-avatar2.png", nickname: "Ethan Reed", level: "Level 2", status: true, points: 210 },
    { id: 8, avatar: "/user-avatar/user-avatar3.png", nickname: "Sophia Price", level: "Level 3", status: false, points: 340 },
    { id: 9, avatar: "/user-avatar/user-avatar4.png", nickname: "Logan Hayes", level: "Level 2", status: false, points: 160 },
    { id: 10, avatar: "/user-avatar/user-avatar5.png", nickname: "Isabella Ford", level: "Level 4", status: true, points: 470 },
];

const LidersTableComponent = () => {
    const sortedUsers = defaultUsers.sort((a, b) => b.points - a.points);

    return (
        <div className='min-h-[calc(100vh-121px)] bg-[#FFB57D] flex flex-col items-center gap-4 py-8'>
            <h1 className='text-2xl font-semibold text-center'>Leaderboard</h1>
            <div className='flex flex-col w-11/12 gap-4 max-w-5xl'>
                <div className="grid grid-cols-4 font-semibold text-center bg-[#ffcfac] p-2 rounded-xl">
                    <div>User</div>
                    <div>Level</div>
                    <div>Status</div>
                    <div>Points</div>
                </div>
                {sortedUsers.map((user) => (
                    <div key={user.id} className="grid grid-cols-4 items-center gap-4 p-4 bg-[#FFDABE] rounded-4xl">
                        <div className="flex items-center gap-4">
                            <img
                                src={user.avatar}
                                alt={user.nickname}
                                className="w-14 h-14 rounded-full object-cover"
                            />
                            <div className="text-lg font-medium">{user.nickname}</div>
                        </div>
                        <div className="text-gray-600 text-center">{user.level}</div>
                        <div className="text-gray-600 text-center">{user.status ? "Online" : "Offline"}</div>
                        <div className="text-gray-800 text-center font-semibold">{user.points}</div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default LidersTableComponent;