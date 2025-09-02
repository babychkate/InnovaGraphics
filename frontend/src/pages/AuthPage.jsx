import React from 'react';
import RegisterLoginCard from '../components/AuthPageComponents/RegisterLoginCard';
import AuthPlanetsCanvas from '@/components/AuthPageComponents/AuthPlanetsCanvas';

const AuthPage = () => {
    return (
        <div className="w-screen h-screen flex justify-center items-center">
            <div className="w-1/3 h-full">
                <AuthPlanetsCanvas />
            </div>
            <div className="w-3/4 h-full flex items-center justify-center">
                <RegisterLoginCard />
            </div>
        </div>
    );
}

export default AuthPage;