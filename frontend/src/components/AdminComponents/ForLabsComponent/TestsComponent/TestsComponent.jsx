import { Button } from '@/components/ui/button';
import React, { useEffect, useState } from 'react';
import AddEditTestComponent from './AddEditTestComponent/AddEditTestComponent';
import TestDetails from './TestDetails/TestDetails';
import DeleteTestModalComponent from './DeleteTestModalComponent/DeleteTestModalComponent';
import { clearPlanetErrors, deleteTest, getAllTests } from '@/redux/test/Action';
import { useDispatch, useSelector } from 'react-redux';

const TestsComponent = () => {
    const dispatch = useDispatch();
    const [selectedTest, setSelectedTest] = useState(null);
    const [editTest, setEditTest] = useState(null);
    const [testToDelete, setTestToDelete] = useState(null);

    const handleDelete = (test) => {
        dispatch(deleteTest(test.id));
        setTestToDelete(null);
    };

    const tests = useSelector(state => state.test.tests);

    useEffect(() => {
        dispatch(getAllTests());
    }, [dispatch]);

    if (selectedTest) {
        return <TestDetails test={selectedTest} onBack={() => setSelectedTest(null)} />;
    }

    if (editTest !== null) {
        return (
            <AddEditTestComponent
                test={editTest}
                onBack={() => setEditTest(null)}
            />
        );
    }

    return (
        <div className="flex flex-col gap-4 px-4">
            {tests.map((test) => (
                <div key={test.id} className="group cursor-pointer" onClick={() => setSelectedTest(test)}>
                    <div className="bg-[#C2D3FD] p-4 rounded-3xl shadow-md transition hover:scale-[1.01]">
                        <div className="grid grid-cols-4 gap-4 items-center">
                            <div>
                                <h2 className="text-lg font-semibold">{test.name}</h2>
                                <p className="text-sm text-gray-700">{test.theme}</p>
                            </div>

                            <div className="text-md text-gray-600 text-center">
                                <span className="font-medium text-black">{test.planetName}</span>
                            </div>

                            <div className="text-sm text-gray-600 text-center">
                                <span className="font-medium text-black">{test.timeLimit} хв</span>
                            </div>

                            <div className="flex gap-2">
                                <Button
                                    className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer text-white hover:text-white"
                                    variant="ghost"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        setEditTest(test);
                                    }}
                                >
                                    Редагувати
                                </Button>
                                <Button
                                    variant="destructive"
                                    className="cursor-pointer"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        setTestToDelete(test);
                                    }}
                                >
                                    Видалити
                                </Button>
                            </div>
                        </div>
                    </div>
                </div>
            ))}
            <div
                className="w-full bg-[#C2D3FD] hover:bg-[#abc3ff] cursor-pointer rounded-3xl p-4 flex items-center gap-8 transition hover:scale-[1.005]"
                onClick={() => {
                    setEditTest({ id: 0 });
                    dispatch(clearPlanetErrors());
                }}
            >
                <img src="/add_circle.png" alt="Add" />
                <div className="text-xl">Додати тест</div>
            </div>

            {testToDelete && (
                <DeleteTestModalComponent
                    test={testToDelete}
                    onConfirm={() => handleDelete(testToDelete)}
                    onCancel={() => setTestToDelete(null)}
                />
            )}
        </div>
    );
};

export default TestsComponent;