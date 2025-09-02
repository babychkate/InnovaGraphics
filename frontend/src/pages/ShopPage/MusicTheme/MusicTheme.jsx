import { Button } from '@/components/ui/button';
import React, { useState } from 'react';
import AddMusicThemeModal from './AddMusicThemeModal/AddMusicThemeModal';

const themes = [
    { id: 1, name: 'Interstellar - Hans Zimmer - Main Theme', price: 100, image: '/app-theme/app-theme1.png' },
    { id: 2, name: 'Interstellar - Hans Zimmer - Main Theme', price: 50, image: '/app-theme/app-theme2.png' },
    { id: 3, name: 'Interstellar - Hans Zimmer - Main Theme', price: 150, image: '/app-theme/app-theme3.png' },
    { id: 4, name: 'Interstellar - Hans Zimmer - Main Theme', price: 100, image: '/app-theme/app-theme4.png' },
    { id: 5, name: 'Interstellar - Hans Zimmer - Main Theme', price: 200, image: '/app-theme/app-theme5.png' },
];

const MusicTheme = () => {
    const [selectedTheme, setSelectedTheme] = useState(null);

    const handleBuy = (theme) => {
        setSelectedTheme(theme);
    };

    const handleCloseModal = () => {
        setSelectedTheme(null);
    };

    return (
        <>
            <div className='min-h-[calc(100vh-141px)] bg-[#85A7FA] flex flex-col gap-4 py-8 px-4'>
                <h1 className='text-2xl font-semibold text-center'>МУЗИЧНІ ТЕМИ</h1>

                {themes.map((theme) => (
                    <div
                        key={theme.id}
                        className="w-full bg-[#C2D3FD] rounded-4xl shadow-md flex items-center justify-between px-6 py-4 gap-4"
                    >
                        <div className="flex items-center gap-4">
                            <img src={theme.image} alt={theme.name} className="w-16 h-16 rounded-md object-cover" />
                            <span className="text-lg font-medium">{theme.name}</span>
                        </div>
                        <div className='flex items-center justify-between gap-3'>
                            <div className="flex items-center gap-2 text-md font-bold">
                                {theme.price}
                                <img src="/coin.png" alt="coin" className="w-6 h-6" />
                            </div>
                            <div className='flex items-center justify-between gap-2 '>
                                <Button className="text-sm px-10 py-2 cursor-pointer">Спробувати</Button>
                                <Button
                                    className="text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer"
                                    onClick={() => handleBuy(theme)}
                                >
                                    Купити
                                </Button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>

            {selectedTheme && (
                <AddMusicThemeModal theme={selectedTheme} onClose={handleCloseModal} />
            )}
        </>
    );
}

export default MusicTheme;