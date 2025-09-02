import { Button } from '@/components/ui/button';
import { deleteAvatar, getAllAvatars } from '@/redux/shop_items/Action';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import AddEditAvatarModalComponent from './AddEditAvatarModalComponent/AddEditAvatarModalComponent';
import DeleteAvatarModalComponent from './DeleteAvatarModalComponent/DeleteAvatarModalComponent';
import AvatarDetails from './AvatarDetails/AvatarDetails';

const AvatarsComponent = () => {
    const [selectedAvatar, setSelectedAvatar] = useState(null);
    const [editAvatar, setEditAvatar] = useState(null);
    const [avatarToDelete, setAvatarToDelete] = useState(null);

    const dispatch = useDispatch();
    const avatars = useSelector(state => state.shopItems.shopItems);
    console.log(avatars);

    const handleDelete = (avatar) => {
        dispatch(deleteAvatar(avatar.id));
        setAvatarToDelete(null);
    };

    useEffect(() => {
        dispatch(getAllAvatars());
    }, [dispatch]);

    if (selectedAvatar) {
        return <AvatarDetails avatar={selectedAvatar} onBack={() => setSelectedAvatar(null)} />;
    }

    if (editAvatar) {
        return <AddEditAvatarModalComponent avatar={editAvatar} onBack={() => setEditAvatar(null)} />;
    }

    return (
        <div className="flex flex-col gap-6 px-10 py-6">
            <h1 className="text-2xl font-bold mb-4">Аватари</h1>

            <div className="flex flex-col gap-4">
                <div className="grid grid-cols-1 gap-4">
                    <div className="w-full grid grid-cols-[auto_1fr_1fr] items-center gap-4">
                        <div></div>
                        <div className="font-bold">Назва</div>
                    </div>
                    {avatars?.map((avatar) => {
                        return (
                            <div
                                key={avatar?.id}
                                onClick={() => setSelectedAvatar(avatar)}
                                className="w-full bg-[#C2D3FD] cursor-pointer rounded-4xl p-4 flex justify-between items-center transition hover:scale-[1.005]"
                            >
                                {/* Ліва частина: зображення, назва, теми */}
                                <div className="flex items-center justify-start gap-4 w-full">
                                    {/* Фото */}
                                    <img
                                        src={avatar?.photoPath || '/placeholder.jpg'}
                                        alt="video preview"
                                        className="w-24 h-24 rounded-full object-cover"
                                    />
                                    <h1 className='font-bold text-2xl'>{avatar?.name}</h1>
                                </div>

                                {/* Права частина: кнопки */}
                                <div className="flex gap-2">
                                    <Button
                                        className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] text-white cursor-pointer"
                                        variant="ghost"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            setEditAvatar({ ...avatar, id: avatar?.id || 0 });
                                        }}
                                    >
                                        Редагувати
                                    </Button>
                                    <Button
                                        variant="destructive"
                                        className="cursor-pointer"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            setAvatarToDelete(avatar);
                                        }}
                                    >
                                        Видалити
                                    </Button>
                                </div>
                            </div>
                        );
                    })}
                    <div
                        className="w-full bg-[#C2D3FD] hover:bg-[#abc3ff] cursor-pointer rounded-4xl p-4 flex items-center gap-8 transition hover:scale-[1.005]"
                        onClick={() => {
                            setEditAvatar({ id: 0 });
                        }}
                    >
                        <img src="/add_circle.png" alt="Add" />
                        <div className="text-xl">Додати новий аватар</div>
                    </div>
                </div>
            </div>


            {avatarToDelete && (
                <DeleteAvatarModalComponent
                    avatar={avatarToDelete}
                    onConfirm={() => handleDelete(avatarToDelete)}
                    onCancel={() => setAvatarToDelete(null)}
                />
            )}
        </div>
    );
}

export default AvatarsComponent;