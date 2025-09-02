import HtmlRunner from '@/components/HtmlRunner/HtmlRunner';
import { Button } from '@/components/ui/button';
import { getTestByPlanetId, startTest } from '@/redux/test/Action';
import { getTheoryByPlanetId } from '@/redux/theory/Action';
import { ArrowLeft } from 'lucide-react';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';

const PlanetMaterialPage = () => {
    const user = useSelector(state => state.auth.user);
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const theory = useSelector(state => state.theory.theory);
    const test = useSelector(state => state.test.test);
    const { planetId } = useParams();

    console.log(test);

    useEffect(() => {
        dispatch(getTheoryByPlanetId(planetId));
        dispatch(getTestByPlanetId(planetId));
    }, [dispatch, planetId]);

    const handleNext = () => {
        if (test?.id) {
            dispatch(startTest(test?.id, { userEmail: user?.email }));
            navigate(`/info-planet/1/quiz/${test?.id}`);
        }
    };

    return (
        <div className="pt-[72px] min-h-screen bg-black p-10 relative">
            <Button
                variant="ghost"
                onClick={() => navigate(`/planets/${planetId}/exercise`)}
                className='absolute top-20 left-10 z-20 p-2 hover:bg-white/50 rounded-full transition-all duration-300 ease-in-out'
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="bg-white text-black p-10 rounded-3xl shadow-lg w-full">
                {theory && <HtmlRunner content={theory} />}
            </div>

            <div className="flex justify-end mt-6">
                <Button
                    className="text-sm px-10 py-2 cursor-pointer"
                    onClick={handleNext}
                >
                    До тесту {/* СТРІЛКА */}
                </Button>
            </div>
        </div>
    );
}

export default PlanetMaterialPage;