import React from 'react';
import { X } from 'lucide-react';
import { OrbitControls } from '@react-three/drei';
import Planet from '@/components/AuthPageComponents/Planet';
import Orbit from '@/components/Orbit/Orbit';
import { Canvas, useFrame } from '@react-three/fiber';
import Stars from '@/components/Stars/Stars';
import { Button } from '@/components/ui/button';

const CameraLogger = () => {
    useFrame(({ camera }) => {
        console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null;
};

const TryAppThemeModal = ({ theme, onClose }) => {
    return (
        <div className="fixed inset-0 flex justify-center items-center z-50 bg-black/50">
            <div
                className="relative w-[75%] h-[85%] bg-white p-5 rounded-2xl flex flex-col justify-between translate-y-5"
            >
                <Button
                    variant="ghost"
                    onClick={() => onClose()}
                    className="absolute top-4 right-4 z-10 hover:text-red-500"
                >
                    <X size={30} />
                </Button>

                <div className="flex-1 relative z-0 bg-cover bg-center overflow-hidden" style={{ backgroundImage: `url(${theme.image})` }}>
                    <Canvas
                        camera={{
                            position: [13.816328849321339, 4.920889564384977, 21.52252220153435],
                            fov: 75,
                        }}
                    >
                        <ambientLight intensity={0.3} />
                        <directionalLight position={[10, 10, 10]} intensity={1} />

                        <Stars count={15000} size={1000} />

                        <Orbit radius={11} numSegments={100} rotation={{ x: 0, y: -1, z: -0.5 }} position={{ x: 0, y: -4, z: 20 }}>
                            <Planet position={[0, -4.5, 20]} size={5.5} textureUrl="./sun_texture2.jpg" />
                        </Orbit>

                        <Orbit radius={18.5} numSegments={100} rotation={{ x: 0, y: -1, z: -0.5 }} position={{ x: 0, y: -4, z: 20 }}>
                            <Planet position={[5.5, -2, 10.5]} size={2} textureUrl="./sun_texture.jpg" to="/info-planet" />
                        </Orbit>

                        <Orbit radius={28} numSegments={100} rotation={{ x: 0, y: -1, z: -0.5 }} position={{ x: 0, y: -4, z: 20 }}>
                            <Planet position={[-10, 4, 7]} size={5} textureUrl="./planet_texture3.jpg" to="/lab1-planet" />
                        </Orbit>

                        <Orbit radius={36} numSegments={100} rotation={{ x: 0, y: -1, z: -0.5 }} position={{ x: 0, y: -4, z: 20 }}>
                            <Planet position={[17, 0, 0]} size={3.25} textureUrl="./planet_texture2.jpg" to="/lab2-planet" />
                        </Orbit>

                        <Orbit radius={6.5} numSegments={100} rotation={{ x: 0, y: -0.3, z: -0.85 }} position={{ x: 17, y: 0, z: 0 }}>
                            <Planet position={[13, 4.5, 0]} size={1} textureUrl="./planet_texture1.jpg" to="/shop" />
                        </Orbit>

                        <Orbit radius={48.5} numSegments={100} rotation={{ x: 0, y: -1, z: -0.5 }} position={{ x: 0, y: -4, z: 20 }}>
                            <Planet position={[0, 10, -12]} size={3} textureUrl="./planet_texture4.jpg" to="/lab3-planet" />
                        </Orbit>

                        <Orbit radius={71} numSegments={100} rotation={{ x: 0, y: -1, z: -0.5 }} position={{ x: 0, y: -4, z: 20 }}>
                            <Planet position={[-40, 16, 0]} size={4} textureUrl="./sun_texture2.jpg" to="/lab4-planet" />
                        </Orbit>

                        <Planet position={[22.5, 18, -43.5]} size={4} textureUrl="./planet_texture3.jpg" to="/lab5-planet" />

                        <OrbitControls />
                        <CameraLogger />
                    </Canvas>
                </div>
                <div className="p-4 flex justify-end bg-white bg-opacity-80 z-10">
                    <Button
                        onClick={() => onClose()}
                        className="px-6 py-2 rounded-lg bg-blue-500 text-white hover:bg-blue-600 transition"
                    >
                        Ок
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default TryAppThemeModal;