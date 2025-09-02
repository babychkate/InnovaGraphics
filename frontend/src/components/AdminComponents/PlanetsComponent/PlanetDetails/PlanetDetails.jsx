import Planet from '@/components/AuthPageComponents/Planet';
import { Button } from '@/components/ui/button';
import { OrbitControls } from '@react-three/drei';
import { Canvas } from '@react-three/fiber';
import { ArrowLeft } from 'lucide-react';
import React from 'react';

const PlanetDetails = ({ planet, onBack }) => {
    return (
        <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center bg-white">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute top-1 left-1 z-20 p-2 rounded-full cursor-pointer transition-all duration-300 ease-in-out hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex items-center justify-between gap-30">
                <div className="flex flex-col items-start gap-4">
                    <h2 className="text-2xl"><span className='font-bold'>Назва:</span> {planet?.name}</h2>
                    <p className="text-gray-600 font-medium"><span className='font-bold'>Тема:</span> {planet?.topic}</p>
                    <p className="text-gray-600 font-medium"><span className='font-bold'>Номер:</span> {planet?.number}</p>
                    <p className="text-gray-600 font-medium"><span className='font-bold'>Мінімальна енергія:</span> {planet?.requiredEnergy}</p>
                    <p className="text-gray-600 font-medium"><span className='font-bold'>Максимум підказок:</span> {planet?.maxHintCount}</p>
                    <p className="text-gray-600 font-medium"><span className='font-bold'>Втрата енергії:</span> {planet?.energyLost}</p>
                </div>

                <div className="h-80 w-80">
                    <Canvas className="rounded-full" camera={{ position: [0, 0, 1.75] }}>
                        <ambientLight />
                        <directionalLight position={[5, 5, 5]} />
                        <Planet size={1} textureUrl="/planet_texture4.jpg" clickable={false} />
                        <OrbitControls enableZoom={false} />
                    </Canvas>
                </div>
            </div>
        </div>
    );
};

export default PlanetDetails;