import { Input } from '@/components/ui/input';
import { Select, SelectContent, SelectGroup, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Search } from 'lucide-react';
import React, { useState } from 'react';
import UserDetails from './UserDetails/UserDetails';

const users = [
    { id: 1, avatar: "/user-avatar/user-avatar1.png", nickname: "Нікнейм 1", level: 1, status: true, energy: 250, coin: 50, points: 1200 },
    { id: 2, avatar: "/user-avatar/user-avatar2.png", nickname: "Нікнейм 2", level: 2, status: true, energy: 250, coin: 30, points: 980 },
    { id: 3, avatar: "/user-avatar/user-avatar3.png", nickname: "Нікнейм 3", level: 3, status: false, energy: 250, coin: 70, points: 1400 },
    { id: 4, avatar: "/user-avatar/user-avatar4.png", nickname: "Нікнейм 4", level: 4, status: false, energy: 250, coin: 20, points: 860 },
    { id: 5, avatar: "/user-avatar/user-avatar5.png", nickname: "Нікнейм 5", level: 5, status: true, energy: 250, coin: 90, points: 2000 },
    { id: 6, avatar: "/user-avatar/user-avatar1.png", nickname: "Нікнейм 6", level: 1, status: true, energy: 250, coin: 15, points: 450 },
    { id: 7, avatar: "/user-avatar/user-avatar2.png", nickname: "Нікнейм 7", level: 2, status: true, energy: 250, coin: 35, points: 1000 },
    { id: 8, avatar: "/user-avatar/user-avatar3.png", nickname: "Нікнейм 8", level: 3, status: false, energy: 250, coin: 60, points: 1300 },
    { id: 9, avatar: "/user-avatar/user-avatar4.png", nickname: "Нікнейм 9", level: 3, status: false, energy: 250, coin: 25, points: 900 },
    { id: 10, avatar: "/user-avatar/user-avatar5.png", nickname: "Нікнейм 10", level: 4, status: true, energy: 250, coin: 85, points: 1950 },
];

const UsersComponent = () => {
    const [selectedUser, setSelectedUser] = useState(null);

    if (selectedUser) {
        return <UserDetails user={selectedUser} onBack={() => setSelectedUser(null)} />;
    }

    return (
        <div className='flex flex-col gap-4 p-6'>
            <div className='flex items-center gap-8'>
                <h1 className='text-2xl font-bold'>Користувачі</h1>
                <h2>усі</h2>
                <h2>лідери</h2>
            </div>
            <div className='flex items-center gap-4'>
                <div className='relative w-full max-w-sm'>
                    <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500 w-5 h-5" />
                    <Input className="pl-10 bg-white placeholder-gray-500" placeholder="Шукати за назвою" />
                </div>
                <div>
                    <Select>
                        <SelectTrigger className="cursor-pointer bg-white w-[350px]">
                            <SelectValue placeholder="Фільтрувати за темою" />
                        </SelectTrigger>
                        <SelectContent>
                            <SelectGroup>
                                <SelectItem value="nickname">Нікнейм</SelectItem>
                                <SelectItem value="level">Рівень</SelectItem>
                                <SelectItem value="points">Бали</SelectItem>
                            </SelectGroup>
                        </SelectContent>
                    </Select>
                </div>
            </div>
            <div className="flex flex-col gap-4 overflow-y-auto max-h-[540px] pr-2">
                <div className="grid grid-cols-4 items-center gap-4 text-xl">
                    <div className="text-center">Назва</div>
                    <div className="text-center">Рівень</div>
                    <div className="text-center">Статус</div>
                    <div className="text-center">Ресурси</div>
                </div>
                {users.map((user) => (
                    <div 
                        key={user.id} 
                        onClick={() => setSelectedUser(user)}
                        className="grid grid-cols-4 items-center gap-4 p-4 bg-[#85A7FA] rounded-4xl cursor-pointer transition hover:scale-[1.005]"
                    >
                        <div className="flex items-center gap-4">
                            <img
                                src={user.avatar}
                                alt={user.nickname}
                                className="w-14 h-14 rounded-full object-cover"
                            />
                            <div className="text-lg font-medium text-center">{user.nickname}</div>
                        </div>
                        <div className="text-center">{user.level}</div>
                        <div className="text-center">{user.status ? "Онлайн" : "Офлайн"}</div>
                        <div className="flex items-center justify-center gap-4">
                            <div className="flex items-center space-x-1">
                                <img src="/star.png" alt="Star" className="w-5 h-5" />
                                <span className="text-black text-sm">{user.points}</span>
                            </div>
                            <div className="flex items-center space-x-1">
                                <img src="/coin.png" alt="Coin" className="w-5 h-5" />
                                <span className="text-black text-sm">{user.coin}</span>
                            </div>
                            <div className="flex items-center space-x-1">
                                <img src="/energy.png" alt="Energy" className="w-5 h-5" />
                                <span className="text-black text-sm">{user.energy}</span>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default UsersComponent;