import { Button } from '@/components/ui/button';
import React from 'react';

const hints = [
    { id: 1, count: 1, price: 400, description: "Практичне завдання 'Лаба 1'" },
    { id: 2, count: 2, price: 900, description: "Практичне завдання 'Лаба 1'" },
    { id: 3, count: 1, price: 450, description: "Практичне завдання 'Лаба 2'" },
    { id: 4, count: 2, price: 1000, description: "Практичне завдання 'Лаба 2'" },
    { id: 5, count: 1, price: 600, description: "Практичне завдання 'Лаба 3'" },
    { id: 6, count: 2, price: 1200, description: "Практичне завдання 'Лаба 4'" },
    { id: 7, count: 1, price: 1000, description: "Практичне завдання 'Лаба 5'" },
];

const BuyHint = () => {
    return (
        <div className='min-h-[calc(100vh-141px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
            <h1 className='text-2xl font-semibold text-center'>ПІДКАЗКИ ДО ЗАВДАНЬ</h1>
            <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 px-4'>
                {hints.map(hint => (
                    <div
                        key={hint.id}
                        className='relative bg-[#C2D3FD] rounded-2xl shadow-lg p-4 flex flex-col items-center justify-between'
                    >
                        <div className='absolute top-3 right-3 text-black text-sm flex items-center gap-2 font-bold px-2 py-1 rounded'>
                            {hint.price}
                            <img src="/coin.png" alt="Coin" className='w-7 h-7' />
                        </div>

                        <div className="flex-1 flex items-center justify-center flex-col text-center gap-4 w-full p-20">
                            <div className='flex items-center justify-center gap-4'>
                                <img src="/plus-circle.png" alt="Plus" />
                                <h2 className='text-2xl'>{hint.count}</h2>
                                <img src="/hint.png" alt="Hint" />
                            </div>
                            <div>
                                <p>{hint.description}</p>
                            </div>
                        </div>

                        <Button className="mt-4 text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer">
                            Купити
                        </Button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default BuyHint;