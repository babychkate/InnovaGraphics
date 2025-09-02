import { Button } from '@/components/ui/button';
import { ArrowLeft } from 'lucide-react';
import React from 'react';

const AvatarDetails = ({ avatar, onBack }) => {
    return (
        <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex items-center justify-between gap-30">
                <div className="flex flex-col items-start gap-4">
                    <h2 className="text-2xl"><span className='font-bold'>Назва:</span> {avatar?.name}</h2>
                    <p className="text-gray-600 font-medium"><span className='font-bold'>Тип:</span> Аватар</p>
                    <p className="text-gray-600 font-medium"><span className='font-bold'>Ціна:</span> {avatar?.price}</p>
                </div>
                <div className="h-90 w-90 flex items-center justify-center">
                    <img src={avatar?.photoPath} alt="Image" />
                </div>
            </div>
        </div>
    );
}

export default AvatarDetails;