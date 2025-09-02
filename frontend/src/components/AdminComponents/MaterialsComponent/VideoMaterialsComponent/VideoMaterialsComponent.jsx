import React, { useEffect, useState } from 'react';
import AddEditVideoMaterialModalComponent from './AddEditVideoMaterialModalComponent/AddEditVideoMaterialModalComponent';
import { Button } from '@/components/ui/button';
import { useDispatch, useSelector } from 'react-redux';
import { deleteMaterial, getAllMaterialThemes } from '@/redux/material/Action';
import DeleteVideoModalComponent from './DeleteVideoModalComponent/DeleteVideoModalComponent';
import VideoMaterialDetails from './VideoMaterialDetails/VideoMaterialDetails';

const VideoMaterialsComponent = ({ videos }) => {
    const [selectedVideo, setSelectedVideo] = useState(null);
    const [editVideo, setEditVideo] = useState(null);
    const [videoToDelete, setVideoToDelete] = useState(null);

    const dispatch = useDispatch();
    const topics = useSelector(state => state.material.themes);

    const handleDelete = (video) => {
        dispatch(deleteMaterial(video.id));
        setVideoToDelete(null);
    };

    useEffect(() => {
        dispatch(getAllMaterialThemes());
    }, [dispatch]);

    if (selectedVideo) {
        return <VideoMaterialDetails video={selectedVideo} onBack={() => setSelectedVideo(null)} topics={topics} />;
    }

    if (editVideo !== null) {
        return (
            <AddEditVideoMaterialModalComponent
                video={editVideo}
                onBack={() => setEditVideo(null)}
            />
        );
    }

    return (
        <div className="flex flex-col gap-6 px-10 py-6">
            <h1 className="text-2xl font-bold mb-4">Теоретичні матеріали</h1>

            <div className="flex flex-col gap-4">
                <div className="grid grid-cols-1 gap-4">
                    <div className="w-full grid grid-cols-[auto_1fr_1fr] items-center gap-4">
                        <div></div>
                        <div className="font-bold">Назва</div>
                    </div>
                    {videos?.map((video) => {
                        return (
                            <div
                                key={video?.id}
                                onClick={() => setSelectedVideo(video)}
                                className="w-full bg-[#C2D3FD] cursor-pointer rounded-4xl p-4 flex justify-between items-center transition hover:scale-[1.005]"
                            >
                                {/* Ліва частина: зображення, назва, теми */}
                                <div className="flex items-start gap-4 w-full">
                                    {/* Фото */}
                                    <img
                                        src={video?.photoPath || '/placeholder.jpg'}
                                        alt="video preview"
                                        className="w-24 h-24 rounded-xl object-cover"
                                    />

                                    {/* Назва і теми */}
                                    <div className="flex flex-col">
                                        <h2 className="text-lg font-bold mb-2">{video?.name || '—'}</h2>
                                        {Array.isArray(video?.theme) && video.theme.length > 0 ? (
                                            <div className="text-sm text-gray-700 list-decimal list-inside flex gap-4">
                                                {video.theme.map((topic, index) => (
                                                    <span key={index}>{topics?.[topic]}</span>
                                                ))}
                                            </div>
                                        ) : (
                                            <p className="text-sm text-gray-700">Тем немає</p>
                                        )}
                                    </div>
                                </div>

                                {/* Права частина: кнопки */}
                                <div className="flex gap-2">
                                    <Button
                                        className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] text-white cursor-pointer"
                                        variant="ghost"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            setEditVideo({ ...video, id: video?.id || 0 });
                                        }}
                                    >
                                        Редагувати
                                    </Button>
                                    <Button
                                        variant="destructive"
                                        className="cursor-pointer"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            setVideoToDelete(video);
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
                            setEditVideo({ id: 0 });
                        }}
                    >
                        <img src="/add_circle.png" alt="Add" />
                        <div className="text-xl">Додати нове відео</div>
                    </div>
                </div>
            </div>


            {videoToDelete && (
                <DeleteVideoModalComponent
                    video={videoToDelete}
                    onConfirm={() => handleDelete(videoToDelete)}
                    onCancel={() => setVideoToDelete(null)}
                />
            )}
        </div>
    );
}

export default VideoMaterialsComponent;