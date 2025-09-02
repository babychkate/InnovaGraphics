import { Input } from '@/components/ui/input';
import { ArrowLeft, Search } from 'lucide-react';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import React, { useEffect, useState } from 'react';
import { Button } from '@/components/ui/button';
import { useDispatch, useSelector } from 'react-redux';
import { getAllTests, setSelectedTest } from '@/redux/test/Action';

const formatTime = (timeStr) => {
    if (!timeStr) return '0 хв';

    const [hours, minutes, seconds] = timeStr.split(':').map(Number);

    const parts = [];
    if (hours > 0) parts.push(`${hours} ${hours === 1 ? 'година' : hours < 5 ? 'години' : 'годин'}`);
    if (minutes > 0) parts.push(`${minutes} ${minutes === 1 ? 'хвилина' : minutes < 5 ? 'хвилини' : 'хвилин'}`);
    if (hours === 0 && minutes === 0) parts.push(`${seconds} с`);

    return parts.join(' ');
};

const TestsListComponent = ({ onSelectOpponent }) => {
    const dispatch = useDispatch();
    const tests = useSelector(state => state.test.tests);
    const testsForBattle = tests.filter(test => test.planetId === null);
    // const [selectedTest, setSelectedTest] = useState(null);

    const selectedTest = useSelector(state => state.test.selectedTest);

    useEffect(() => {
        dispatch(getAllTests());
    }, [dispatch]);

    return (
        <>
            {!selectedTest && (
                <div className='min-h-[calc(100vh-121px)] bg-[#FFB57D] flex flex-col gap-4 py-8 px-4'>
                    <h1 className='text-2xl font-semibold text-center'>Оберіть тест та випробуйте свої сили</h1>
                    <div className='flex items-center gap-4'>
                        <div className='relative w-full max-w-sm'>
                            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500 w-5 h-5" />
                            <Input className="pl-10 bg-white placeholder-gray-500" placeholder="Шукати за назвою" />
                        </div>
                        <div>
                            <Select>
                                <SelectTrigger className="cursor-pointer bg-white w-[250px]">
                                    <SelectValue placeholder="Фільтрувати за темою" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectGroup>
                                        <SelectItem value="theme-1">Тема 1</SelectItem>
                                        <SelectItem value="theme-2">Тема 2</SelectItem>
                                        <SelectItem value="theme-3">Тема 3</SelectItem>
                                        <SelectItem value="theme-4">Тема 4</SelectItem>
                                        <SelectItem value="theme-5">Тема 5</SelectItem>
                                    </SelectGroup>
                                </SelectContent>
                            </Select>
                        </div>
                    </div>

                    {testsForBattle.map((test) => (
                        <div
                            key={test.id}
                            onClick={() => dispatch(setSelectedTest(test))}
                            className="w-full bg-[#FFDABE] rounded-4xl shadow-md flex items-center justify-between px-6 py-4 gap-4 transition hover:scale-[1.01] cursor-pointer"
                        >
                            <div className="px-4 flex flex-col">
                                <h2 className="text-lg font-semibold">{test.name}</h2>
                                <p className="text-sm text-gray-600">{test.theme}</p>
                            </div>
                            <div className='px-10 flex justify-between gap-4'>
                                <div className=' text-black font-semibold flex gap-2'>
                                    {test?.winnerResult || 100}
                                    <img src="/coin.png" alt="Coin" className='w-6 h-6' />
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {selectedTest && (
                <div className='min-h-[calc(100vh-121px)] bg-[#FFDABE] flex flex-col gap-4 py-8 px-4'>
                    <Button
                        variant="ghost"
                        onClick={() => dispatch(setSelectedTest(null))}
                        className='absolute top-32 left-3 z-20 p-2 hover:bg-white/50 rounded-full transition-all duration-300 ease-in-out'
                    >
                        <ArrowLeft size={28} />
                    </Button>

                    <h1 className='text-center text-3xl font-bold'>{selectedTest.name}</h1>
                    <div className='flex flex-col px-10'>
                        <h2><span className='font-bold'>Tema: </span>{selectedTest.theme}</h2>
                        <div className='flex gap-2'><span span className='font-bold'>Бали:</span>
                            <div className=' text-black font-semibold flex gap-2'>
                                {selectedTest.winnerResult || 100}
                                <img src="/coin.png" alt="Coin" className='w-6 h-6' />
                            </div>
                        </div>
                        <div><span className='font-bold'>Опис:</span>
                            <div className='flex justify-between gap-4'>
                                <p className='2/3'>
                                    Час виконання: {formatTime(selectedTest.timeLimit)}
                                </p>
                                <img src="/task/task-1.png" alt="Image" className='w-120 h-75' />
                            </div>
                        </div>
                    </div>
                    <div className='flex justify-center'>
                        <Button
                            onClick={() => onSelectOpponent(selectedTest)}
                            className="text-sm px-10 py-2 bg-[#E2853E] hover:bg-[#FFB57D] cursor-pointer"
                        >
                            Обрати суперника
                        </Button>
                    </div>
                </div>
            )}
        </>
    );
}

export default TestsListComponent;