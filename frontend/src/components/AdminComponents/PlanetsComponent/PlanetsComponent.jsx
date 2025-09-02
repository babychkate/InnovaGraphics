import React, { useEffect, useState } from 'react';
import { Button } from '@/components/ui/button';
import { Canvas } from '@react-three/fiber';
import Planet from '@/components/AuthPageComponents/Planet';
import { OrbitControls } from '@react-three/drei';
import PlanetDetails from './PlanetDetails/PlanetDetails';
import AddEditPlanetComponent from './AddEditPlanetComponent/AddEditPlanetComponent';
import DeletePlanetModalComponent from './DeletePlanetModalComponent/DeletePlanetModalComponent';
import { useDispatch, useSelector } from 'react-redux';
import { clearPlanetErrors, deletePlanet, getAllPlanets } from '@/redux/planet/Action';

const PlanetsComponent = () => {
    const dispatch = useDispatch();

    const [selectedPlanet, setSelectedPlanet] = useState(null);
    const [editPlanet, setEditPlanet] = useState(null);
    const [planetToDelete, setPlanetToDelete] = useState(null);

    const handleDelete = (planet) => {
        dispatch(deletePlanet(planet.id));
        setPlanetToDelete(null);
    };

    const planets = useSelector(state => state.planet.planets);

    useEffect(() => {
        dispatch(getAllPlanets());
    }, [dispatch]);

    if (selectedPlanet) {
        return <PlanetDetails planet={selectedPlanet} onBack={() => setSelectedPlanet(null)} />;
    }

    if (editPlanet !== null) {
        return (
            <AddEditPlanetComponent
                planet={editPlanet}
                onBack={() => setEditPlanet(null)}
            />
        );
    }

    return (
        <div className="flex flex-col gap-6 px-10 py-6">
            <h1 className="text-2xl font-bold mb-4">Планети</h1>

            <div className="flex flex-col gap-4">
                <div className="grid grid-cols-1 gap-4">
                    <div className="w-full grid grid-cols-[auto_1fr_1fr_1fr_1fr_1fr] items-center gap-4">
                        <div></div>
                        <div className="font-bold text-center">Назва</div>
                        <div className="font-bold text-center">Тема</div>
                        <div className="font-bold text-center">Номер</div>
                        <div className="font-bold text-center">Ціна</div>
                        <div></div>
                    </div>
                    {planets?.map((planet, index) => (
                        <div
                            key={planet?.id}
                            onClick={() => setSelectedPlanet(planet)}
                            className="w-full bg-[#C2D3FD] cursor-pointer rounded-4xl p-4 grid grid-cols-[auto_1fr_1fr_2fr_1.5fr_auto] items-center gap-4 transition hover:scale-[1.005]"
                        >
                            <div className="w-24 h-24">
                                <Canvas className="rounded-full" camera={{ position: [0, 0, 2] }}>
                                    <ambientLight />
                                    <directionalLight position={[5, 5, 5]} />
                                    <Planet size={1} textureUrl="/planet_texture4.jpg" clickable={false} />
                                    <OrbitControls enableZoom={false} />
                                </Canvas>
                            </div>

                            <h2 className="text-lg font-bold">{planet?.name}</h2>
                            <p className="text-center">{planet?.topic}</p>
                            <p className="text-center">{planet?.number}</p>
                            <div className="flex items-center justify-center space-x-1">
                                <img src="/coin.png" alt="Coin" className="w-5 h-5" />
                                <span className="text-black text-sm">{planet?.price}</span>
                            </div>

                            <div className="flex gap-2">
                                <Button
                                    className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer text-white hover:text-white"
                                    variant="ghost"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        setEditPlanet({ ...planet, id: planet.id || 0 });
                                    }}
                                >
                                    Редагувати
                                </Button>
                                <Button
                                    variant="destructive"
                                    className="cursor-pointer"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        setPlanetToDelete(planet);
                                    }}
                                >
                                    Видалити
                                </Button>
                            </div>
                        </div>
                    ))}
                    <div
                        className="w-full bg-[#C2D3FD] hover:bg-[#abc3ff] cursor-pointer rounded-4xl p-4 flex items-center gap-8 transition hover:scale-[1.005]"
                        onClick={() => {
                            setEditPlanet({ id: 0 });
                            dispatch(clearPlanetErrors());
                        }}
                    >
                        <img src="/add_circle.png" alt="Add" />
                        <div className="text-xl">Додати нову планету</div>
                    </div>
                </div>
            </div>

            {planetToDelete && (
                <DeletePlanetModalComponent
                    planet={planetToDelete}
                    onConfirm={() => handleDelete(planetToDelete)}
                    onCancel={() => setPlanetToDelete(null)}
                />
            )}
        </div>
    );
};

export default PlanetsComponent;