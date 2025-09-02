import { Button } from '@/components/ui/button';
import { ArrowLeft } from 'lucide-react';
import React from 'react';

const CaseDetails = ({ testCase, onBack }) => {
    return (
        <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex items-center justify-center w-full max-w-3xl">
                <div className="flex flex-col items-start gap-4 bg-white rounded-xl p-8 w-full">
                    <h2 className="text-2xl"><span className="font-bold">Вхідні дані:</span> {testCase?.input}</h2>
                    <h2 className="text-2xl"><span className="font-bold">Очікувані дані:</span> {testCase?.output}</h2>
                </div>
            </div>
        </div>
    );
};

export default CaseDetails;