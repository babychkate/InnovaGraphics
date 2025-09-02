import Planet from '@/components/AuthPageComponents/Planet';
import { Button } from '@/components/ui/button';
import { getPlanetById } from '@/redux/planet/Action';
import { OrbitControls } from '@react-three/drei';
import { Canvas } from '@react-three/fiber';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';

const Lab1PlanetPage = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const planet = useSelector(state => state.planet.planet);
    const { planetId } = useParams();

    const getTextureByPlanetId = (id) => {
        switch (id) {
            case "EA8E4C69-699A-4A23-8554-5CEDB72B635B":
                return "/planet_texture/blue1.jpg";
            case "03A146EB-D2D9-4283-BC2B-031F026AA7FA":
                return "/planet_texture/green.jpeg";
            case "AC4B8F5B-DB7B-44BA-96B2-6AE26BC85F3C":
                return "/planet_texture/r1.jpg";
            case "3895D417-4178-4E3D-80AC-7DA8C1785899":
                return "/planet_texture/purple.jpeg";
            case "2DEF880A-D118-4D82-B00C-18C6E156225A":
                return "/planet_texture/colorful.jpeg";
            case "A30E1F22-6D93-4983-9CDC-B6F8902E8730":
                return "/planet_texture/y2.jpeg";
            default:
                return "/planet_texture3.jpg"; 
        }
    };

    useEffect(() => {
        dispatch(getPlanetById(planetId));
    }, [dispatch]);

    return (
        <div className="pt-[72px] h-screen flex bg-black p-10">
            <div className="w-1/2 h-full bg-[#cecece] rounded-l-4xl">
                <Canvas camera={{ position: [0, 0, 10], fov: 75 }}>
                    <ambientLight intensity={2} />
                    <directionalLight position={[10, 10, 5]} intensity={1} />

                    <Planet position={[0, 0, 0]} size={4} textureUrl={getTextureByPlanetId(planetId)} clickable={false} />
                    <OrbitControls />
                </Canvas>
            </div>
            <div className="w-1/2 p-10 flex flex-col justify-between rounded-r-4xl bg-white text-black">
                <div>
                    <h1 className="text-4xl font-bold mb-3">{planet?.name}</h1>
                    <h2 className='text-xl mb-6'>
                        <span className='font-semibold'>Тема: </span>{planet?.topic}
                    </h2>
                    <p className="whitespace-pre-line">
                        Це планета <strong>{planet?.name}</strong>, присвячена темі: <strong>{planet?.topic}</strong>.

                        Щоб перейти до завдання, вам потрібно щонайменше <strong>{planet?.requiredEnergy}</strong> одиниць енергії.
                        За невдалі спроби ви втрачаєте <strong>{planet?.energyLost}</strong> енергії.

                        На цій планеті дозволено використати стільки підказок: <strong>{planet?.maxHintCount}</strong>.
                        Ви вже використали <strong>{planet?.currentHintCount}</strong> з них.

                        Бажаємо успіхів у дослідженні!
                    </p>
                </div>

                <div className='flex justify-end'>
                    <Button
                        onClick={() => navigate("./exercise")}
                        className="cursor-pointer text-sm px-10 py-2 mt-10 self-start"
                    >
                        Почати
                    </Button>
                </div>
            </div>
        </div>
    );
}

export default Lab1PlanetPage;