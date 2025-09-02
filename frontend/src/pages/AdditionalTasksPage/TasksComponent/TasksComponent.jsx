"use client";

import { Input } from '@/components/ui/input';
import { ArrowLeft, Heart, Search } from 'lucide-react';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import React, { useRef, useState } from 'react';
import { Button } from '@/components/ui/button';
import GetTaskResultComponent from './GetTaskResultComponent/GetTaskResultComponent';
import Editor from "@monaco-editor/react";

const tasks = [
    {
        id: 1,
        name: 'Додаткове завдання 1',
        theme: "Фрактальна графіка",
        level: "Легкий",
        description: "Lorem ipsum dolor sit amet...",
        image: "/task/task-1.png",
        isLicked: true,
        result: 100,
    },
    {
        id: 2,
        name: 'Додаткове завдання 2',
        theme: "Засоби комп’ютерної графіки",
        level: "Складний",
        description: "Lorem ipsum dolor sit amet...",
        image: "/task/task-1.png",
        isLicked: true,
        result: 100,
    },
    {
        id: 3,
        name: 'Додаткове завдання 3',
        theme: "Програмування рухомих зображень",
        level: "Середній",
        description: "Lorem ipsum dolor sit amet...",
        image: "/task/task-1.png",
        isLicked: false,
        result: 100,
    },
    {
        id: 4,
        name: 'Додаткове завдання 4',
        theme: "Фрактальна графіка",
        level: "Легкий",
        description: "Lorem ipsum dolor sit amet...",
        image: "/task/task-1.png",
        isLicked: true,
        result: 100,
    },
    {
        id: 5,
        name: 'Додаткове завдання 5',
        theme: "Засоби комп’ютерної графіки",
        level: "Складний",
        description: "Lorem ipsum dolor sit amet...",
        image: "/task/task-1.png",
        isLicked: true,
        result: 100,
    },
    {
        id: 6,
        name: 'Додаткове завдання 6',
        theme: "Програмування рухомих зображень",
        level: "Середній",
        description: "Lorem ipsum dolor sit amet...",
        image: "/task/task-1.png",
        isLicked: false,
        result: 100,
    },
];

const TasksComponent = () => {
    const [selectedTask, setSelectedTask] = useState(null);
    const [showTaskComponent, setShowTaskComponent] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [code, setCode] = useState("// ctx - це контекст малювання\n// canvas - це елемент canvas\n\nconst drawSquare = (canvas, ctx) => {\n    ctx.clearRect(0, 0, canvas.width, canvas.height);\n\n    const size = 100;\n    const x = canvas.width / 2 - size / 2;\n    const y = canvas.height / 2 - size / 2;\n\n    ctx.fillStyle = '#2354E1';\n    ctx.fillRect(x, y, size, size);\n}\n\ndrawSquare(canvas, ctx);");
    const canvasRef = useRef(null);

    const drawSquare = () => {
        const canvas = canvasRef.current;
        if (!canvas) return;

        const ctx = canvas.getContext("2d");
        if (!ctx) return;

        ctx.clearRect(0, 0, canvas.width, canvas.height);

        const size = 100;
        const x = canvas.width / 2 - size / 2;
        const y = canvas.height / 2 - size / 2;

        ctx.fillStyle = "#2354E1";
        ctx.fillRect(x, y, size, size);
    };

    return (
        <div className={`min-h-[calc(100vh-121px)] ${!selectedTask ? 'bg-[#85A7FA]' : 'bg-[#C2D3FD]'} flex flex-col gap-4 py-8`}>
            {!selectedTask && (
                <>
                    <h1 className='text-2xl font-semibold text-center'>
                        Оберіть завдання та випробуйте свої сили
                    </h1>
                    <div className='flex items-center px-4 gap-4'>
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

                    <div className='flex flex-col gap-4 px-4'>
                        {tasks.map((task) => (
                            <div
                                key={task.id}
                                className="cursor-pointer group"
                                onClick={() => setSelectedTask(task)}
                            >
                                <div className="relative flex items-start bg-[#C2D3FD] p-4 rounded-4xl shadow-md overflow-hidden transition hover:scale-[1.01]">
                                    <Heart
                                        className={`absolute top-4 right-4 w-5 h-5 ${task.isLicked ? "text-red-500" : "text-gray-400"}`}
                                    />
                                    <div className="px-4 flex justify-between w-full">
                                        <div>
                                            <h2 className="text-lg font-semibold">{task.name}</h2>
                                            <p className="text-sm text-gray-600">{task.theme}</p>
                                        </div>
                                        <div className='px-10 flex justify-between gap-4'>
                                            <p className="text-sm text-gray-700 line-clamp-3">
                                                {task.level}
                                            </p>
                                            <div className=' text-black font-semibold flex gap-2'>
                                                {task.result}
                                                <img src="/coin.png" alt="Coin" className='w-6 h-6' />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </>
            )}

            {selectedTask && !showTaskComponent && (
                <div>
                    <Button
                        variant="ghost"
                        onClick={() => setSelectedTask(null)}
                        className='absolute top-32 left-3 z-20 p-2 hover:bg-white/50 rounded-full transition-all duration-300 ease-in-out'
                    >
                        <ArrowLeft size={28} />
                    </Button>

                    <h1 className='text-center text-3xl font-bold'>{selectedTask.name}</h1>
                    <div className='flex flex-col px-10'>
                        <h2><span className='font-bold'>Tema: </span>{selectedTask.theme}</h2>
                        <div className='flex gap-2'><span className='font-bold'>Бали:</span>
                            <div className=' text-black font-semibold flex gap-2'>
                                {selectedTask.result}
                                <img src="/coin.png" alt="Coin" className='w-6 h-6' />
                            </div>
                        </div>
                        <h2><span className='font-bold'>Рівень: </span>{selectedTask.level}</h2>
                        <div><span className='font-bold'>Опис:</span>
                            <p>
                                Lorem ipsum dolor sit amet...
                            </p>
                            <div className='flex justify-between gap-4 mt-4'>
                                <img src={`${selectedTask.image}`} alt="Image" className='w-120 h-75' />
                                <p>
                                    Lorem ipsum dolor sit amet...
                                </p>
                            </div>
                            <div className='flex justify-end'>
                                <Button
                                    onClick={() => setShowTaskComponent(true)}
                                    className="text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer"
                                >
                                    Перейти до виконання
                                </Button>
                            </div>
                        </div>
                    </div>
                </div>
            )}

            {showTaskComponent && (
                <div>
                    <Button
                        variant="ghost"
                        onClick={() => setShowTaskComponent(false)}
                        className='absolute top-32 left-3 z-20 p-2 hover:bg-white/50 rounded-full transition-all duration-300 ease-in-out'
                    >
                        <ArrowLeft size={28} />
                    </Button>

                    <div className="flex justify-between gap-8 px-10 py-4 h-[80vh]">
                        <div className="w-1/2 bg-white rounded-2xl shadow-lg p-4 flex justify-center items-center">
                            <canvas ref={canvasRef} className="w-full h-full"></canvas>
                        </div>

                        <div className="w-1/2 flex flex-col">
                            <div className="flex-1 bg-white rounded-2xl shadow-lg p-4">
                                <Editor
                                    height="100%"
                                    defaultLanguage="javascript"
                                    defaultValue={code}
                                    onChange={(value) => setCode(value || '')}
                                    // theme="vs-dark"
                                />
                            </div>

                            <div className="flex flex-col items-end mt-4 gap-4">
                                <Button
                                    onClick={() => {
                                        try {
                                            const canvas = canvasRef.current;
                                            const ctx = canvas?.getContext('2d');  // Ініціалізація ctx
                                            const userCode = new Function("canvas", "ctx", code); // Передаємо canvas та ctx як параметри
                                            userCode(canvas, ctx);
                                        } catch (err) {
                                            console.error("Помилка під час виконання коду:", err);
                                            alert("Сталася помилка під час виконання коду. Перевірте свій код.");
                                        }
                                    }}
                                    className="text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer"
                                >
                                    Спробувати
                                </Button>
                                <Button
                                    className="text-sm px-10 py-2 cursor-pointer"
                                    onClick={() => setIsModalOpen(true)}
                                >
                                    Завершити завдання
                                </Button>
                            </div>
                        </div>
                    </div>
                </div>
            )}

            {isModalOpen && (
                <GetTaskResultComponent task={selectedTask} onClose={() => setIsModalOpen(false)} />
            )}
        </div>
    );
};

export default TasksComponent;