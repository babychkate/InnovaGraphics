import React, { useEffect, useState } from 'react';
import { Link, useParams, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import PlanetBackground from '@/components/PlanetBackground/PlanetBackground';
import SmallPlanets from '@/components/SmallPlanets/SmallPlanets';
import UserAssistant from '@/components/UserAssistant/UserAssistant';
import { Canvas, useFrame } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import { Button } from '@/components/ui/button';
import { completeTest, getTestWithQuestions } from '@/redux/test/Action';
import { reportTestCompleted } from '@/services/signalRService';

const assistantPhrases = [
    "Keep going, you're doing great!",
    "Don't stop now!",
    "Well done! More fun ahead!",
    "You're on the right track!",
];

const CameraLogger = () => {
    useFrame(({ camera }) => {
        // console.log("Camera Position:", camera.position.x, camera.position.y, camera.position.z);
    });
    return null;
};

const MatrixQuizPage = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { quizId } = useParams();
    const questionsFromRedux = useSelector(state => state.test.questions);
    const loading = useSelector(state => state.test.loading);
    const test = useSelector(state => state.test.test);
    const user = useSelector(state => state.auth.user);
    const battleId = useSelector(state => state.test.battleId);

    const [localQuestions, setLocalQuestions] = useState([]);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [answered, setAnswered] = useState([]);
    const [assistantText, setAssistantText] = useState('');
    const [remainingTime, setRemainingTime] = useState(null);

    useEffect(() => {
        if (quizId) {
            dispatch(getTestWithQuestions(quizId));
        }
    }, [quizId, dispatch]);

    useEffect(() => {
        if (questionsFromRedux.length) {
            setLocalQuestions(questionsFromRedux.map(q => ({ ...q })));
        }
    }, [questionsFromRedux]);

    useEffect(() => {
        if (!test?.additionalData?.startTime || !test?.additionalData?.timeLimit) return;

        const start = new Date(test.additionalData.startTime);
        const [h, m, s] = test.additionalData.timeLimit.split(':').map(Number);
        const end = new Date(start.getTime() + h * 3600000 + m * 60000 + s * 1000);

        const updateTimer = () => {
            const now = new Date();
            const diff = end - now;
            if (diff <= 0) {
                setRemainingTime("00:00");
                clearInterval(intervalId);
                handleCompleteTest();
            } else {
                const minutes = String(Math.floor(diff / 60000)).padStart(2, '0');
                const seconds = String(Math.floor((diff % 60000) / 1000)).padStart(2, '0');
                setRemainingTime(`${minutes}:${seconds}`);
            }
        };

        updateTimer();
        const intervalId = setInterval(updateTimer, 1000);
        return () => clearInterval(intervalId);
    }, [test]);

    const handleAnswerSelect = (questionId, answerId) => {
        setLocalQuestions(prev =>
            prev.map(q =>
                q.id === questionId ? { ...q, selectedAnswerId: answerId } : q
            )
        );
        if (!answered.includes(currentIndex)) {
            setAnswered(prev => [...prev, currentIndex]);
        }
    };

    const handleCompleteTest = () => {
        const userAnswers = localQuestions.reduce((acc, q) => {
            acc[q.id] = q.selectedAnswerId || null;
            return acc;
        }, {});

        const data = {
            userEmail: user.email,
            userAnswers,
            endTime: new Date().toISOString(),
        };

        dispatch(completeTest(quizId, data));
        console.log("Test completed with data:", test, data);
        if (test?.planetId == null) {
            reportTestCompleted(battleId, test?.additionalData?.competitionTime || 0);
        }

        navigate('./result');
    };

    const handleSelect = (index) => {
        setCurrentIndex(index);
        if (!answered.includes(index)) {
            setAnswered(prev => [...prev, index]);
        }
    };

    const handleNext = () => {
        if (currentIndex < localQuestions.length - 1) {
            const randomPhrase = assistantPhrases[Math.floor(Math.random() * assistantPhrases.length)];
            setAssistantText(randomPhrase);
            setCurrentIndex(currentIndex + 1);
        }
    };

    const handlePrev = () => {
        if (currentIndex > 0) {
            setCurrentIndex(currentIndex - 1);
        }
    };

    const isFirst = currentIndex === 0;
    const isLast = currentIndex === localQuestions.length - 1;
    const currentQuestion = localQuestions[currentIndex];

    if (loading || !localQuestions.length) {
        return (
            <div className="w-screen h-screen flex items-center justify-center text-white text-2xl">
                Loading quiz...
            </div>
        );
    }

    return (
        <div className='relative w-screen h-screen text-white'>
            <PlanetBackground />

            <div className="absolute inset-20 bg-[#D8E3FF] p-6 max-w-full max-h-full rounded-2xl mx-auto overflow-auto">
                <div className='mb-4'>
                    <Link to="/info-planet" className="text-2xl font-bold text-blue-400 underline">
                        {test?.theme}
                    </Link>
                </div>

                <div className='w-full bg-white text-black p-6 text-xl rounded-xl grid gap-8'>
                    <div className="grid grid-cols-3 gap-6">
                        {/* Question index panel */}
                        <div className="col-span-1 bg-gray-100 p-4 rounded-lg shadow">
                            <h2 className="text-lg font-bold mb-2">Questions</h2>
                            <div className="grid grid-cols-7 gap-2">
                                {localQuestions.map((q, index) => {
                                    let bg = "bg-gray-200";
                                    let text = "text-black";
                                    if (index < currentIndex && answered.includes(index)) {
                                        bg = "bg-gray-600";
                                        text = "text-white";
                                    } else if (index === currentIndex) {
                                        bg = "bg-blue-500";
                                        text = "text-white";
                                    }
                                    return (
                                        <div
                                            key={q.id}
                                            onClick={() => handleSelect(index)}
                                            className={`${bg} ${text} w-[40px] h-[40px] flex items-center justify-center rounded-md text-sm cursor-pointer hover:opacity-90`}
                                            title={`Question ${index + 1}`}
                                        >
                                            {index + 1}
                                        </div>
                                    );
                                })}
                            </div>
                        </div>

                        {/* Question content */}
                        <div className="col-span-2 bg-blue-50 p-4 rounded-lg shadow relative">
                            <div className="absolute top-0 right-0 m-2 text-2xl font-bold text-blue-700">
                                {remainingTime !== null ? `${remainingTime}` : '...'}
                            </div>

                            <h2 className="text-lg font-bold mb-4">{currentQuestion?.text}</h2>

                            <div className="grid gap-3">
                                {currentQuestion?.answers?.map((answer) => (
                                    <button
                                        key={answer.id}
                                        onClick={() => handleAnswerSelect(currentQuestion.id, answer.id)}
                                        className={`w-full text-left border rounded-md px-4 py-2 transition ${currentQuestion?.selectedAnswerId === answer.id
                                            ? 'bg-blue-500 text-white'
                                            : 'bg-white border-blue-300 hover:bg-blue-100'
                                            }`}
                                    >
                                        {answer.text}
                                    </button>
                                ))}
                            </div>

                            <div className='flex justify-between mt-6'>
                                <Button size="lg" onClick={handlePrev} disabled={isFirst} className="cursor-pointer">
                                    Back
                                </Button>
                                {isLast ? (
                                    <Button
                                        size="lg"
                                        onClick={handleCompleteTest}
                                        className="bg-blue-600 text-white hover:bg-blue-700 cursor-pointer"
                                    >
                                        Завершити тест
                                    </Button>
                                ) : (
                                    <Button size="lg" onClick={handleNext} className="cursor-pointer">
                                        Next
                                    </Button>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Canvas planets */}
            <div className='absolute bottom-0 left-0 w-[400px] h-[400px]'>
                <Canvas camera={{ position: [5.15, 3.13, -3.55] }}>
                    <directionalLight position={[10, 10, 10]} intensity={1} />
                    <ambientLight />
                    <SmallPlanets />
                    <CameraLogger />
                    <OrbitControls />
                </Canvas>
            </div>

            {currentIndex > 0 && (
                <UserAssistant top="85px" right="105px" text={assistantText} />
            )}
        </div>
    );
};

export default MatrixQuizPage;