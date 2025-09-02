import { Button } from '@/components/ui/button';
import { X } from 'lucide-react';
import React from 'react';

const GetTaskResultComponent = ({ task, onClose }) => {
    return (
        <div className="fixed inset-0 flex justify-center items-center z-50 bg-black/50">
            <div
                className="w-[33%] h-[30%] bg-white p-5 rounded-2xl flex flex-col justify-between translate-y-5"
            >
                <div className="p-4 flex justify-end bg-white bg-opacity-80 z-10">
                    <Button
                        variant="ghost"
                        onClick={() => onClose()}
                        className="absolute top-4 right-4 z-10 cursor-pointer hover:text-red-500"
                    >
                        <X size={30} />
                    </Button>
                </div>


                <div className="flex-1 flex items-center flex-col text-center text-xl font-semibold text-gray-800">
                    <h1 className='mb-3'>Завдання успішно виконано!</h1>
                    <div className='flex gap-2'>Ваша нагорода:
                        <div className=' text-black font-semibold flex gap-2'>
                            {task.result}
                            <img src="/coin.png" alt="Coin" className='w-6 h-6' />
                        </div>
                    </div>
                </div>

                <div className="p-4 flex justify-end bg-white bg-opacity-80 z-10">
                    <Button
                        onClick={() => onClose()}
                        className="px-6 py-2 rounded-lg bg-blue-500 text-white hover:bg-blue-600 transition cursor-pointer"
                    >
                        Ок
                    </Button>
                </div>
            </div>
        </div>
    );
}

export default GetTaskResultComponent;