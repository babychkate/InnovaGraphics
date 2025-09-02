import HtmlRunner from '@/components/HtmlRunner/HtmlRunner';
import PlanetBackground from '@/components/PlanetBackground/PlanetBackground';
import { Button } from '@/components/ui/button';
import { getTestBySubtopic, startTest } from '@/redux/test/Action';
import { getTheoryById } from '@/redux/theory/Action';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate, useParams } from 'react-router-dom';

const MatricesInfoPage = () => {
    const user = useSelector(state => state.auth.user);
    const test = useSelector(state => state.test.test);
    const theory = useSelector(state => state.theory.theory);
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const { subtopic } = useParams();

    useEffect(() => {
        if (!subtopic) return;

        switch (subtopic) {
            case 'geom-figures':
                dispatch(getTheoryById("E1E003A2-C8E3-43C5-AD13-2DAB72C5956F"));
                break;
            case 'matrixes':
                dispatch(getTheoryById("8D986C8C-73D9-44FE-89BE-A79230648153"));
                break;
            case 'vectors':
                dispatch(getTheoryById("2CF1AA59-8E12-40CD-ADE3-5D5117411C87"));
                break;
            case 'polynomials':
                dispatch(getTheoryById("6D6EC420-AC2B-47DD-95A8-89B81BBB0172"));
                break;
            case 'topological-dimension':
                dispatch(getTheoryById("01A58DD0-C2A3-4C17-B882-322E435913AE"));
                break;
            case 'cubic-curves':
                dispatch(getTheoryById("5E048755-7A79-41A8-AC4F-3C6F8B887489"));
                break;
            case 'complex-numbers':
                dispatch(getTheoryById("57A35715-81C4-46B6-9AD0-DD5F10067DA0"));
                break;
            default:
                console.warn(`Невідомий subtopic: ${subtopic}`);
        }
    }, [dispatch, subtopic]);

    const handleStartTest = async () => {
        if (test?.id) {
            await dispatch(startTest(test?.id, { userEmail: user?.email }));
            navigate(`/info-planet/${subtopic}/quiz/${quizId}`);
        }
    };

    return (
        <div className='relative w-screen h-screen text-white'>
            <PlanetBackground />

            <div className="absolute inset-20 bg-[#D8E3FF] p-6 max-w-full max-h-full rounded-2xl mx-auto overflow-auto">
                <div className='mb-4'>
                    <Link to="/info-planet" className="text-2xl font-bold text-blue-400 underline">
                        Лінійна алгебра / Матриці
                    </Link>
                </div>

                <div className="bg-white text-black p-10 rounded-3xl shadow-lg w-full">
                    {theory && <HtmlRunner content={theory} />}
                </div>

                <div className="flex justify-end mt-6">
                    <Button
                        className="text-sm px-10 py-2"
                        onClick={handleStartTest}
                        disabled={test?.isCompleted}
                    >
                        До тесту <span className="ml-2">→</span>
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default MatricesInfoPage;