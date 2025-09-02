import AuthPlanetsCanvas from '@/components/AuthPageComponents/AuthPlanetsCanvas';
import ConfirmPasswordComponent from '@/components/AuthPageComponents/ConfirmPasswordComponent';
import React from 'react';

const ConfirmPasswordPage = () => {
    return (
        <div className="w-screen h-screen flex justify-center items-center">
            <div className="w-1/3 h-full">
                <AuthPlanetsCanvas />
            </div>
            <div className="w-3/4 h-full flex items-center justify-center">
                <ConfirmPasswordComponent />
            </div>
        </div>
    );
}

export default ConfirmPasswordPage;