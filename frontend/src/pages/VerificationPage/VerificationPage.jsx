import AcceptVerificationCode from '@/components/AuthPageComponents/AcceptVerificationCode';
import AuthPlanetsCanvas from '@/components/AuthPageComponents/AuthPlanetsCanvas';
import React from 'react';

const VerificationPage = () => {
    return (
        <div className="w-screen h-screen flex justify-center items-center">
            <div className="w-1/3 h-full">
                <AuthPlanetsCanvas />
            </div>
            <div className="w-3/4 h-full flex items-center justify-center">
                <AcceptVerificationCode />
            </div>
        </div>
    );
}

export default VerificationPage;