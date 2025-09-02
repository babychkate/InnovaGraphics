import Planet from '@/components/AuthPageComponents/Planet';
import { Button } from '@/components/ui/button';
import { OrbitControls, Text } from '@react-three/drei';
import { Canvas } from '@react-three/fiber';
import React from 'react';

const planets = [
    { id: 1, price: 90, name: 'Лаба 1', textureUrl: "/planet_texture/green.jpeg" },
    { id: 2, price: 90, name: 'Лаба 2', textureUrl: "/planet_texture/r1.jpg" },
    { id: 3, price: 150, name: 'Лаба 3', textureUrl: "/planet_texture/purple.jpeg" },
    { id: 4, price: 150, name: 'Лаба 4', textureUrl: "/planet_texture/colorful.jpeg" },
    { id: 5, price: 150, name: 'Лаба 5', textureUrl: "/planet_texture/y2.jpeg" },
];

const AccessToPlanet = () => {
    return (
        <div className='min-h-[calc(100vh-141px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
            <h1 className='text-2xl font-semibold text-center'>ДОСТУП ДО ПЛАНЕТ</h1>
            <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 px-4'>
                {planets.map(planet => (
                    <div
                        key={planet.id}
                        className='relative bg-[#C2D3FD] rounded-2xl shadow-lg p-6 flex flex-col items-center justify-between h-[330px]'
                    >
                        <div className='absolute top-3 right-3 text-black text-sm flex items-center gap-2 font-bold px-2 py-1 rounded'>
                            {planet.price}
                            <img src="/coin.png" alt="Coin" className='w-7 h-7' />
                        </div>

                        <Canvas
                            camera={
                                {
                                    position: [0, 0, 4.5],
                                    fov: 75,
                                }
                            }
                        >
                            <ambientLight intensity={0.3} />
                            <directionalLight position={[10, 10, 10]} intensity={1} />
                            <Planet
                                position={[0, 0, 0]}
                                size={2.5}
                                textureUrl={planet.textureUrl}
                                clickable={false}
                            />

                            <Text
                                position={[0, 0, 2.5]}
                                fontSize={0.5}           
                                color="white"           
                                anchorX="center"       
                                anchorY="middle"        
                            >
                                {planet.name}
                            </Text>

                            <OrbitControls />
                        </Canvas>

                        <div className='flex gap-2 mt-4'>
                            <Button className="text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer">Розблокувати</Button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default AccessToPlanet;