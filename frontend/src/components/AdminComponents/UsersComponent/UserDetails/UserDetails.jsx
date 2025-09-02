import { Button } from '@/components/ui/button';
import { ArrowLeft } from 'lucide-react';
import React, { useState } from 'react';

const UserDetails = ({ user, onBack }) => {
    if (!user) return null;

    const [activeTab, setActiveTab] = useState('tests'); // Триматиме, який блок активний

    const tests = [
        { id: 1, name: 'Тест з математики', theme: "Комп’ютерні моделі кольорів", coinResult: 100, starResult: 85 },
        { id: 2, name: 'Тест з математики', theme: "Комп’ютерні моделі кольорів", coinResult: 100, starResult: 85 },
        { id: 3, name: 'Тест з математики', theme: "Комп’ютерні моделі кольорів", coinResult: 100, starResult: 85 },
        { id: 4, name: 'Тест з математики', theme: "Комп’ютерні моделі кольорів", coinResult: 100, starResult: 85 },
    ];

    const tasks = [
        { id: 1, name: 'Завдання 1', theme: "Комп’ютерні моделі кольорів", coinResult: 100, starResult: 85 },
        { id: 2, name: 'Завдання 1', theme: "Комп’ютерні моделі кольорів", coinResult: 100, starResult: 85 },
        { id: 3, name: 'Завдання 1', theme: "Комп’ютерні моделі кольорів", coinResult: 100, starResult: 85 },
        { id: 4, name: 'Завдання 1', theme: "Комп’ютерні моделі кольорів", coinResult: 100, starResult: 85 },
    ];

    const battles = [
        { id: 1, name: 'Тест з математики', theme: "Комп’ютерні моделі кольорів", oponent: "Опонент", battleResult: "Перемога" },
        { id: 2, name: 'Тест з математики', theme: "Комп’ютерні моделі кольорів", oponent: "Опонент", battleResult: "Перемога" },
        { id: 3, name: 'Тест з математики', theme: "Комп’ютерні моделі кольорів", oponent: "Опонент", battleResult: "Перемога" },
        { id: 4, name: 'Тест з математики', theme: "Комп’ютерні моделі кольорів", oponent: "Опонент", battleResult: "Поразка" },
    ];

    const handleTabClick = (tab) => {
        setActiveTab(tab);
    };

    return (
        <div className="relative flex flex-col gap-6 py-10 px-10">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute -top-3 -left-3 z-20 p-2 rounded-full cursor-pointer transition-all duration-300 ease-in-out hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>
            <div className="flex items-center gap-10">
                <div className="flex-shrink-0">
                    <img src={user.avatar} alt={user.nickname} className="w-32 h-32 rounded-full object-cover" />
                </div>

                <div className="flex flex-col items-start gap-4">
                    <h2 className="text-2xl font-bold">Ім’я користувача: {user.nickname}</h2>
                    <p className="text-gray-600">Група: {user.group || "Група не вказана"}</p>
                    <p className="text-gray-600 font-medium">Рівень: {user.level}</p>
                </div>

                <div className="flex flex-col gap-4 text-sm text-gray-700">
                    <div className="flex items-center gap-2">
                        <img src="/energy.png" alt="Energy" className="w-5 h-5" />
                        <span>{user.energy}</span>
                    </div>
                    <div className="flex items-center gap-2">
                        <img src="/coin.png" alt="Coin" className="w-5 h-5" />
                        <span>{user.coin}</span>
                    </div>
                    <div className="flex items-center gap-2">
                        <img src="/star.png" alt="Points" className="w-5 h-5" />
                        <span>{user.points}</span>
                    </div>
                </div>
            </div>

            <div className="flex gap-4">
                <Button
                    variant="ghost"
                    onClick={() => handleTabClick('tests')}
                    className={`flex-1 py-2 text-xl text-center text-black ${activeTab === 'tests' ? 'font-bold' : ''}`}
                >
                    Пройдені тести
                </Button>
                <Button
                    variant="ghost"
                    onClick={() => handleTabClick('tasks')}
                    className={`flex-1 py-2 text-xl text-center text-black ${activeTab === 'tasks' ? 'font-bold' : ''}`}
                >
                    Практичні завдання
                </Button>
                <Button
                    variant="ghost"
                    onClick={() => handleTabClick('battles')}
                    className={`flex-1 py-2 text-xl text-center text-black ${activeTab === 'battles' ? 'font-bold' : ''}`}
                >
                    Батли
                </Button>
            </div>

            <div className="grid grid-cols-1 gap-4">
                {activeTab === 'tests' && (
                    <div>
                        <div className="flex flex-col gap-2">
                            {tests.map(test => (
                                <div key={test.id} className="flex items-center justify-between bg-[#85A7FA] rounded-4xl p-4">
                                    <div>
                                        <h2 className="text-lg font-semibold">{test.name}</h2>
                                        <p className="text-sm text-gray-600">{test.theme}</p>
                                    </div>
                                    <div className="flex justify-between gap-4 px-4 ">
                                        <div className="flex items-center space-x-1">
                                            <img src="/star.png" alt="Star" className="w-5 h-5" />
                                            <span className="text-black text-sm">{test.starResult}</span>
                                        </div>
                                        <div className="flex items-center space-x-1">
                                            <img src="/coin.png" alt="Coin" className="w-5 h-5" />
                                            <span className="text-black text-sm">{test.coinResult}</span>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                )}

                {activeTab === 'tasks' && (
                    <div>
                        <div className="flex flex-col gap-2">
                            {tasks.map(task => (
                                <div key={task.id} className="flex items-center justify-between bg-[#85A7FA] rounded-4xl p-4">
                                    <div>
                                        <h2 className="text-lg font-semibold">{task.name}</h2>
                                        <p className="text-sm text-gray-600">{task.theme}</p>
                                    </div>
                                    <div className="flex justify-between gap-4 px-4 ">
                                        <div className="flex items-center space-x-1">
                                            <img src="/star.png" alt="Star" className="w-5 h-5" />
                                            <span className="text-black text-sm">{task.starResult}</span>
                                        </div>
                                        <div className="flex items-center space-x-1">
                                            <img src="/coin.png" alt="Coin" className="w-5 h-5" />
                                            <span className="text-black text-sm">{task.coinResult}</span>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                )}

                {activeTab === 'battles' && (
                    <div>
                        <div className="flex flex-col gap-2">
                            <div className="flex justify-between px-4">
                                <div className="flex-1"></div>
                                <div className="flex-1 text-center text-xl font-bold">Опонент</div>
                                <div className="flex-1 text-center text-xl font-bold">Результат</div>
                            </div>

                            {battles.map(battle => (
                                <div key={battle.id} className="flex items-center justify-between bg-[#85A7FA] rounded-4xl p-4">
                                    <div className="flex-1">
                                        <h2 className="text-lg font-semibold">{battle.name}</h2>
                                        <p className="text-sm text-gray-600">{battle.theme}</p>
                                    </div>
                                    <div className="flex-1 text-center">{battle.oponent}</div>
                                    <div className="flex-1 text-center">{battle.battleResult}</div>
                                </div>
                            ))}
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default UserDetails;