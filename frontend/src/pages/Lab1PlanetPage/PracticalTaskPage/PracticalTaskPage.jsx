import { Button } from '@/components/ui/button';
import { getExerciseByPlanetId } from '@/redux/exercise/Action';
import { ArrowLeft } from 'lucide-react';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useParams } from 'react-router-dom';

const PracticalTaskPage = () => {
    const { planetId } = useParams();
    const dispatch = useDispatch();
    const exercises = useSelector(state => state.exercise.exercises);

    useEffect(() => {
        dispatch(getExerciseByPlanetId(planetId));
    }, [dispatch, planetId]);

    return (
        <div className="pt-[72px] relative h-screen flex bg-black p-10">
            <Link to={`/planets/${planetId}/exercise`}>
                <Button
                    variant="ghost"
                    className='absolute top-20 left-10 z-20 p-2 cursor-pointer text-white hover:bg-white/50 hover:text-white rounded-full transition-all duration-300 ease-in-out'
                >
                    <ArrowLeft size={28} />
                </Button>
            </Link>

            <div className="w-full h-full bg-white rounded-4xl p-10 overflow-y-auto">
                <div className='flex flex-col gap-10'>
                    <h1 className='text-center text-4xl font-bold'>Практичні завдання</h1>

                    {exercises.length === 0 ? (
                        <p className='text-center text-xl'>Наразі немає доступних завдань.</p>
                    ) : (
                        <div className='flex flex-col gap-8'>
                            {exercises.map((exercise, index) => (
                                <div key={exercise.id} className='border p-6 rounded-xl shadow-md bg-gray-100'>
                                    <h2 className='text-2xl font-semibold text-[#2354E1]'>{exercise.name}</h2>
                                    <p className='text-sm text-gray-700 whitespace-pre-line mt-2'>{exercise.description}</p>
                                    <div className='mt-4 flex justify-between items-center text-sm text-gray-600'>
                                        <span><strong>Тема:</strong> {exercise.theme}</span>
                                        <span><strong>Нагорода:</strong> {exercise.reward} монет</span>
                                    </div>
                                    <Link
                                        to={`./${exercise.id}`}
                                        className='mt-4 inline-block text-blue-600 hover:underline'
                                    >
                                        Перейти до завдання →
                                    </Link>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default PracticalTaskPage;