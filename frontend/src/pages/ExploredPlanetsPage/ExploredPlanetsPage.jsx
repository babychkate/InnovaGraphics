import BigPlanets from '@/components/BigPlanets/BigPlanets';
import MoveBack from '@/components/MoveBack/MoveBack';
import { getAllPlanets } from '@/redux/planet/Action';
import { OrbitControls } from '@react-three/drei';
import { Canvas, useFrame } from '@react-three/fiber';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';

const CameraLogger = () => {
    useFrame(({ camera }) => {
        console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null;
};

const ExploredPlanetsPage = () => {
    const dispatch = useDispatch();
    const planets = useSelector(state => state.planet.planets);

    useEffect(() => {
        dispatch(getAllPlanets());
    }, [dispatch]);

    return (
        <div className="relative h-screen overflow-hidden pt-[72px]">
            <MoveBack to="/my-profile" />

            <div className='relative z-10 flex flex-col py-10 px-30'>
                <h1 className='text-2xl font-bold mb-6'>Досліджені планети</h1>

                {/* Контейнер з прокруткою */}
                <div className='max-h-[500px] overflow-y-auto w-full rounded-xl shadow-lg'>
                    <div className='grid grid-cols-3 bg-white bg-opacity-80 text-sm rounded-xl overflow-hidden'>
                        <div className='p-8 text-center text-xl font-semibold border-r'>Планети</div>
                        <div className='p-8 text-center text-xl font-semibold border-r'>Взаємодія</div>
                        <div className='p-8 text-center text-xl font-semibold'>Нагороди</div>

                        {planets
                            .filter((planet) => planet.isUnlock)
                            .map((planet) => (
                                <React.Fragment key={planet.id}>
                                    <div className='flex items-center justify-center gap-4 p-8 border-t border-r'>
                                        <img src={planet?.imagePath} alt={planet?.name} className='w-12 h-12 rounded-full object-cover' />
                                        <div>
                                            <div><span className='underline'>Планета: </span>{planet?.name}</div>
                                            <div className='text-gray-600'><span className='underline'>Рівень: </span>{planet?.number}</div>
                                        </div>
                                    </div>

                                    <div className='flex items-center justify-center flex-col p-4 border-t border-r'>
                                        <div>Завершено: {planet?.interaction?.completed}</div>
                                        <div>Віддано восстаннє: {planet?.interaction?.restored}</div>
                                    </div>

                                    <div className='flex items-center justify-center p-4 gap-8 border-t'>
                                        <div>{planet?.rewards?.energy} ⚡</div>
                                        <div>{planet?.rewards?.stars} ⭐</div>
                                    </div>
                                </React.Fragment>
                            ))}
                    </div>
                </div>
            </div>

            {/* Canvas background */}
            <div className="absolute top-0 left-0 w-full h-full z-0">
                <Canvas>
                    <directionalLight position={[-10, -10, -10]} intensity={1} />
                    <ambientLight />
                    <BigPlanets />
                    <CameraLogger />
                    <OrbitControls />
                </Canvas>
            </div>
        </div>
    );
};

export default ExploredPlanetsPage;