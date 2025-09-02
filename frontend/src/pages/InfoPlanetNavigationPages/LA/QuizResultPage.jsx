import PlanetBackground from '@/components/PlanetBackground/PlanetBackground';
import { Button } from '@/components/ui/button';
import UserAssistant from '@/components/UserAssistant/UserAssistant';
import React from 'react';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import {
    PieChart,
    Pie,
    Cell,
    ResponsiveContainer
} from 'recharts';

const QuizResultPage = () => {
    const { test } = useSelector((state) => state.test);

    if (!test) {
        return <div>Зачекайте, тест ще не завершено.</div>;
    }

    const { testResultDetails } = test;

    const correct = testResultDetails?.correctAnswers;
    const incorrect = testResultDetails?.incorrectAnswers;
    const unanswered = testResultDetails?.unanswered;

    const totalQuestions = correct + incorrect + unanswered;

    const data = [
        { name: 'Правильно', value: correct },
        { name: 'Неправильно', value: incorrect },
        { name: 'Без відповіді', value: unanswered },
    ];

    const COLORS = ['#15803d', '#b91c1c', '#eab308'];

    return (
        <div className='relative w-screen h-screen text-white'>
            <PlanetBackground />

            <div className="absolute inset-20 bg-[#D8E3FF] p-6 max-w-full max-h-full rounded-2xl mx-auto">
                <div className='mb-4'>
                    <Link to="/info-planet" className="text-2xl font-bold text-blue-400 underline">
                        Лінійна алгебра / Матриці
                    </Link>
                </div>

                <div className='w-full bg-white text-black p-6 text-xl rounded-xl flex flex-col items-center justify-center gap-4'>
                    <div>
                        <h1 className='text-3xl font-bold'>Результат</h1>
                    </div>
                    <div className='flex items-center justify-center gap-6'>
                        <div className='flex items-center justify-between gap-4'>
                            <div className='bg-green-700 rounded-full w-5 h-5'></div>
                            <h2>{correct} правильно</h2>
                        </div>
                        <div className='flex items-center justify-between gap-4'>
                            <div className='bg-yellow-300 rounded-full w-5 h-5'></div>
                            <h2>{unanswered} без відповіді</h2>
                        </div>
                        <div className='flex items-center justify-between gap-4'>
                            <div className='bg-red-700 rounded-full w-5 h-5'></div>
                            <h2>{incorrect} не правильно</h2>
                        </div>
                    </div>

                    {/* Діаграма */}
                    <div className="w-full flex items-center justify-center mt-12">
                        <div className="relative w-72 h-72">
                            <ResponsiveContainer width="100%" height="100%">
                                <PieChart>
                                    <Pie
                                        data={data}
                                        cx="50%"
                                        cy="50%"
                                        innerRadius={110}
                                        outerRadius={130}
                                        fill="#8884d8"
                                        dataKey="value"
                                        stroke="none"
                                    >
                                        {data.map((entry, index) => (
                                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                                        ))}
                                    </Pie>
                                </PieChart>
                            </ResponsiveContainer>
                            <div className="absolute inset-0 flex items-center justify-center text-5xl font-bold text-black">
                                {correct}/{totalQuestions}
                            </div>
                        </div>
                    </div>

                    {/* Кнопка */}
                    <div className="absolute bottom-14 right-10">
                        <Link to="/info-planet">
                            <Button size="lg" className="bg-blue-600 text-white hover:bg-blue-700 cursor-pointer">
                                Перейти до списку тем
                            </Button>
                        </Link>
                    </div>
                </div>
            </div>

            <UserAssistant top="85px" right="105px" text="Good result!" />
        </div>
    );
}

export default QuizResultPage;