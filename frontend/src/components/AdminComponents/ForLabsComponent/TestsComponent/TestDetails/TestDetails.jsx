import { Button } from '@/components/ui/button';
import { ArrowLeft } from 'lucide-react';
import React, { useEffect, useState } from 'react';
import TestBuilder from './TestBuilder/TestBuilder';
import { useDispatch, useSelector } from 'react-redux';
import { getPlanetById } from '@/redux/planet/Action';

const TestDetails = ({ test, onBack }) => {
    const dispatch = useDispatch();
    const planet = useSelector(state => state.planet.planet);
    const [isTestBuilderOpen, setIsTestBuilderOpen] = useState(false);

    const handleGoToTestBuilder = () => {
        setIsTestBuilderOpen(true);
    };

    useEffect(() => {
        dispatch(getPlanetById(test?.planetId));
    }, [dispatch]);

    if (isTestBuilderOpen) {
        return (
            <div className="relative min-h-[calc(100vh-200px)]">
                <Button
                    variant="ghost"
                    onClick={() => setIsTestBuilderOpen(false)}
                    className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
                >
                    <ArrowLeft size={28} />
                </Button>
                <TestBuilder testId={test?.id} />
            </div>
        );
    }

    return (
        <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex items-center justify-center">
                <div className="flex flex-col items-start gap-4">
                    <h2 className="text-2xl"><span className="font-bold">Назва:</span> {test?.name}</h2>
                    <p className="text-gray-600 font-medium"><span className="font-bold">Тема:</span> {test?.theme}</p>
                    <p className="text-gray-600 font-medium">
                        <span className="font-bold">Планета:</span> {planet?.name ? planet.name : "Це тест для мультиплеєра"}
                    </p>
                    <p className="text-gray-600 font-medium">
                        <span className="font-bold">Часовий ліміт:</span> {test?.timeLimit}
                    </p>
                </div>
            </div>

            <Button
                variant="link"
                className="absolute bottom-6 right-6 cursor-pointer"
                onClick={handleGoToTestBuilder}
            >
                Перейти до запитань
            </Button>
        </div>
    );
};

export default TestDetails;