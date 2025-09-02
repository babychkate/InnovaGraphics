import React from 'react';
import { Canvas } from "@react-three/fiber";
import Planet from './Planet';
import Stars from '../Stars/Stars';
import Orbit from '../Orbit/Orbit';

const PlanetsCanvas = () => {
    return (
        <div className='w-full h-full'>
            <Canvas camera={{ position: [0, 0, 10], fov: 75 }} style={{ background: "black" }}>
                <ambientLight intensity={0.3} />
                <directionalLight position={[10, 10, 10]} intensity={1} />

                <Stars count={5000} size={10} />
                <Orbit radius={5} numSegments={100} rotation={{ x: 1.5, y: -1.65, z: -1.35 }} position={{ x: 9, y: 0, z: 0 }}>
                </Orbit>
                <Orbit radius={5.5} numSegments={100} rotation={{ x: 1.5, y: -1.65, z: -1.13 }} position={{ x: 8, y: 0, z: 0 }}>
                </Orbit>
                <Orbit radius={6} numSegments={100} rotation={{ x: 1.5, y: -1.65, z: -1 }} position={{ x: 7, y: 0, z: 0 }}>
                    <Planet position={[2.75, 3, 0]} size={0.5} textureUrl="/planet_texture1.jpg" />
                </Orbit>
                <Orbit radius={6.5} numSegments={100} rotation={{ x: 1.5, y: -1.65, z: -0.9 }} position={{ x: 6, y: 0, z: 0 }}>
                    <Planet position={[1, 4, 0]} size={0.75} textureUrl="/planet_texture2.jpg" />
                </Orbit>
                <Orbit radius={7} numSegments={100} rotation={{ x: 1.5, y: -1.65, z: -0.8 }} position={{ x: 5, y: 0, z: 0 }}>
                </Orbit>
                <Orbit radius={7.5} numSegments={100} rotation={{ x: 1.5, y: -1.65, z: -0.7 }} position={{ x: 4, y: 0, z: 0 }}>
                    <Planet position={[-3.2, 0, 0]} size={1.4} textureUrl="/planet_texture3.jpg" />
                </Orbit>
            </Canvas>
        </div>
    );
};

export default PlanetsCanvas;