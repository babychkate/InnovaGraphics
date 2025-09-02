import AuthPlanetsCanvas from '@/components/AuthPageComponents/AuthPlanetsCanvas';
import ForgotPasswordComponent from '@/components/AuthPageComponents/ForgotPasswordComponent';
import React from 'react';

const ForgotPasswordPage = () => {
    return (
        <div className="w-screen h-screen flex justify-center items-center">
            <div className="w-1/3 h-full">
                <AuthPlanetsCanvas />
            </div>
            <div className="w-3/4 h-full flex items-center justify-center">
                <ForgotPasswordComponent />
            </div>
        </div>
    );
}

export default ForgotPasswordPage;