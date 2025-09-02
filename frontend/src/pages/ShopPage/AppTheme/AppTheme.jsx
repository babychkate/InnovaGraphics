import { Button } from '@/components/ui/button';
import React, { useState } from 'react';
import TryAppThemeModal from './TryAppThemeModal/TryAppThemeModal';

const themes = [
    { id: 1, name: 'Sun empire', price: 90, image: '/app-theme/app-theme1.png' },
    { id: 2, name: 'Nebula Galaxy', price: 90, image: '/app-theme/app-theme2.png' },
    { id: 3, name: 'Yellow Space', price: 150, image: '/app-theme/app-theme3.png' },
    { id: 4, name: 'Starry Background', price: 150, image: '/app-theme/app-theme4.png' },
    { id: 5, name: 'Cold World', price: 150, image: '/app-theme/app-theme5.png' },
    { id: 6, name: 'Pink Space', price: 150, image: '/app-theme/app-theme6.png' },
    { id: 7, name: 'Sand Infinity', price: 150, image: '/app-theme/app-theme7.png' },
    { id: 8, name: 'Snow Palace', price: 150, image: '/app-theme/app-theme8.png' },
];

const AppTheme = () => {
    const [selectedTheme, setSelectedTheme] = useState(null);

    const handleTryClick = (theme) => {
        setSelectedTheme(theme);
    };

    const handleCloseModal = () => {
        setSelectedTheme(null);
    };

    return (
        <>
            <div className='min-h-[calc(100vh-141px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
                <h1 className='text-2xl font-semibold text-center'>ФОН ПРОГРАМИ</h1>
                <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 px-4'>
                    {themes.map(theme => (
                        <div
                            key={theme.id}
                            className='relative bg-[#C2D3FD] rounded-2xl shadow-lg p-4 flex flex-col items-center justify-between'
                        >
                            <div className='absolute top-3 right-3 text-black text-sm flex items-center gap-2 font-bold px-2 py-1 rounded'>
                                {theme.price}
                                <img src="/coin.png" alt="Coin" className='w-7 h-7' />
                            </div>

                            <img
                                src={theme.image}
                                alt={theme.name}
                                className='w-40 h-40 object-cover rounded-lg'
                            />

                            <h2 className='text-lg font-semibold mt-4'>{theme.name}</h2>

                            <div className='flex gap-2 mt-4'>
                                <Button className="text-sm px-10 py-2 cursor-pointer" onClick={() => handleTryClick(theme)}>Спробувати</Button>
                                <Button className="text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer">Купити</Button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>

            {selectedTheme && (
                <TryAppThemeModal theme={selectedTheme} onClose={handleCloseModal} />
            )}
        </>
    );
}

export default AppTheme;