import { Button } from '@/components/ui/button';
import { X } from 'lucide-react';
import React from 'react';

const TryBuyAvatar = ({ avatar, onClose }) => {
    return (
        <div className="fixed inset-0 flex justify-center items-center z-50 bg-black/50">
            <div
                className="relative w-[60%] h-[60%] bg-white p-5 rounded-2xl flex flex-col justify-center items-center gap-5 translate-y-10"
            >
                <Button
                    variant="ghost"
                    onClick={() => onClose()}
                    className="absolute top-4 right-4 z-10 hover:text-red-500"
                >
                    <X size={30} />
                </Button>
                <div className="flex justify-center items-center flex-col gap-3">
                    <img
                        src={avatar?.photoPath}
                        alt="Avatar"
                        className="max-h-full max-w-full object-contain"
                    />
                    <h1 className="text-center text-2xl font-semibold">{avatar?.name}</h1>
                </div>
                <Button
                    onClick={() => onClose()}
                    className="absolute bottom-4 right-4 px-6 py-2 rounded-lg bg-blue-500 text-white hover:bg-blue-600 transition cursor-pointer"
                >
                    Ок
                </Button>
            </div>
        </div>
    );
}

export default TryBuyAvatar;