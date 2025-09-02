import { Button } from '@/components/ui/button';
import { ArrowLeft } from 'lucide-react';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getPlanetById } from '@/redux/planet/Action';
import MyEditor from '@/components/MyEditor/MyEditor';

const TheoryDetails = ({ theory, onBack }) => {
    const dispatch = useDispatch();
    const planet = useSelector(state => state.planet.planet);
    useEffect(() => {
        if (theory?.planetId) {
            dispatch(getPlanetById(theory.planetId));
        }
    }, [dispatch, theory?.planetId]);

    return (
        <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex items-center justify-center w-full max-w-4xl">
                <div className="flex flex-col items-start gap-4 bg-white rounded-xl p-8 w-full">
                    <h2 className="text-2xl"><span className="font-bold">Назва:</span> {planet?.name}</h2>
                    <p className="text-gray-600 font-medium"><span className="font-bold">Тема:</span> {planet?.topic}</p>
                    <div className='flex flex-col gap-2 w-full'>
                        <p className="text-gray-600 font-medium mb-2"><span className="font-bold">Теорія:</span></p>
                        <MyEditor value={theory?.content || ''} readOnly={true} />
                    </div>
                </div>
            </div>
        </div>
    );
};

export default TheoryDetails;