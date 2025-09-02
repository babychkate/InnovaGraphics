import { Canvas, useFrame } from '@react-three/fiber';
import React from 'react';
import Planet from '../AuthPageComponents/Planet';
import { OrbitControls } from '@react-three/drei';

const CameraLogger = () => {
    useFrame(({ camera }) => {
        console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null;
};

const PlanetBackground = () => {
    return (
        <div className='w-screen h-screen bg-black'>
            <Canvas
                camera={
                    {
                        position: [10, 0, 0],
                        fov: 75,
                    }
                }
            >
                <ambientLight intensity={0.5} />
                <directionalLight position={[10, -10, 10]} intensity={2} />
                <Planet position={[0, 3.75, -10]} size={8} textureUrl="/planet_texture4.jpg" clickable={false} />

                <OrbitControls />
                <CameraLogger />
            </Canvas>
        </div>
    );
}

export default PlanetBackground;