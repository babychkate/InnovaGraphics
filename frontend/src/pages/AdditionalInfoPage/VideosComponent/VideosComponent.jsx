import { Input } from '@/components/ui/input';
import { ArrowLeft, Heart, Search } from 'lucide-react';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import React, { useState } from 'react';
import { Button } from '@/components/ui/button';
import { useDispatch, useSelector } from 'react-redux';
import { extractVideoIdByUrl } from '@/redux/material/Action';

const VideosComponent = ({ videos, topics }) => {
    const dispatch = useDispatch();
    const [selectedVideo, setSelectedVideo] = useState(null);
    const videoId = useSelector(state => state.material.videoId);

    const handleSelectVideo = (video) => {
        setSelectedVideo(video);
        dispatch(extractVideoIdByUrl(video?.link));
    }

    return (
        <div className='min-h-[calc(100vh-121px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
            {!selectedVideo && (
                <>
                    <h1 className='text-2xl font-semibold text-center'>
                        Дивіться та навчайтеся: Відеоуроки з Графіки
                    </h1>
                    <div className='flex items-center px-4 gap-4'>
                        <div className='relative w-full max-w-sm'>
                            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500 w-5 h-5" />
                            <Input className="pl-10 bg-white placeholder-gray-500" placeholder="Шукати за назвою" />
                        </div>
                        <div>
                            <Select>
                                <SelectTrigger className="cursor-pointer bg-white w-[250px]">
                                    <SelectValue placeholder="Фільтрувати за темою" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectGroup>
                                        {Object.entries(topics).map(([key, value]) => (
                                            <SelectItem key={key} value={key}>
                                                {value}
                                            </SelectItem>
                                        ))}
                                    </SelectGroup>
                                </SelectContent>
                            </Select>
                        </div>
                    </div>

                    <div className='flex flex-col gap-4 px-4'>
                        {videos.map((video) => (
                            <div key={video.id} onClick={() => handleSelectVideo(video)} className="cursor-pointer group">
                                <div className="relative flex items-center bg-[#C2D3FD] p-4 rounded-4xl shadow-md overflow-hidden transition hover:scale-[1.01]">
                                    <img
                                        src={video?.photoPath}
                                        alt={video?.name}
                                        className="w-48 h-32 object-cover rounded-4xl"
                                    />
                                    <div className="p-4 flex flex-col justify-between">
                                        <h2 className="text-lg font-semibold">{video?.name}</h2>
                                        <div className="text-sm text-gray-600">
                                            {Array.isArray(video?.theme) && video?.theme.length > 0 ? (
                                                <div className="text-sm text-gray-700 list-decimal list-inside flex gap-4">
                                                    {video?.theme.map((topic, index) => (
                                                        <span key={index}>{topics?.[topic]}</span>
                                                    ))}
                                                </div>
                                            ) : (
                                                <p className="text-sm text-gray-700">Тем немає</p>
                                            )}
                                        </div>
                                        <p className="text-sm mt-2 text-gray-700 line-clamp-3 max-w-[1000px]">
                                            {video?.description}
                                        </p>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </>
            )}

            {selectedVideo && (
                <div className="flex flex-col px-4 w-full max-w-7xl mx-auto">
                    <Button
                        variant="ghost"
                        onClick={() => setSelectedVideo(null)}
                        className='absolute top-32 left-3 z-20 p-2 hover:bg-white/50 rounded-full transition-all duration-300 ease-in-out'
                    >
                        <ArrowLeft size={28} />
                    </Button>

                    <div className="flex flex-col px-15">
                        <h1 className="text-2xl font-bold mb-4">{selectedVideo?.name}</h1>
                        <div className="w-full h-[500px]">
                            <iframe
                                className="w-full h-full"
                                src={`https://www.youtube.com/embed/${videoId}`}
                                title={selectedVideo?.name}
                                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                allowFullScreen
                            />
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default VideosComponent;