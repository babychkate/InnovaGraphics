import Planet from '@/components/AuthPageComponents/Planet';
import SpaceStation from '@/components/SpaceStation/SpaceStation';
import { OrbitControls } from '@react-three/drei';
import { Canvas, useFrame } from '@react-three/fiber';
import React, { Suspense } from 'react';

const CameraLogger = () => {
    useFrame(({ camera }) => {
        console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null;
};

const DefaultMultiplayerComponent = () => {
    return (
        <div className="relative w-full min-h-[calc(100vh-121px)]">
            <Canvas
                camera={{ position: [0, 0, 10], fov: 75 }}
                style={{ position: 'absolute', top: 0, left: 0, zIndex: 0 }}
            >
                <ambientLight intensity={1} />
                <directionalLight position={[-5, 5, 5]} intensity={1} />

                {/* <Planet position={[27, 5, -5]} size={5} textureUrl="/planet_texture/green.jpeg" rotation={[0, 2, 0]} clickable={false} />
                <Planet position={[-20, 3.5, 1]} size={2.25} textureUrl="/planet_texture/blue1.jpg" clickable={false} />
                <Planet position={[-13.5, -4, 9]} size={1.25} textureUrl="/planet_texture/y1.jpeg" clickable={false} />
                <Planet position={[-5, 3, 0]} size={0.5} textureUrl="/planet_texture/r1.jpg" clickable={false} />
                <Planet position={[1.5, -1.15, 7.75]} size={0.25} textureUrl="/planet_texture/y2.jpeg" clickable={false} />
                <Planet position={[4, -1.1, 5.5]} size={0.4} textureUrl="/planet_texture/shop.jpeg" r clickable={false} /> */}

                <Suspense fallback={null}>
                    <SpaceStation
                        position={[0, 0.5, 0]}
                        scale={3.5}
                        rotation={[Math.PI / 4, 0, -Math.PI / 10]}
                        clickable={false}
                    />
                </Suspense>

                <CameraLogger />
                <OrbitControls />
            </Canvas>

            <div className="z-20 flex flex-col items-center justify-between text-center absolute inset-0  p-10">
                <div className="w-full ">
                    <h1 className='text-3xl font-bold md:text-4xl mb-6'>
                        МУЛЬТИПЛЕЄР: ВИКЛИК ПРИЙНЯТО!
                    </h1>
                </div>
                <div className='max-w-[60vw] text-xl font-semibold'>
                    <p>
                        Змагайтеся з іншими у виконанні цікавих завдань з комп'ютерної графіки. 
                        Покажіть свої навички, отримуйте бали та піднімайтеся в рейтингу! 
                        Обирайте завдання та доведіть свою майстерність.
                    </p>
                </div>
            </div>
        </div>
    );
}

export default DefaultMultiplayerComponent;