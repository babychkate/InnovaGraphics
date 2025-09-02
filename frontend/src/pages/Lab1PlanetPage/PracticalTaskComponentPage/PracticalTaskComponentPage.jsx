import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { clearOutput, runCode, runLab1Code, runLab3Code, runLab4Code, runLab5Code, testCode } from '@/redux/code/Action';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectTrigger,
    SelectValue
} from "@/components/ui/select";
import { Button } from '@/components/ui/button';
import Editor from "@monaco-editor/react";
import BezierCurve from './BezierCurve/BezierCurve';
import { useNavigate, useParams } from 'react-router-dom';
import ColorModels from './ColorModels/ColorModels';
import TwoDimensionalShapes from './TwoDimensionalShapes/TwoDimensionalShapes';
import Fractals from './Fractals/Fractals';
import AffineTransformation from './AffineTransformation/AffineTransformation';
import { getAllCasesByExerciseId } from '@/redux/case/Action';
import { Link, CheckCircle2, XCircle } from 'lucide-react';

const PracticalTaskComponentPage = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { planetId, taskId } = useParams();
    const [language, setLanguage] = useState('javascript');
    const [code, setCode] = useState('');

    const rawPoints = useSelector((state) => state.code.points);
    const outputText = useSelector((state) => state.code.outputText);
    const image = useSelector(state => state.image);

    const [inputPoints, setInputPoints] = useState([]);

    const cases = useSelector((state) => state.case.cases);
    const caseResults = useSelector(state => state.code.caseResults);
    const listErrors = useSelector(state => state.code.listErrors);

    console.log("Case Results:", caseResults);
    console.log("List Errors:", listErrors);

    useEffect(() => {
        dispatch(clearOutput());
    }, [dispatch]);

    useEffect(() => {
        dispatch(getAllCasesByExerciseId(taskId));
    }, [dispatch, taskId]);

    console.log("Cases:", cases);

    const handleRun = () => {
        switch (planetId) {
            case "03A146EB-D2D9-4283-BC2B-031F026AA7FA":
                dispatch(runLab1Code({
                    sourceCode: code,
                    taskType: "TwoDimensionalShapes",
                    exerciseId: taskId
                }));
                break
            case "AC4B8F5B-DB7B-44BA-96B2-6AE26BC85F3C":
                dispatch(runCode({
                    sourceCode: code,
                    taskType: "RecursiveBezierTest",
                    exerciseId: taskId
                }));
                break;
            case "3895D417-4178-4E3D-80AC-7DA8C1785899":
                dispatch(runLab3Code({
                    sourceCode: code,
                    taskType: "FractalTest",
                    exerciseId: taskId
                }));
                break;
            case "2DEF880A-D118-4D82-B00C-18C6E156225A":
                dispatch(runLab4Code({
                    sourceCode: code,
                    sourceImageBase64: image.imageBase64.split(",")[1],
                    taskType: "ImageProcessing",
                    exerciseId: taskId
                }));
                break;
            case "A30E1F22-6D93-4983-9CDC-B6F8902E8730":
                dispatch(runLab5Code({
                    sourceCode: code,
                    taskType: "AffineTransformationTest",
                    exerciseId: taskId,
                    inputPoints: inputPoints
                }));
                break;
            default:
                alert("Функція запуску не підтримується для цієї планети.");
                break;
        }
    };

    const handleTest = () => {
        switch (planetId) {
            case "03A146EB-D2D9-4283-BC2B-031F026AA7FA":
                dispatch(testCode({
                    sourceCode: code,
                    taskType: "TwoDimensionalShapes",
                    exerciseId: taskId
                }));
                break
            case "AC4B8F5B-DB7B-44BA-96B2-6AE26BC85F3C":
                dispatch(testCode({
                    sourceCode: code,
                    taskType: "RecursiveBezierTest",
                    exerciseId: taskId
                }));
                break;
            case "3895D417-4178-4E3D-80AC-7DA8C1785899":
                dispatch(testCode({
                    sourceCode: code,
                    taskType: "FractalTest",
                    exerciseId: taskId
                }));
                break;
            case "2DEF880A-D118-4D82-B00C-18C6E156225A":
                dispatch(testCode({
                    sourceCode: code,
                    taskType: "ImageProcessing",
                    exerciseId: taskId
                }));
                break;
            case "A30E1F22-6D93-4983-9CDC-B6F8902E8730":
                dispatch(testCode({
                    sourceCode: code,
                    taskType: "AffineTransformationTest",
                    exerciseId: taskId
                }));
                break;
            default:
                alert("Функція тестування не підтримується для цієї планети.");
                break;
        }
    };

    const renderPlanetComponent = () => {
        switch (planetId) {
            case "03A146EB-D2D9-4283-BC2B-031F026AA7FA":
                return <TwoDimensionalShapes rawPoints={rawPoints?.filter(p => p.length === 2)} />
            case "AC4B8F5B-DB7B-44BA-96B2-6AE26BC85F3C":
                return <BezierCurve rawPoints={rawPoints?.filter(p => p.length === 2)} />;
            case "3895D417-4178-4E3D-80AC-7DA8C1785899":
                return <Fractals rawPoints={rawPoints?.filter(p => p.length === 2)} />
            case "2DEF880A-D118-4D82-B00C-18C6E156225A":
                return <ColorModels />;
            case "A30E1F22-6D93-4983-9CDC-B6F8902E8730":
                return <AffineTransformation setInputPoints={setInputPoints} />
            default:
                return <div className="text-center text-gray-500 mt-4">Немає візуалізації для цієї планети</div>;
        }
    };

    return (
        <div className="pt-[72px] flex h-screen gap-4">
            <div className="w-1/2 flex flex-col">
                {renderPlanetComponent()}

                {outputText && (
                    <div className="mt-4 bg-gray-100 p-4 rounded-xl text-sm font-mono">
                        <h2 className="font-bold mb-2">Output:</h2>
                        {outputText}
                    </div>
                )}
            </div>

            <div className="w-1/2 flex flex-col p-4">
                <div className='flex justify-between items-center mb-4'>
                    <h1 className='text-2xl font-bold'>Практичне завдання</h1>
                    <img src="/codesandbox.png" alt="Sandbox" className='w-5 h-5' />
                </div>
                <div className="mb-4">
                    <div className="flex gap-4 justify-between items-center">
                        <div className='flex justify-start items-center gap-4'>
                            <Select value={language} onValueChange={setLanguage}>
                                <SelectTrigger className="w-[180px]">
                                    <SelectValue placeholder="Мова" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectGroup>
                                        <SelectItem value="javascript">JavaScript</SelectItem>
                                        <SelectItem value="csharp">C#</SelectItem>
                                    </SelectGroup>
                                </SelectContent>
                            </Select>

                            <Button onClick={handleRun} className="text-sm px-10 py-2 cursor-pointer">
                                Run
                            </Button>
                            <Button onClick={handleTest} className="text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer">
                                Test
                            </Button>
                        </div>

                        <Button onClick={() => navigate("/")}>
                            Завершити практичне
                        </Button>
                    </div>
                </div>

                <div className="flex-1 bg-white rounded-2xl">
                    <Editor
                        height="100%"
                        language={language}
                        value={code}
                        onChange={(newCode) => setCode(newCode)}
                        theme="vs-dark"
                    />
                </div>

                <div className="mt-4">
                    <h2 className="font-semibold text-lg mb-2">Тестові кейси</h2>
                    {cases && cases.length > 0 ? (
                        <table className="w-full border border-gray-300 text-sm">
                            <thead>
                                <tr className="bg-gray-100">
                                    <th className="border p-2">#</th>
                                    <th className="border p-2">Вхідні дані</th>
                                    <th className="border p-2">Очікуваний результат</th>
                                    <th className="border p-2">Результат</th>
                                </tr>
                            </thead>
                            <tbody>
                                {cases.map((item, index) => {
                                    const isSuccess = caseResults?.[item.id] === true;
                                    // Знайдемо помилку, якщо є, по id кейса
                                    const errorMessage = listErrors?.find(err => err.includes(item.id)) || '';

                                    return (
                                        <tr key={item.id || index} className="hover:bg-gray-50">
                                            <td className="border p-2 text-center">{index + 1}</td>
                                            <td className="border p-2 whitespace-pre-wrap">{item.input}</td>
                                            <td className="border p-2 whitespace-pre-wrap">{item.output}</td>
                                            <td className="border p-2 whitespace-pre-wrap">
                                                <div className="flex items-center gap-2">
                                                    {isSuccess ? (
                                                        <>
                                                            <CheckCircle2 className="text-green-600" size={18} />
                                                            <span className="text-green-600">Успіх</span>
                                                        </>
                                                    ) : (
                                                        <>
                                                            <XCircle className="text-red-600" size={18} />
                                                            <span className="text-red-600">Помилка</span>
                                                        </>
                                                    )}
                                                </div>
                                                {errorMessage && (
                                                    <div className="mt-1 text-xs text-red-700 whitespace-pre-wrap">
                                                        {errorMessage}
                                                    </div>
                                                )}
                                            </td>
                                        </tr>
                                    );
                                })}
                            </tbody>
                        </table>
                    ) : (
                        <p className="text-gray-500">Немає тестових кейсів для цього завдання.</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default PracticalTaskComponentPage;