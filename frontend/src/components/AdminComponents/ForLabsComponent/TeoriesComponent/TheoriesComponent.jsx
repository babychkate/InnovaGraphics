import React, { useEffect, useState } from 'react';
import { Button } from '@/components/ui/button';
import { useDispatch, useSelector } from 'react-redux';
import { deleteTheory, getAllTheories } from '@/redux/theory/Action';
import AddEditTheoryComponent from './AddEditTheoryComponent/AddEditTheoryComponent';
import { getAllPlanets } from '@/redux/planet/Action';
import DeleteTheoryModalComponent from './DeleteTheoryModalComponent/DeleteTheoryModalComponent';
import TheoryDetails from './TheoryDetails/TheoryDetails';

const TheoriesComponent = () => {
    const dispatch = useDispatch();

    const [selectedTheory, setSelectedTheory] = useState(null);
    const [editTheory, setEditTheory] = useState(null);
    const [theoryToDelete, setTheoryToDelete] = useState(null);

    const theories = useSelector(state => state.theory.theories);
    const planets = useSelector(state => state.planet.planets);

    const handleDelete = (teory) => {
        dispatch(deleteTheory(teory.id));
        setTheoryToDelete(null);
    };

    useEffect(() => {
        dispatch(getAllTheories());
        dispatch(getAllPlanets());
    }, [dispatch]);

    if (selectedTheory) {
        return (
            <TheoryDetails
                theory={selectedTheory}
                onBack={() => setSelectedTheory(null)}
            />
        );
    }

    if (editTheory !== null) {
        return (
            <AddEditTheoryComponent
                theory={editTheory}
                onBack={() => setEditTheory(null)}
            />
        );
    }

    return (
        <div className="flex flex-col gap-6 px-10 py-6">
            <h1 className="text-2xl font-bold mb-4">Теоретичні матеріали</h1>

            <div className="flex flex-col gap-4">
                <div className="grid grid-cols-1 gap-4">
                    <div className="w-full grid grid-cols-[auto_1fr_1fr] items-center gap-4">
                        <div></div>
                        <div className="font-bold">Назва</div>
                    </div>
                    {theories?.map((theory) => {
                        const planet = planets.find(p => p.id === theory.planetId);

                        return (
                            <div
                                key={theory?.id}
                                onClick={() => setSelectedTheory(theory)}
                                className="w-full bg-[#C2D3FD] cursor-pointer rounded-4xl p-4 flex justify-between items-center transition hover:scale-[1.005]"
                            >
                                {/* Ліва колонка: Назва планети і тема */}
                                <div className="flex flex-col">
                                    <h2 className="text-lg font-bold">{planet?.name || '—'}</h2>
                                    <p className="text-sm text-gray-700">{planet?.topic || '—'}</p>
                                </div>

                                {/* Права колонка: Кнопки */}
                                <div className="flex gap-2">
                                    <Button
                                        className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer text-white hover:text-white"
                                        variant="ghost"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            setEditTheory({ ...theory, id: theory.id || 0 });
                                        }}
                                    >
                                        Редагувати
                                    </Button>
                                    <Button
                                        variant="destructive"
                                        className="cursor-pointer"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            setTheoryToDelete(theory);
                                        }}
                                    >
                                        Видалити
                                    </Button>
                                </div>
                            </div>
                        );
                    })}
                    <div
                        className="w-full bg-[#C2D3FD] hover:bg-[#abc3ff] cursor-pointer rounded-4xl p-4 flex items-center gap-8 transition hover:scale-[1.005]"
                        onClick={() => {
                            setEditTheory({ id: 0 });
                        }}
                    >
                        <img src="/add_circle.png" alt="Add" />
                        <div className="text-xl">Додати нову теорію</div>
                    </div>
                </div>
            </div>

            {theoryToDelete && (
                <DeleteTheoryModalComponent
                    onConfirm={() => handleDelete(theoryToDelete)}
                    onCancel={() => setTheoryToDelete(null)}
                />
            )}
        </div>
    );
};

export default TheoriesComponent;