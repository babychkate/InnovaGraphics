import { Button } from '@/components/ui/button';
import React from 'react';

const resources = [
    { id: 1, count: 100, price: 150 },
    { id: 2, count: 150, price: 1000 },
    { id: 3, count: 5000, price: 4500 },
    { id: 4, count: 1, price: 5000 },
    { id: 5, count: 5, price: 15000 },
];

const BuyResource = () => {
    return (
        <div className='min-h-[calc(100vh-141px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
            <h1 className='text-2xl font-semibold text-center'>ПОПОВНІТЬ РЕСУРСИ</h1>
            <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 px-4'>
                {resources.map(resource => (
                    <div
                        key={resource.id}
                        className='relative bg-[#C2D3FD] rounded-2xl shadow-lg p-4 flex flex-col items-center justify-between'
                    >
                        <div className='absolute top-3 right-3 text-black text-sm flex items-center gap-2 font-bold px-2 py-1 rounded'>
                            {resource.price}
                            <img src="/coin.png" alt="Coin" className='w-7 h-7' />
                        </div>

                        <div className="flex-1 flex items-center justify-center w-full p-20">
                            <div className='flex items-center justify-center gap-4'>
                                <img src="/plus-circle.png" alt="Plus" />
                                <h2 className='text-2xl'>{resource.count}</h2>
                                <img src="/energy.png" alt="Energy" />
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

export default BuyResource;