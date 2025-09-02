import Planet from '@/components/AuthPageComponents/Planet';
import { OrbitControls } from '@react-three/drei';
import { Canvas, useFrame } from '@react-three/fiber';
import React from 'react';

const CameraLogger = () => {
    useFrame(({ camera }) => {
        console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null;
};

const DefaultAdditionInfoPageComponent = () => {
    return (
        <div className="relative w-full min-h-[calc(100vh-121px)]">
            <Canvas
                camera={{ position: [-1.8028553490365375, 0.5774402647019847, 9.819178954024105], fov: 50 }}
                style={{ position: 'absolute', top: 0, left: 0, zIndex: 0 }}
            >
                <ambientLight intensity={2} />
                <pointLight position={[10, 10, 10]} />

                <Planet
                    position={[-9, -4.5, 0]}
                    size={4}
                    rotation={[2, -1, -1]}
                    textureUrl="/test-texture.png"
                />

                <Planet
                    position={[13, 6, 0]}
                    size={6}
                    rotation={[0.3, -1, -0.5]}
                    textureUrl="/test-texture.png"
                />

                <OrbitControls />
                <CameraLogger />
            </Canvas>

            <div className="z-20 flex flex-col items-center justify-center text-center absolute inset-0">
                <div className="w-full relative top-[-10%]">
                    <h1 className='text-3xl md:text-4xl mb-4'>
                        ПОГЛИБЛЮЙТЕ СВОЇ ЗНАННЯ <br />
                        <span className='font-bold block mt-2'>КОМП’ЮТЕРНОЇ ГРАФІКИ</span>
                    </h1>
                    <p className='text-xl'>
                        Тут ви знайдете безліч корисних матеріалів для поглибленого <br />
                        вивчення комп'ютерної графіки: відеоуроки, лекції, посилання на <br />
                        ресурси з 3D-моделями та текстурами, а також приклади готових реалізацій. <br />
                        Усі навчальні матеріали є безкоштовними.
                    </p>
                </div>
            </div>
        </div>
    );
}

export default DefaultAdditionInfoPageComponent;