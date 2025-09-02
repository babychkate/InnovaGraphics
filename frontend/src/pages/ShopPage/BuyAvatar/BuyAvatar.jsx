import { Button } from '@/components/ui/button';
import { Plus } from 'lucide-react';
import React, { use, useEffect, useState } from 'react';
import TryBuyAvatar from './TryBuyAvatar/TryBuyAvatar';
import { useDispatch, useSelector } from 'react-redux';
import { buyAvatar, getAllAvatars } from '@/redux/shop_items/Action';
import { getCurrentUser } from '@/redux/auth/Action';

const BuyAvatar = () => {
    const dispatch = useDispatch();
    const user = useSelector(state => state.auth.user);
    const avatars = useSelector(state => state.shopItems.shopItems);
    const [selectedAvatar, setSelectedAvatar] = useState(null);

    console.log("User:", user);

    const handleTryClick = (avatar) => {
        setSelectedAvatar(avatar);
    };

    const handleCloseModal = () => {
        setSelectedAvatar(null);
    };

    const handleBuy = async (avatarId) => {
        console.log("Buying avatar with ID:", avatarId);
        await dispatch(buyAvatar({ userId: user.id, shopItemId: avatarId }));
        await dispatch(getCurrentUser());
    }

    useEffect(() => {
        dispatch(getAllAvatars());
    }, [dispatch]);

    console.log(avatars);

    return (
        <>
            <div className='min-h-[calc(100vh-141px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
                <h1 className='text-2xl font-semibold text-center'>АВАТАРИ ДЛЯ ВАС</h1>
                <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 px-4'>
                    <div
                        className='bg-[#C2D3FD] rounded-2xl shadow-lg p-4 flex flex-col items-center justify-center'
                    >
                        <div className='bg-[#85A7FA] p-15 rounded-full'>
                            <div className='w-20 h-20 border-3 border-white rounded-full flex items-center justify-center cursor-pointer'>
                                <Plus size={40} className="text-white" />
                            </div>
                        </div>
                        <div className='flex gap-2 mt-4'>
                            <Button className="text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer">Завантажити аватар з пристрою</Button>
                        </div>
                    </div>
                    {avatars.map(avatar => (
                        <div
                            key={avatar.id}
                            className='relative bg-[#C2D3FD] rounded-2xl shadow-lg p-4 flex flex-col items-center justify-between'
                        >
                            <div className='absolute top-3 right-3 text-black text-sm flex items-center gap-2 font-bold px-2 py-1 rounded'>
                                {avatar.price}
                                <img src="/coin.png" alt="Coin" className='w-7 h-7' />
                            </div>

                            <div className='flex flex-col gap-4'>
                                <img
                                    src={avatar?.photoPath}
                                    className='w-40 h-40 object-cover rounded-full'
                                />
                                <h1 className='text-lg text-center font-semibold'>{avatar.name}</h1>
                            </div>

                            <div className='flex gap-2 mt-4'>
                                <Button className="text-sm px-10 py-2 cursor-pointer" onClick={() => handleTryClick(avatar)}>Спробувати</Button>
                                <Button className="text-sm px-10 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer" onClick={() => handleBuy(avatar.id)}>Купити</Button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>

            {selectedAvatar && (
                <TryBuyAvatar avatar={selectedAvatar} onClose={handleCloseModal} />
            )}
        </>
    );
}

export default BuyAvatar;