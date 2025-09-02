import Planet from '@/components/AuthPageComponents/Planet';
import { Button } from '@/components/ui/button';
import { OrbitControls } from '@react-three/drei';
import { Canvas, useFrame } from '@react-three/fiber';
import React from 'react';

const CameraLogger = () => {
    useFrame(({ camera }) => {
        console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null;
};

const DefaultComponent = ({ setInterfaceOption }) => {
    return (
        <div className="relative w-full min-h-[calc(100vh-121px)]">
            <Canvas
                camera={{ position: [1.912451664002106, 16.475390911011576, 39.85599444894605], fov: 50 }}
                style={{ position: 'absolute', top: 0, left: 0, zIndex: 0 }}
            >
                <ambientLight intensity={2} />
                <pointLight position={[-10, -10, -10]} />

                <Planet
                    position={[0, 24, -10]}
                    size={35}
                    textureUrl="/test-texture.png"
                />

                <OrbitControls />
                <CameraLogger />
            </Canvas>

            <div className="z-20 flex flex-col items-center justify-between text-center absolute inset-0 py-20">
                <div className="w-full text-white">
                    <h1 className='text-3xl font-bold md:text-4xl mb-6'>
                        НОВІ ВИКЛИКИ - НОВІ МОЖЛИВОСТІ
                    </h1>
                    <p className='text-xl'>
                        Готові до наступного рівня? <br />
                        Додаткові завдання – це ваш шанс проявити себе, поглибити свої знання та <br />
                        отримати за це більше монет
                    </p>
                </div>

                <Button 
                    onClick={() => setInterfaceOption("tasks")}
                    className="mt-8 px-10 py-2 font-bold bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer"
                >
                    До завдань →
                </Button>
            </div>
        </div>
    );
}

export default DefaultComponent;