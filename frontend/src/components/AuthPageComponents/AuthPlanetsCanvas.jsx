import React from 'react'
import PlanetsCanvas from './PlanetsCanvas'
import { Canvas, useFrame } from '@react-three/fiber'
import { OrbitControls } from '@react-three/drei'
import Planet from './Planet';

const CameraLogger = () => {
    useFrame(({ camera }) => {
        console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null; 
};

const AuthPlanetsCanvas = () => {
    return (
        <div className='h-screen flex justify-center items-center'>
            <PlanetsCanvas />
            <div className="absolute left-0 bottom-2 w-[800px] h-[800px]">
                <Canvas camera={{ position: [-0.664, -7.6779, 8.925] }}>
                    <ambientLight intensity={0.75} />
                    <directionalLight position={[10, 10, 10]} intensity={1} />
                    <Planet position={[0, -5, 2]} size={2} textureUrl="/sun_texture2.jpg" />
                    <OrbitControls />
                    <CameraLogger />
                </Canvas>
            </div>
        </div>
    )
}

export default AuthPlanetsCanvas