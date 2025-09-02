import BigPlanets from '@/components/BigPlanets/BigPlanets';
import MoveBack from '@/components/MoveBack/MoveBack';
import { Button } from '@/components/ui/button';
import { getUserCertificate } from '@/redux/certificate/Action';
import { OrbitControls } from '@react-three/drei';
import { Canvas } from '@react-three/fiber';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { saveAs } from 'file-saver';

const CertificatePage = () => {
    const dispatch = useDispatch();
    const [isCertificateAvailable, setIsCertificateAvailable] = useState(true);

    const userId = useSelector(state => state.auth.user?.id);
    const certificate = useSelector(state => state.certificate.certificate);
    const error = useSelector(state => state.certificate.errors);

    useEffect(() => {
        if (userId) {
            dispatch(getUserCertificate(userId));
        }
    }, [dispatch, userId]);

    useEffect(() => {
        if (error) {
            setIsCertificateAvailable(false);
        } else {
            setIsCertificateAvailable(true);
        }
    }, [error]);

    const handleDownload = async (e) => {
        e.preventDefault();
        if (!certificate?.imageUrl) return;

        try {
            const response = await fetch(certificate.imageUrl, {
                method: 'GET',
                credentials: 'include',
            });

            if (!response.ok) {
                throw new Error('Не вдалося завантажити файл');
            }

            const blob = await response.blob();
            saveAs(blob, 'certificate.png');
        } catch (error) {
            console.error('Помилка при завантаженні сертифіката:', error);
        }
    };

    return (
        <div className="relative h-screen overflow-hidden pt-[72px]">
            <MoveBack to="/my-profile" />

            {isCertificateAvailable && certificate
                ?
                <>
                    <div className='flex items-center justify-center'>
                        <div className='relative z-10 flex flex-col items-end justify-center h-[calc(100vh-100px)] gap-4'>
                            <img src={`${certificate?.imageUrl}`} alt="Your certificate" className='w-[800px]' />

                            <Button
                                className='text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer'
                                onClick={(e) => handleDownload(e)}
                            >
                                Завантажити сертифікат
                            </Button>
                        </div>
                    </div>

                    <div className="absolute top-0 left-0 w-full h-full z-0">
                        <Canvas>
                            <directionalLight position={[-10, -10, -10]} intensity={1} />
                            <ambientLight />
                            <BigPlanets />
                            <OrbitControls />
                        </Canvas>
                    </div>
                </>
                :
                <div className='relative z-10 flex items-center justify-center flex-col h-[calc(100vh-216px)] text-center px-4'>
                    <h1 className='text-5xl font-bold mb-4'>Наразі Ваш сертифікат недоступний</h1>
                    <h2 className='text-xl text-gray-600'>{error || "Для отримання сертифікату Вам потрібно пройти усі планети"}</h2>
                </div>
            }
        </div>
    );
};

export default CertificatePage;