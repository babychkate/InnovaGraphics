import { OrbitControls } from '@react-three/drei';
import { Canvas } from '@react-three/fiber';
import React, { Suspense } from 'react';
import { Astronaut } from './Astronaut';

const UserPlayer = () => {
    return (
        <div className='h-full'>
            <Canvas camera={{ position: [0, 0, 10], zoom: 1 }}>
                <perspectiveCamera makeDefault position={[0, 0, 5]} fov={75} near={0.1} far={1000} />
                <OrbitControls />

                <hemisphereLight intensity={1} />
                <spotLight
                    position={[10, 10, 10]}
                    angle={0.4}
                    penumbra={1}
                    intensity={2}
                    castShadow
                />

                <Suspense fallback={null}>
                    <Astronaut position={[0, -6, 0]} />
                </Suspense>
            </Canvas>
        </div>
    );
}

export default UserPlayer;