import { Button } from '@/components/ui/button';
import { ArrowLeft } from 'lucide-react';
import React from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';

const ExercisePage = () => {
    const planetId = useParams().planetId;
    const navigate = useNavigate();

    return (
        <div className="pt-[72px] relative h-screen flex bg-black p-10">
            <Button
                variant="ghost"
                onClick={() => navigate(`/planets/${planetId}`)}
                className='absolute top-20 left-10 z-20 p-2 hover:bg-white/50 rounded-full transition-all duration-300 ease-in-out'
            >
                <ArrowLeft size={28} />
            </Button>
            <div className="w-full h-full bg-white rounded-4xl p-10">
                <div className='flex flex-col gap-20'>
                    <div className='flex flex-col'>
                        <p className='text-xl'>
                            <span className='font-bold'>1. Теоретична частина. </span>У цьому розділі Ви ознайомитеся із основними поняттями та функціями,
                            які знадобляться для виконання практичного завдання до лабораторної роботи. Ви можете пройти
                            Швидкий тест, якщо Ви вже добре володієте теоретичним метеріалом, щоб одразу перейти до виконання
                            практичних завдань.
                        </p>
                        <div className='flex flex-col items-start gap-2'>
                            <Link to="./materials">
                                <Button className="mt-4 px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer">
                                    Теоретичні матеріали
                                </Button>
                            </Link>
                        </div>
                    </div>
                    <div className='flex flex-col'>
                        <p className='text-xl'>
                            <span className='font-bold'>2. Практичне завдання. </span>Після засвоєння теоретичного матеріалу
                            Ви отримуєте доступ до виконання практичного завдання по темі лабораторної роботи.
                            Залежно від правильності виконання завдання та кількості допущених помилок - Ви отримуєте
                            відповідну кількість балів.
                        </p>
                        <div className='flex flex-col items-start mt-4'>
                            <Link to="./practical-task">
                                <Button className="px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer">
                                    Практичне завдання
                                </Button>
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ExercisePage;