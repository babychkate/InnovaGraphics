import React, { useEffect, useState } from 'react';
import CaseDetails from './CaseDetails/CaseDetails';
import AddEditCaseComponent from './AddEditCaseComponent/AddEditCaseComponent';
import DeleteCaseModalComponent from './DeleteCaseModalComponent/DeleteCaseModalComponent';
import { deleteCase, getAllCases } from '@/redux/case/Action';
import { useDispatch, useSelector } from 'react-redux';
import { Button } from '@/components/ui/button';

const CasesComponent = () => {
    const dispatch = useDispatch();

    const [selectedCase, setSelectedCase] = useState(null);
    const [editCase, setEditCase] = useState(null);
    const [caseToDelete, setCaseToDelete] = useState(null);

    const cases = useSelector(state => state.case.cases);

    const handleDelete = (testCase) => {
        dispatch(deleteCase(testCase.id));
        setCaseToDelete(null);
    };

    useEffect(() => {
        dispatch(getAllCases());
    }, [dispatch]);

    if (selectedCase) {
        return (
            <CaseDetails
                testCase={selectedCase}
                onBack={() => setSelectedCase(null)}
            />
        );
    }

    if (editCase !== null) {
        return (
            <AddEditCaseComponent
                testCase={editCase}
                onBack={() => setEditCase(null)}
            />
        );
    }

    return (
        <div className="flex flex-col gap-6 px-10 py-6">
            <h1 className="text-2xl font-bold mb-4">Теоретичні матеріали</h1>

            <div className="flex flex-col gap-4">
                <div className="grid grid-cols-1 gap-4">
                    <div className="w-full grid grid-cols-3 items-center gap-4">
                        <div className="font-bold">Вхідні дані</div>
                        <div className="font-bold">Очікувані дані</div>
                        <div></div>
                    </div>
                    {cases.map((c) => (
                        <div key={c.id} className="group cursor-pointer" onClick={() => setSelectedCase(c)}>
                            <div className="bg-[#C2D3FD] p-4 rounded-3xl shadow-md transition hover:scale-[1.01]">
                                <div className="grid grid-cols-3 gap-4 items-center">
                                    <div>
                                        <h2 className="text-lg font-semibold">{c.input}</h2>
                                    </div>

                                    <div>
                                        <h2 className="text-lg font-semibold">{c.output}</h2>
                                    </div>

                                    <div className="flex gap-2">
                                        <Button
                                            className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer text-white hover:text-white"
                                            variant="ghost"
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                setEditCase(c);
                                            }}
                                        >
                                            Редагувати
                                        </Button>
                                        <Button
                                            variant="destructive"
                                            className="cursor-pointer"
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                setCaseToDelete(c);
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
                        className="w-full bg-[#C2D3FD] hover:bg-[#abc3ff] cursor-pointer rounded-4xl p-4 flex items-center gap-8 transition hover:scale-[1.005]"
                        onClick={() => {
                            setEditCase({ id: 0 });
                        }}
                    >
                        <img src="/add_circle.png" alt="Add" />
                        <div className="text-xl">Додати новий тестовий сценарій</div>
                    </div>
                </div>
            </div>

            {caseToDelete && (
                <DeleteCaseModalComponent
                    onConfirm={() => handleDelete(caseToDelete)}
                    onCancel={() => setCaseToDelete(null)}
                />
            )}
        </div>
    );
}

export default CasesComponent;